using Common.Validation;

namespace BirthdayDashing.API.StartupConfig
{
    public interface IAppSettings
    {
        string SigningKey { get; }
        int UserAuthorizationTokenExpireTimeInDay { get; }
        int MinAge { get;  }
        int MaxAge { get; }

    }
}