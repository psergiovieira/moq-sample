using Domain.Entities;
using Domain.Interfaces.Validators;

namespace Domain.Interfaces.Services
{
    public interface IUserService
    {
        User GetById(int id);
        bool Create(User user,string password, string confirmPassword, IPasswordValidator passwordValidator);
        User UpdateEmail(User userData, IEmailValidator emailValidator);
        void Delete(int id);
    }
}
