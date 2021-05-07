using Common.Feedback;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BirthdayDashing.API.Helper
{
    public static class CustomBadRequest
    {
        public static Feedback<bool> ConstructErrorMessages(ActionContext context)
        {
            Dictionary<string, string[]> Errors = new();

            foreach (var keyModelStatePair in context.ModelState)
            {
                var key = keyModelStatePair.Key;
                var errors = keyModelStatePair.Value.Errors;
                if (errors != null && errors.Count > 0)
                {
                    var errorMessages = new string[errors.Count];
                    for (var i = 0; i < errors.Count; i++)
                        errorMessages[i] = string.IsNullOrEmpty(errors[i].ErrorMessage) ? "The input is not valid." : errors[i].ErrorMessage;
                    Errors.Add(key, errorMessages);
                }
            }
            return new Feedback<bool>(false, MessageType.LogicalError, Errors);
        }
    }
}
