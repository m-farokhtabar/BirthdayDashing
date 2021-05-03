namespace BirthdayDashing.API.StartupConfig
{
    public interface IAppSettings
    {
        string SigningKey { get; set; }
        int UserExpiteTimeinDay { get; set; }
    }
}