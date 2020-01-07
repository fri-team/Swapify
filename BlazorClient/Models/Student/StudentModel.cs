using System.ComponentModel.DataAnnotations;

namespace BlazorClient.Models.Student
{
    public class StudentModel
    {
        [Required(ErrorMessage = "Osobné číslo študenta je povinné.")]
        public string PersonalNumber { get; set; }

        [Required(ErrorMessage = "Email je povinný.")]
        [EmailAddress(ErrorMessage = "Zadaná emailová adresa nie je validná.")]
        public string Email { get; set; }
    }
}
