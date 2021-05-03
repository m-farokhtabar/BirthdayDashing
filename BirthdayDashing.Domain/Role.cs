using BirthdayDashing.Domain.Base;

namespace BirthdayDashing.Domain
{
    public class Role : Entity
    {
        public const string Admin = "Administrator";
        public const string User = "User";

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
