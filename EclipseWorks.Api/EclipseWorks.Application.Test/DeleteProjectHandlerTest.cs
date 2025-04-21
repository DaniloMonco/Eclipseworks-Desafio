using EclipseWorks.Application.Project.DeleteProject;
using EclipseWorks.Domain.Events;
using EclipseWorks.Domain.Repository;
using MediatR;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace EclipseWorks.Application.Test
{
    public class DeleteProjectHandlerTest
    {
        private readonly IProjectRepository _repository;
        private readonly EclipseWorks.Domain.Events.IPublisher _publisher;

        public DeleteProjectHandlerTest()
        {
            _repository = Substitute.For<IProjectRepository>();
            _publisher = Substitute.For<EclipseWorks.Domain.Events.IPublisher>();
        }

        [Fact]
        public async Task Give_me_success_when_task_is_deleted()
        {
            var request = new DeleteProjectCommand
            {
                ProjectId = Guid.NewGuid(),
            };

            var projectMock = new Domain.Entities.Project("project 1", null, Guid.NewGuid());
            _repository.GetAsync(request.ProjectId, Arg.Any<CancellationToken>()).Returns(projectMock);

            var handler = new DeleteProjectHandler(_repository, _publisher);
            var result = await handler.Handle(request, CancellationToken.None);
            Assert.Equal(Unit.Value, result);
            await _repository.Received().DeleteAsync(Arg.Any<Domain.Entities.Project>(), Arg.Any<CancellationToken>());
            await _publisher.Received().Publish(Arg.Any<ProjectDeletedEvent>());
        }

        [Fact]
        public async Task Give_me_exception_when_project_not_found()
        {
            var request = new DeleteProjectCommand
            {
                ProjectId = Guid.NewGuid(),
            };

            _repository.GetAsync(request.ProjectId, Arg.Any<CancellationToken>()).ReturnsNull();

            var handler = new DeleteProjectHandler(_repository, _publisher);
            var action = () => handler.Handle(request, CancellationToken.None);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(action);
            Assert.Equal("Value cannot be null. (Parameter 'Project')", exception.Message);
        }

        [Fact]
        public async Task Give_me_exception_when_cannot_delete_project()
        {
            var request = new DeleteProjectCommand
            {
                ProjectId = Guid.NewGuid(),
            };

            var projectMock = new Domain.Entities.Project("project 1", null, Guid.NewGuid());
            projectMock.AddTask(new Domain.ValueObjects.Task(Domain.Entities.PriorityEnum.Medium, "Task", string.Empty, request.ProjectId));
            _repository.GetAsync(request.ProjectId, Arg.Any<CancellationToken>()).Returns(projectMock);

            var handler = new DeleteProjectHandler(_repository, _publisher);
            var action = () => handler.Handle(request, CancellationToken.None);

            var exception = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Equal("Unable to delete with unfinished tasks", exception.Message);
        }

    }
}