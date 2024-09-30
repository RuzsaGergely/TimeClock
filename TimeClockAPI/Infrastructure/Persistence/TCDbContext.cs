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
    }
}
