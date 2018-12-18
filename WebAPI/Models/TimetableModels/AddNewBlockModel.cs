using FRITeam.Swapify.Entities;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.UserModels
{
    public class AddNewBlockModel
    {
        [Required(ErrorMessage = "Používateľ je povinný.")]
        public User User { get; set; }
        [Required(ErrorMessage = "Blok je povinný.")]
        public Block Block { get; set; }
    }
}
