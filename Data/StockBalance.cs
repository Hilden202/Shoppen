using System;
using ConsoleShoppen.Models;

namespace ConsoleShoppen.Data
{
    public class StockBalance
    {
        public static async Task RunAsync(ICollection<Product> allProducts)
        {
            using (var myDb = new MyDbContext())
            {
                bool loop = true;

                while (loop)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("--------------------------------------------");
                    Console.WriteLine("|               Lagersaldo                 |");
                    Console.WriteLine("--------------------------------------------");
                    Console.ResetColor();

                    var productList = allProducts
                                     .OrderBy(p => p.Name)
                                     .ToList();

                    Console.WriteLine("Totalt " + productList.Count + " produkttyper i listan.");
                    Console.WriteLine("--------------------------------------------");

                    int i = 1;
                    foreach (var product in productList)
                    {
                        Console.WriteLine("[" + i + "] " + product.Name?.PadRight(36) + product.Stock + "st");
                        i++;
                    }

                    Console.WriteLine("[0] Tillbaka");
                    Console.WriteLine("--------------------------------------------");
                    Console.WriteLine("Välj en produkt att fylla på: ");

                    if (!int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out int nr))
                    {
                        Console.WriteLine("Ogiltig inmatning, vänligen välj ett giltigt nummer.");
                        Thread.Sleep(2000);
                        Console.Clear();
                        continue;
                    }
                    if (nr == 0)
                    {
                        loop = false;
                        break;
                    }
                    else if (nr > 0 && nr <= productList.Count) // Kontrollera att valet är inom intervallet
                    {
                        // Hämta vald produkt baserat på index
                        var selectedProduct = productList[nr - 1];

                        Console.WriteLine("--------------------------------------------");
                        Console.WriteLine("Produkt: " + selectedProduct.Name);
                        Console.WriteLine("Saldo: " + selectedProduct.Stock);
                        Console.WriteLine("--------------------------------------------");


                        Console.WriteLine("[1] Fyll på produkt");
                        Console.WriteLine("[0] Tillbaka ");

                        if (!int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out int nr2))
                        {
                            Console.WriteLine("Ogiltig inmatning, vänligen välj ett giltigt nummer.");
                            Thread.Sleep(2000);
                            Console.Clear();
                            continue;
                        }
                        if (nr2 == 0)
                        {
                            Console.Clear();
                            continue;
                        }
                        else if (nr2 == 1)
                        {
                            // lägga till produkt --->
                            Product.AddStock(selectedProduct.Id);
                            Thread.Sleep(2000);
                            Console.Clear();

                            // Uppdatera listan
                            allProducts = myDb.Products.ToList();
                        }
                        else
                        {
                            Console.WriteLine("Ogiltigt val, vänligen välj ett giltigt alternativ.");
                            Thread.Sleep(2000);
                            Console.Clear();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Produkt med ID " + nr + " hittades inte.");
                        Thread.Sleep(2000);
                        Console.Clear();
                    }
                }

            }
        }
    }

}
