using EclipseWorks.Application.Project.Common;
using EclipseWorks.Domain.Entities;

namespace EclipseWorks.Application.Project.GetTasksByProject
{
    public class GetTasksByProjectResult
    {
        public Guid ReferenceKey { get; set; }
        public TaskPriorityEnum Priority { get; set; }
        public TaskStatusEnum Status { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;

    }
}
