using BirthdayDashing.Application.Authorization;
using BirthdayDashing.Application.Configuration.Data;
using BirthdayDashing.Application.Dtos.Comment.Output;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BirthdayComment.Application.Comments
{
    public class CommentReadService : ICommentReadService
    {
        private readonly IReadDbSet DbSet;
        private IDbConnection DbEntities => DbSet.GetDbEntities();
        private readonly IAuthorizationService AuthorizationService;

        public CommentReadService(IReadDbSet dbSet, IAuthorizationService authorizationService)
        {
            DbSet = dbSet;
            AuthorizationService = authorizationService;
        }
        public async Task<CommentDto> GetAsync(Guid id)
        {
            const string Query = "SELECT [Id],[UserId],[DashingId],[ParentId],[Content],[Type],[MediaUrl],[Active],[CreatedDate] FROM [Comment] WHERE [Id]=@Id";
            CommentDto result = await DbEntities.QueryFirstOrDefaultAsync<CommentDto>(Query, new { id });
            if (result != null)
                AuthorizationService.JustOwnerAuthorized(result.UserId);
            return result;
        }
        public async Task<IList<CommentListDto>> GetByDashingIdAsync(Guid DashingId, DateTime? CreatedDate = null)
        {
            List<CommentListDto> result = null;
            Dictionary<Guid, CommentListDto> CommentDictionary = new();
            IEnumerable<CommentListDto> data = await DbEntities.QueryAsync<CommentListDto>("CommentsList", param: new { DashingId, AuthorizationService.UserId, CreatedDate }, commandType: CommandType.StoredProcedure);
            if (data is not null)
            {
                result = data.Where(x => x.ParentId == null).Select(x => new CommentListDto()
                {
                    Id = x.Id,
                    ParentId = x.ParentId,
                    Content = x.Content,
                    CreatedDate = x.CreatedDate,
                    MediaUrl = x.MediaUrl,
                    UserFullName = x.UserFullName,
                    UserId = x.UserId,
                    UserImageUrl = x.UserImageUrl,
                    Children = data.Where(y => y.ParentId == x.Id).ToList()
                }).ToList();
                //foreach (var item in data.Where(x => x.ParentId == null))
                //{
                //    result.Add(item);
                //    List<CommentListDto> Children = data.Where(x => x.ParentId == item.Id).ToList();
                //    if (Children?.Count > 0)
                //    {
                //        item.Children = new List<CommentListDto>();
                //        item.Children.AddRange(Children);
                //    }
                //}
            }
            return result;
        }
        public async Task<IList<CommentListDto>> GetChildren(Guid id, DateTime? CreatedDate = null)
        {
            if (!CreatedDate.HasValue)
                CreatedDate = DateTime.Now;
            const string Query = "SELECT [CC].[Id],[CC].[UserId],[CC].[ParentId],[CC].[Content],[CC].[MediaUrl],[CC].[CreatedDate]," +
                                 "[User].FirstName + ' ' + [User].LastName AS [UserFullName],[User].imageUrl As [UserImageUrl] FROM " +
                                 "(SELECT [Comment].[Id],[Comment].[UserId],[Comment].[ParentId],[Comment].[Content],[Comment].[MediaUrl],[Comment].[CreatedDate] FROM [Comment] " +
                                 "WHERE [ParentId]=@Id AND ([UserId]=@UserId OR [Active]=1) AND [CreatedDate]<=@CreatedDate) AS CC " +
                                 "LEFT JOIN [User] ON [CC].[UserId] = [User].[Id] " +
                                 "ORDER BY [CreatedDate] DESC OFFSET 0 ROWS FETCH FIRST 10 ROWS ONLY";
            return (await DbEntities.QueryAsync<CommentListDto>(Query, param: new { id, AuthorizationService.UserId, CreatedDate })).ToList();
        }
    }
}
