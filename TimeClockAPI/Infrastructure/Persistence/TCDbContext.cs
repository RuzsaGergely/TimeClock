using Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Infrastructure.Persistence
{
    public class TCDbContext : DbContext
    {
        public TCDbContext(DbContextOptions<TCDbContext> options) : base(options)
        {
        }

        public DbSet<ClockEntry> ClockEntries  { get; set; }
        public DbSet<User> Users { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Product>()
        //     .HasMany(mc => mc.BillOfMaterials)
        //     .WithOne(mc => mc.Product)
        //     .OnDelete(DeleteBehavior.NoAction);

        //}
    }
}
