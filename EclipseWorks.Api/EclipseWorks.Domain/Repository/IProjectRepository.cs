
using EclipseWorks.Domain.Entities;

namespace EclipseWorks.Domain.Repository
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Entities.Project>> GetAllAsync(Guid userId, CancellationToken cancellationToken);
        Task<Entities.Project> GetAsync(Guid id, CancellationToken cancellationToken);
        Task CreateAsync(Entities.Project project, CancellationToken cancellationToken);
        Task UpdateAsync(Entities.Project project, CancellationToken cancellationToken);
        Task DeleteAsync(Entities.Project project, CancellationToken cancellationToken);
        Task DeleteAsync(Project project, ValueObjects.Task task, CancellationToken cancellationToken);
        Task UpdateAsync(Project project, ValueObjects.Task task, CancellationToken cancellationToken);
        Task CreateAsync(Project project, ValueObjects.Task task, CancellationToken cancellationToken);
    }
}
