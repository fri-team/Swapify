using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Backend.Config;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace FRITeam.Swapify.Backend
{
    public class UserService : IUserService
    {
        private readonly IMongoDatabase _database;
        private readonly EnvironmentConfig _env;

        public UserService(IMongoDatabase database, IOptions<EnvironmentConfig> env)
        {
            _database = database;
            _env = env.Value;
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
            var secret = Encoding.ASCII.GetBytes(_env.JwtSecret);
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
