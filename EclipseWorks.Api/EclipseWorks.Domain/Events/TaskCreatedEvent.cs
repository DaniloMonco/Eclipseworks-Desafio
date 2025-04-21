using EclipseWorks.Domain.Entities;

namespace EclipseWorks.Domain.Events
{
    public class TaskCreatedEvent : DomainEvent<ValueObjects.Task>
    {
        public TaskCreatedEvent(ValueObjects.Task data, Guid userId) : base(data, userId)
        {
        }
    }

}
