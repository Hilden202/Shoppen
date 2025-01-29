using System;
using Microsoft.EntityFrameworkCore;

namespace ConsoleShoppen.Models
{
    public class Cart
    {
        private const decimal Moms = 0.25m; // 25% moms


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

        public static void AddToCart(Product selectedProduct)// Lägg till i kundvagn --->
        {
            using (var context = new MyDbContext())
            {
                // Hitta eller skapa en aktiv kundvagn
                var cart = context.Carts.Include(c => c.CartProducts).FirstOrDefault(c => c.Status == "Active");

                if (cart == null)
                {
                    Cart.CreateNewCart();
                }

                if (cart != null)
                {
                    // Kolla om produkten redan finns i kundvagnen
                    var existingProductInCart = cart.CartProducts.FirstOrDefault(p => p.ProductId == selectedProduct.Id);

                    if (existingProductInCart != null)
                    {
                        // Uppdatera mängden om produkten redan finns
                        existingProductInCart.Quantity += 1;
                        cart.TotalPrice += selectedProduct.Price.GetValueOrDefault();
                    }
                    else
                    {
                        // Lägg till produkten som en ny post
                        cart.CartProducts.Add(new CartProduct
                        {
                            Product = selectedProduct,
                            Quantity = 1,
                            CartId = cart.Id,
                            ProductId = selectedProduct.Id
                        });
                        cart.TotalPrice += selectedProduct.Price.GetValueOrDefault();
                    }
                    try
                    {
                        context.SaveChanges();
                        Console.WriteLine(selectedProduct.Name + " har lagts till i varukorgen!");
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ett fel uppstod när produkten skulle sparas i varukorgen: " + ex.Message);
                        Console.ResetColor();
                        if (ex.InnerException != null)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                            Console.ResetColor();
                        }
                        Console.ReadKey();
                    }
                }
            }
        }

        public static void CompleteCart(int cartId, decimal shippingCost)
        {
            using (var context = new MyDbContext())
            {
                var cart = context.Carts
                                  .Include(c => c.CartProducts)
                                  .ThenInclude(cp => cp.Product)
                                  .FirstOrDefault(c => c.Id == cartId);

                if (cart != null)
                {
                    // Beräkna totalpriset för kundvagnen inklusive frakt och moms
                    decimal totalProductPrice = cart.CartProducts.Sum(cp => cp.Quantity * cp.Product.Price.GetValueOrDefault());
                    decimal totalWithTax = totalProductPrice * (1 + Moms);
                    cart.TotalPrice = totalWithTax + shippingCost;

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