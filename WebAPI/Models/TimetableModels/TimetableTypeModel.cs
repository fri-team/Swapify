using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.TimetableModels
{
    public class TimetableTypeModel
    {
        [Required(ErrorMessage = "Email je nesprávny.")]
        public string Email { get; set; }
    }
}
