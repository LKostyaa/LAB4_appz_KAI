using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ChildrenLeisure.DAL.Entities;

namespace ChildrenLeisure.DAL.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<EntertainmentZone> EntertainmentZones { get; set; }
        public DbSet<Attraction> Attractions { get; set; }
        public DbSet<FairyCharacter> FairyCharacters { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Налаштування зв'язків та обмежень
            modelBuilder.Entity<Order>()
                .HasMany(o => o.SelectedAttractions)
                .WithMany();

            modelBuilder.Entity<Order>()
                .HasMany(o => o.SelectedZones)
                .WithMany();
        }
    }
}
