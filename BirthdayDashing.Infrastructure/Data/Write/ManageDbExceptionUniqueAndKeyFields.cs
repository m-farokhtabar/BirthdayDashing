namespace BirthdayDashing.Infrastructure.Data.Write
{
    public class ManageDbExceptionUniqueAndKeyFields : IManageDbExceptionUniqueAndKeyFields
    {
        private const string Email = "Email";
        
        private const string UserRole_Role = "Role";
        private const string UserRole_User = "User";
        
        private const string Dashing_User = "User";
        
        private const string Comment_Dashing = "Dashing";
        private const string Comment_User = "User";
        private const string Comment_Comment = "Parent comment";


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
