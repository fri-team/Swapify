using System.Threading.Tasks;
using FluentAssertions;
using FRITeam.Swapify.Backend;
using FRITeam.Swapify.Entities;
using MongoDB.Driver;
using Xunit;

namespace BackendTest
{
    [Collection("Database collection")]
    public class UserServiceTest : IClassFixture<Mongo2GoFixture>
    {
        private readonly Mongo2GoFixture _mongoFixture;

        public UserServiceTest(Mongo2GoFixture mongoFixture)
        {
            _mongoFixture = mongoFixture;
        }

        [Fact]
        public async Task AddUserTest()
        {
            IMongoDatabase database = _mongoFixture.MongoClient.GetDatabase("UsersDB");
            UserService userService = new UserService(database);
            User userToAdd = new User();

            await userService.AddAsync(userToAdd);

            userToAdd.Id.Should().NotBeEmpty();
        }
    }
}
