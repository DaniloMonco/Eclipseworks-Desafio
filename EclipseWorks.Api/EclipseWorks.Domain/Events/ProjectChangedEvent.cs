using EclipseWorks.Domain.Entities;

namespace EclipseWorks.Domain.Events
{
    public class ProjectChangedEvent : DomainEvent<Project>
    {
        public ProjectChangedEvent(Project data, Guid userId) : base(data, userId)
        {
        }
    }

}
