using System.ComponentModel.DataAnnotations;
using FRITeam.Swapify.Entities;

namespace WebAPI.Models.TimetableModels
{
    public class UpdateBlockModel
    {
        [Required(ErrorMessage = "Používateľ je povinný.")]
        public User User { get; set; }

        [Required(ErrorMessage = "Starý blok je povinný.")]
        public TimetableBlock OldTimetableBlock { get; set; }

        [Required(ErrorMessage = "Nový blok je povinný.")]
        public TimetableBlock NewTimetableBlock { get; set; }
    }
}
