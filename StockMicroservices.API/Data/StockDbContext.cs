using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockMicroservices.API.Models.Daos;

namespace StockMicroservices.API.Data
{
    public class StockDbContext : DbContext
    {
        #region Properties
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StockPrice> StockPrices { get; set; }
        public DbSet<StockHistory> StockHistories { get; set; }
        public DbSet<StockHolder> StockHolders { get; set; }
        public DbSet<StockHolderPosition> StockHolderPositions { get; set; }
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
                        .ToTable("Stock")
                        .HasKey(s => s.Id);

            modelBuilder.Entity<StockPrice>()
                        .ToTable("StockPrice")
                        .HasKey(s => s.Id);

            modelBuilder.Entity<StockPrice>()
                        .HasOne(s => s.Stock)
                        .WithMany()
                        .HasForeignKey(s => s.StockId);

            modelBuilder.Entity<StockHistory>()
                        .ToTable("StockHistory")
                        .HasKey(s => s.Id);

            modelBuilder.Entity<StockHistory>()
                        .HasOne<Stock>()
                        .WithMany(s => s.StockHistories)
                        .HasForeignKey(s => s.StockId);

            modelBuilder.Entity<StockHolder>()
                        .ToTable("StockHolder")
                        .HasKey(s => s.Username);

            modelBuilder.Entity<StockHolderPosition>()
                        .ToTable("StockHolderPosition")
                        .HasKey(s => s.Id);

            modelBuilder.Entity<StockHolderPosition>()
                        .HasOne(s => s.StockHolder)
                        .WithMany(s => s.StockHolderPositions)
                        .HasForeignKey(s => s.StockHolderId);
        }

        #endregion
    }
}
