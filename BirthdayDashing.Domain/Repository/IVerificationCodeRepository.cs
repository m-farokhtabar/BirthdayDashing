using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BirthdayDashing.Domain.Repository
{
    public interface IVerificationCodeRepository
    {
        Task<List<VerificationCode>> GetAsync(Guid UserId, string Token);
        Task AddAsync(VerificationCode entity);
    }
}
