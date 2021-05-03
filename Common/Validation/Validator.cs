using System;
using System.Text.RegularExpressions;

namespace Common.Validation
{
    public static class Validator
    {
        public static bool EmailIsValid(string email)
        {
            const string Pattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";
            return Regex.IsMatch(email, Pattern);
        }
        public static bool BirthdayIsValid(DateTime birthday)
        {
            return birthday.Year > 1850;
        }
        public static bool StringIsNotNullOrWhiteSpace(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }
    }
}
