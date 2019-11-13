using System.ComponentModel.DataAnnotations;

namespace BlazorClient.Models.UserModels
{
    public class LoginModel
    {
        [EmailAddress(ErrorMessage = "Nesprávny formát emailu.")]
        [Required(ErrorMessage = "Prihlasovacie meno je povinné.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Heslo je povinné.")]
        public string Password { get; set; }
    }
}
