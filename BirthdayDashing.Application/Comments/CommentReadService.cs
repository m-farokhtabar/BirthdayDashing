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
        public async Task<IEnumerable<CommentListDto>> GetByDashingIdAsync(Guid DashingId, DateTime? Date = null)
        {
            string DateCondition;
            if (Date is null)
                DateCondition = "";
            else
                DateCondition = "AND [CreatedDate]>=@Date";
            string Query = "SELECT " +
                                 "RecentCommentsUser.[Id],RecentCommentsUser.[UserId],RecentCommentsUser.[ParentId],RecentCommentsUser.[Content],RecentCommentsUser.[MediaUrl],RecentCommentsUser.[CreatedDate]," +
                                 "RecentCommentsUser.[FullName],RecentCommentsUser.[imageUrl]," +
                                 "InnerRecentCommentsUser.[Id],InnerRecentCommentsUser.[UserId],InnerRecentCommentsUser.[ParentId],InnerRecentCommentsUser.[Content],InnerRecentCommentsUser.[MediaUrl],InnerRecentCommentsUser.[CreatedDate]," +
                                 "InnerRecentCommentsUser.[FullName],InnerRecentCommentsUser.[imageUrl]" +
                                 "FROM " +
            "(SELECT RecentComments.[Id],[UserId],[ParentId],[Content],[MediaUrl],[CreatedDate],([User].[FirstName] + ' ' + [User].[LastName]) AS FullName,[User].[imageUrl] FROM " +
            $"(SELECT[Id],[UserId],[ParentId],[Content],[MediaUrl],[CreatedDate] FROM [Comment] WHERE [DashingId] = @DashingId AND[ParentId] IS NULL AND ([UserId] = @UserId OR [Active] = 1) {DateCondition}" +
            "ORDER BY [CreatedDate] DESC OFFSET 0 ROWS FETCH FIRST 15 ROWS ONLY) AS RecentComments LEFT JOIN[User] ON RecentComments.UserId = [User].Id) AS RecentCommentsUser " +
            "LEFT JOIN 	" +
            "(SELECT InnerRecentComments.[Id],[UserId],[ParentId],[Content],[MediaUrl],[CreatedDate], ([User].[FirstName] + ' ' + [User].[LastName]) AS FullName,[User].[imageUrl] FROM " +
            $"(SELECT[Id],[UserId],[ParentId],[Content],[MediaUrl],[CreatedDate] FROM [Comment] WHERE [DashingId] = @DashingId AND [ParentId] IS NOT NULL AND ([UserId] = @UserId OR [Active] = 1) {DateCondition}) As InnerRecentComments " +
            "LEFT JOIN[User] ON InnerRecentComments.UserId = [User].Id) AS InnerRecentCommentsUser " +
            "ON InnerRecentCommentsUser.[ParentId] = RecentCommentsUser.[Id] ORDER BY RecentCommentsUser.[CreatedDate] DESC, InnerRecentCommentsUser.[CreatedDate] DESC";

            var CommentDictionary = new Dictionary<Guid, CommentListDto>();
            return (await DbEntities.QueryAsync<CommentListDto, CommentListDto, CommentListDto>(Query, (Comment, InnerComment) =>
             {
                 if (!CommentDictionary.TryGetValue(Comment.Id, out CommentListDto CommentEntry))
                 {
                     CommentEntry = Comment;
                     CommentEntry.Children = new List<CommentListDto>();
                     CommentDictionary.Add(CommentEntry.Id, CommentEntry);
                 }
                 CommentEntry.Children.Add(InnerComment);
                 return CommentEntry;
             }, param: new { DashingId, AuthorizationService.UserId, Date })).Distinct().AsList();
        }
    }
}
