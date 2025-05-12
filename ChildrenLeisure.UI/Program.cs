
using ChildrenLeisure.BLL.Services;
using ChildrenLeisure.DAL.Data;
using ChildrenLeisure.DAL.Entities;
using ChildrenLeisure.DAL.Repositories;
using Microsoft.EntityFrameworkCore;


namespace ChildrenLeisure.UI
{
    public class Program
    {
        static void Main(string[] args)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=children_leisure.db")
            .Options;

            using var context = new AppDbContext(options);
            context.Database.EnsureCreated();

            // Репозиторії
            var zoneRepo = new BaseRepository<EntertainmentZone>(context);
            var attractionRepo = new BaseRepository<Attraction>(context);
            var characterRepo = new BaseRepository<FairyCharacter>(context);
            var orderRepo = new BaseRepository<Order>(context);

            // Сервіси
            var pricingService = new PricingService();
            var entertainmentService = new EntertainmentService(zoneRepo, attractionRepo, characterRepo);
            var orderService = new OrderService(orderRepo, attractionRepo, characterRepo, pricingService);

            // Початкові дані
            SeedData(context);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ДИТЯЧЕ ДОЗВІЛЛЯ ===");
                Console.WriteLine("1. Переглянути зони розваг");
                Console.WriteLine("2. Переглянути атракціони");
                Console.WriteLine("3. Переглянути казкових героїв");
                Console.WriteLine("4. Створити замовлення");
                Console.WriteLine("5. Підтвердити замовлення");
                Console.WriteLine("0. Вийти");
                Console.Write("Вибір: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowZones(entertainmentService);
                        break;
                    case "2":
                        ShowAttractions(entertainmentService);
                        break;
                    case "3":
                        ShowCharacters(entertainmentService);
                        break;
                    case "4":
                        CreateOrder(orderService, entertainmentService);
                        break;
                    case "5":
                        ConfirmOrder(orderService);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Невірний вибір.");
                        break;
                }

                Console.WriteLine("\nНатисніть будь-яку клавішу для продовження...");
                Console.ReadKey();
            }
        }

        static void ShowZones(EntertainmentService service)
        {
            Console.WriteLine("=== ЗОНИ РОЗВАГ ===");
            foreach (var z in service.GetAllZones())
                Console.WriteLine($"{z.Id}. {z.Name} - {z.Description} ({z.BasePrice} грн)");
        }

        static void ShowAttractions(EntertainmentService service)
        {
            Console.WriteLine("=== АТРАКЦІОНИ ===");
            foreach (var a in service.GetAllAttractions())
                Console.WriteLine($"{a.Id}. {a.Name} - {a.Description} ({a.Price} грн)");
        }

        static void ShowCharacters(EntertainmentService service)
        {
            Console.WriteLine("=== КАЗКОВІ ГЕРОЇ ===");
            foreach (var c in service.GetAllFairyCharacters())
                Console.WriteLine($"{c.Id}. {c.Name} ({c.PricePerHour} грн/год) - {c.Description}");
        }

        static void CreateOrder(OrderService service, EntertainmentService entertainmentService)
        {
            Console.WriteLine("=== СТВОРЕННЯ ЗАМОВЛЕННЯ ===");
            Console.Write("Ім'я замовника: ");
            string name = Console.ReadLine();
            Console.Write("Телефон: ");
            string phone = Console.ReadLine();

            Console.Write("Це день народження? (так/ні): ");
            bool isBday = Console.ReadLine().Trim().ToLower() == "так";

            ShowCharacters(entertainmentService);
            Console.Write("ID казкового героя (або пропустіть): ");
            string charInput = Console.ReadLine();
            int? charId = int.TryParse(charInput, out int cId) ? cId : null;

            ShowAttractions(entertainmentService);
            Console.Write("ID атракціонів через кому (або пропустіть): ");
            var attrInput = Console.ReadLine();
            int[]? attrIds = attrInput?.Split(',').Select(s => int.TryParse(s.Trim(), out int val) ? val : -1).Where(id => id > 0).ToArray();

            ShowZones(entertainmentService);
            Console.Write("ID зон через кому (або пропустіть): ");
            var zoneInput = Console.ReadLine();
            int[]? zoneIds = zoneInput?.Split(',').Select(s => int.TryParse(s.Trim(), out int val) ? val : -1).Where(id => id > 0).ToArray();

            var order = service.CreateOrder(name, phone, isBday, charId, attrIds, zoneIds);

            Console.WriteLine($"ЗАМОВЛЕННЯ СТВОРЕНО. ЦІНА: {order.TotalPrice} грн. Статус: {order.Status}");
        }

        static void ConfirmOrder(OrderService service)
        {
            Console.Write("Введіть ID замовлення для підтвердження: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                try
                {
                    var updated = service.ConfirmOrder(id);
                    Console.WriteLine($"Замовлення {id} підтверджено. Статус: {updated.Status}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Невірне значення ID.");
            }
        }

        static void SeedData(AppDbContext context)
        {
            if (!context.EntertainmentZones.Any())
            {
                context.EntertainmentZones.AddRange(
                    new EntertainmentZone { Name = "Зона пригод", Description = "Місце для активних ігор", BasePrice = 300 },
                    new EntertainmentZone { Name = "Тиха зона", Description = "Малювання, книги", BasePrice = 200 }
                );
            }

            if (!context.Attractions.Any())
            {
                context.Attractions.AddRange(
                    new Attraction { Name = "Батути", Description = "Веселі стрибки", Price = 150 },
                    new Attraction { Name = "Карусель", Description = "Класична дитяча розвага", Price = 100 }
                );
            }

            if (!context.FairyCharacters.Any())
            {
                context.FairyCharacters.AddRange(
                    new FairyCharacter { Name = "Пірат Джек", Costume = "Пірат", PricePerHour = 250, Description = "Піратські пригоди" },
                    new FairyCharacter { Name = "Фея Лілі", Costume = "Фея", PricePerHour = 300, Description = "Магічне шоу" }
                );
            }

            context.SaveChanges();
        }
    }
}