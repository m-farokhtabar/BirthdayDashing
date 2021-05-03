using BirthdayDashing.Application.Data;
using Dapper;
using System;
using System.Data;
using System.Threading.Tasks;

namespace BirthdayDashing.Application.Requests.Read.Roles
{
    public class RoleReadService : IRoleReadService
    {
        private readonly IReadDbSet DbSet;
        private IDbConnection DbEntities => DbSet.GetDbEntities();
        public RoleReadService(IReadDbSet dbSet)
        {
            DbSet = dbSet;
        }
        public async Task<Guid?> GetIdByNameAsync(string name)
        {
            return await DbEntities.QueryFirstOrDefaultAsync<Guid?>("SELECT [Id] FROM [Role] WHERE [Name]=@Name", new { name });
        }
    }
}
