using FRITeam.Swapify.SwapifyBase.Entities;
using System.ComponentModel.DataAnnotations;
using WebAPI.Models.TimetableModels;

namespace WebAPI.Models.UserModels
{
    public class AddNewBlockModel
    {
        [Required(ErrorMessage = "Používateľ je povinný.")]
        public User User { get; set; }
        [Required(ErrorMessage = "Blok je povinný.")]
        public TimetableBlock TimetableBlock { get; set; }
    }
}
