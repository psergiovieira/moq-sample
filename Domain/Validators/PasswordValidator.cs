using Domain.Interfaces.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Validators
{
    public class PasswordValidator : IPasswordValidator
    {
        public bool PasswordIsValid(string password, string confirmPassword)
        {
            if (password.Equals(confirmPassword) && StrengthIsValid(password))
                return true;

            return false;
        }

        private bool StrengthIsValid(string password)
        {
            if (password == null || password.Length < 8)
                return false;

            return true;
        }
    }
}
