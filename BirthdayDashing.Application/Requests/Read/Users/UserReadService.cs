using BirthdayDashing.Application.Data;
using BirthdayDashing.Application.Dtos.Users.Input;
using BirthdayDashing.Application.Dtos.Users.Output;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BirthdayDashing.Application.Requests.Read.Users
{
    public class UserReadService : IUserReadService
    {
        private readonly IReadDbSet DbSet;
        private IDbConnection DbEntities => DbSet.GetDbEntities();
        public UserReadService(IReadDbSet dbSet)
        {
            DbSet = dbSet;
        }
        public async Task<UserDto> Get(Guid id)
        {
            return await DbEntities.QueryFirstOrDefaultAsync<UserDto>("SELECT [Id],[Email],[Birthday],[PostalCode],[FirstName],[LastName],[PhoneNumber],[ImageUrl] FROM [User] WHERE [Id]=@Id", new { id });
        }
        public async Task<UserWithRolesNameDto> GetAuthentocateDataAsync(LoginDto login)
        {
            const string Query = "SELECT [CurrentUser].[Id],[CurrentUser].[Password],[Role].[Name] FROM (SELECT [Id],[Password] FROM [User] WHERE [Email]=@Email) AS [CurrentUser]" +
                                        "LEFT JOIN [UserRole] ON [CurrentUser].[Id]=[UserRole].[UserId] LEFT JOIN [Role] ON [Role].[Id] = [UserRole].[RoleId]";

            UserWithRolesNameDto ResultUserWithRolesName = null;
            var Result = (await DbEntities.QueryAsync<UserWithRolesNameDto, RoleNameDto, UserWithRolesNameDto>(Query,
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
            param: new { login.Email })).FirstOrDefault();

            return Result;
        }
    }
}
