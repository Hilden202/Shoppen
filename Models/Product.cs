using System;
using System.Numerics;

namespace ConsoleShoppen.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ProductInfo { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }

        public List<CartProduct> CartProducts { get; set; } = new List<CartProduct>();

        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

        public static void AddStock(int productId)
        {
            using (var context = new MyDbContext())
            {
                Console.WriteLine("Ange antal att lägga till: ");
                if (int.TryParse(Console.ReadLine(), out int addStock))
                {
                    var product = context.Products.FirstOrDefault(p => p.Id == productId);
                    if (product != null)
                    {
                        product.Stock += addStock;
                        context.SaveChanges();
                        Console.WriteLine("Lager för " + product.Name + " har uppdaterats.");
                    }
                    else
                    {
                        Console.WriteLine("Produkt med det angivna ID:t hittades inte.");
                    }
                }
                else
                {
                    Console.WriteLine("Ogiltig inmatning. Ange ett giltigt antal.");
                }
            }
        }
    }
}
