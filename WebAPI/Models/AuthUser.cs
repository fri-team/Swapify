using System;
using System.IdentityModel.Tokens.Jwt;

namespace WebAPI.Models
{
    public class AuthUser
    {
        public string Token { get; set; }
        public DateTime ValidTo { get; set; }

        public AuthUser(JwtSecurityToken token)
        {
            Token = token.RawData;
            ValidTo = token.ValidTo;
        }
    }
}
