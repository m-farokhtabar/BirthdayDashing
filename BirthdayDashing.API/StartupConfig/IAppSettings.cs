using BirthdayDashing.Application.Configuration.Setting;

namespace BirthdayDashing.API.StartupConfig
{
    public interface IAppSettings : ISettings
    {
        string SigningKey { get; }
        int UserAuthorizationTokenExpireTimeInDay { get; }
    }
}