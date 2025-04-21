using EclipseWorks.Audit.Infrastructure.Dao;
using EclipseWorks.Audit.Infrastructure.RabbitMq;
using EclipseWorks.Audit.Messages;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace EclipseWorks.Audit
{
    public class ProjectDeletedWorker : BackgroundService
    {
        private readonly ILogger<ProjectDeletedWorker> _logger;
        public readonly RabbitMqConnector _connector;
        private readonly ProjectMessageDao _messageDao;

        public ProjectDeletedWorker(ILogger<ProjectDeletedWorker> logger, RabbitMqConnector connector, ProjectMessageDao messageDao)
        {
            _logger = logger;
            _connector = connector;
            _messageDao = messageDao;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connection = await _connector.CreateConnection();
            var channel = await connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queue: "project.deleted.queue",
                                             durable: true,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var projectDeletedMessage = JsonConvert.DeserializeObject<ProjectDeletedMessage>(message);

                await _messageDao.SaveAsync(projectDeletedMessage, MessageAction.Deleted);
            };

            await channel.BasicConsumeAsync(queue: "project.deleted.queue",
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
