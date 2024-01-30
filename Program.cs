using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ConsoleAppCoreHomeWork2;
class Program
{
    static void Main()
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("appsettings.json");
        var config = builder.Build();
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
        var options = optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection")).Options;
        using (ApplicationContext db = new ApplicationContext(options))
        {
            //        //        if (!db.Products.Any())
            //        //        {
            //        //            var products = new List<Product>
            //        //            {
            //        //                new Product { Name = "Телефон", Price = 30000M },
            //        //                new Product { Name = "Ноутбук", Price = 60000M },
            //        //                new Product { Name = "Наушники", Price = 8000M }
            //        //            };
            //        //            db.Products.AddRange(products);
            //        //            db.SaveChanges();
            //        //        }
            //        //    }
            //        //}
            //        // Создаем сервис для обработки заказов
            //        var orderService = new OrderService(db);
            //        //        //Добавление нового заказа
            //        //        orderService.AddOrder("Олег Паномарюк", new List<int> { 1, 2 });
            //        //    }
            //        //}
            //        // Просмотр созданных заказов                                                                         
            //        var orders = db.Orders.Include(o => o.Products).ToList();
            //        foreach (var order in orders)
            //        {
            //            Console.WriteLine($"Заказ {order.Id}, Клиент: {order.CustomerName}");
            //            foreach (var product in order.Products)
            //            {
            //                Console.WriteLine($"  Продукт: {product.Name}, Цена: {product.Price}");
            //            }
            //        }

            //        //            // Удаление заказа
            //        if (orders.Any())
            //        {
            //            int orderIdToDelete = orders.First().Id;
            //            orderService.DeleteOrder(orderIdToDelete);

            //            Console.WriteLine($"Заказ {orderIdToDelete} удален.");
            //        }
        }
    }

public class ApplicationContext : DbContext
    {
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        public ApplicationContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

    }
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int? OrderId { get; set; }
        public Order Order { get; set; }
    }
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public List<Product> Products { get; set; }
    }
    public class OrderService
    {
        private readonly ApplicationContext db;

        public OrderService(ApplicationContext context)
        {
            db = context;
        }

        public void AddOrder(string customerName, List<int> productIds)
        {
            var order = new Order { CustomerName = customerName };
            db.Orders.Add(order);
            db.SaveChanges();
            foreach (var id in productIds)
            {
                var product = db.Products.FirstOrDefault(p => p.Id == id);
                if (product != null)
                {
                    product.OrderId = order.Id;
                }
            }
            db.SaveChanges();
        }
        public List<Order> GetOrders()
        {
            return db.Orders.Include(o => o.Products).ToList();
        }
        public void DeleteOrder(int orderId)
        {
            var order = db.Orders.Include(o => o.Products).FirstOrDefault(o => o.Id == orderId);
            if (order != null)
            {

                foreach (var product in order.Products)
                {
                    product.OrderId = null;
                }
                db.Orders.Remove(order);
                db.SaveChanges();
            }
        }
    }
}


    