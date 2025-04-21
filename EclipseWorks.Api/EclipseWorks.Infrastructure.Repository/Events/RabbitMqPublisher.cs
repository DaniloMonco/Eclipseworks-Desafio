using EclipseWorks.Domain.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EclipseWorks.Infrastructure.Events
{

    public class RabbitMqPublisher : IPublisher
    {
        private readonly RabbitMqConnector _connector;

        public RabbitMqPublisher(RabbitMqConnector connector)
        {
            _connector = connector;
        }

        protected async Task SendTo(string exchange, byte[] message)
        {
            var channel = await _connector.CreateChannel();
            await channel.BasicPublishAsync(
                exchange: exchange,
                routingKey: "",
                body: message
            );
        }

        public async Task Publish(ProjectCreatedEvent @event)
        {
            var json = JsonConvert.SerializeObject(@event);
            var utf8Bytes = Encoding.UTF8.GetBytes(json);
            await SendTo("project.created", utf8Bytes);
        }

        public async Task Publish(ProjectChangedEvent @event)
        {
            var json = JsonConvert.SerializeObject(@event);
            var utf8Bytes = Encoding.UTF8.GetBytes(json);
            await SendTo("project.changed", utf8Bytes);
        }

        public async Task Publish(ProjectDeletedEvent @event)
        {
            var json = JsonConvert.SerializeObject(@event);
            var utf8Bytes = Encoding.UTF8.GetBytes(json);
            await SendTo("project.deleted", utf8Bytes);
        }

        public async Task Publish(TaskCreatedEvent @event)
        {
            var json = JsonConvert.SerializeObject(@event);
            var utf8Bytes = Encoding.UTF8.GetBytes(json);
            await SendTo("task.created", utf8Bytes);
        }

        public async Task Publish(TaskChangedEvent @event)
        {
            var json = JsonConvert.SerializeObject(@event);
            var utf8Bytes = Encoding.UTF8.GetBytes(json);
            await SendTo("task.changed", utf8Bytes);
        }

        public async Task Publish(TaskDeletedEvent @event)
        {
            var json = JsonConvert.SerializeObject(@event);
            var utf8Bytes = Encoding.UTF8.GetBytes(json);
            await SendTo("task.deleted", utf8Bytes);
        }
    }

}
