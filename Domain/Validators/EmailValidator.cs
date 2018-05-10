using Domain.Interfaces.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Validators
{
    public class EmailValidator : IEmailValidator
    {
        public void ValidateEmail(string email)
        {
            var address = new System.Net.Mail.MailAddress(email);
        }
    }
}
