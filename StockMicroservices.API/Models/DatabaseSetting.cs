namespace StockMicroservices.API.Models
{
    public class DatabaseSetting
    {
        /// <summary>Full MongoDB connection string, for example mongodb://root:password@mongo:27017/?authSource=admin.</summary>
        public string ConnectionString { get; set; } = string.Empty;

        /// <summary>Database name.</summary>
        public string Name { get; set; } = "stocks";

        /// <summary>Seed demo stock data when the collection is empty.</summary>
        public bool SeedOnStartup { get; set; } = true;
    }
}
