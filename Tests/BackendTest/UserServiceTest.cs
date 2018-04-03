using System;
using System.Threading.Tasks;
using FluentAssertions;
using FRITeam.Swapify.Backend;
using FRITeam.Swapify.BackendTest;
using FRITeam.Swapify.Entities;
using MongoDB.Driver;
using Xunit;

namespace BackendTest
{
    public class UserServiceTest
    {
        [Fact]
        public async Task AddUserTest()
        {
            DBSettings.InitDBSettings(MongoRunnerType.Test);
            UserService userService = new UserService(DBSettings.Database);
            User userToAdd = new User();

            await userService.AddAsync(userToAdd);

            userToAdd.Id.Should().NotBeEmpty(); // id was set?
        }
    }
}
