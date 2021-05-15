using BirthdayDashing.Application.Dtos.Comment.Input;
using System;
using System.Threading.Tasks;

namespace BirthdayDashing.Application.Comments
{
    public interface ICommentWriteService
    {
        Task AddAsync(AddCommentDto comment);        
        Task UpdateAsync(Guid id, UpdateCommentDto comment);
        Task<bool> ToggleActiveAsync(Guid id);
    }
}