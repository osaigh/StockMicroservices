using Microsoft.EntityFrameworkCore;
using StockMicroservices.API.Models.Daos;

namespace StockMicroservices.API.Data
{
    public class StockDbContext : DbContext
    {
        #region Properties
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StockHistory> StockHistories { get; set; }
        public DbSet<StockOrder> StockOrders { get; set; }
        public DbSet<StockPosition> StockPositions { get; set; }
        #endregion

        #region Constructor
        public StockDbContext(DbContextOptions options) : base(options)
        {

        }
        #endregion

        #region Methods
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stock>()
                        .ToTable("Stock");

            modelBuilder.Entity<StockHistory>()
                        .ToTable("StockHistory");

            modelBuilder.Entity<StockHistory>()
                        .HasOne(s => s.Stock)
                        .WithMany(s => s.StockHistories)
                        .HasForeignKey(s => s.StockId);

            modelBuilder.Entity<StockOrder>()
                        .ToTable("StockOrder");

            modelBuilder.Entity<StockOrder>()
                        .HasOne(s => s.Stock)
                        .WithMany()
                        .HasForeignKey(s => s.StockId);

            modelBuilder.Entity<StockPosition>()
                        .ToTable("StockPosition");

            modelBuilder.Entity<StockPosition>()
                        .HasOne(s => s.Stock)
                        .WithMany()
                        .HasForeignKey(s => s.StockId);

        }

        #endregion
    }
}
