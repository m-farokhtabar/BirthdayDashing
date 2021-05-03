namespace BirthdayDashing.API.StartupConfig
{
    public class AppSettings : IAppSettings
    {
        public string SigningKey { get; set; }
        public int UserExpiteTimeinDay { get; set; }
    }
}
