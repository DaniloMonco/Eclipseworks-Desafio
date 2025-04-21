using EclipseWorks.Application.Project.GetTasksByProject;
using EclipseWorks.Domain.Entities;
using EclipseWorks.Domain.Repository;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace EclipseWorks.Application.Test
{
    public class GetTasksByProjectHandlerTest
    {
        private readonly IProjectRepository _repository;

        public GetTasksByProjectHandlerTest()
        {
            _repository = Substitute.For<IProjectRepository>();
        }

        [Fact]
        public async Task Give_me_projects_when_userid_have_permission()
        {
            var projectMock = new Domain.Entities.Project("project 1", null, Guid.NewGuid());
            var task1Mock = projectMock.AddTask(new Domain.ValueObjects.Task(Domain.Entities.PriorityEnum.Medium, "Task 1", string.Empty, projectMock.Id));
            var task2Mock = projectMock.AddTask(new Domain.ValueObjects.Task(Domain.Entities.PriorityEnum.Low, "Task 2", string.Empty, projectMock.Id));
            _repository.GetAsync(projectMock.Id, Arg.Any<CancellationToken>()).Returns(projectMock);

            var request = new GetTasksByProjectQuery
            {
                ProjectId = projectMock.Id,
            };

            var handler = new GetTasksByProjectHandler(_repository);
            var result = await handler.Handle(request, CancellationToken.None);
            Assert.Equal(2, result.Count());
            Assert.Equal(task1Mock.ReferenceKey, result.ElementAt(0).ReferenceKey);
            Assert.Equal(task1Mock.Name, result.ElementAt(0).Name);
            Assert.Equal(task1Mock.Comments, result.ElementAt(0).Comments);
            Assert.Equal(task1Mock.Status, (StatusEnum)result.ElementAt(0).Status);
            Assert.Equal(task1Mock.Priority, (PriorityEnum)result.ElementAt(0).Priority);

            Assert.Equal(task2Mock.ReferenceKey, result.ElementAt(1).ReferenceKey);
            Assert.Equal(task2Mock.Name, result.ElementAt(1).Name);
            Assert.Equal(task2Mock.Comments, result.ElementAt(1).Comments);
            Assert.Equal(task2Mock.Status, (StatusEnum)result.ElementAt(1).Status);
            Assert.Equal(task2Mock.Priority, (PriorityEnum)result.ElementAt(1).Priority);


            await _repository.Received().GetAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Give_me_exception_when_projects_not_found_by_userid()
        {
            var request = new GetTasksByProjectQuery
            {
                ProjectId = Guid.NewGuid(),
            };

            _repository.GetAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).ReturnsNull();

            var handler = new GetTasksByProjectHandler(_repository);
            var action = () => handler.Handle(request, CancellationToken.None);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(action);
            Assert.Equal("Value cannot be null. (Parameter 'Project')", exception.Message);
        }

    }
}