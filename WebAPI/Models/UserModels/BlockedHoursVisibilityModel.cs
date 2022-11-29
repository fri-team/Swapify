using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.UserModels
{
    public class BlockedHoursVisibilityModel
    {
        [Required(ErrorMessage = "Email je zlý.")]
        public string Email { get; set; }
    }
}
