using EclipseWorks.Audit.Infrastructure.Dao;
using EclipseWorks.Audit.Infrastructure.RabbitMq;
using EclipseWorks.Audit.Messages;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace EclipseWorks.Audit
{
    public class ProjectCreatedWorker : BackgroundService
    {
        private readonly ILogger<ProjectCreatedWorker> _logger;
        public readonly RabbitMqConnector _connector;
        private readonly ProjectMessageDao _messageDao;

        public ProjectCreatedWorker(ILogger<ProjectCreatedWorker> logger, RabbitMqConnector connector, ProjectMessageDao messageDao)
        {
            _logger = logger;
            _connector = connector;
            _messageDao = messageDao;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connection = await _connector.CreateConnection();
            var channel = await connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queue: "project.created.queue",
                                             durable: true,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var projectCreatedMessage = JsonConvert.DeserializeObject<ProjectCreatedMessage>(message);

                await _messageDao.SaveAsync(projectCreatedMessage, MessageAction.Created);
                
            };

            await channel.BasicConsumeAsync(queue: "project.created.queue",
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
