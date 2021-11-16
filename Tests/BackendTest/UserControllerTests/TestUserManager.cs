using System;
using FRITeam.Swapify.SwapifyBase.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace BackendTest.UserControllerTests
{
    public class TestUserManager : UserManager<User>
    {
        public TestUserManager()
            : base(new Mock<IUserStore<User>>().Object,
                  new Mock<IOptions<IdentityOptions>>().Object,
                  new Mock<IPasswordHasher<User>>().Object,
                  new[] { new Mock<IUserValidator<User>>().Object },
                  new[] { new Mock<IPasswordValidator<User>>().Object },
                  new Mock<ILookupNormalizer>().Object,
                  new Mock<IdentityErrorDescriber>().Object,
                  new Mock<IServiceProvider>().Object,
                  new Mock<ILogger<UserManager<User>>>().Object)
        { }
    }
}
