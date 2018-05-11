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
using System.Collections.Generic;
using System.Linq;

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
        private IEmailValidator _emailValidator;
        private IList<User> _users;

        public UnitTestUser()
        {
            _repository = new Mock<IRepository<User>>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _service = new UserService(_repository.Object, _unitOfWork.Object);
            _passwordValidator = new PasswordValidator();
            _emailValidator = new EmailValidator();
            _user = new User(ID_MAIN_USER, "paulosv", "paulosv@mail.com");
            _users = new List<User>() { _user };
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
            int usersCountBeforeInsert = _users.Count;
            _repository.Setup(x => x.Create(It.IsAny<User>())).Callback<User>((s) => _users.Add(s));
            var user = new User(1, "fulano", "fulano@mail.com");
            var userWasCreated = _service.Create(user, "12345678", "12345678", _passwordValidator, _emailValidator);

            Assert.AreEqual(true, userWasCreated);
            Assert.AreEqual(usersCountBeforeInsert + 1, _users.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CanCreateUserWithEmptyName()
        {
            int usersCountBeforeInsert = _users.Count;
            _repository.Setup(x => x.Create(It.IsAny<User>())).Callback<User>((s) => _users.Add(s));
            var user = new User(1, string.Empty, "fulano@mail.com");
            var userWasCreated = _service.Create(user, "12345678", "12345678", _passwordValidator, _emailValidator);

            Assert.AreEqual(true, userWasCreated);
            Assert.AreEqual(usersCountBeforeInsert + 1, _users.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CanCreateUserWithAlreadyExistingId()
        {
            _repository.Setup(x => x.Query(null, null)).Returns(_users.AsQueryable());

            var user = new User(1, "fulano", "fulanomail.com");
            var userWasCreated = _service.Create(user, "12345678", "12345678", _passwordValidator, _emailValidator);

            Assert.AreEqual(true, userWasCreated);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CanCreateUserWithAlreadyExistingName()
        {
            _repository.Setup(x => x.Query(null, null)).Returns(_users.AsQueryable());

            var user = new User(100, "paulosv", "fulanomail.com");
            var userWasCreated = _service.Create(user, "12345678", "12345678", _passwordValidator, _emailValidator);

            Assert.AreEqual(true, userWasCreated);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void CanCreateUserWithInvalidEmail()
        {
            var user = new User(1, "fulano", "fulanomail.com");
            var userWasCreated = _service.Create(user, "12345678", "12345678", _passwordValidator, _emailValidator);

            Assert.AreEqual(true, userWasCreated);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Password is accepting empty password")]
        public void CanCreateUserWithoutSetPassword()
        {
            var user = new User(1, "fulano", "fulano@mail.com");
            var userWasCreated = _service.Create(user, string.Empty, string.Empty, _passwordValidator, _emailValidator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Password length can be too small")]
        public void CanCreateUserWithPasswordLengthTooSmall()
        {
            var user = new User(1, "fulano", "fulano@mail.com");
            var userWasCreated = _service.Create(user, "123", "123", _passwordValidator, _emailValidator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Service is not validating if password and confirm password match")]
        public void CanCreateUserWithInvalidConfirmationPassword()
        {
            var user = new User(1, "fulano", "fulano@mail.com");
            var userWasCreated = _service.Create(user, "12345678", "12245678", _passwordValidator, _emailValidator);
        }

        [TestMethod]
        public void CanUpdateUserEmail()
        {
            _repository.Setup(x => x.GetById(ID_MAIN_USER)).Returns(_user);

            var newDataUser = new User(1, "paulosv", "paulo@mail.com");
            _service.UpdateEmail(newDataUser, _emailValidator);

            Assert.AreEqual(newDataUser.Email, _user.Email);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "System is accepting invalid user")]
        public void CanUpdateUserEmailWithInvalidId()
        {
            _repository.Setup(x => x.GetById(ID_MAIN_USER)).Returns(_user);

            var newDataUser = new User(100, "paulosv", "paulo@mail.com");
            _service.UpdateEmail(newDataUser, _emailValidator);

            Assert.AreEqual(newDataUser.Email, _user.Email);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "System is accepting empty email on update")]
        public void CanUpdateEmailWithEmailEmpty()
        {
            _repository.Setup(x => x.GetById(ID_MAIN_USER)).Returns(_user);

            var newDataUser = new User(1, "paulosv", string.Empty);
            _service.UpdateEmail(newDataUser, _emailValidator);

            Assert.AreEqual(newDataUser.Email, _user.Email);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "System is accepting null email on update")]
        public void CanUpdateEmailWithNullValue()
        {
            _repository.Setup(x => x.GetById(ID_MAIN_USER)).Returns(_user);

            var newDataUser = new User(1, "paulosv", null);
            _service.UpdateEmail(newDataUser, _emailValidator);

            Assert.AreEqual(newDataUser.Email, _user.Email);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void CanUpdateEmailWithInvalidAddress()
        {
            _repository.Setup(x => x.GetById(ID_MAIN_USER)).Returns(_user);

            var newDataUser = new User(1, "paulosv", "pauloATmail.com");
            _service.UpdateEmail(newDataUser, _emailValidator);

            Assert.AreEqual(newDataUser.Email, _user.Email);
        }

        [TestMethod]
        public void CanDeleteUser()
        {
            _repository.Setup(x => x.GetById(ID_MAIN_USER)).Returns(_user);
            _service.Delete(1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CanDeleteUserNonExistent()
        {
            _repository.Setup(x => x.GetById(ID_MAIN_USER)).Returns(_user);
            _service.Delete(10000000);
        }
    }
}
