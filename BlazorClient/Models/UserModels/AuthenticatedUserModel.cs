using System;

namespace BlazorClient.Models.UserModels
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
    }
}
