using FRITeam.Swapify.Entities;
using System;

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

        public AuthenticatedUserModel(User user, string token, DateTime validTo)
        {
            if (user != null)
            {
                UserName = user.UserName;
                Email = user.Email;
                Name = user.Name;
                Surname = user.Surname;
                StudentId = user.Student?.Id.ToString();
                }
            Token = token;
            ValidTo = validTo;            
        }

        public AuthenticatedUserModel(string token, DateTime validTo) : this(null, token, validTo)
        {
        }
    }
}
