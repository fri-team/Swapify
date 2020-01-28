using FRITeam.Swapify.Backend;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Backend.Settings;
using FRITeam.Swapify.Entities;
using Identity = Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading.Tasks;
using WebAPI.Controllers;
using WebAPI.Models.UserModels;
using Xunit;

namespace BackendTest.UserControllerTests
{
    public class UserControllerTests
    {
        private readonly Mock<ILogger<UserController>> _loggerMock;
        private readonly Mock<IOptions<EnvironmentSettings>> _envSettingsMock;
        private const string TesterEmail = "tester@testovaci.com";

        public UserControllerTests()
        {
            _loggerMock = new Mock<ILogger<UserController>>();
            EnvironmentSettings envSettings = new EnvironmentSettings() { BaseUrl = "http://localhost:3000/", JwtSecret = "DoNotUseMeInProduction", Environment = "Development" };
            _envSettingsMock = new Mock<IOptions<EnvironmentSettings>>();
            _envSettingsMock.Setup(x => x.Value).Returns(envSettings);
        }

        #region Mocks
        private UserService MockUserService(bool createUserSuccess, bool findUserByIdSuccess = false,
            bool findUserByEmailSuccess = false, bool confirmEmailSuccess = false, bool emailAlreadyConfirmed = false,
            bool loginSuccess = false, bool resetPasswordSuccess = false)
        {
            return new UserService(_envSettingsMock.Object,
                MockUserManager(createUserSuccess, findUserByIdSuccess, findUserByEmailSuccess,
                    confirmEmailSuccess, emailAlreadyConfirmed, resetPasswordSuccess).Object,
                MockSignInManager(loginSuccess).Object);
        }

        private static Mock<TestUserManager> MockUserManager(bool createUserSuccess, bool findUserByIdSuccess,
             bool findUserByEmailSuccess, bool confirmEmailSuccess, bool emailAlreadyConfirmed, bool resetPasswordSuccess)
        {
            var testUserManager = new Mock<TestUserManager>();
            testUserManager.Setup(x => x.DeleteAsync(It.IsAny<User>()))
                           .ReturnsAsync(Identity.IdentityResult.Success);
            testUserManager.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<User>()))
                           .ReturnsAsync("token");
            testUserManager.Setup(x => x.GeneratePasswordResetTokenAsync(It.IsAny<User>()))
                           .ReturnsAsync("token");

            if (createUserSuccess)
                testUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                               .ReturnsAsync(Identity.IdentityResult.Success);
            else
                testUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                               .ReturnsAsync(new Identity.IdentityResult());

            if (findUserByEmailSuccess)
            {
                User user = new User();
                if (emailAlreadyConfirmed)
                    user.EmailConfirmed = true;
                testUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                               .ReturnsAsync(user);
            }
            else
                testUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                               .ReturnsAsync((User)null);

            if (findUserByIdSuccess)
            {
                User user = new User();
                if (emailAlreadyConfirmed)
                    user.EmailConfirmed = true;
                testUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                               .ReturnsAsync(user);
            }
            else
                testUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                               .ReturnsAsync((User)null);

            if (confirmEmailSuccess)
                testUserManager.Setup(x => x.ConfirmEmailAsync(It.IsAny<User>(), It.IsAny<string>()))
                               .ReturnsAsync(Identity.IdentityResult.Success);
            else
                testUserManager.Setup(x => x.ConfirmEmailAsync(It.IsAny<User>(), It.IsAny<string>()))
                               .ReturnsAsync(new Identity.IdentityResult());

