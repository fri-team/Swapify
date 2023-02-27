using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.UserModels
{
    public class BlockedHoursVisibilityModel
    {
        [Required(ErrorMessage = "Email je zlý.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Je potrebné zadať ci sa zobrazuju blokovane hodiny.")]
        public string BlockedHours { get; set; }
    }
}
