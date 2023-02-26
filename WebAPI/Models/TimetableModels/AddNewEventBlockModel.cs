using FRITeam.Swapify.SwapifyBase.Entities;
using System.ComponentModel.DataAnnotations;
using WebAPI.Models.TimetableModels;

namespace WebAPI.Models.UserModels
{
    public class AddNewEventBlockModel
    {
        [Required(ErrorMessage = "Používateľ je povinný.")]
        public User User { get; set; }
        [Required(ErrorMessage = "Event je povinný.")]
        public TimetableEvent TimetableEvent { get; set; }
    }
}
