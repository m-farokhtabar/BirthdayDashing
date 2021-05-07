using BirthdayDashing.Domain.SeedWork;

namespace BirthdayDashing.Domain.Roles
{
    public class Role : Entity
    {
        public const string Admin = "Administrator";
        public const string User = "User";
        public const string AdminOrUser = "Administrator, User";

        /// <summary>
        /// just for Mapping
        /// </summary>
        private Role()
        {

        }

        public Role(string name)
        {
            Name = name;
        }
        public string Name { get; private set; }
    }
}
