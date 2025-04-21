using EclipseWorks.Domain.Entities;
using EclipseWorks.Domain.Events;
using EclipseWorks.Domain.Repository;
using MediatR;

namespace EclipseWorks.Application.Project.CreateProject
{
    public class CreateProjectHandler : IRequestHandler<CreateProjectCommand, CreateProjectResult>
    {
        private readonly IProjectRepository _repository;
        private readonly Domain.Events.IPublisher _publisher;

        public CreateProjectHandler(IProjectRepository repository, Domain.Events.IPublisher publisher)
        {
            _repository = repository;
            _publisher = publisher;
        }

        public async Task<CreateProjectResult> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var model = new Domain.Entities.Project(request.Name, request.Description, request.UserId);
            await _repository.CreateAsync(model, cancellationToken);

            await _publisher.Publish(new ProjectCreatedEvent(model, request.UserId));
            return new CreateProjectResult { Id = model.Id };
        }
    }
}
