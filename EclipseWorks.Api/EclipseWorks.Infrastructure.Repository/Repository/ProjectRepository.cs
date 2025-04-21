using Dapper;
using EclipseWorks.Domain.Entities;
using EclipseWorks.Domain.Repository;
using EclipseWorks.Infrastructure.Common;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client.Logging;

namespace EclipseWorks.Infrastructure.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DapperContext _context;

        public ProjectRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Project project, Domain.ValueObjects.Task task, CancellationToken cancellationToken)
        {
            var queryTask = @"insert into public.""Task"" (""ReferenceKey"", ""Priority"", ""Status"", ""Name"", ""Comments"", ""ProjectId"", ""UpdateAt"")
                              values (:ReferenceKey, :Priority, :Status, :Name, :Comments, :ProjectId, :UpdateAt)";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(queryTask, new
                {
                    task.ReferenceKey,
                    task.Priority,
                    task.Status,
                    task.Name,
                    task.Comments,
                    task.UpdateAt,
                    ProjectId = project.Id
                });
            }
        }

        public async Task CreateAsync(Project project, CancellationToken cancellationToken)
        {
            var queryProject = @"insert into public.""Project"" (""Id"", ""Name"", ""Description"", ""UserId"") values (:Id, :Name, :Description, :UserId)";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(queryProject, new
                {
                    project.Id,
                    project.Name,
                    project.Description,
                    project.UserId
                });
            }
        }

        public async Task DeleteAsync(Project project, CancellationToken cancellationToken)
        {
            var queryProject = @"delete from public.""Project"" where ""Id"" = :Id";
            var queryTask = @"delete from public.""Task"" where ""ReferenceKey"" = :ReferenceKey";
            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                try
                {
                    if (project.Tasks.Any())
                    {
                        foreach (var task in project.Tasks)
                        {
                            await connection.ExecuteAsync(queryTask, new
                            {
                                task.ReferenceKey
                            }, transaction);
                        }
                    }

                    await connection.ExecuteAsync(queryProject, new
                    {
                        project.Id
                    }, transaction);


                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task DeleteAsync(Project project, Domain.ValueObjects.Task task, CancellationToken cancellationToken)
        {
            var queryTask = @"delete from public.""Task"" where ""ReferenceKey"" = :ReferenceKey";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(queryTask, new { task.ReferenceKey });
            }
        }

        public async Task<IEnumerable<Project>> GetAllAsync(Guid userId, CancellationToken cancellationToken)
        {
            var query = @"SELECT ""Id"", ""Name"", ""Description"", ""UserId"" FROM public.""Project"" where ""UserId"" = :UserId";
            using (var connection = _context.CreateConnection())
            {
                var projects = await connection.QueryAsync<Project>(query, new { userId });
                return projects;
            }
        }

        public async Task<Project> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            Project? project = null;
            var queryProject = @"SELECT ""Id"", ""Name"", ""Description"", ""UserId"" FROM public.""Project"" where ""Id"" = :Id";
            var queryTask = @"SELECT ""ReferenceKey"", ""Priority"", ""Status"", ""Name"", ""Comments"", ""UpdateAt"", ""ProjectId""  FROM public.""Task"" WHERE ""ProjectId"" = :ProjectId";
            using (var connection = _context.CreateConnection())
            {
                project = await connection.QueryFirstOrDefaultAsync<Project>(queryProject, new { id });
                var tasks = await connection.QueryAsync<Domain.ValueObjects.Task>(queryTask, new { ProjectId = id });
                foreach (var task in tasks)
                    project?.AddTask(task);
            }
            return project;
        }

        public async Task UpdateAsync(Project project, CancellationToken cancellationToken)
        {
            var queryProject = @"update public.""Project"" 
                              set ""Name"" = :Name, ""Description"" = :Description, ""UserId"" = :UserId
                              where ""Id"" = :Id";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(queryProject, new { project.Name, project.Description, project.UserId, project.Id});
            }
        }

        public async Task UpdateAsync(Project project, Domain.ValueObjects.Task task, CancellationToken cancellationToken)
        {
            var queryTask = @"update public.""Task"" 
                              set ""Status"" = :Status, ""Name"" = :Name, ""Comments"" = :Comments, ""UpdateAt"" = :UpdateAt 
                              where ""ReferenceKey"" = :ReferenceKey";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(queryTask, new {task.Status, task.Name, task.Comments, task.UpdateAt,  task.ReferenceKey });
            }
        }
    }
}
