using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.UserModels
{
    public class DeleteUserModel
    {
        [Required(ErrorMessage = "Email je povinný.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Heslo je povinné.")]
        public string Password { get; set; }
    }
}
