using BirthdayDashing.Domain.Comment;
using BirthdayDashing.Infrastructure.Data.Write;
using BirthdayDashing.Infrastructure.Repository.Base;
using Dapper;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BirthdayDashing.Infrastructure.Repository
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        public CommentRepository(IDbSet dbSet) : base(dbSet)
        {
        }

        public async Task AddAsync(Comment entity)
        {
            const string command = "INSERT INTO [Comment]([Id],[UserId],[DashingId],[ParentId],[Content],[Type],[MediaUrl],[Active],[CreatedDate],[CreatedById],[LastEditById],[LastEditDate])" +
                                   " VALUES(@Id,@UserId,@DashingId,@ParentId,@Content,@Type,@MediaUrl,@Active,@CreatedDate,@CreatedById,@LastEditById,@LastEditDate)";
            await DbEntities.ExecuteAsync(command, new
            {
                entity.Id,
                entity.UserId,
                entity.DashingId,
                entity.ParentId,
                entity.Content,
                entity.Type,
                entity.MediaUrl,
                entity.Active,
                entity.CreatedDate,
                entity.CreatedById,
                entity.LastEditById,
                entity.LastEditDate
            }, Transaction);
        }

        public async Task<Comment> GetAsync(Guid id)
        {
            return (await DbEntities.QueryAsync<Comment>("SELECT * FROM [Comment] WHERE [Comment].[Id] = @id", param: new { id }, transaction: Transaction)).FirstOrDefault();
        }
        public async Task UpdateAsync(Comment entity)
        {
            const string command = "UPDATE [dbo].[Comment] SET " +
                                   "[Content]=@Content,[MediaUrl]=@MediaUrl,[LastEditById]=@LastEditById,[LastEditDate]=@LastEditDate " +
                                   "WHERE [Id]=@Id AND [RowVersion]=@RowVersion";
            await DbEntities.ExecuteAsync(command, new
            {
                entity.Content,
                entity.MediaUrl,
                entity.LastEditById,
                entity.LastEditDate,
                entity.Id,
                entity.RowVersion
            }, Transaction);
        }
        public async Task UpdateActiveAsync(Comment entity)
        {
            const string command = "UPDATE [Comment] SET [Active]=@Active WHERE [Id]=@Id AND [RowVersion]=@RowVersion";
            await DbEntities.ExecuteAsync(command, new { entity.Active, entity.Id, entity.RowVersion }, Transaction);
        }
    }
}
