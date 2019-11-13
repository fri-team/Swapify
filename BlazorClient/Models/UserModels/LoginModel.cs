using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.UserModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Prihlasovacie meno je povinné.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Heslo je povinné.")]
        public string Password { get; set; }
    }
}
