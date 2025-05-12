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
        //protected static void Seed(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Entities.Attraction>().HasData(
        //        new Attraction { Name = "Батути", Description = "Веселі стрибки", Price = 150 },
        //            new Attraction { Name = "Карусель", Description = "Класична дитяча розвага", Price = 100 }
        //            );
        //    modelBuilder.Entity<Entities.EntertainmentZone>().HasData(
        //        new EntertainmentZone { Name = "Зона пригод", Description = "Місце для активних ігор", BasePrice = 300 },
        //            new EntertainmentZone { Name = "Тиха зона", Description = "Малювання, книги", BasePrice = 200 }
        //            );
        //    modelBuilder.Entity<Entities.FairyCharacter>().HasData(
        //        new FairyCharacter { Name = "Пірат Джек", Costume = "Пірат", PricePerHour = 250, Description = "Піратські пригоди" },
        //            new FairyCharacter { Name = "Фея Лілі", Costume = "Фея", PricePerHour = 300, Description = "Магічне шоу" }
        //        );
        //    modelBuilder.Entity<Entities.Order>().HasData();

        //}
    }
}