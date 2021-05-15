using System;

namespace BirthdayDashing.Application.Dtos.VerficationCodes.Output
{
    public class VerificationCodeDataForVerifyDto
    {
        public DateTime ExpireDate { get; set; }
        public VerificationTypeDto Type { get; set; }
    }
    public enum VerificationTypeDto
    {
        ConfirmUserByEmail,
        ForgotPasswordByEmail,
        ConfirmUserBySMS
    }
}
