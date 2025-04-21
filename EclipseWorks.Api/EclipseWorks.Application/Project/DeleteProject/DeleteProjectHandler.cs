using EclipseWorks.Domain.Events;
using EclipseWorks.Domain.Repository;
using MediatR;

namespace EclipseWorks.Application.Project.DeleteProject
{
    public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand, Unit>
    {
        private readonly IProjectRepository _repository;
        private readonly Domain.Events.IPublisher _publisher;

        public DeleteProjectHandler(IProjectRepository repository, Domain.Events.IPublisher publisher)
        {
            _repository = repository;
            _publisher = publisher;
        }

        public async Task<Unit> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {

            var project = await _repository.GetAsync(request.ProjectId, cancellationToken);
            if (project is null)
                throw new ArgumentNullException("Project");
            if (!project.CanBeRemoved())
                throw new ArgumentException("Unable to delete with unfinished tasks");

            await _repository.DeleteAsync(project, cancellationToken);

            await _publisher.Publish(new ProjectDeletedEvent(project, project.UserId));
            return Unit.Value;

        }
    }
}
