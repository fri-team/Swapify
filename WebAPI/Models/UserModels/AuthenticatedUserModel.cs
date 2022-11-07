using FRITeam.Swapify.SwapifyBase.Entities;
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
        public string StudentId { get; set; }
        public string PersonalNumber { get; set; }
        public string FirstTimePN { get; set; }

        public AuthenticatedUserModel(User user, JwtSecurityToken token)
        {
            if (user != null)
            {
                UserName = user.UserName;
                Email = user.Email;
                Name = user.Name;
                Surname = user.Surname;
                StudentId = user.BaseUser?.Id.ToString();
                PersonalNumber = user.BaseUser?.PersonalNumber;
            }
            FirstTimePN = "";
            Token = token.RawData;
            ValidTo = token.ValidTo;
        }

        public AuthenticatedUserModel(JwtSecurityToken token) : this(null, token)
        {
        }

        public AuthenticatedUserModel() { }
    }
}
