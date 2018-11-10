using FRITeam.Swapify.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace WebAPI.Models.UserModels
{
    public class AuthenticatedUserModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Token { get; set; }
        public DateTime ValidTo { get; set; }

        public AuthenticatedUserModel(User user, JwtSecurityToken token)
        {
            UserName = user.UserName;
            Email = user.Email;
            Name = user.Name;
            Surname = user.Surname;
            Token = token.RawData;
            ValidTo = token.ValidTo;
        }
    }
}