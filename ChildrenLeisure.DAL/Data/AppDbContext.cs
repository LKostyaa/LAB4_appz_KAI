using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ChildrenLeisure.DAL.Entities;

namespace ChildrenLeisure.DAL.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base() { }

        public DbSet<Attraction> Attractions { get; set; }
        public DbSet<FairyCharacter> FairyCharacters { get; set; }
        public DbSet<Order> Orders { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlite("Data Source=children_leisure.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Налаштування зв'язків та обмежень
            modelBuilder.Entity<Order>()
                .HasMany(o => o.SelectedAttractions)
                .WithMany();
            Seed(modelBuilder);
        }
        protected static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.Attraction>().HasData(
                new Attraction {Id = 1, Name = "Батути", Description = "Веселі стрибки", Price = 150 },
                    new Attraction {Id = 2, Name = "Карусель", Description = "Класична дитяча розвага", Price = 100 }
                    );
            modelBuilder.Entity<Entities.FairyCharacter>().HasData(
                new FairyCharacter {Id = 1, Name = "Пірат Джек", Costume = "Пірат", PricePerHour = 250, Description = "Піратські пригоди" },
                    new FairyCharacter {Id = 2, Name = "Фея Лілі", Costume = "Фея", PricePerHour = 300, Description = "Магічне шоу" }
                );
        }
    }
}