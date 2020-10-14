using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.DatabaseModels
{
    public class SwapifyDatabaseSettings : ISwapifyDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }

    }

    public interface ISwapifyDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
