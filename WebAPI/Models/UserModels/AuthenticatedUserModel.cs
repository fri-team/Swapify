using System;
using System.IdentityModel.Tokens.Jwt;

namespace WebAPI.Models.UserModels
{
    public class AuthenticatedUserModel
    {
        public string Token { get; set; }
        public DateTime ValidTo { get; set; }

        public AuthenticatedUserModel(JwtSecurityToken token)
        {
            Token = token.RawData;
            ValidTo = token.ValidTo;
        }
    }
}
