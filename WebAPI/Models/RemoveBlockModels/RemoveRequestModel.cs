using WebAPI.Models.RemoveBlockModel;

namespace WebAPI.Models.RemoveBlockModels
{
    public class RemoveRequestModel
    {
        public string StudentId { get; set; }
        public RemovedBlockModel RemoveBlock { get; set; }

        public RemoveRequestModel() { }
    }
}
