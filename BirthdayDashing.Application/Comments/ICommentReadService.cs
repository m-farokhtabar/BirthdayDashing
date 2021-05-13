using BirthdayDashing.Application.Dtos.Comment.Output;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BirthdayComment.Application.Comments
{
    public interface ICommentReadService
    {
        Task<CommentDto> GetAsync(Guid id);
        Task<IEnumerable<CommentListDto>> GetByDashingIdAsync(Guid DashingId, DateTime? Date = null);
    }
}