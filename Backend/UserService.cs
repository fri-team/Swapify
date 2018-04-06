using System;
using System.Threading.Tasks;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using MongoDB.Driver;

namespace FRITeam.Swapify.Backend
{
    public class UserService : IUserService
    {
        private readonly IMongoDatabase _database;

        public UserService(IMongoDatabase database)
        {
            _database = database;
        }

        #region Implementation of IUserService

        public async Task AddAsync(User entityToAdd)
        {
            await _database.GetCollection<User>(nameof(User)).InsertOneAsync(entityToAdd);
        }

        #endregion
    }
}
