using Domain.Interfaces.Validators;
using Infrastructure.BasicTypes;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class User : IIdentity
    {
        public int Id { get; }
        
        /// <summary>
        /// A user name is a name that uniquely identifies the user
        /// </summary>
        public string Name { get; }

        public string Email { get; set; }

        public string Password { get; private set; }

        private IList<string> _passwordsHistory;

        public User(int id, string name, string email)
        {
            //TODO validate all arguments
            Id = id;
            Name = name;
            Email = email;
            _passwordsHistory = new List<string>();
        }

        internal void CreatePassword(string password, string confirmPassword, IPasswordValidator passwordValidator)
        {
            if (!passwordValidator.PasswordIsValid(password, confirmPassword))
                throw new ArgumentException();

            Password = password;
        }

        public void UpdatePassword(string password, string confirmPassword)
        {
            //TODO Add Security Rules
            //TODO verify if the password match with comfirmation of passworld
            Password = password;
        }              
    }
}
