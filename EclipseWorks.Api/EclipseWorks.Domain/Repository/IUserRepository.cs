
using EclipseWorks.Domain.Entities;

namespace EclipseWorks.Domain.Repository
{
    public interface IUserRepository
    {
        Task<User> GetAsync(Guid id, CancellationToken cancellationToken);
    }
}
