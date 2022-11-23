 namespace WebAPI.Models.DatabaseModels
{
    public class SwapifyDatabaseSettings : ISwapifyDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string HostName { get; set; }
        public string Port { get; set; }

    }

    public interface ISwapifyDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string HostName { get; set; }
        string Port { get; set; }
    }
}
