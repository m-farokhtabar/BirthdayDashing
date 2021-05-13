using BirthdayDashing.Application.Authorization;
using BirthdayDashing.Application.Dtos.Comment.Input;
using BirthdayDashing.Domain.Comment;
using BirthdayDashing.Domain.SeedWork;
using Common.Exception;
using System;
using System.Threading.Tasks;
using static Common.Exception.Messages;

namespace BirthdayDashing.Application.Comments
{
    public class CommentWriteService : ICommentWriteService
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly ICommentRepository Repository;
        private readonly IAuthorizationService AuthorizationService;

        public CommentWriteService(IUnitOfWork unitOfWork, ICommentRepository repository, IAuthorizationService authorizationService)
        {
            UnitOfWork = unitOfWork;
            Repository = repository;
            AuthorizationService = authorizationService;
        }
        public async Task AddAsync(AddCommentDto comment)
        {
            if (comment.UserId.HasValue)
                AuthorizationService.JustOwnerAuthorized(comment.UserId);

            Comment entity = new(comment.UserId, comment.DashingId, comment.ParentId, comment.Content, (CommentType)((int)comment.Type), comment.MediaUrl, AuthorizationService.UserId);
            try
            {
                await Repository.AddAsync(entity);
                UnitOfWork.SaveChanges();
            }
            catch
            {
                UnitOfWork.RollBack();
                throw;
            }
        }
        public async Task UpdateAsync(Guid id, UpdateCommentDto comment)
        {
            Comment entity = await Repository.GetAsync(id);
            if (entity is null)
                throw new ManualException(DATA_IS_NOT_FOUND.Replace("{0}", "Comment"), ExceptionType.NotFound, nameof(id));            
            AuthorizationService.JustOwnerAuthorized(entity.UserId);

            entity.Update(comment.Content, comment.MediaUrl, AuthorizationService.UserId);
            try
            {
                await Repository.UpdateAsync(entity);
                UnitOfWork.SaveChanges();
            }
            catch
            {
                UnitOfWork.RollBack();
                throw;
            }
        }
        public async Task<bool> ToggleActiveAsync(Guid id)
        {
            Comment entity = await Repository.GetAsync(id);
            if (entity is null)
                throw new ManualException(DATA_IS_NOT_FOUND.Replace("{0}", "Comment"), ExceptionType.NotFound, nameof(id));

            AuthorizationService.JustOwnerAuthorized(entity.UserId);

            entity.ToggleActive(AuthorizationService.UserId);
            try
            {
                await Repository.UpdateActiveAsync(entity);
                UnitOfWork.SaveChanges();
            }
            catch
            {
                UnitOfWork.RollBack();
                throw;
            }
            return entity.Active;
        }
    }
}
