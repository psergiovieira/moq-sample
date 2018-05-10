using Domain.Entities;
using Domain.Interfaces.Services;
using Domain.Services;
using Infrastructure.Repository;
using Infrastructure.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTest
{
    [TestClass]
    public class UnitTestUser
    {
        private Mock<IRepository<User>> _repository;
        private Mock<IUnitOfWork> _unitOfWork;
        private IUserService _service;
        private User _user;

        [TestInitialize]
        public void Initialize()
        {
            _repository = new Mock<IRepository<User>>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _service = new UserService(_repository.Object, _unitOfWork.Object);

            _user = new User(1, "paulosv", "paulosv@mail.com");
        }

        [TestMethod]
        public void CanGetUser()
        {
            _repository.Setup(x => x.GetById(1)).Returns(_user);
            var user = _service.GetById(1);

            Assert.AreEqual(user, _user);
        }

        [TestMethod]
        public void CanCreateUser()
        {
            var user = new User(1, "fulano", "fulano@mail.com");
            user.CreatePassword("12356", "123456");
            var userWasCreated = _service.Create(user);

            Assert.AreEqual(true, userWasCreated);
        }

        [TestMethod]
        public void CanUpdateUserEmail()
        {
            _repository.Setup(x => x.GetById(1)).Returns(_user);

            var newDataUser = new User(1, "paulosv", "paulo@mail.com");            
            _service.UpdateEmail(newDataUser);

            Assert.AreEqual(newDataUser.Email, _user.Email);
        }

        [TestMethod]
        public void CanDeleteUser()
        {
            _repository.Setup(x => x.GetById(1)).Returns(_user);
            _service.Delete(1);
        }
    }
}
