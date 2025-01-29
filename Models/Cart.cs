using System;
using Microsoft.EntityFrameworkCore;

namespace ConsoleShoppen.Models
{
    public class Cart
    {
        private const decimal Moms = 0.25m; // 25% moms

        private const decimal StandardShippingCost = 50.00m;  // Standard fraktkostnad 50 kr
        private const decimal ExpressShippingCost = 100.00m;  // Express fraktkostnad 100 kr

        public int Id { get; set; }
        public decimal? TotalPrice { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateCompleted { get; set; }
        public string Status { get; set; }

        public List<CartProduct> CartProducts { get; set; } = new List<CartProduct>();

        // Hämta den aktiva varukorgen
        public static Cart GetActiveCart()
        {
            using (var context = new MyDbContext())
            {
                // Hämta den aktiva varukorgen där status är "Active"
                var activeCart = context.Carts
                    .Include(c => c.CartProducts)
                    .ThenInclude(cp => cp.Product)
                    .FirstOrDefault(c => c.Status == "Active");

                return activeCart;
            }
        }

        public decimal GetMoms()
        {
            return Moms;
        }

        // Om jag vill skapa ny varukorg
        public static void CreateNewCart()
        {
            using (var context = new MyDbContext())
            {
                var newCart = new Cart
                {
                    TotalPrice = 0,
                    DateAdded = DateTime.Now,
                    Status = "Active"
                };

                context.Carts.Add(newCart);
                context.SaveChanges();
            }
        }

        public static void CompleteCart(int cartId)
        {
            using (var context = new MyDbContext())
            {
                var cart = context.Carts
                                  .Include(c => c.CartProducts)
                                  .ThenInclude(cp => cp.Product)
                                  .FirstOrDefault(c => c.Id == cartId);

                if (cart != null)
                {
                    // Beräkna totalpriset för kundvagnen
                    cart.TotalPrice = cart.CartProducts.Sum(cp => cp.Quantity * cp.Product.Price.GetValueOrDefault() * (1 + Moms));

                    // Uppdatera status och datum
                    cart.Status = "Completed";
                    cart.DateCompleted = DateTime.Now;

                    // För varje produkt i kundvagnen, minska lagret
                    foreach (var cartProduct in cart.CartProducts)
                    {
                        var product = context.Products.FirstOrDefault(p => p.Id == cartProduct.Product.Id);
                        if (product != null)
                        {
                            product.Stock -= cartProduct.Quantity;
                        }
                    }

                    // Spara ändringarna
                    context.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Kundvagnen finns inte.");
                }
            }
        }
        public int GetTotalQuantity() // Summering av antal produkter i kundvagn
        {
            return CartProducts.Sum(cp => cp.Quantity);
        }

        public decimal GetShippingCost(char shippingChoice)
        {
            switch (shippingChoice)
            {
                case '1': // Standard frakt
                    return StandardShippingCost;
                case '2': // Express frakt
                    return ExpressShippingCost;
                default:
                    Console.WriteLine("Ogiltigt val, standard frakt tillämpas.");
                    return 0m; // Default till standard om ogiltigt val
            }
        }

        public decimal GetTotalPriceWithShipping(char shippingChoice)
        {
            var shippingCost = GetShippingCost(shippingChoice);
            return (decimal)(TotalPrice + shippingCost);
        }

    }
}

