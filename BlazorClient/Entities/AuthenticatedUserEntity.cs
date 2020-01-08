using System;

namespace BlazorClient.Entities
{
    public class AuthenticatedUserEntity
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Token { get; set; }
        public DateTime ValidTo { get; set; }
        public string StudentId { get; set; }
        public Boolean IsAuthenticated { get; set; }
    }
}
