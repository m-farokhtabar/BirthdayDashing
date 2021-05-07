using BirthdayDashing.Domain.SeedWork;
using Common.Exception;
using System;
using System.Text;
using static Common.Exception.Messages;

namespace BirthdayDashing.Domain.VerificationCodes
{
    public class VerificationCode : Entity
    {
        /// <summary>
        /// just for Mapping
        /// </summary>
        private VerificationCode()
        {

        }
        public VerificationCode(Guid userId, int ExpiredTimeInMinute, VerificationType type = VerificationType.ConfirmUserByEmail)
        {
            UserId = userId;
            Token = GenerateRandomToken();
            if (ExpiredTimeInMinute <= 0)
                throw new ManualException(DATA_IS_NOT_VALID.Replace("{0}", "Expire time"), ExceptionType.InValid, nameof(ExpiredTimeInMinute));
            ExpireDate = DateTime.Now.AddMinutes(ExpiredTimeInMinute);
            Type = type;
        }
        public Guid UserId { get; private set; }
        public string Token { get; private set; }
        public DateTime ExpireDate { get; private set; }
        public VerificationType Type { get; private set; }

        private string GenerateRandomToken()
        {
            Random rnd = new(DateTime.Now.Ticks.GetHashCode());
            StringBuilder Token = new();
            for (int i = 0; i < 7; i++)
                Token.Append((char)(rnd.Next(1, 9) + 48));
            return Token.ToString();
        }

    }
    public enum VerificationType
    {
        ConfirmUserByEmail,
        ForgotPasswordByEmail,
        ConfirmUserBySMS
    }
}
