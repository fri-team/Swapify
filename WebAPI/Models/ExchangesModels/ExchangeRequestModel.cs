using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.Exchanges
{
    public class ExchangeRequestModel
    {
        [Required(ErrorMessage = "Študent id je povinné")]
        public string StudentId { get; set; }
        [Required(ErrorMessage = "Blok na výmenu je povinný")]
        public BlockForExchangeModel BlockFrom { get; set; }
        [Required(ErrorMessage = "Blok na výmenu je povinný")]
        public BlockForExchangeModel BlockTo { get; set; }

        public ExchangeRequestModel() { }
    }
}
