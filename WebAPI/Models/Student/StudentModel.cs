using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.UserModels
{
    public class UserModel
    {
        [Required(ErrorMessage = "Osobné číslo používateľa je povinné.")]
        public string PersonalNumber { get; set; }

        [Required(ErrorMessage = "Email je povinný.")]
        [EmailAddress(ErrorMessage = "Zadaná emailová adresa nie je validná.")]
        public string Email { get; set; }
    }
}
