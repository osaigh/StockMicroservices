namespace StockMicroservices.StockMarketUpdater
{
    public sealed class StockUpdaterOptions
    {
        public int IntervalMilliseconds { get; set; } = 3000;

        public string[] StockNames { get; set; } = System.Array.Empty<string>();
    }
}
