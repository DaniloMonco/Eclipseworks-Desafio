using EclipseWorks.Audit.Infrastructure.Dao;
using EclipseWorks.Audit.Infrastructure.RabbitMq;
using EclipseWorks.Audit.Messages;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace EclipseWorks.Audit
{
    public class TaskDeletedWorker : BackgroundService
    {
        private readonly ILogger<TaskDeletedWorker> _logger;
        public readonly RabbitMqConnector _connector;
        public readonly TaskMessageDao _messageDao;

        public TaskDeletedWorker(ILogger<TaskDeletedWorker> logger, RabbitMqConnector connector, TaskMessageDao messageDao)
        {
            _logger = logger;
            _connector = connector;
            _messageDao = messageDao;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connection = await _connector.CreateConnection();
            var channel = await connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queue: "task.deleted.queue",
                                             durable: true,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var taskDeletedMessage = JsonConvert.DeserializeObject<TaskDeletedMessage>(message);

                await _messageDao.SaveAsync(taskDeletedMessage, MessageAction.Deleted);
            };

            await channel.BasicConsumeAsync(queue: "task.deleted.queue",
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
