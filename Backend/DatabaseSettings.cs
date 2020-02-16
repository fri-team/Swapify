using FRITeam.Swapify.Backend.Interfaces;

namespace FRITeam.Swapify.Backend
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
