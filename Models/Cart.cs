using System;
using Microsoft.EntityFrameworkCore;

namespace ConsoleShoppen.Models
{
    public class Cart
    {
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

        public static void CompleteCart(int cartId) // Todo kolla över varför totalpriset justeras om man har fler av samma produkt vid Completed..
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
                    cart.TotalPrice = cart.CartProducts.Sum(cp => cp.Quantity * cp.Product.Price.GetValueOrDefault());

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

    }
}

