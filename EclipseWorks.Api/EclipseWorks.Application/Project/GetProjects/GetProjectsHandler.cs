using EclipseWorks.Domain.Entities;
using EclipseWorks.Domain.Repository;
using MediatR;

namespace EclipseWorks.Application.Project.GetProjects
{
    public class GetProjectsHandler : IRequestHandler<GetProjectsQuery, IEnumerable<GetProjectsResult>>
    {
        private readonly IProjectRepository _repository;

        public GetProjectsHandler(IProjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<GetProjectsResult>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
        {
            var projects = await _repository.GetAllAsync(request.UserId, cancellationToken);
            if (projects is null)
                throw new ArgumentNullException("Projects");

            return projects.Select(p => new GetProjectsResult
            {
                Description = p.Description,
                Id = p.Id,
                Name = p .Name
            });
        }
    }
}
