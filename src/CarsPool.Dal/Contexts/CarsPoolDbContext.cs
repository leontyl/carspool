using CarsPool.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarsPool.Dal.Contexts
{
    public class CarsPoolDbContext : DbContext
    {
        public CarsPoolDbContext(DbContextOptions<CarsPoolDbContext> options) : base(options)
        {
            // Create database if doesn't exist
            Database.EnsureCreated();
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<CarDriver> CarDriver { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>().ToTable("Cars");
            modelBuilder.Entity<Driver>().ToTable("Drivers");
            modelBuilder.Entity<CarDriver>().ToTable("CarDriver");
        }
    }
}
