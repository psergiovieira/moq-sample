using Domain.Entities;
using Domain.Interfaces.Services;
using Domain.Interfaces.Validators;
using Infrastructure.Generics;
using Infrastructure.Repository;
using Infrastructure.UnitOfWork;
using System;

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

        public bool Create(User user, string password, string confirmPassword, IPasswordValidator passwordValidator)
        {
            //TODO verify if user exists on database, email is valid 
            user.CreatePassword(password, confirmPassword, passwordValidator);
            Repository.Create(user);
            return true;
        }

        public User UpdateEmail(User userData, IEmailValidator emailValidator)
        {               
            var user = GetById(userData.Id);
            if (user == null)
                throw new ArgumentException();

            emailValidator.ValidateEmail(userData.Email);

            user.Email = userData.Email;
            Repository.Update(user);

            return user;
        }

        public void Delete(int id)
        {
            //TODO Validate
            Repository.Delete(id);
        }
    }
}
