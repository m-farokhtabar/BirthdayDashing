using System;
using System.Threading.Tasks;

namespace BirthdayDashing.Domain.Users
{
    public interface IUserRepository
    {
        Task<User> GetAsync(Guid Id);
        Task<User> GetByEmailAsync(string email);

        Task AddAsync(User entity);
        
        Task UpdateAsync(User entity);
        Task UpdateIsApprovedAsync(User entity);
        Task UpdatePasswordAsync(User entity);
        Task UpdateLockOutThresholdAsync(User entity);
        Task UpdateLastLoginDateAsync(User entity);
    }
}
