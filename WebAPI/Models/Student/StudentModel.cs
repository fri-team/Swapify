
using FRITeam.Swapify.Entities;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.UserModels
{
    public class StudentModel
    {
        [Required(ErrorMessage = "Číslo študijnej skupiny je povinné")]
        public string GroupNumber { get; set; }

        [Required(ErrorMessage = "Chýbajú údaje o používateľovi")]
        public User user { get; set; }
    }
}
