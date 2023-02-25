using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Exchanges
{
    public class ExchangeRequestModel
    {
        [Required(ErrorMessage = "Timetable id je povinné")]
        public string timetableId { get; set; }
        [Required(ErrorMessage = "Blok na výmenu je povinný")]
        public BlockForExchangeModel BlockFrom { get; set; }
        [Required(ErrorMessage = "Blok na výmenu je povinný")]
        public BlockForExchangeModel BlockTo { get; set; }

        public ExchangeRequestModel() { }
    }
}
