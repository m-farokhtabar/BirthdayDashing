using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Common.Validation
{
    public sealed class ImageValidatorAttribute : ValidationAttribute
    {
        private readonly long MaxFileSize;
        private readonly string OPTExtensionRegularExpression = @"(\.({0}))$";
        private readonly bool IsRequired;

        private bool ExtentionIsValid = true;
        private bool MaxLengthIsValid = true;
        private bool RequiredIsValid = true;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxFileSize">bytes</param>
        /// <param name="extentions">Exmaple jpg|jpeg|png|bmp|tif</param>
        /// <param name="isRequired"></param>
        public ImageValidatorAttribute(long maxFileSize, string extentions, bool isRequired = false)
        {
            MaxFileSize = maxFileSize;
            IsRequired = isRequired;
            OPTExtensionRegularExpression = OPTExtensionRegularExpression.Replace("{0}", extentions);
        }

        public override bool IsValid(object value)
        {
            bool IsValid;
            if (value is not null)
            {
                if (!(value is IFormFile file))
                    IsValid = false;
                else
                {
                    ExtentionIsValid = Regex.Match(file.FileName, OPTExtensionRegularExpression, RegexOptions.IgnoreCase).Success;
                    MaxLengthIsValid = file.Length <= MaxFileSize;
                    IsValid = ExtentionIsValid && MaxLengthIsValid;
                }
            }
            else
            {
                IsValid = !IsRequired;
                RequiredIsValid = !IsRequired;
            }
            return IsValid;
        }
        public override string FormatErrorMessage(string name)
        {
            if (!ExtentionIsValid)
                return $"The {name} is not valid image";
            else if (!MaxLengthIsValid)
                return $"The {name} Size is more than {MaxFileSize / 1024} Kb";
            else if (!RequiredIsValid)
                return $"{name} is required";
            else
                return "";
        }
    }
}
