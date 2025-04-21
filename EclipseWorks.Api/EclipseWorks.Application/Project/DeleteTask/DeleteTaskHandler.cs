using EclipseWorks.Domain.Events;
using EclipseWorks.Domain.Repository;
using MediatR;

namespace EclipseWorks.Application.Project.DeleteTask
{
    public class DeleteTaskHandler : IRequestHandler<DeleteTaskCommand, Unit>
    {
        private readonly IProjectRepository _repository;
        private readonly Domain.Events.IPublisher _publisher;

        public DeleteTaskHandler(IProjectRepository repository, Domain.Events.IPublisher publisher)
        {
            _repository = repository;
            _publisher = publisher;
        }

        public async Task<Unit> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var project = await _repository.GetAsync(request.ProjectId, cancellationToken);
            if (project is null)
                throw new ArgumentNullException("Project");

            var task = project.RemoveTask(request.ReferenceKey);
            
            await _repository.DeleteAsync(project, task, cancellationToken);

            await _publisher.Publish(new TaskDeletedEvent(task, project.UserId));

            return Unit.Value;

        }
    }

}
