using System;
using System.Threading.Tasks;

namespace BirthdayDashing.Domain.Comment
{
    public interface ICommentRepository
    {
        Task<Comment> GetAsync(Guid Id);
        Task AddAsync(Comment entity);
        Task UpdateAsync(Comment entity);
        Task UpdateActiveAsync(Comment entity);
    }
}
