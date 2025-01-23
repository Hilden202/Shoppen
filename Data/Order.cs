using System;
using ConsoleShoppen.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleShoppen.Data
{
    public class Order
    {
        public static void OrderCheckout()
        {
            using (var context = new MyDbContext())
            {
                Cart activeCart = Cart.GetActiveCart();

                if (activeCart == null)
                {
                    Console.WriteLine("Det finns ingen aktiv varukorg.");
                    return;
                }

                while (true)
                {
                    ShowCart(activeCart);

                    Console.WriteLine("Välj produkt att ändra/Ta bort: ");
                    Console.WriteLine("Tryck [P] för att slutföra köp: ");

                    var key = Console.ReadKey(true).Key;

                    if (key >= ConsoleKey.D0 && key <= ConsoleKey.D9)
                    {
                        // Försök att konvertera till int och hantera produktval
                        if (!int.TryParse(key.ToString().Substring(1), out int nr))
                        {
                            Console.WriteLine("Ogiltig inmatning, vänligen välj en giltig inmatning.");
                            Thread.Sleep(2000);
                            Console.Clear();
                            continue;
                        }
                        if (nr == 0)
                        {
                            break;
                        }
                        else if (nr > 0 && nr <= activeCart.CartProducts.Count)
                        {
                            var selectedCartProduct = activeCart.CartProducts[nr - 1]; // Hämta det valda CartProduct

                            if (selectedCartProduct != null)
                            {
                                EditProduct(activeCart, context, selectedCartProduct);
                            }
                        }

                    }
                    else if (key == ConsoleKey.P)
                    {
                        //Todo Kod för att gå vidare med betalning..
                        // Slutför köpet
                        Cart.CompleteCart(activeCart.Id);
                        Cart.CreateNewCart();
                        Console.WriteLine("Köp har slutförts.");
                        Thread.Sleep(2000);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Ogiltig inmatning, vänligen välj ett giltigt alternativ.");
                        Thread.Sleep(2000);
                        Console.Clear();
                    }
                }
            }
        }
        public static void ShowCart(Cart activeCart)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("|                 Varukorg                 |");
            Console.WriteLine("--------------------------------------------");
            Console.ResetColor();

            int totalQuantity = activeCart.GetTotalQuantity(); // Total kvantitet av produkter

            Console.WriteLine("Totalt " + totalQuantity + " produkter i listan.");
            Console.WriteLine("--------------------------------------------");


            int i = 1;
            foreach (var cartProduct in activeCart.CartProducts)
            {
                Console.WriteLine("[" + i + "] " + cartProduct.Product.Name.PadRight(32) + "Antal: " + cartProduct.Quantity);
                i++;
            }
            Console.WriteLine("[0] Tillbaka");
            Console.WriteLine("--------------------------------------------");

            var cartList = activeCart.CartProducts
                          .OrderBy(p => p.Product.Id)
                          .ToList();

            // Beräkna totalpriset för alla produkter i varukorgen baserat på kvantitet och pris
            decimal totalSum = cartList.Sum(p => p.Quantity * p.Product.Price.GetValueOrDefault());

            Console.WriteLine("Totalt pris: ".PadRight(36) + totalSum.ToString("0.##") + "kr");
            Console.WriteLine("--------------------------------------------");
        }

        public static void EditProduct(Cart activeCart, MyDbContext context, CartProduct selectedCartProduct)
        {
            bool editingProduct = true;

            while (editingProduct)
            {
                Console.Clear();
                ShowCart(activeCart);

                decimal totalProductSum = selectedCartProduct.Quantity * selectedCartProduct.Product.Price.GetValueOrDefault();
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("Produkt: " + selectedCartProduct.Product.Name);
                Console.WriteLine("Om produkten: " + selectedCartProduct.Product.ProductInfo);
                Console.WriteLine("Antal: " + selectedCartProduct.Quantity);
                Console.WriteLine("Summa: " + totalProductSum.ToString("0.##") + "kr");
                Console.WriteLine("--------------------------------------------");

                Console.WriteLine("[1] Lägg till produkt: ");
                Console.WriteLine("[2] Ta bort produkt: ");
                Console.WriteLine("[0] Tillbaka: ");

                if (!int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out int nr))
                {
                    Console.WriteLine("Ogiltig inmatning, vänligen välj ett giltigt nummer.");
                    Thread.Sleep(2000);
                    Console.Clear();
                    continue;
                }
                switch (nr)
                {
                    case 1: // Lägg till produkt
                        selectedCartProduct.Quantity++;

                        var productToUpdate = context.CartProducts.FirstOrDefault(cp => cp.Id == selectedCartProduct.Id);
                        if (productToUpdate != null)
                        {
                            // Uppdatera kvantiteten i databasen
                            productToUpdate.Quantity = selectedCartProduct.Quantity;
                            context.SaveChanges();
                        }
                        break;

                    case 2: // Ta bort produkt
                        selectedCartProduct.Quantity--;

                        if (selectedCartProduct.Quantity > 0)
                        {
                            context.CartProducts.Update(selectedCartProduct);
                            context.SaveChanges();
                        }
                        else if (selectedCartProduct.Quantity <= 0)
                        {
                            activeCart.CartProducts.Remove(selectedCartProduct);

                            context.CartProducts.Remove(selectedCartProduct);
                            context.SaveChanges();
                            editingProduct = false;
                            Console.Clear();
                        }
                        break;

                    case 0:
                        editingProduct = false;
                        Console.Clear();
                        break;

                    default:
                        Console.WriteLine("Ogiltig inmatning, vänligen välj ett giltigt alternativ.");
                        Thread.Sleep(2000);
                        break;
                }
            }

        }
    }
}