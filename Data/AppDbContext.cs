using Microsoft.EntityFrameworkCore;
using GandamarCloudAPI.Models;

namespace GandamarCloudAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<CloudProduct> Products { get; set; }
        public DbSet<CloudSupplier> Suppliers { get; set; }
        public DbSet<CloudOnlineOrder> Orders { get; set; }
        public DbSet<CloudOnlineOrderDetail> OrderDetails { get; set; }
        public DbSet<CloudStockEntry> StockEntries { get; set; }
        public DbSet<CloudUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<CloudOnlineOrder>()
                .HasMany(o => o.Details)
                .WithOne()
                .HasForeignKey(d => d.OrderSyncId);
        }
    }
}
