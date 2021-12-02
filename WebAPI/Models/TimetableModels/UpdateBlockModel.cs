using FRITeam.Swapify.SwapifyBase.Entities;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.TimetableModels
{
    public class UpdateBlockModel
    {
        [Required(ErrorMessage = "Používateľ je povinný.")]
        public User User { get; set; }

        [Required(ErrorMessage = "Blok je povinný.")]
        public TimetableBlock TimetableBlock { get; set; }
    }
}
