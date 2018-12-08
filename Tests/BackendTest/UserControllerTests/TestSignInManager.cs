using FRITeam.Swapify.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace BackendTest.UserControllerTests
{
    public class TestSignInManager : SignInManager<User>
    {
        public TestSignInManager()
            : base(new Mock<TestUserManager>().Object,
                  new HttpContextAccessor(),
                  new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                  new Mock<IOptions<IdentityOptions>>().Object,
                  new Mock<ILogger<SignInManager<User>>>().Object,
                  new Mock<IAuthenticationSchemeProvider>().Object)
        { }
    }
}
