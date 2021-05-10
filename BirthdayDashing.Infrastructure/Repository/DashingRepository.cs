using BirthdayDashing.Domain.Dashing;
using BirthdayDashing.Infrastructure.Data.Write;
using BirthdayDashing.Infrastructure.Repository.Base;
using Dapper;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BirthdayDashing.Infrastructure.Repository
{
    public class DashingRepository : BaseRepository, IDashingRepository
    {
        public DashingRepository(IDbSet dbSet) : base(dbSet)
        {
        }

        public async Task AddAsync(Dashing entity)
        {
            const string command = "INSERT INTO [Dashing]([Id],[UserId],[Birthday],[Title],[PostalCode],[DashingAmount],[Active],[Deleted],[LastEditByID],[LastEditDate]," +
                                   "[CreatedByID],[CreatedDate],[BackgroundUUID],[Name],[CurrentYearBirthday],[City],[State],[TitleUpdated]) " +
                                   "VALUES(@Id,@UserId,@Birthday,@Title,@PostalCode,@DashingAmount,@Active,@Deleted,@LastEditById,@LastEditDate," +
                                   "@CreatedById,@CreatedDate,@BackgroundUUID,@Name,@CurrentYearBirthday,@City,@State,@TitleUpdated)";
            await DbEntities.ExecuteAsync(command, new
            {
                entity.Id,
                entity.UserId,
                entity.Birthday,
                entity.Title,
                entity.PostalCode,
                entity.DashingAmount,
                entity.Active,
                entity.Deleted,
                entity.LastEditById,
                entity.LastEditDate,
                entity.CreatedById,
                entity.CreatedDate,
                entity.BackgroundUUID,
                entity.Name,
                entity.CurrentYearBirthday,
                entity.City,
                entity.State,
                entity.TitleUpdated
            }, Transaction);
        }

        public async Task<Dashing> GetAsync(Guid id)
        {
            return (await DbEntities.QueryAsync<Dashing>("SELECT * FROM [Dashing] WHERE [Dashing].[Id] = @id", param: new { id }, transaction: Transaction)).FirstOrDefault();
        }
        public async Task UpdateAsync(Dashing entity)
        {
            const string command = "UPDATE [dbo].[Dashing] SET " +
                                   "[Title]=@Title,[PostalCode]=@PostalCode,[LastEditByID]=@LastEditById,[LastEditDate]=@LastEditDate," +
                                   "[BackgroundUUID]=@BackgroundUUID,[Name]=@Name,[CurrentYearBirthday]=@CurrentYearBirthday,[City]=@City,[State]=@State,[TitleUpdated]=@TitleUpdated " +
                                   "WHERE [Id]=@Id AND [RowVersion]=@RowVersion";
            await DbEntities.ExecuteAsync(command, new
            {
                entity.Title,
                entity.PostalCode,
                entity.LastEditById,
                entity.LastEditDate,
                entity.BackgroundUUID,
                entity.Name,
                entity.CurrentYearBirthday,
                entity.City,
                entity.State,
                entity.TitleUpdated,
                entity.Id,
                entity.RowVersion
            }, Transaction);
        }
        public async Task UpdateActiveAsync(Dashing entity)
        {
            const string command = "UPDATE [Dashing] SET [Active]=@Active WHERE [Id]=@Id AND [RowVersion]=@RowVersion";
            await DbEntities.ExecuteAsync(command, new { entity.Active, entity.Id, entity.RowVersion }, Transaction);
        }
        public async Task UpdateDeletedAsync(Dashing entity)
        {
            const string command = "UPDATE [Dashing] SET [Deleted]=@Deleted WHERE [Id]=@Id AND [RowVersion]=@RowVersion";
            await DbEntities.ExecuteAsync(command, new { entity.Deleted, entity.Id, entity.RowVersion }, Transaction);
        }
    }
}
