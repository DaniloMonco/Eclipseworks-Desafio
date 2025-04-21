using EclipseWorks.Api.Features.Project.Common;

namespace EclipseWorks.Api.Features.Project.GetTasksByProject
{
    public record GetTasksByProjectResponse(Guid ReferenceKey, TaskPriorityEnum Priority, TaskStatusEnum Status, string Name, string Comments);
}
