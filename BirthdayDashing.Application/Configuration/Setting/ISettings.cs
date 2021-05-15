namespace BirthdayDashing.Application.Configuration.Setting
{
    public interface ISettings
    {
        int MinAge { get;  }
        int MaxAge { get; }
        int MaxLockOutThreshold { get; }
    }
}