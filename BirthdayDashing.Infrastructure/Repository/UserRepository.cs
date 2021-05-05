using BirthdayDashing.Domain;
using BirthdayDashing.Domain.Repository;
using BirthdayDashing.Infrastructure.Data.Write;
using BirthdayDashing.Infrastructure.Repository.Base;
using Dapper;
using System;
using System.Threading.Tasks;

namespace BirthdayDashing.Infrastructure.Repository
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IDbSet dbSet) : base(dbSet)
        {
        }

        public async Task AddAsync(User entity)
        {
            const string command = "INSERT INTO [User]([Id],[Email],[Password],[Birthday],[PostalCode],[FirstName],[LastName],[PhoneNumber],[ImageUrl],[IsApproved]) " +
                                   "VALUES(@Id,@Email,@Password,@Birthday,@PostalCode,@FirstName,@LastName,@PhoneNumber,@ImageUrl,@IsApproved); " +
                                   "INSERT INTO [UserRole]([UserId],[RoleId]) VALUES(@Id,@RoleId)";
            await DbEntities.ExecuteAsync(command, new { Id = entity.Id, Email = entity.Email, Password = entity.Password, Birthday = entity.Birthday, PostalCode = entity.PostalCode, FirstName = entity.FirstName, LastName = entity.LastName, PhoneNumber = entity.PhoneNumber, ImageUrl = entity.ImageUrl, IsApproved = entity.IsApproved, RoleId = entity.UserRoles.AsList()[0].RoleId }, Transaction);
        }

        public async Task<User> GetAsync(Guid id)
        {
            return await DbEntities.QueryFirstOrDefaultAsync<User>("SELECT * FROM [User] WHERE [Id]=@Id", new { id }, Transaction);
        }

        public async Task UpdateAsync(User entity)
        {
            const string command = "UPDATE [User] SET " +
                                   "[Birthday]=@Birthday ,[PostalCode]=@PostalCode ,[FirstName] = @FirstName,[LastName] = @LastName, [PhoneNumber] = @PhoneNumber,[ImageUrl] = @ImageUrl " +
                                   "WHERE [Id]=@Id AND [RowVersion]=@RowVersion";
            await DbEntities.ExecuteAsync(command, new { Birthday = entity.Birthday, PostalCode = entity.PostalCode, FirstName = entity.FirstName, LastName = entity.LastName, PhoneNumber = entity.PhoneNumber, ImageUrl = entity.ImageUrl, Id = entity.Id, RowVersion = entity.RowVersion }, Transaction);
        }
        public async Task UpdateIsApprovedAsync(User entity)
        {
            const string command = "UPDATE [User] SET [IsApproved]=@IsApproved WHERE [Id]=@Id AND [RowVersion]=@RowVersion";
            await DbEntities.ExecuteAsync(command, new { IsApproved = entity.IsApproved, Id = entity.Id, RowVersion = entity.RowVersion }, Transaction);
        }
        public async Task UpdatePasswordAsync(User entity)
        {
            const string command = "UPDATE [User] SET [Password]=@Password WHERE [Id]=@Id AND [RowVersion]=@RowVersion";
            await DbEntities.ExecuteAsync(command, new { Password = entity.Password, Id = entity.Id, RowVersion = entity.RowVersion }, Transaction);
        }
    }
}
