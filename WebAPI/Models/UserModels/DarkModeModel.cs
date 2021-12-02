using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.UserModels
{
    public class DarkModeModel
    {
        [Required(ErrorMessage = "Email je zlý.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Je potrebné zadať dark mode.")]
        public string DarkMode { get; set; }
    }
}
