using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.Exchanges
{
    public class ExchangeRequest
    {
        public string StudentId { get; set; }
        public BlockForExchange BlockFrom { get; set; }
        public BlockForExchange BlockTo { get; set; }

        public ExchangeRequest() { }
    }
}
