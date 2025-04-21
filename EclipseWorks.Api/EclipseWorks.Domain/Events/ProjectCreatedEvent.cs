using EclipseWorks.Domain.Entities;

namespace EclipseWorks.Domain.Events
{
    public class ProjectCreatedEvent : DomainEvent<Project>
    {
        public ProjectCreatedEvent(Project data, Guid userId) : base(data, userId)
        {
        }
    }
}
