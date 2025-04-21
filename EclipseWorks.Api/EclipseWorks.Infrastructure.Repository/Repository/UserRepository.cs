using Dapper;
using EclipseWorks.Domain.Entities;
using EclipseWorks.Domain.Repository;
using EclipseWorks.Infrastructure.Common;

namespace EclipseWorks.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _context;

        public UserRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<User> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var query = "SELECT Id, Position FROM UserRole where Id = :Id";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<User>(query, new { id });   
            }
        }

    }
}
