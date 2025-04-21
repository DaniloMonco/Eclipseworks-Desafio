using EclipseWorks.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseWorks.Domain.DAO
{
    public interface IReportDao
    {
        Task<IEnumerable<AvgTasksCompletedUserReport>> GetAvgTasksCompletedUserReportAsync(decimal intervalDays, CancellationToken cancellationToken);
    }
}
