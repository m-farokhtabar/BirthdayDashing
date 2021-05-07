namespace BirthdayDashing.Infrastructure.Data.Write
{
    public interface IManageDbExceptionUniqueAndKeyFields
    {
        string FindUniqueOrKeyFieldsInMessage(string Message);
    }
}