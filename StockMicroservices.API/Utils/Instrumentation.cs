namespace StockMicroservices.API.Utils
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// It is recommended to use a custom type to hold references for ActivitySource.
    /// This avoids possible type collisions with other components in the DI container.
    /// </summary>
    public class Instrumentation : IDisposable
    {
        internal const string ActivitySourceName = "StockAPI";
        internal const string ActivitySourceVersion = "1.0.1";
        public static string ServiceName = string.Empty;

        public Instrumentation()
        {
            Console.WriteLine("Instrumentation created");
            Debug.WriteLine("Instrumentation created");
            this.ActivitySource = new ActivitySource(ServiceName, ActivitySourceVersion);
        }

        public ActivitySource ActivitySource { get; }

        public void Dispose()
        {
            this.ActivitySource.Dispose();
        }
    }
}
