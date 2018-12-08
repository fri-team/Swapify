using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.UserModels
{
    public class SetNewPasswordModel
    {
        [Required(ErrorMessage = "UserId je povinné.")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Heslo je povinné.")]
        [StringLength(100, ErrorMessage = "Heslo musí obsahovať aspoň {2} znakov.", MinimumLength = 8)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Potvrdenie hesla je povinné.")]
        [Compare("Password", ErrorMessage = "Heslá sa nezhodujú.")]
        public string PasswordAgain { get; set; }

        [Required(ErrorMessage = "Token je povinný.")]        
        public string Token { get; set; }
    }
}
