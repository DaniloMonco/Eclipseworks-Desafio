using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseWorks.Domain.Events
{
    public interface IPublisher
    {
        Task Publish(ProjectCreatedEvent @event);
        Task Publish(ProjectChangedEvent @event);
        Task Publish(ProjectDeletedEvent @event);
        Task Publish(TaskCreatedEvent @event);
        Task Publish(TaskChangedEvent @event);
        Task Publish(TaskDeletedEvent @event);
    }
}
