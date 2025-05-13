
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
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=children_leisure.db")
            .Options;

            using var context = new AppDbContext(options);
            context.Database.EnsureCreated();

            // Репозиторії
            var attractionRepo = new BaseRepository<Attraction>(context);
            var characterRepo = new BaseRepository<FairyCharacter>(context);
            var orderRepo = new BaseRepository<Order>(context);

            // Сервіси
            var pricingService = new PricingService();
            var entertainmentService = new EntertainmentService(attractionRepo, characterRepo);
            var orderService = new OrderService(orderRepo, attractionRepo, characterRepo, pricingService);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ДИТЯЧЕ ДОЗВІЛЛЯ ===");
                Console.WriteLine("1. Переглянути атракціони");
                Console.WriteLine("2. Переглянути казкових героїв");
                Console.WriteLine("3. Створити замовлення");
                Console.WriteLine("4. Підтвердити замовлення");
                Console.WriteLine("0. Вийти");
                Console.Write("Вибір: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAttractions(entertainmentService);
                        break;
                    case "2":
                        ShowCharacters(entertainmentService);
                        break;
                    case "3":
                        CreateOrder(orderService, entertainmentService);
                        break;
                    case "4":
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

            Console.Write("Це день народження? (y/n): ");
            bool isBday = Console.ReadLine().Trim().ToLower() == "y";

            ShowCharacters(entertainmentService);
            Console.Write("ID казкового героя (або пропустіть): ");
            string charInput = Console.ReadLine();
            int? charId = int.TryParse(charInput, out int cId) ? cId : null;

            ShowAttractions(entertainmentService);
            Console.Write("ID атракціонів через кому (або пропустіть): ");
            var attrInput = Console.ReadLine();
            int[]? attrIds = attrInput?.Split(',').Select(s => int.TryParse(s.Trim(), out int val) ? val : -1).Where(id => id > 0).ToArray();

            var order = service.CreateOrder(name, phone, isBday, charId, attrIds);

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
    }
}