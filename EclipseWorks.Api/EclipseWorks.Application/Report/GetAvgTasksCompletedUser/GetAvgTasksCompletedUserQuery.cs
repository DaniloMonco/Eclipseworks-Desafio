using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseWorks.Application.Report.GetAvgTasksCompletedUser
{
    public record GetAvgTasksCompletedUserQuery(int IntervalDays, Guid UserId) : IRequest<IEnumerable<GetAvgTasksCompletedUserResult>>;
}