            if (resetPasswordSuccess)
                testUserManager.Setup(x => x.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                               .ReturnsAsync(Identity.IdentityResult.Success);
            else
                testUserManager.Setup(x => x.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                               .ReturnsAsync(new Identity.IdentityResult());

            return testUserManager;
        }

        private static Mock<TestSignInManager> MockSignInManager(bool loginSuccess)
        {
            var testSignInManager = new Mock<TestSignInManager>();
            if (loginSuccess)
                testSignInManager.Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                                 .ReturnsAsync(Identity.SignInResult.Success);
            else
                testSignInManager.Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                             .ReturnsAsync(new Identity.SignInResult());
            return testSignInManager;
        }

        private static IEmailService MockEmailService(bool emailSentSuccess)
        {
            var _emailServiceMock = new Mock<IEmailService>();
            _emailServiceMock.Setup(x => x.SendConfirmationEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                             .Returns(emailSentSuccess);
            return _emailServiceMock.Object;
        }
        #endregion Mock

        #region TestData
        private static RegisterModel CreateRegisterModel()
        {
            return new RegisterModel
            {
                Name = "Tester",
                Surname = "Testovaci",
                Email = TesterEmail,
                Password = "Heslo.123",
                PasswordAgain = "Heslo.123"
            };
        }

        private static ConfirmEmailModel CreateConfirmEmailModel()
        {
            return new ConfirmEmailModel
            {
                Token = "token",
                UserId = Guid.NewGuid().ToString()
            };
        }

        private static LoginModel CreateLoginModel()
        {
            return new LoginModel
            {
                Email = "tester@testovaci.com",
                Password = "Heslo.123"
            };
        }

        private static ResetPasswordModel CreateResetPasswordModel()
        {
            return new ResetPasswordModel
            {
                Email = "tester@testovaci.com"
            };
        }

        private static SetNewPasswordModel CreateSetNewPasswordModel()
        {
            return new SetNewPasswordModel
            {
                UserId = Guid.NewGuid().ToString(),
                Token = "token",
                Password = "Heslo.123",
                PasswordAgain = "Heslo.123"
            };
        }

        private static SendEmailConfirmTokenAgainModel CreateSendEmailConfirmTokenAgain()
        {
            return new SendEmailConfirmTokenAgainModel
            {
                Email = "tester@testovaci.com"
            };
        }

        private static MailUsModel CreateMailUs()
        {
            return new MailUsModel
            {
                Email = "tester@testovaci.com",
                Content = "Content of email for tests."
            };
        }
        #endregion

        [Fact]
        public async Task Register_AddUserIdentityError_BadRequestObject()
        {
            bool createUserSuccess = false;
            UserController controller = new UserController(_loggerMock.Object,
                MockUserService(createUserSuccess),
                MockEmailService(true),
                _envSettingsMock.Object);
            RegisterModel registerModel = CreateRegisterModel();

            var result = await controller.Register(registerModel);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Register_EmailServiceError_BadRequest()
        {
            bool createUserSuccess = true;
            bool findUserByIdSuccess = true;
            bool findUserByEmailSuccess = true;
            UserController controller = new UserController(_loggerMock.Object,
                MockUserService(createUserSuccess, findUserByIdSuccess, findUserByEmailSuccess),
                MockEmailService(false),
                _envSettingsMock.Object);
            RegisterModel registerModel = CreateRegisterModel();

            var result = await controller.Register(registerModel);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Register_RegisterUser_Ok()
        {
            bool createUserSuccess = true;
            bool findUserByIdSuccess = true;
            bool findUserByEmailSuccess = true;
            UserController controller = new UserController(_loggerMock.Object,
                 MockUserService(createUserSuccess, findUserByIdSuccess, findUserByEmailSuccess),
                 MockEmailService(true),
                _envSettingsMock.Object);
            RegisterModel registerModel = CreateRegisterModel();

            var result = await controller.Register(registerModel);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task ConfirmEmail_UserNotExists_BadRequest()
        {
            bool createUserSuccess = true;
            bool findUserByIdSuccess = false;
            bool findUserByEmailSuccess = true;
            UserController controller = new UserController(_loggerMock.Object,
                MockUserService(createUserSuccess, findUserByIdSuccess, findUserByEmailSuccess),
                MockEmailService(true),
                _envSettingsMock.Object);

            ConfirmEmailModel confirmEmailModel = CreateConfirmEmailModel();

            var result = await controller.ConfirmEmail(confirmEmailModel);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task ConfirmEmail_ConfirmEmailIdentityError_BadRequest()
        {
            bool createUserSuccess = true;
            bool findUserByIdSuccess = true;
            bool findUserByEmailSuccess = true;
            UserController controller = new UserController(_loggerMock.Object,
                MockUserService(createUserSuccess, findUserByIdSuccess, findUserByEmailSuccess),
                MockEmailService(true),
                _envSettingsMock.Object);
            ConfirmEmailModel confirmEmailModel = CreateConfirmEmailModel();

            var result = await controller.ConfirmEmail(confirmEmailModel);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task ConfirmEmail_EmailAlreadyConfirmed_Ok()
        {
            bool createUserSuccess = true;
            bool findUserByIdSuccess = true;
            bool findUserByEmailSuccess = true;
            bool confirmEmailSuccess = true;
            bool emailAlreadyConfirmed = true;
            UserController controller = new UserController(_loggerMock.Object,
                MockUserService(createUserSuccess, findUserByIdSuccess, findUserByEmailSuccess,
                    confirmEmailSuccess, emailAlreadyConfirmed),
                MockEmailService(true),
                _envSettingsMock.Object);
            ConfirmEmailModel confirmEmailModel = CreateConfirmEmailModel();

            var result = await controller.ConfirmEmail(confirmEmailModel);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task ConfirmEmail_ConfirmSuccess_Ok()
        {
            bool createUserSuccess = true;
            bool findUserByIdSuccess = true;
            bool findUserByEmailSuccess = true;
            bool confirmEmailSuccess = true;
            UserController controller = new UserController(_loggerMock.Object,
                MockUserService(createUserSuccess, findUserByIdSuccess, findUserByEmailSuccess, confirmEmailSuccess),
                MockEmailService(true),
                _envSettingsMock.Object);
            ConfirmEmailModel confirmEmailModel = CreateConfirmEmailModel();

            var result = await controller.ConfirmEmail(confirmEmailModel);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Login_UserNotExists_BadRequestObject()
        {
            bool createUserSuccess = true;
            bool findUserByIdSuccess = true;
            UserController controller = new UserController(_loggerMock.Object,
                MockUserService(createUserSuccess, findUserByIdSuccess),
                MockEmailService(true),
                _envSettingsMock.Object);
            LoginModel loginModel = CreateLoginModel();

            var result = await controller.Login(loginModel);

            Assert.IsType<BadRequestObjectResult>(result);

            dynamic badRequestObject = (BadRequestObjectResult)result;
            string error = badRequestObject.Value.Error;
            Assert.Equal($"Používateľ {TesterEmail} neexistuje.", error);
        }

        [Fact]
        public async Task Login_EmailNotConfirmed_ObjectResult()
        {
            bool createUserSuccess = true;
            bool findUserByIdSuccess = true;
            bool findUserByEmailSuccess = true;
            UserController controller = new UserController(_loggerMock.Object,
                MockUserService(createUserSuccess, findUserByIdSuccess, findUserByEmailSuccess),
                MockEmailService(true),
                _envSettingsMock.Object);
            LoginModel loginModel = CreateLoginModel();

            var result = await controller.Login(loginModel);

            Assert.IsType<ObjectResult>(result);

            ObjectResult badRequestObject = (ObjectResult)result;
            string error = (string)badRequestObject.Value;
            int statusCode = (int)badRequestObject.StatusCode;
            Assert.Equal("Pre prihlásenie prosím potvrď svoju emailovú adresu.", error);
            Assert.Equal(403, statusCode);
        }

        [Fact]
        public async Task Login_WrongPassword_BadRequestObject()
        {
            bool createUserSuccess = true;
            bool findUserByIdSuccess = true;
            bool findUserByEmailSuccess = true;
            bool confirmEmailSuccess = true;
            bool emailAlreadyConfirmed = true;
            UserController controller = new UserController(_loggerMock.Object,
                MockUserService(createUserSuccess, findUserByIdSuccess, findUserByEmailSuccess,
                    confirmEmailSuccess, emailAlreadyConfirmed),
                MockEmailService(true),
                _envSettingsMock.Object);
            LoginModel loginModel = CreateLoginModel();

            var result = await controller.Login(loginModel);
            Assert.IsType<BadRequestObjectResult>(result);

            dynamic badRequestObject = (BadRequestObjectResult)result;
            string error = badRequestObject.Value.Error;
            Assert.Equal("Zadané heslo nie je správne.", error);

        }

        [Fact]
        public async Task Login_LoginSuccess_Ok()
        {
            bool createUserSuccess = true;
            bool findUserByIdSuccess = true;
            bool findUserByEmailSuccess = true;
            bool confirmEmailSuccess = true;
            bool emailAlreadyConfirmed = true;
            bool loginSuccess = true;
            UserController controller = new UserController(_loggerMock.Object,
                MockUserService(createUserSuccess, findUserByIdSuccess, findUserByEmailSuccess,
                    confirmEmailSuccess, emailAlreadyConfirmed, loginSuccess),
                MockEmailService(true),
                _envSettingsMock.Object);
            LoginModel loginModel = CreateLoginModel();

            var result = await controller.Login(loginModel);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task ResetPassword_UserNotExists_BadRequestObject()
        {
            bool createUserSuccess = true;
            bool findUserByIdSuccess = true;
            bool findUserByEmailSuccess = false;
            UserController controller = new UserController(_loggerMock.Object,
                MockUserService(createUserSuccess, findUserByIdSuccess, findUserByEmailSuccess),
                MockEmailService(true),
                _envSettingsMock.Object);
            ResetPasswordModel resetPassModel = CreateResetPasswordModel();

            var result = await controller.ResetPassword(resetPassModel);

            Assert.IsType<BadRequestObjectResult>(result);

            dynamic badRequestObject = (BadRequestObjectResult)result;
            string error = badRequestObject.Value.Error;
            Assert.Equal($"Používateľ {TesterEmail} neexistuje.", error);
        }

        [Fact]
        public async Task ResetPassword_EmailNotConfirmed_BadRequestObject()
        {
            bool createUserSuccess = true;
            bool findUserByIdSuccess = true;
            bool findUserByEmailSuccess = true;
            bool confirmEmailSuccess = true;
            UserController controller = new UserController(_loggerMock.Object,
                MockUserService(createUserSuccess, findUserByIdSuccess, findUserByEmailSuccess,
                    confirmEmailSuccess),
                MockEmailService(true),
                _envSettingsMock.Object);
            ResetPasswordModel resetPassModel = CreateResetPasswordModel();

            var result = await controller.ResetPassword(resetPassModel);

            Assert.IsType<BadRequestObjectResult>(result);

            dynamic badRequestObject = (BadRequestObjectResult)result;
            string error = badRequestObject.Value.Error;
            Assert.Equal("Najskôr prosím potvrď svoju emailovú adresu.", error);
        }

        [Fact]
        public async Task ResetPassword_EmailServiceError_BadRequestResult()
        {
            bool createUserSuccess = true;
            bool findUserByIdSuccess = true;
            bool findUserByEmailSuccess = true;
            bool confirmEmailSuccess = true;
            bool emailAlreadyConfirmed = true;
            UserController controller = new UserController(_loggerMock.Object,
                MockUserService(createUserSuccess, findUserByIdSuccess, findUserByEmailSuccess,
                    confirmEmailSuccess, emailAlreadyConfirmed),
                MockEmailService(false),
                _envSettingsMock.Object);
            ResetPasswordModel resetPassModel = CreateResetPasswordModel();

            var result = await controller.ResetPassword(resetPassModel);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task ResetPassword_ResetSucces_Ok()
        {
            bool createUserSuccess = true;
            bool findUserByIdSuccess = true;
            bool findUserByEmailSuccess = true;
            bool confirmEmailSuccess = true;
            bool emailAlreadyConfirmed = true;
            UserController controller = new UserController(_loggerMock.Object,
                MockUserService(createUserSuccess, findUserByIdSuccess, findUserByEmailSuccess,
                    confirmEmailSuccess, emailAlreadyConfirmed),
                MockEmailService(true),
                _envSettingsMock.Object);
            ResetPasswordModel resetPassModel = CreateResetPasswordModel();

            var result = await controller.ResetPassword(resetPassModel);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task SetNewPassword_UserNotExists_BadRequest()
        {
            bool createUserSuccess = true;
            bool findUserByIdSuccess = false;
            UserController controller = new UserController(_loggerMock.Object,
                MockUserService(createUserSuccess, findUserByIdSuccess),
                MockEmailService(true),
                _envSettingsMock.Object);
            SetNewPasswordModel setNewPassModel = CreateSetNewPasswordModel();

            var result = await controller.SetNewPassword(setNewPassModel);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task SetNewPassword_ResetPassError_BadRequestObjectResult()
        {
            bool createUserSuccess = true;
            bool findUserByIdSuccess = true;
            UserController controller = new UserController(_loggerMock.Object,
                MockUserService(createUserSuccess, findUserByIdSuccess),
                MockEmailService(true),
                _envSettingsMock.Object);
            SetNewPasswordModel setNewPassModel = CreateSetNewPasswordModel();

            var result = await controller.SetNewPassword(setNewPassModel);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task SetNewPassword_ResetPassSuccess_Ok()
        {
            bool createUserSuccess = true;
            bool findUserByIdSuccess = true;
            bool findUserByEmailSuccess = true;
            bool confirmEmailSuccess = true;
            bool emailAlreadyConfirmed = true;
            bool loginSuccess = true;
            bool resetPasswordSuccess = true;
            UserController controller = new UserController(_loggerMock.Object,
                MockUserService(createUserSuccess, findUserByIdSuccess, findUserByEmailSuccess,
                    confirmEmailSuccess, emailAlreadyConfirmed, loginSuccess, resetPasswordSuccess),
                MockEmailService(true),
                _envSettingsMock.Object);
            SetNewPasswordModel setNewPassModel = CreateSetNewPasswordModel();

            var result = await controller.SetNewPassword(setNewPassModel);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task SendEmailConfirmTokenAgain_UserNotExists_BadRequestObjectResult()
        {
            bool createUserSuccess = true;
            bool findUserByIdSuccess = true;
            bool findUserByEmailSuccess = false;
            UserController controller = new UserController(_loggerMock.Object,
                MockUserService(createUserSuccess, findUserByIdSuccess, findUserByEmailSuccess),
                MockEmailService(true),
                _envSettingsMock.Object);
            SendEmailConfirmTokenAgainModel sendEmailConfirmTokenModel = CreateSendEmailConfirmTokenAgain();

            var result = await controller.SendEmailConfirmTokenAgain(sendEmailConfirmTokenModel);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task SendEmailConfirmTokenAgain_EmailAlreadyConfirmed_BadRequestObjectResult()
        {
            bool createUserSuccess = true;
            bool findUserByIdSuccess = true;
            bool findUserByEmailSuccess = true;
            bool confirmEmailSuccess = true;
            bool emailAlreadyConfirmed = true;
            UserController controller = new UserController(_loggerMock.Object,
                MockUserService(createUserSuccess, findUserByIdSuccess, findUserByEmailSuccess,
                confirmEmailSuccess, emailAlreadyConfirmed),
                MockEmailService(true),
                _envSettingsMock.Object);
            SendEmailConfirmTokenAgainModel sendEmailConfirmTokenModel = CreateSendEmailConfirmTokenAgain();

            var result = await controller.SendEmailConfirmTokenAgain(sendEmailConfirmTokenModel);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task SendEmailConfirmTokenAgain_EmailServiceError_BadRequestResult()
        {
            bool createUserSuccess = true;
            bool findUserByIdSuccess = true;
            bool findUserByEmailSuccess = true;
            UserController controller = new UserController(_loggerMock.Object,
                MockUserService(createUserSuccess, findUserByIdSuccess, findUserByEmailSuccess),
                MockEmailService(false),
                _envSettingsMock.Object);
            SendEmailConfirmTokenAgainModel sendEmailConfirmTokenModel = CreateSendEmailConfirmTokenAgain();

            var result = await controller.SendEmailConfirmTokenAgain(sendEmailConfirmTokenModel);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task SendEmailConfirmTokenAgain_EmailSuccess_Ok()
        {
            bool createUserSuccess = true;
            bool findUserByIdSuccess = true;
            bool findUserByEmailSuccess = true;
            UserController controller = new UserController(_loggerMock.Object,
                MockUserService(createUserSuccess, findUserByIdSuccess, findUserByEmailSuccess),
                MockEmailService(true),
                _envSettingsMock.Object);
            SendEmailConfirmTokenAgainModel sendEmailConfirmTokenModel = CreateSendEmailConfirmTokenAgain();

            var result = await controller.SendEmailConfirmTokenAgain(sendEmailConfirmTokenModel);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task MailUs_EmailSuccess_Ok()
        {
            bool createUserSuccess = true;
            bool findUserByIdSuccess = true;
            bool findUserByEmailSuccess = true;
            bool confirmEmailSuccess = true;
            bool emailAlreadyConfirmed = true;
            bool loginSuccess = true;
            UserController controller = new UserController(_loggerMock.Object,
                MockUserService(createUserSuccess, findUserByIdSuccess, findUserByEmailSuccess,
                    confirmEmailSuccess, emailAlreadyConfirmed, loginSuccess),
                MockEmailService(true),
                _envSettingsMock.Object);

            var mailUsModel = CreateMailUs();

            var result = await controller.MailUs(mailUsModel);

            Assert.IsType<OkResult>(result);
        }
    }
}
