namespace BlazorClient.Models.UserModels
{
    public class LoginResult
    {
        public bool Successful { get; set; }
        public string Error { get; set; }
        public AuthenticatedUserModel AuthenticatedUser { get; set; }
    }
}
