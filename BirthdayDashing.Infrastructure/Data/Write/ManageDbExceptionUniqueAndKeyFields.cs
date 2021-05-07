namespace BirthdayDashing.Infrastructure.Data.Write
{
    public class ManageDbExceptionUniqueAndKeyFields : IManageDbExceptionUniqueAndKeyFields
    {
        private const string Email = "Email";
        private const string UserRole_Role = "Role";
        public string FindUniqueOrKeyFieldsInMessage(string Message)
        {
            foreach (var Field in typeof(ManageDbExceptionUniqueAndKeyFields).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic))
            {
                if (Message.Contains(Field.Name, System.StringComparison.OrdinalIgnoreCase))
                    return Field.GetValue(typeof(ManageDbExceptionUniqueAndKeyFields)).ToString();
            }
            return null;
        }
    }
}
