using BirthdayDashing.Domain.VerificationCodes;
using BirthdayDashing.Infrastructure.Data.Write;
using BirthdayDashing.Infrastructure.Repository.Base;
using Dapper;
using System.Threading.Tasks;

namespace BirthdayDashing.Infrastructure.Repository
{
    public class VerificationCodeRepository : BaseRepository, IVerificationCodeRepository
    {
        public VerificationCodeRepository(IDbSet dbSet) : base(dbSet)
        { 
        }

        public async Task AddAsync(VerificationCode entity)
        {
            const string command = "INSERT INTO [VerificationCode]([Id],[UserId],[Token],[ExpireDate],[Type]) VALUES(@Id,@UserId,@Token,@ExpireDate,@Type)";
            await DbEntities.ExecuteAsync(command, new {entity.Id, entity.UserId, entity.Token, entity.ExpireDate, entity.Type }, Transaction);
        }
    }
}
