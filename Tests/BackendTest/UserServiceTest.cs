using System.Threading.Tasks;
using Backend.Config;
using FluentAssertions;
using FRITeam.Swapify.Backend;
using FRITeam.Swapify.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Xunit;

namespace BackendTest
{
    [Collection("Database collection")]
    public class UserServiceTest : IClassFixture<Mongo2GoFixture>
    {
        private readonly Mongo2GoFixture _mongoFixture;
        private readonly IOptions<EnvironmentConfig> _env;

        public UserServiceTest(Mongo2GoFixture mongoFixture)
        {
            _mongoFixture = mongoFixture;
            _env = Options.Create(new EnvironmentConfig());
        }

        [Fact]
        public async Task AddUserTest()
        {
            IMongoDatabase database = _mongoFixture.MongoClient.GetDatabase("UsersDB");
            UserService userService = new UserService(database, _env);
            User userToAdd = new User();

            await userService.AddAsync(userToAdd);

            userToAdd.Id.Should().NotBeEmpty();
        }
    }
}
