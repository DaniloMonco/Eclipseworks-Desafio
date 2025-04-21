using EclipseWorks.Application.Project.GetProjects;
using EclipseWorks.Domain.Repository;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace EclipseWorks.Application.Test
{
    public class GetProjectsHandlerTest
    {
        private readonly IProjectRepository _repository;

        public GetProjectsHandlerTest()
        {
            _repository = Substitute.For<IProjectRepository>();
        }

        [Fact]
        public async Task Give_me_projects_when_userid_have_permission()
        {
            var projectMock = new Domain.Entities.Project("project 1", null, Guid.NewGuid());
            var taskMock = projectMock.AddTask(new Domain.ValueObjects.Task(Domain.Entities.PriorityEnum.Medium, "Task", string.Empty, projectMock.Id));
            _repository.GetAllAsync(projectMock.UserId, Arg.Any<CancellationToken>()).Returns(new[] { projectMock });


            var request = new GetProjectsQuery
            {
                UserId = projectMock.UserId,
            };

            var handler = new GetProjectsHandler(_repository);
            var result = await handler.Handle(request, CancellationToken.None);
            Assert.Single(result);
            Assert.Equal(projectMock.Name, result.First().Name);
            Assert.Equal(projectMock.Id, result.First().Id);
            Assert.Equal(projectMock.Description, result.First().Description);

            await _repository.Received().GetAllAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Give_me_exception_when_projects_not_found_by_userid()
        {
            var request = new GetProjectsQuery
            {
                UserId = Guid.NewGuid(),
            };

            _repository.GetAllAsync(request.UserId, Arg.Any<CancellationToken>()).ReturnsNull();

            var handler = new GetProjectsHandler(_repository);
            var action = () => handler.Handle(request, CancellationToken.None);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(action);
            Assert.Equal("Value cannot be null. (Parameter 'Projects')", exception.Message);
        }

    }
}