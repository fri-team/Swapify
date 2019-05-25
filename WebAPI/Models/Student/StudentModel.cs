
using FRITeam.Swapify.Entities;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.UserModels
{
    public class StudentModel
    {
        [Required(ErrorMessage = "Osobné číslo študenta je povinné.")]
        public string StudentNumber { get; set; }

        [Required(ErrorMessage = "Email je povinný.")]
        [EmailAddress(ErrorMessage = "Zadaná emailová adresa nie je validná.")]
        public string Email { get; set; }
    }
}
