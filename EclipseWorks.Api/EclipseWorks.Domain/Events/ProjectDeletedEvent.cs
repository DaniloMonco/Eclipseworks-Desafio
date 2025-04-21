using EclipseWorks.Domain.Entities;

namespace EclipseWorks.Domain.Events
{
    public class ProjectDeletedEvent : DomainEvent<Project>
    {
        public ProjectDeletedEvent(Project data, Guid userId) : base(data, userId)
        {
        }
    }
}
