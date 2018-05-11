using Domain.Entities;
using Domain.Interfaces.Services;
using Domain.Interfaces.Validators;
using Infrastructure.Generics;
using Infrastructure.Repository;
using Infrastructure.UnitOfWork;
using System;
using System.Linq;

namespace Domain.Services
{
    public class UserService : ServiceBase<User>, IUserService
    {
        public UserService(IRepository<User> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
        }

        public User GetById(int id)
        {
            return Repository.GetById(id);
        }

        public bool Create(User user, string password, string confirmPassword,
             IPasswordValidator passwordValidator, IEmailValidator emailValidator)
        {
            if (UserAlreadyExists(user.Id, user.Name))
                throw new InvalidOperationException();

            emailValidator.ValidateEmail(user.Email);
            user.CreatePassword(password, confirmPassword, passwordValidator);
            Repository.Create(user);
            return true;
        }

        private bool UserAlreadyExists(int id, string name)
        {
            return Repository.Query().Any(c => c.Id == id || c.Name.ToLower().Equals(name.ToLower()));
        }

        public User UpdateEmail(User userData, IEmailValidator emailValidator)
        {
            var user = GetById(userData.Id);
            VerifyIfUserIsNull(user);       

            emailValidator.ValidateEmail(userData.Email);

            user.Email = userData.Email;
            Repository.Update(user);

            return user;
        }

        private void VerifyIfUserIsNull(User user)
        {
            if (user == null)
                throw new ArgumentException();
        }

        public void Delete(int id)
        {
            var user = GetById(id);
            VerifyIfUserIsNull(user);

            Repository.Delete(id);
        }
    }
}
