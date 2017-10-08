using System;
using System.ComponentModel.DataAnnotations.Schema;
using BugStoreModels;
using Microsoft.EntityFrameworkCore;
using BugStoreModels.ViewModels;

namespace BugStoreDAL.EF
{
    public partial class StoreContext : DbContext
    {
        public StoreContext()
        {
        }

        public StoreContext(DbContextOptions options) : base(options)
        {
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    @"Data Source=TRUNGCT;Initial Catalog=BugStoreDb;Integrated Security=True;",
                options => options.EnableRetryOnFailure());
            }
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }

        [NotMapped]
        public DbSet<OrderProductInfo> ViewModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(e => e.CurrentPrice)
                .HasColumnType("decimal(19,4)");

            modelBuilder.Entity<Product>()
                .Property(e => e.TimeStamp);

            modelBuilder.Entity<Product>()
                .Property(e => e.UnitCost)
                .HasColumnType("decimal(19,4)");

            modelBuilder.Entity<Order>()
                .Property(e => e.TimeStamp);
        }
    }
}
