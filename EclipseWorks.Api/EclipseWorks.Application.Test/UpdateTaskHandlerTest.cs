using EclipseWorks.Application.Project.Common;
using EclipseWorks.Application.Project.UpdateTask;
using EclipseWorks.Domain.Events;
using EclipseWorks.Domain.Repository;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace EclipseWorks.Application.Test
{
    public class UpdateTaskHandlerTest
    {
        private readonly IProjectRepository _repository;
        private readonly EclipseWorks.Domain.Events.IPublisher _publisher;

        public UpdateTaskHandlerTest()
        {
            _repository = Substitute.For<IProjectRepository>();
            _publisher = Substitute.For<EclipseWorks.Domain.Events.IPublisher>();
        }

        [Fact]
        public async Task Give_me_success_when_task_is_changed()
        {
            var projectMock = new Domain.Entities.Project("project 1", null, Guid.NewGuid());
            var taskMock = projectMock.AddTask(new Domain.ValueObjects.Task(Domain.Entities.PriorityEnum.Medium, "Task", string.Empty, projectMock.Id));
            _repository.GetAsync(projectMock.Id, Arg.Any<CancellationToken>()).Returns(projectMock);


            var request = new UpdateTaskCommand
            {
                ReferenceKey = taskMock.ReferenceKey,
                ProjectId = projectMock.Id,
                Comments = taskMock.Comments,
                Name = "Task Changed",
                Status = (TaskStatusEnum)taskMock.Status
            };

            var handler = new UpdateTaskHandler(_repository, _publisher);
            var result = await handler.Handle(request, CancellationToken.None);
            Assert.Equal(projectMock.Id, result.ProjectId);
            Assert.Equal(taskMock.ReferenceKey, result.ReferenceKey);

            await _repository.Received().UpdateAsync(Arg.Any<Domain.Entities.Project>(), Arg.Any<Domain.ValueObjects.Task>(), Arg.Any<CancellationToken>());
            await _publisher.Received().Publish(Arg.Any<TaskChangedEvent>());
        }

        [Fact]
        public async Task Give_me_exception_when_project_not_found()
        {
            var request = new UpdateTaskCommand
            {
                ReferenceKey = Guid.NewGuid(),
                ProjectId = Guid.NewGuid(),
                Comments = string.Empty,
                Name = "Task Changed",
                Status = TaskStatusEnum.Done
            }; 
            
            _repository.GetAsync(request.ProjectId, Arg.Any<CancellationToken>()).ReturnsNull();

            var handler = new UpdateTaskHandler(_repository, _publisher);
            var action = () => handler.Handle(request, CancellationToken.None);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(action);
            Assert.Equal("Value cannot be null. (Parameter 'Project')", exception.Message);
        }

    }
}