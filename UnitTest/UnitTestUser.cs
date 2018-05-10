using Domain.Entities;
using Domain.Interfaces.Services;
using Domain.Interfaces.Validators;
using Domain.Services;
using Domain.Validators;
using Infrastructure.Repository;
using Infrastructure.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace UnitTest
{
    [TestClass]
    public class UnitTestUser
    {
        private Mock<IRepository<User>> _repository;
        private Mock<IUnitOfWork> _unitOfWork;
        private IUserService _service;
        private User _user;
        private const int ID_MAIN_USER = 1;
        private IPasswordValidator _passwordValidator;

        [TestInitialize]
        public void Initialize()
        {
            _repository = new Mock<IRepository<User>>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _service = new UserService(_repository.Object, _unitOfWork.Object);
            _passwordValidator = new PasswordValidator();
            _user = new User(ID_MAIN_USER, "paulosv", "paulosv@mail.com");
        }

        [TestMethod]
        public void CanGetUser()
        {
            _repository.Setup(x => x.GetById(ID_MAIN_USER)).Returns(_user);
            var user = _service.GetById(1);

            Assert.AreEqual(_user, user);
        }

        [TestMethod]
        public void CanGetUserWithInvalidId()
        {
            _repository.Setup(x => x.GetById(ID_MAIN_USER)).Returns(_user);
            const int ID_INVALIDO = 0;
            var user = _service.GetById(ID_INVALIDO);

            Assert.IsNull(user);
        }

        [TestMethod]
        public void CanCreateUser()
        {
            var user = new User(1, "fulano", "fulano@mail.com");
            var userWasCreated = _service.Create(user, "12345678", "12345678", _passwordValidator);

            Assert.AreEqual(true, userWasCreated);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CanCreateUserWithoutSetPassword()
        {
            var user = new User(1, "fulano", "fulano@mail.com");
            var userWasCreated = _service.Create(user, string.Empty, string.Empty, _passwordValidator);
        }

        [TestMethod]
        public void CanUpdateUserEmail()
        {
            _repository.Setup(x => x.GetById(ID_MAIN_USER)).Returns(_user);

            var newDataUser = new User(1, "paulosv", "paulo@mail.com");
            _service.UpdateEmail(newDataUser);

            Assert.AreEqual(newDataUser.Email, _user.Email);
        }

        [TestMethod]
        public void CanDeleteUser()
        {
            _repository.Setup(x => x.GetById(ID_MAIN_USER)).Returns(_user);
            _service.Delete(1);
        }
    }
}
