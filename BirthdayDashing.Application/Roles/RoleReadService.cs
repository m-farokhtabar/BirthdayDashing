using BirthdayDashing.Application.Configuration.Data;
using BirthdayDashing.Application.Dtos.Roles.Output;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BirthdayDashing.Application.Roles
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
        public async Task<List<RoleDto>> GetAllAsync()
        {
            return (await DbEntities.QueryAsync<RoleDto>("SELECT [Id],[Name] FROM [Role]")).AsList();
        }
    }
}
