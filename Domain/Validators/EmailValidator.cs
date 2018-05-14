using Domain.Interfaces.Validators;
using Infrastructure.Regex;
using System;
using System.Text.RegularExpressions;

namespace Domain.Validators
{
    public class EmailValidator : IEmailValidator
    {
        static Regex ValidEmailRegex = RegexHelper.CreateValidEmailRegex();
        public void ValidateEmail(string email)
        {
            var parameterName = "email";
            if (email == null)
                throw new ArgumentNullException(parameterName);

            if (string.Empty == email)
                throw new ArgumentException(parameterName);

            if (!ValidEmailRegex.IsMatch(email))
                throw new FormatException(parameterName);
        }       
    }
}
