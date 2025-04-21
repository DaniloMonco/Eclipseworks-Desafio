using EclipseWorks.Application.Project.CreateProject;
using EclipseWorks.Application.Project.CreateTask;
using EclipseWorks.Domain.Events;
using EclipseWorks.Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseWorks.Application.Project.UpdateTask
{
    public class UpdateTaskHandler : IRequestHandler<UpdateTaskCommand, UpdateTaskResult>
    {
        private readonly IProjectRepository _repository;
        private readonly Domain.Events.IPublisher _publisher;

        public UpdateTaskHandler(IProjectRepository repository, Domain.Events.IPublisher publisher)
        {
            _repository = repository;
            _publisher = publisher;
        }

        public async Task<UpdateTaskResult> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var project = await _repository.GetAsync(request.ProjectId, cancellationToken);
            if (project is null)
                throw new ArgumentNullException("Project");

            var task = project.ChangeTask(request.ReferenceKey, request.Name, request.Comments, (Domain.Entities.StatusEnum)request.Status);

            await _repository.UpdateAsync(project, task, cancellationToken);

            await _publisher.Publish(new TaskChangedEvent(task, project.UserId));

            return new UpdateTaskResult { ReferenceKey = request.ReferenceKey, ProjectId = request.ProjectId };
        }
    }
}
