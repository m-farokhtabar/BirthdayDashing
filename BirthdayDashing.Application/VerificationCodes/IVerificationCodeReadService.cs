using BirthdayDashing.Application.Dtos.VerficationCodes.Output;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BirthdayDashing.Application.VerificationCodes
{
    public interface IVerificationCodeReadService
    {
        Task<List<VerificationCodeDataForVerifyDto>> GetDataForVerifyAsync(Guid UserId, string Token);
    }
}