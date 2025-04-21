using EclipseWorks.Application.Project.DeleteTask;
using EclipseWorks.Domain.Events;
using EclipseWorks.Domain.Repository;
using MediatR;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Threading;

namespace EclipseWorks.Application.Test
{
    public class DeleteTaskHandlerTest
    {
        private readonly IProjectRepository _repository;
        private readonly EclipseWorks.Domain.Events.IPublisher _publisher;

        public DeleteTaskHandlerTest()
        {
            _repository = Substitute.For<IProjectRepository>();
            _publisher = Substitute.For<EclipseWorks.Domain.Events.IPublisher>();
        }

        [Fact]
        public async Task Give_me_success_when_task_is_deleted()
        {
            var projectMock = new Domain.Entities.Project("project 1", null, Guid.NewGuid());
            var taskMock = projectMock.AddTask(new Domain.ValueObjects.Task(Domain.Entities.PriorityEnum.Medium, "Task", string.Empty, projectMock.Id));
            _repository.GetAsync(projectMock.Id, Arg.Any<CancellationToken>()).Returns(projectMock);


            var request = new DeleteTaskCommand
            {
                ReferenceKey = taskMock.ReferenceKey,
                ProjectId = projectMock.Id,
            };

            var handler = new DeleteTaskHandler(_repository, _publisher);
            var result = await handler.Handle(request, CancellationToken.None);
            Assert.Equal(Unit.Value, result);
            await _repository.Received().DeleteAsync(Arg.Any<Domain.Entities.Project>(), Arg.Any<Domain.ValueObjects.Task>(), Arg.Any<CancellationToken>());
            await _publisher.Received().Publish(Arg.Any<TaskDeletedEvent>());
        }

        [Fact]
        public async Task Give_me_exception_when_project_not_found()
        {
            var request = new DeleteTaskCommand
            {
                ReferenceKey = Guid.NewGuid(),
                ProjectId = Guid.NewGuid(),
            };

            _repository.GetAsync(request.ProjectId, Arg.Any<CancellationToken>()).ReturnsNull();

            var handler = new DeleteTaskHandler(_repository, _publisher);
            var action = () => handler.Handle(request, CancellationToken.None);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(action);
            Assert.Equal("Value cannot be null. (Parameter 'Project')", exception.Message);
        }

    }
}