using BirthdayDashing.Application.Authorization;
using BirthdayDashing.Application.Configuration.Data;
using BirthdayDashing.Application.Dtos.Dashings.Output;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BirthdayDashing.Application.Dashings
{
    public class DashingReadService : IDashingReadService
    {
        private readonly IReadDbSet DbSet;
        private IDbConnection DbEntities => DbSet.GetDbEntities();
        private readonly IAuthorizationService AuthorizationService;

        public DashingReadService(IReadDbSet dbSet, IAuthorizationService authorizationService)
        {
            DbSet = dbSet;
            AuthorizationService = authorizationService;
        }
        public async Task<DashingDto> GetAsync(Guid id)
        {
            const string Query = "SELECT [Id],[UserId],[Birthday],[Title],[PostalCode],[DashingAmount],[Active],[Deleted]," +
                                 "[BackgroundUUID],[Name],[CurrentYearBirthday],[City],[State],[TitleUpdated] " +
                                 "FROM [Dashing] WHERE [Id]=@Id";
            var result = await DbEntities.QueryFirstOrDefaultAsync<DashingDto>(Query, new { id });
            if (result != null)
                AuthorizationService.Authorized(result.UserId);
            return result;
        }
        public async Task<IEnumerable<DashingDto>> GetByUserIdAsync(Guid userId)
        {
            const string Query = "SELECT [Id],[UserId],[Birthday],[Title],[PostalCode],[DashingAmount],[Active],[Deleted]," +
                                 "[BackgroundUUID],[Name],[CurrentYearBirthday],[City],[State],[TitleUpdated] " +
                                 "FROM [Dashing] WHERE [UserId]=@UserId AND [Deleted]=0 ORDER BY [CreatedDate] DESC";
            AuthorizationService.Authorized(userId);
            return (await DbEntities.QueryAsync<DashingDto>(Query, new { userId })).AsList();
        }
    }
}
