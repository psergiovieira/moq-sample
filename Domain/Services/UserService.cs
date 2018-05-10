using Domain.Entities;
using Domain.Interfaces.Services;
using Infrastructure.Generics;
using Infrastructure.Repository;
using Infrastructure.UnitOfWork;

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

        public bool Create(User user)
        {
            //TODO validations
            Repository.Create(user);
            return true;
        }

        public User UpdateEmail(User userData)
        {
            //Validate user data
            var user = GetById(userData.Id);
            user.Email = userData.Email;

            return user;
        }

        public void Delete(int id)
        {
            //TODO Validate
            Repository.Delete(id);
        }
    }
}
