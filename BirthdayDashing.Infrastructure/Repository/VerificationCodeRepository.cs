using BirthdayDashing.Domain.VerificationCodes;
using BirthdayDashing.Infrastructure.Data.Write;
using BirthdayDashing.Infrastructure.Repository.Base;
using Dapper;
using System;
using System.Collections.Generic;
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
            await DbEntities.ExecuteAsync(command, new {Id = entity.Id, UserId = entity.UserId, Token = entity.Token, ExpireDate = entity.ExpireDate, Type = entity.Type }, Transaction);
        }

        public async Task<List<VerificationCode>> GetAsync(Guid UserId,string Token)
        {
            return (await DbEntities.QueryAsync<VerificationCode>("SELECT * FROM [VerificationCode] WHERE UserId=@UserId AND [Token]=@Token", new { UserId, Token }, Transaction)).AsList();
        }
    }
}
