using Dapper;
using EclipseWorks.Audit.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipseWorks.Audit.Infrastructure.Dao
{
    public class ProjectMessageDao
    {
        private readonly DapperContext _context;

        public ProjectMessageDao(DapperContext context)
        {
            _context = context;
        }

        public async Task SaveAsync(ProjectBaseMessage message, MessageAction action)
        {
            var query = @"insert into public.""ProjectAudit""(""Id"", ""OccurredIn"", ""UserId"", ""Action"", ""ProjectId"", ""ProjectName"", ""ProjectDescription"", ""ProjectUserId"")
                                     values (:Id, :OccurredIn, :UserId, :Action, :ProjectId, :ProjectName, :ProjectDescription, :ProjectUserId)";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new
                {
                    Id = Guid.NewGuid(),
                    OccurredIn = message.TimeStamp,
                    message.UserId,
                    Action = action,
                    ProjectId = message.Data.Id,
                    ProjectName = message.Data.Name,
                    ProjectDescription = message.Data.Description,
                    ProjectUserId = message.Data.UserId,
                });
            }
        }
    }
}
