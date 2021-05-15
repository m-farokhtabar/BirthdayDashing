using BirthdayDashing.Domain.Users;
using BirthdayDashing.Infrastructure.Data.Write;
using BirthdayDashing.Infrastructure.Repository.Base;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BirthdayDashing.Infrastructure.Repository
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IDbSet dbSet) : base(dbSet)
        {
        }

        private async Task<User> GetByKeyAsync<T>(T key, string KeyName)
        {
            string Query = $"SELECT [CurrentUser].*, [UserRole].[RoleId] FROM (SELECT * FROM [User] WHERE [{KeyName}]=@key) AS [CurrentUser] LEFT JOIN [UserRole] ON [CurrentUser].[Id] = [UserRole].[UserId]";

            User CurrentUser = null;
            List<UserRole> CurrentUserRoles = null;

            var Result = (await DbEntities.QueryAsync<User, UserRole, User>(Query,
            (User, UserRole) =>
            {
                if (CurrentUser == null)
                {
                    CurrentUser = User;
                    CurrentUserRoles = new List<UserRole>();
                }
                if (UserRole != null)
                {
                    CurrentUserRoles.Add(new UserRole(CurrentUser.Id, UserRole.RoleId));
                }
                return CurrentUser;
            },
            splitOn: "RoleId",
            param: new { key }, transaction: Transaction)).FirstOrDefault();
            if (Result != null && CurrentUserRoles?.Count > 0)
                Result.GetType().GetField("userRoles", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(Result, CurrentUserRoles);

            return Result;
        }
        public async Task<User> GetByEmailAsync(string email) => await GetByKeyAsync<string>(email, "Email");
        public async Task<User> GetAsync(Guid id) => await GetByKeyAsync<Guid>(id, "Id");


        public async Task AddAsync(User entity)
        {
            const string command = "INSERT INTO [User]([Id],[Email],[Password],[Birthday],[PostalCode],[FirstName],[LastName],[PhoneNumber],[ImageUrl],[IsApproved],[LastLoginDate],[LockOutThreshold]) " +
                                   "VALUES(@Id,@Email,@Password,@Birthday,@PostalCode,@FirstName,@LastName,@PhoneNumber,@ImageUrl,@IsApproved,@LastLoginDate,@LockOutThreshold); " +
                                   "INSERT INTO [UserRole]([UserId],[RoleId]) VALUES(@Id,@RoleId)";
            await DbEntities.ExecuteAsync(command, new { entity.Id, entity.Email, entity.Password, entity.Birthday, entity.PostalCode, entity.FirstName, entity.LastName, entity.PhoneNumber, entity.ImageUrl, entity.IsApproved, entity.UserRoles.AsList()[0].RoleId, entity.LastLoginDate, entity.LockOutThreshold }, Transaction);
        }


        public async Task UpdateAsync(User entity)
        {
            const string command = "UPDATE [User] SET " +
                                   "[Birthday]=@Birthday ,[PostalCode]=@PostalCode ,[FirstName] = @FirstName,[LastName] = @LastName, [PhoneNumber] = @PhoneNumber,[ImageUrl] = @ImageUrl " +
                                   "WHERE [Id]=@Id AND [RowVersion]=@RowVersion";
            await DbEntities.ExecuteAsync(command, new { entity.Birthday, entity.PostalCode, entity.FirstName, entity.LastName, entity.PhoneNumber, entity.ImageUrl, entity.Id, entity.RowVersion }, Transaction);
        }
        public async Task UpdateIsApprovedAsync(User entity)
        {
            const string command = "UPDATE [User] SET [IsApproved]=@IsApproved WHERE [Id]=@Id AND [RowVersion]=@RowVersion";
            await DbEntities.ExecuteAsync(command, new { entity.IsApproved, entity.Id, entity.RowVersion }, Transaction);
        }
        public async Task UpdatePasswordAsync(User entity)
        {
            const string command = "UPDATE [User] SET [Password]=@Password,[LockOutThreshold]=@LockOutThreshold WHERE [Id]=@Id AND [RowVersion]=@RowVersion";
            await DbEntities.ExecuteAsync(command, new { entity.Password, entity.LockOutThreshold, entity.Id, entity.RowVersion }, Transaction);
        }
        public async Task UpdateLockOutThresholdAsync(User entity)
        {
            const string command = "UPDATE [User] SET [LockOutThreshold]=@LockOutThreshold WHERE [Id]=@Id AND [RowVersion]=@RowVersion";
            await DbEntities.ExecuteAsync(command, new { entity.LockOutThreshold, entity.Id, entity.RowVersion }, Transaction);
        }
        public async Task UpdateLastLoginDateAsync(User entity)
        {
            const string command = "UPDATE [User] SET [LastLoginDate]=@LastLoginDate WHERE [Id]=@Id AND [RowVersion]=@RowVersion";
            await DbEntities.ExecuteAsync(command, new { entity.LastLoginDate, entity.Id, entity.RowVersion }, Transaction);
        }
    }
}
