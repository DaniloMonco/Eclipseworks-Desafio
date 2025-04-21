using EclipseWorks.Domain.Entities;

namespace EclipseWorks.Domain.Events
{
    public class TaskDeletedEvent : DomainEvent<ValueObjects.Task>
    {
        public TaskDeletedEvent(ValueObjects.Task data, Guid userId) : base(data, userId)
        {
        }
    }

}
