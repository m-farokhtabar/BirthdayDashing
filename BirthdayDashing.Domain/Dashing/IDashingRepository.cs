using System;
using System.Threading.Tasks;

namespace BirthdayDashing.Domain.Dashing
{
    public interface IDashingRepository
    {
        Task<Dashing> GetAsync(Guid Id);
        Task AddAsync(Dashing entity);
        Task UpdateAsync(Dashing entity);
        Task UpdateActiveAsync(Dashing entity);
        Task UpdateDeletedAsync(Dashing entity);
    }
}
