using System;
using System.Text.RegularExpressions;

namespace Common.Validation
{
    public static class Validator
    {
        public const string EmailPattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";
        public const string PostalCodePattern = @"(^\d{5}(-\d{4})?$)|(^[ABCEGHJKLMNPRSTVXYabceghjklmnprstvxy]{1}\d{1}[ABCEGHJKLMNPRSTVWXYZabceghjklmnprstv‌​xy]{1} *\d{1}[ABCEGHJKLMNPRSTVWXYZabceghjklmnprstvxy]{1}\d{1}$)";        
        public static int MinAge { get; set; }
        public static int MaxAge { get; set; }

        public static bool BirthdayIsValid(DateTime birthday)
        {
            return birthday.Year >= (DateTime.Now.Year - MaxAge) && birthday.Year <= (DateTime.Now.Year - MinAge);
        }

        public static bool EmailIsValid(string email)
        {
            const string Pattern = EmailPattern;
            return Regex.IsMatch(email, Pattern);
        }
        public static bool StringIsNotNullOrWhiteSpace(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }
        public static bool PostalCodeIsValid(string postalCode)
        {
            const string Pattern = PostalCodePattern;
            return Regex.IsMatch(postalCode, Pattern);
        }
    }
}
