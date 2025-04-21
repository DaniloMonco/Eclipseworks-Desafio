using EclipseWorks.Audit.Infrastructure.Dao;
using EclipseWorks.Audit.Infrastructure.RabbitMq;
using EclipseWorks.Audit.Messages;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace EclipseWorks.Audit
{
    public class TaskChangedWorker : BackgroundService
    {
        private readonly ILogger<TaskChangedWorker> _logger;
        public readonly RabbitMqConnector _connector;
        private readonly TaskMessageDao _messageDao;

        public TaskChangedWorker(ILogger<TaskChangedWorker> logger, RabbitMqConnector connector, TaskMessageDao messageDao)
        {
            _logger = logger;
            _connector = connector;
            _messageDao = messageDao;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connection = await _connector.CreateConnection();
            var channel = await connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queue: "task.changed.queue",
                                             durable: true,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var taskChangedMessage = JsonConvert.DeserializeObject<TaskChangedMessage>(message);

                await _messageDao.SaveAsync(taskChangedMessage, MessageAction.Changed);
            };

            await channel.BasicConsumeAsync(queue: "task.changed.queue",
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
