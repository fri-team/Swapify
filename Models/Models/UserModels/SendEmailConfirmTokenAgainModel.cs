using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.UserModels
{
    public class SendEmailConfirmTokenAgainModel
    {
        [Required(ErrorMessage = "Email je povinný.")]
        [EmailAddress(ErrorMessage = "Zadaná emailová adresa nie je validná.")]
        public string Email { get; set; }
    }
}
