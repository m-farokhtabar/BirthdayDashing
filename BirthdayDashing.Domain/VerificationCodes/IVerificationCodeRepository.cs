using System.Threading.Tasks;

namespace BirthdayDashing.Domain.VerificationCodes
{
    public interface IVerificationCodeRepository
    {        
        Task AddAsync(VerificationCode entity);
    }
}
