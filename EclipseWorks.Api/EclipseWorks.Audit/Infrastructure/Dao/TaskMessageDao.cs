using Dapper;
using EclipseWorks.Audit.Messages;

namespace EclipseWorks.Audit.Infrastructure.Dao
{
    public class TaskMessageDao
    {
        private readonly DapperContext _context;

        public TaskMessageDao(DapperContext context)
        {
            _context = context;
        }

        public async Task SaveAsync(TaskBaseMessage message, MessageAction action)
        {
            var query = @"insert into public.""TaskAudit""(""Id"", ""OccurredIn"", ""UserId"", ""Action"", ""TaskReferenceKey"", ""TaskPriority"", ""TaskStatus"", ""TaskName"", ""TaskComments"", ""TaskUpdateAt"", ""ProjectId"")
                                     values (:Id, :OccurredIn, :UserId, :Action, :TaskReferenceKey, :TaskPriority, :TaskStatus, :TaskName, :TaskComments, :TaskUpdateAt, :ProjectId)";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new
                {
                    Id = Guid.NewGuid(),
                    OccurredIn = message.TimeStamp,
                    message.UserId,
                    Action = action,
                    TaskReferenceKey = message.Data.ReferenceKey,
                    TaskPriority = message.Data.Priority,
                    TaskStatus = message.Data.Status,
                    TaskName = message.Data.Name,
                    TaskComments = message.Data.Comments,
                    TaskUpdateAt = message.Data.UpdateAt,
                    message.Data.ProjectId
                });
            }
        }
    }
}
