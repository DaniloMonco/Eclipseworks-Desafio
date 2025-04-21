using EclipseWorks.Domain.Entities;

namespace EclipseWorks.Domain.Events
{
    public class TaskChangedEvent : DomainEvent<ValueObjects.Task>
    {
        public TaskChangedEvent(ValueObjects.Task data, Guid userId) : base(data, userId)
        {
        }
    }

}
