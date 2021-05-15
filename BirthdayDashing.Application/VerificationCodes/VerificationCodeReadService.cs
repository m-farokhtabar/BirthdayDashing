using BirthdayDashing.Application.Configuration.Data;
using BirthdayDashing.Application.Dtos.VerficationCodes.Output;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BirthdayDashing.Application.VerificationCodes
{
    public class VerificationCodeReadService : IVerificationCodeReadService
    {
        private readonly IReadDbSet DbSet;
        private IDbConnection DbEntities => DbSet.GetDbEntities();
        public VerificationCodeReadService(IReadDbSet dbSet)
        {
            DbSet = dbSet;
        }

        public async Task<List<VerificationCodeDataForVerifyDto>> GetDataForVerifyAsync(Guid UserId, string Token)
        {
            return (await DbEntities.QueryAsync<VerificationCodeDataForVerifyDto>("SELECT ExpireDate,Type FROM [VerificationCode] WHERE UserId=@UserId AND [Token]=@Token", new { UserId, Token })).AsList();
        }
    }
}
