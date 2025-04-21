using EclipseWorks.Application.Project.CreateProject;
using EclipseWorks.Domain.Entities;
using EclipseWorks.Domain.Events;
using EclipseWorks.Domain.Repository;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Linq.Expressions;
using System.Threading;

namespace EclipseWorks.Application.Test
{
    public class CreateProjectHandlerTest
    {
        private readonly IProjectRepository _repository;
        private readonly IPublisher _publisher;

        public CreateProjectHandlerTest()
        {
            _repository = Substitute.For<IProjectRepository>();
            _publisher = Substitute.For<IPublisher>();
        }

        [Fact]
        public async Task Give_me_success_when_project_is_created()
        {
            var request = new CreateProjectCommand
            {
                UserId = Guid.NewGuid(),
                Name = "Project 1",
                Description = "Description"
            };
            var handler = new CreateProjectHandler(_repository, _publisher);
            var result = await handler.Handle(request, CancellationToken.None);
            Assert.NotEqual(Guid.Empty, result.Id);
            await _repository.Received().CreateAsync(Arg.Any<Domain.Entities.Project>(), Arg.Any<CancellationToken>());
            await _publisher.Received().Publish(Arg.Any<ProjectCreatedEvent>());
        }
    }
}