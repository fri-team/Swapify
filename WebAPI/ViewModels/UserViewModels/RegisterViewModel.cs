using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Meno je povinné.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Priezvisko je povinné.")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Email je povinný.")]
        [EmailAddress(ErrorMessage = "Zadaná emailová adresa nie je validná.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Heslo je povinné.")]
        [StringLength(100, ErrorMessage = "Heslo musí obsahovať aspoň {2} znakov.", MinimumLength = 8)]        
        public string Password { get; set; }

        [Required(ErrorMessage = "Potvrdenie hesla je povinné.")]
        [Compare("Password", ErrorMessage = "Heslá sa nezhodujú.")]
        public string PasswordAgain { get; set; }
    }
}
