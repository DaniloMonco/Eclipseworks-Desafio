using EclipseWorks.Api.Features.Project.CreateProject;
using EclipseWorks.Api.Features.Project.CreateTask;
using EclipseWorks.Api.Features.Project.GetProject;
using EclipseWorks.Api.Features.Project.GetTasksByProject;
using EclipseWorks.Api.Features.Project.UpdateTask;
using EclipseWorks.Api.Middleware;
using EclipseWorks.Application.Project.CreateProject;
using EclipseWorks.Application.Project.CreateTask;
using EclipseWorks.Application.Project.DeleteProject;
using EclipseWorks.Application.Project.DeleteTask;
using EclipseWorks.Application.Project.GetProjects;
using EclipseWorks.Application.Project.GetTasksByProject;
using EclipseWorks.Application.Project.UpdateTask;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EclipseWorks.Api.Features.Project
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("User/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<GetProjectResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProjects([FromRoute] Guid userId, CancellationToken cancellationToken)
        {
            var query = new GetProjectsQuery { UserId = userId };
            var result = await _mediator.Send(query , cancellationToken);

            return Ok(result.Select(r => new GetProjectResponse(r.Id, r.Description, r.Name)));
        }


        [HttpPost]
        [ProducesResponseType(typeof(CreateProjectResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateProjectCommand { UserId = request.UserId, Description = request.Description, Name = request.Name };
            var result = await _mediator.Send(command, cancellationToken);
            return Created(string.Empty, new CreateProjectResponse {Id = result.Id });
        }

        [HttpPost("{projectId}/Task")]
        [ProducesResponseType(typeof(CreateTaskResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTask([FromRoute] Guid projectId, [FromBody] CreateTaskRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateTaskCommand
            {
                Comments = request.Comments,
                Name = request.Name,
                ProjectId = projectId,
                Priority = (Application.Project.Common.TaskPriorityEnum)request.Priority
            };
            var result = await _mediator.Send(command, cancellationToken);
            return Created(string.Empty, new CreateTaskResponse {ReferenceKey= result.ReferenceKey });
        }


        [HttpPut("{projectId}/Task/{taskId}")]
        [ProducesResponseType(typeof(UpdateTaskResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTask([FromRoute] Guid projectId, [FromRoute] Guid taskId, [FromBody] UpdateTaskRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateTaskCommand
            {
                Comments = request.Comments,
                Name = request.Name,
                ProjectId = projectId,
                ReferenceKey = taskId,
                Status = (Application.Project.Common.TaskStatusEnum)request.Status
            };
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new UpdateTaskResponse {ProjectId = result.ProjectId, ReferenceKey=result.ReferenceKey });
        }


        [HttpDelete("{projectId}/Task/{taskId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTask([FromRoute] Guid projectId, [FromRoute] Guid taskId, CancellationToken cancellationToken)
        {
            var command = new DeleteTaskCommand { ProjectId = projectId, ReferenceKey = taskId };
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }

        [HttpDelete("{projectId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProject([FromRoute] Guid projectId, CancellationToken cancellationToken)
        {
            var command = new DeleteProjectCommand { ProjectId = projectId };
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }



        [HttpGet("{projectId}/Tasks")]
        [ProducesResponseType(typeof(IEnumerable<GetTasksByProjectResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTasksByProject([FromRoute] Guid projectId, CancellationToken cancellationToken)
        {
            var query = new GetTasksByProjectQuery { ProjectId = projectId };
            var result = await _mediator.Send(query, cancellationToken); 
            return Ok(result.Select(r=> new GetTasksByProjectResponse(r.ReferenceKey, (Common.TaskPriorityEnum)r.Priority, (Common.TaskStatusEnum)r.Status, r.Name, r.Comments)));
        }





    }
}
