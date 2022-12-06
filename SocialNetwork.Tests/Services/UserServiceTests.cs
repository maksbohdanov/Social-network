using BuisnessLogicLayer.Exceptions;
using BuisnessLogicLayer.Interfaces;
using BuisnessLogicLayer.Models;
using BuisnessLogicLayer.Models.DTOs;
using BuisnessLogicLayer.Services;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework.Internal;
using SocialNetwork.Tests.Helpers;

namespace SocialNetwork.Tests.Services
{
    [TestFixture]
    internal class UserServiceTests
    {
        private IUserService _userService;
        private Mock<UserManager<User>> _userManager;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IConfiguration> _configuration;
        private List<User> _users = new();

        [SetUp] 
        public void SetUp() 
        {
            _userManager = UnitTestHelper.MockUserManager(_users);
            _unitOfWork = new Mock<IUnitOfWork>();
            _configuration = new Mock<IConfiguration>();

            _userService = new UserService(
                _unitOfWork.Object,
                UnitTestHelper.CreateMapperProfile(),
                _userManager.Object,
                _configuration.Object);
        }

        [Test]
        public async Task UserService_RegisterAsync_RegistersUser()
        {
            var userModel = new UserRegistrationModel()
            {
                FirstName = "Test",
                LastName = "Test",
                City = "Test",
                BirthDate = DateTime.Now,
                Email = "test@test.com",
                Password = "Test123"
            };

            await _userService.RegisterAsync(userModel);

            _userManager.Verify(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
            Assert.That(_users.Count, Is.EqualTo(1));
        }   

        [Test]
        public async Task UserService_RegisterAsync_ThrowsRegisterUserException()
        {
            var userModel = new UserRegistrationModel()
            {
                FirstName = "Test",
                LastName = "Test",
                City = "Test",
                BirthDate = DateTime.Now,
                Email = "test@test.com",
                Password = ""
            };
            _userManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());
                        
            Func<Task> act = async () => await _userService.RegisterAsync(userModel);                 

            await act.Should().ThrowAsync<RegisterUserException>();
        }

        [Test]
        public async Task UserService_LoginAsync_ReturnsToken()
        {
            var loginModel = new LoginModel() { Email = "test@test.com", Password = "Test123" };
            var user = new User() { FirstName = "FirstName1", Email = "FirstName1@test.com", LastName = "LastName1", City = "City1", BirthDate = DateTime.Today, Id = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e") };

            _userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _userManager.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string>());
            _configuration.SetupGet(x => x[It.Is<string>(s => s == "JWT:Secret")]).Returns("jbIOJN529sgd1Aadg");
            _configuration.SetupGet(x => x[It.Is<string>(s => s == "JWT:ValidIssuer")]).Returns("ValidIssuer");
            _configuration.SetupGet(x => x[It.Is<string>(s => s == "JWT:ValidAudience")]).Returns("ValidAudience");

            var token = await _userService.LoginAsync(loginModel);

            Assert.IsNotNull(token);
            Assert.IsNotEmpty(token);
        }

        [Test]
        public async Task UserService_LoginAsync_ThrowsNotFoundException()
        {
            var loginModel = new LoginModel() { Email = "test@test.com", Password = "Test123" };
            _userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<User>());

            Func<Task> act = async () => await _userService.LoginAsync(loginModel);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public async Task UserService_LoginAsync_ThrowsLoginException()
        {
            var loginModel = new LoginModel() { Email = "test@test.com", Password = "Test123" };
            _userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(UnitTestHelper.Users[0]);

            Func<Task> act = async () => await _userService.LoginAsync(loginModel);

            await act.Should().ThrowAsync<LoginException>();
        }

        [Test]
        public async Task UserService_GetByIdAsync_ReturnsUserModel()
        {
            var expected = new UserDto()
            {
                Id = "0f8fad5b-d9cb-469f-a165-70867728950e",
                FirstName = "FirstName1",
                LastName = "LastName1",
                Email ="email1@test.com",
                City = "City1",
                BirthDate= DateTime.Today
            };
            _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UnitTestHelper.Users[0]);

            var actual = await _userService.GetByIdAsync(string.Empty);

            actual.Should().BeEquivalentTo(expected,o => o
                .Excluding(x => x.RegistrationDate)
                .Excluding(x => x.Chats));
        }

        [Test]
        public async Task UserService_GetByIdAsync_ThrowsNotFoundException()
        {            
            _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<User>());

            Func<Task> act = async () => await _userService.GetByIdAsync(It.IsAny<string>());

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public async Task UserService_GetAllAsync_ReturnsAllUsers()
        {
            _unitOfWork.Setup(x => x.Users.GetAllAsync())
                .ReturnsAsync(UnitTestHelper.Users);

            var users = await _userService.GetAllAsync();
            _unitOfWork.VerifyAll();
        }

        [Test]
        public async Task UserService_UpdateAsync_UpdatesUser()
        {
            var model = new UserDto()
            {
                Id = "0f8fad5b-d9cb-469f-a165-70867728951e",
                FirstName = "new",
                LastName = "LastName1",
                Email = "email1@test.com",
                City = "City1",
                BirthDate = DateTime.Today
            };
            _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UnitTestHelper.Users[0]);

            await _userService.UpdateAsync(model);

            _userManager.Verify(x => x.FindByIdAsync(It.IsAny<string>()), Times.Once);
            _userManager.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Once); 
        }

        [Test]
        public async Task UserService_UpdateAsync_ThrowsNotFoundException()
        {      
            var model = new UserDto()
            {
                Id = "0f8fad5b-d9cb-469f-a165-70867728951e",
                FirstName = "new",
                LastName = "LastName1",
                Email = "email1@test.com",
                City = "City1",
                BirthDate = DateTime.Today
            };
            _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<User>);

            Func<Task> act = async () => await _userService.UpdateAsync(model);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public async Task UserService_UpdateAsync_ThrowsUpdateUserException()
        {
            var model = new UserDto()
            {
                Id = "0f8fad5b-d9cb-469f-a165-70867728951e",
                FirstName = "new",
                LastName = "LastName1",
                Email = "email1@test.com",
                City = "City1",
                BirthDate = DateTime.Today
            };
            _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(UnitTestHelper.Users[0]);
            _userManager.Setup(x => x.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Failed());

            Func<Task> act = async () => await _userService.UpdateAsync(model);

            await act.Should().ThrowAsync<UpdateUserException>();
        }

        [TestCase("")]
        public async Task UserService_DeleteUserAsync_DeletesUser(string id)
        {
            _unitOfWork.Setup(x => x.Users.DeleteByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            await _userService.DeleteUserAsync(id);

            _unitOfWork.Verify(x => x.Users.DeleteByIdAsync(id), Times.Once);
            _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task UserService_DeleteUserAsync_ThrowsNotFoundException()
        {
            _unitOfWork.Setup(x => x.Users.DeleteByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            Func<Task> act = async () => await _userService.DeleteUserAsync(It.IsAny<string>());

            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
