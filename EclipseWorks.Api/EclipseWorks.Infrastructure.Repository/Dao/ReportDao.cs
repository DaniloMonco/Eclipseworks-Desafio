using Dapper;
using EclipseWorks.Domain.DAO;
using EclipseWorks.Domain.Entities;
using EclipseWorks.DTO;
using EclipseWorks.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseWorks.Infrastructure.Dao
{
    public class ReportDao : IReportDao
    {
        private readonly DapperContext _context;

        public ReportDao(DapperContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<AvgTasksCompletedUserReport>> GetAvgTasksCompletedUserReportAsync(decimal intervalDays, CancellationToken cancellationToken)
        {
            var query = string.Format(@"
                            select UserId, avg(TaskCount) Avg
                            from
                            (
                            select p.UserId, p.Id, Count(t.ReferenceKey) as TaskCount
                            from Task t inner join project p 
	                            on t.ProjectId = p.Id
                            where t.status = 2
                            and t.updateAt >= NOW() - INTERVAL '{0} days'
                            group by p.UserId, p.Id
                            ) a
                            group by UserId
                            ", intervalDays);
            using (var connection = _context.CreateConnection())
            {
                var report = await connection.QueryAsync<AvgTasksCompletedUserReport>(query);
                return report;
            }
        }
    }
}
