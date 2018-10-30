using AspNetCore.Identity.MongoDbCore.Models;

namespace FRITeam.Swapify.Entities
{
    public class User : MongoIdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public User()
        {
        }

        public User(string email, string name, string surname)
        {
            Email = UserName = email;
            Name = name;
            Surname = surname;
        }
    }
}
