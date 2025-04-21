using EclipseWorks.Api.Features.Project.Common;

namespace EclipseWorks.Api.Features.Project.UpdateTask
{
    public class UpdateTaskRequest
    {
        public TaskStatusEnum Status { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;

    }
}
