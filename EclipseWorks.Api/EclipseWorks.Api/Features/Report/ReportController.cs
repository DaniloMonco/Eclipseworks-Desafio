using EclipseWorks.Api.Features.Project.GetProject;
using EclipseWorks.Api.Features.Report.AvgTasksCompletedUser;
using EclipseWorks.Api.Middleware;
using EclipseWorks.Application.Project.GetProjects;
using EclipseWorks.Application.Report.GetAvgTasksCompletedUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EclipseWorks.Api.Features.Report
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("User/{userId}")]
        [ProducesResponseType(typeof(GetAvgTasksCompletedUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAvgTasksCompletedUser([FromRoute]Guid userId, CancellationToken cancellationToken)
        {
            var query = new GetAvgTasksCompletedUserQuery(30, userId);
            var result = await _mediator.Send(query, cancellationToken);
            var response = new GetAvgTasksCompletedUserResponse(query.IntervalDays, result.Select(r => new GetAvgTaskCompletedItem(r.UserId, r.Avg)));
            return Ok(response);
        }
    }
}
