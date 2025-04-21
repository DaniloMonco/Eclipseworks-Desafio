using EclipseWorks.Application.Project.Common;
using EclipseWorks.Application.Project.CreateTask;
using EclipseWorks.Domain.Events;
using EclipseWorks.Domain.Repository;
using MediatR;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace EclipseWorks.Application.Test
{
    public class CreateTaskHandlerTest
    {
        private readonly IProjectRepository _repository;
        private readonly EclipseWorks.Domain.Events.IPublisher _publisher;

        public CreateTaskHandlerTest()
        {
            _repository = Substitute.For<IProjectRepository>();
            _publisher = Substitute.For<EclipseWorks.Domain.Events.IPublisher>();
        }

        [Fact]
        public async Task Give_me_success_when_task_is_created()
        {
            var request = new CreateTaskCommand
            {
                ProjectId = Guid.NewGuid(),
                Name = "Task 1",
                Comments = "Description",
                Priority = TaskPriorityEnum.Medium
            };

            var projectMock = new Domain.Entities.Project("project 1", null, Guid.NewGuid());
            _repository.GetAsync(request.ProjectId, Arg.Any<CancellationToken>()).Returns(projectMock);

            var handler = new CreateTaskHandler(_repository, _publisher);
            var result = await handler.Handle(request, CancellationToken.None);
            Assert.NotEqual(Guid.Empty, result.ReferenceKey);
            await _repository.Received().CreateAsync(Arg.Any<Domain.Entities.Project>(), Arg.Any<Domain.ValueObjects.Task>(), Arg.Any<CancellationToken>());
            await _publisher.Received().Publish(Arg.Any<TaskCreatedEvent>());
        }

        [Fact]
        public async Task Give_me_exception_when_project_not_found()
        {
            var request = new CreateTaskCommand
            {
                ProjectId = Guid.NewGuid(),
                Name = "Task 1",
                Comments = "Description",
                Priority = TaskPriorityEnum.Medium
            };

            _repository.GetAsync(request.ProjectId, Arg.Any<CancellationToken>()).ReturnsNull();

            var handler = new CreateTaskHandler(_repository, _publisher);
            var action = () => handler.Handle(request, CancellationToken.None);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(action);
            Assert.Equal("Value cannot be null. (Parameter 'Project')", exception.Message);
        }
    }
}