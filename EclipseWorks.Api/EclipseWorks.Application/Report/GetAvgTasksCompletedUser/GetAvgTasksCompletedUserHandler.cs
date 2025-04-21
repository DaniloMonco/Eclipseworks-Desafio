using EclipseWorks.Application.Project.Common;
using EclipseWorks.Application.Project.GetTasksByProject;
using EclipseWorks.Domain.DAO;
using EclipseWorks.Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseWorks.Application.Report.GetAvgTasksCompletedUser
{
    internal class GetAvgTasksCompletedUserHandler : IRequestHandler<GetAvgTasksCompletedUserQuery, IEnumerable<GetAvgTasksCompletedUserResult>>
    {
        private readonly IReportDao _dao;
        private readonly IUserRepository _userRepository;

        public GetAvgTasksCompletedUserHandler(IReportDao dao, IUserRepository userRepository)
        {
            _dao = dao;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<GetAvgTasksCompletedUserResult>> Handle(GetAvgTasksCompletedUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(request.UserId, cancellationToken);
            if (!user.IsManager())
                throw new ArgumentException("Only manager can visualize report");

            var report = await _dao.GetAvgTasksCompletedUserReportAsync(request.IntervalDays, cancellationToken);
            if (report is null)
                throw new ArgumentNullException("report not found");

            return report.Select(r => new GetAvgTasksCompletedUserResult(r.UserId, r.Avg));
        }
    
    }
}
