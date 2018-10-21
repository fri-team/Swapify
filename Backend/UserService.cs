using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Backend.Settings;
using FRITeam.Swapify.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace FRITeam.Swapify.Backend
{
    public class UserService : IUserService
    {
        private readonly IMongoDatabase _database;
        private readonly EnvironmentSettings _environmentSettings;

        public UserService(IMongoDatabase database, IOptions<EnvironmentSettings> environmentSettings)
        {
            _database = database;
            _environmentSettings = environmentSettings.Value;
        }

        public JwtSecurityToken Authenticate(string login, string password)
        {
            // TODO: check if user exists
            var token = GenerateJwtToken(login);
            return token;
        }

        public async Task AddAsync(User entityToAdd)
        {
            await _database.GetCollection<User>(nameof(User)).InsertOneAsync(entityToAdd);
        }

        private JwtSecurityToken GenerateJwtToken(string login)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = Encoding.ASCII.GetBytes(_environmentSettings.JwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, login) }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = (JwtSecurityToken)tokenHandler.CreateToken(tokenDescriptor);
            return token;
        }
    }
}
