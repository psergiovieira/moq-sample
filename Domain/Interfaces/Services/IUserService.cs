using Domain.Entities;

namespace Domain.Interfaces.Services
{
    public interface IUserService
    {
        User GetById(int id);
        bool Create(User user);
        User UpdateEmail(User userData);
        void Delete(int id);
    }
}
