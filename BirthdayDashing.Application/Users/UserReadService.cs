using BirthdayDashing.Application.Configuration.Data;
using BirthdayDashing.Application.Dtos.Roles.Output;
using BirthdayDashing.Application.Dtos.Users.Output;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BirthdayDashing.Application.Users
{
    public class UserReadService : IUserReadService
    {
        private readonly IReadDbSet DbSet;
        private IDbConnection DbEntities => DbSet.GetDbEntities();
        public UserReadService(IReadDbSet dbSet)
        {
            DbSet = dbSet;
        }
        public async Task<UserDto> GetAsync(Guid id)
        {
            return await DbEntities.QueryFirstOrDefaultAsync<UserDto>("SELECT [Id],[Email],[Birthday],[PostalCode],[FirstName],[LastName],[PhoneNumber],[ImageUrl] FROM [User] WHERE [Id]=@Id", new { id });
        }        
        public async Task<UserEssentialDataDto> GetEssentialDataAsync(Guid id)
        {
            return await GetEssentialDataAsync<Guid>("Id", id);
        }
        public async Task<UserEssentialDataDto> GetEssentialDataByEmailAsync(string email)
        {
            return await GetEssentialDataAsync<string>("Email", email);
        }
        private async Task<UserEssentialDataDto> GetEssentialDataAsync<T>(string fieldName, T value)
        {
            string Query = "SELECT [CurrentUser].[Id], [CurrentUser].[Email], [CurrentUser].[IsApproved], [Role].[Name] FROM " +
                           $"(SELECT [Id],[Email],[IsApproved] FROM [User] WHERE [{fieldName}]=@Value) AS [CurrentUser] " +
                           "LEFT JOIN [UserRole] ON [CurrentUser].[Id]=[UserRole].[UserId] LEFT JOIN [Role] ON [Role].[Id] = [UserRole].[RoleId]";
            UserEssentialDataDto ResultUserWithRolesName = null;
            var Result = (await DbEntities.QueryAsync<UserEssentialDataDto, RoleNameDto, UserEssentialDataDto>(Query,
            (User, Role) =>
            {
                if (ResultUserWithRolesName == null)
                {
                    ResultUserWithRolesName = User;
                    ResultUserWithRolesName.Roles = new List<RoleNameDto>();
                }
                if (Role != null)
                    ResultUserWithRolesName.Roles.Add(Role);
                return ResultUserWithRolesName;
            },
            splitOn: "Name",
            param: new { value })).FirstOrDefault();

            return Result;
        }        
    }
}
