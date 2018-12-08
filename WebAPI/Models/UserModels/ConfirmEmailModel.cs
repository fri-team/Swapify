using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.UserModels
{
    public class ConfirmEmailModel    
    {
        [Required(ErrorMessage = "UserId je povinné.")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Token je povinný.")]
        public string Token { get; set; }
    }
}
