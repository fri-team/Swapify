using AspNetCore.Identity.MongoDbCore.Models;

namespace FRITeam.Swapify.SwapifyBase.Entities
{
    public class User : MongoIdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public UserData UserData { get; set; }
        public bool IsLdapUser { get; set; }
        public bool DarkMode { get; set; }
        public bool ShowBlockedHours { get; set; }

        public User()
        {

        }

        public User(string email, string name, string surname)
        {
            Email = UserName = email;
            Name = name;
            Surname = surname;
            UserData = null;
            IsLdapUser = false;
            DarkMode = false;
            ShowBlockedHours = false;
        }
    }
}
