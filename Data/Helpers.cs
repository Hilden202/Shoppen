using ConsoleShoppen.Models;
using Microsoft.EntityFrameworkCore;

public static class Helpers
{
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
}