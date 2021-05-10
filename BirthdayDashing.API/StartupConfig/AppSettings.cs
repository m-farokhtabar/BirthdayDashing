namespace BirthdayDashing.API.StartupConfig
{
    public class AppSettings : IAppSettings
    {
        public string SigningKey { get; set; }
        public int UserAuthorizationTokenExpireTimeInDay { get; set; }

        public int MinAge { get; set; }

        public int MaxAge { get; set; }
    }
}
