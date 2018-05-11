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

        public User(int id, string name, string email)
        {
            Id = id;
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            Name = name;
            Email = email;
        }

        internal void CreatePassword(string password, string confirmPassword, IPasswordValidator passwordValidator)
        {
            ValidatePassword(password, confirmPassword, passwordValidator);
            Password = password;
        }

        public void UpdatePassword(string password, string confirmPassword, IPasswordValidator passwordValidator)
        {            
            ValidatePassword(password, confirmPassword, passwordValidator);
            Password = password;
        }

        private void ValidatePassword(string password, string confirmPassword, IPasswordValidator passwordValidator)
        {
            if (!passwordValidator.PasswordIsValid(password, confirmPassword))
                throw new ArgumentException();
        }
    }
}
