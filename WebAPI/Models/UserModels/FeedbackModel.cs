using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.UserModels
{
    public class FeedbackModel
    {
        [Required(ErrorMessage = "Email je povinný.")]
        [EmailAddress(ErrorMessage = "Zadaná emailová adresa nie je validná.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Meno užívateľa je povinné.")]
        public string Name { get; set; }

        public string Subject { get; set; }


        [Required(ErrorMessage = "Obsah je povinný.")]
        public string Body { get; set; }
    }
}
