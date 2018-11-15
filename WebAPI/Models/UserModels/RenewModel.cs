using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.UserModels
{
    public class RenewModel
    {
        [Required]
        public string Token { get; set; }
    }
}
