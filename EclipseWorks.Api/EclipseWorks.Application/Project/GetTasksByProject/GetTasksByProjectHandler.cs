using EclipseWorks.Application.Project.Common;
using EclipseWorks.Domain.Repository;
using MediatR;

namespace EclipseWorks.Application.Project.GetTasksByProject
{
    public class GetTasksByProjectHandler : IRequestHandler<GetTasksByProjectQuery, IEnumerable<GetTasksByProjectResult>>
    {
        private readonly IProjectRepository _repository;

        public GetTasksByProjectHandler(IProjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<GetTasksByProjectResult>> Handle(GetTasksByProjectQuery request, CancellationToken cancellationToken)
        {
            var project = await  _repository.GetAsync(request.ProjectId, cancellationToken);
            if (project is null)
                throw new ArgumentNullException("Project");

            return project.Tasks.Select(t => new GetTasksByProjectResult
            {
                Comments = t.Comments,
                Name = t.Name,
                Priority = (TaskPriorityEnum) t.Priority,
                ReferenceKey = t.ReferenceKey,
                Status = (TaskStatusEnum)t.Status
            });
        }
    }
}
