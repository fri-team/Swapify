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
