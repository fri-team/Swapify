using System;
using System.Threading.Tasks;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using FluentAssertions;
using FRITeam.Swapify.Backend;
using FRITeam.Swapify.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using Xunit;

//ToDo
namespace BackendTest
{
    //[Collection("Database collection")]
    public class UserServiceTest : IClassFixture<Mongo2GoFixture>
    {
        //private readonly Mongo2GoFixture _mongoFixture;

        public UserServiceTest(Mongo2GoFixture mongoFixture)
        {
            //_mongoFixture = mongoFixture;
        }

        [Fact]
        public async Task AddUserTest()
        {
            //IMongoDatabase database = _mongoFixture.MongoClient.GetDatabase("UsersDB");
            var userManager = new UserManager<User>(new Mock<IUserStore<User>>().Object,
                                                new Mock<IOptions<IdentityOptions>>().Object,
                                                new Mock<IPasswordHasher<User>>().Object,
                                                new IUserValidator<User>[0],
                                                new IPasswordValidator<User>[0],
                                                new Mock<ILookupNormalizer>().Object,
                                                new Mock<IdentityErrorDescriber>().Object,
                                                new Mock<IServiceProvider>().Object,
                                                new Mock<ILogger<UserManager<User>>>().Object);

            User user = new User("test@test.com", "tester", "Testovaci");
            var result = await userManager.CreateAsync(user, "SecretPassword");
            Assert.NotNull(result);


            //var objectUnderTest = new SweetService(userManager);
            //UserManager<User> userManager = new UserManager<User>(database, null);


            //IdentityOptionsAction = options =>
            //{
            //    options.Password.RequireDigit = true;
            //    options.Password.RequiredLength = 8;
            //    options.Password.RequireNonAlphanumeric = false;
            //    options.Password.RequireUppercase = true;
            //    options.Password.RequireLowercase = true;

            //    options.SignIn.RequireConfirmedEmail = true;

            //    // Lockout settings
            //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            //    options.Lockout.MaxFailedAccessAttempts = 10;

            //    // ApplicationUser settings
            //    options.User.RequireUniqueEmail = true;

            //UserService userService = new UserService(database);
            //User userToAdd = new User();

            //await userService.AddAsync(userToAdd);

            //userToAdd.Id.Should().NotBeEmpty();
        }
    }
}
