namespace WebAPI.Models
{
    public class CredentialsBody
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public bool IsValid() => !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password);
    }
}
