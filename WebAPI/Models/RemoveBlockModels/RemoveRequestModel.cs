using System.ComponentModel.DataAnnotations;
using WebAPI.Models.RemoveBlockModel;

namespace WebAPI.Models.RemoveBlockModels
{
    public class RemoveRequestModel
    {
        [Required(ErrorMessage = "StudentID je povinné.")]
        public string StudentId { get; set; }

        public RemovedBlockModel RemoveBlock { get; set; }
    }
}
