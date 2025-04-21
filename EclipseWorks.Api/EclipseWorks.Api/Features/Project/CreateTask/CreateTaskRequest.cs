using EclipseWorks.Api.Features.Project.Common;

namespace EclipseWorks.Api.Features.Project.CreateTask
{
    public class CreateTaskRequest
    {
        public TaskPriorityEnum Priority { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
    }
}
