using System;
using System.Linq;
using System.Reflection;

namespace Common.Extensions
{
    public static class EnumExtension {
        public static string GetDisplayNameOfEnum(this Enum value)
        {
            Type enumType = value.GetType();
            var enumValue = System.Enum.GetName(enumType, value);
            MemberInfo member = enumType.GetMember(enumValue)[0];
            var DisplayName = member.CustomAttributes.FirstOrDefault(x => x.AttributeType.Name == "DisplayAttribute");

            if (DisplayName != null && DisplayName.NamedArguments.Any(x => x.MemberName == "Name"))
            {
                return DisplayName.NamedArguments.FirstOrDefault(x => x.MemberName == "Name").TypedValue.Value.ToString();
            }
            return enumValue;
        }
    }
}
