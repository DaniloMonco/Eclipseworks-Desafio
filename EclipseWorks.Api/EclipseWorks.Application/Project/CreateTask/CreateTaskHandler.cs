using EclipseWorks.Application.Project.CreateProject;
using EclipseWorks.Domain.Events;
using EclipseWorks.Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EclipseWorks.Application.Project.CreateTask
{
    public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, CreateTaskResult>
    {
        private readonly IProjectRepository _repository;
        private readonly Domain.Events.IPublisher _publisher;

        public CreateTaskHandler(IProjectRepository repository, Domain.Events.IPublisher publisher)
        {
            _repository = repository;
            _publisher = publisher;
        }

        public async Task<CreateTaskResult> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var project = await _repository.GetAsync(request.ProjectId, cancellationToken);
            if (project is null)
                throw new ArgumentNullException("Project");

            var task = new Domain.ValueObjects.Task((Domain.Entities.PriorityEnum)request.Priority, request.Name, request.Comments, request.ProjectId);
            project.AddTask(task);
            await _repository.CreateAsync(project, task, cancellationToken);

            await _publisher.Publish(new TaskCreatedEvent(task, project.UserId));
            return new CreateTaskResult { ReferenceKey = task.ReferenceKey};
        }
    }
}
