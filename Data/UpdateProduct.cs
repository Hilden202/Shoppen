using System;
using ConsoleShoppen.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleShoppen.Data
{
    public class UpdateProduct
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
                    Console.WriteLine("|              Ändra Produkt               |");
                    Console.WriteLine("--------------------------------------------");
                    Console.ResetColor();

                    var productList = allProducts
                                     .OrderBy(p => p.Name)
                                     .ToList();

                    Console.WriteLine("Totalt " + productList.Count + " produkter i listan.");
                    Console.WriteLine("--------------------------------------------");

                    int i = 1;
                    foreach (var product in allProducts)
                    {
                        Console.WriteLine("[" + i + "] " + product.Name);
                        i++;
                    }

                    Console.WriteLine("[0] Tillbaka");
                    Console.WriteLine("--------------------------------------------");
                    Console.WriteLine("Välj en produkt att ändra: ");

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
                    else if (nr > 0)
                    {
                        var selectedProduct = allProducts.Where(p => p.Id == nr).FirstOrDefault();

                        if (selectedProduct != null)
                        {

                            Console.WriteLine("--------------------------------------------");
                            Console.WriteLine("[1] Produkt: " + selectedProduct.Name);
                            Console.WriteLine("[2] Info: " + selectedProduct.ProductInfo);
                            Console.WriteLine("[3] Pris: " + selectedProduct.Price);
                            Console.WriteLine("[0] Tillbaka ");
                            Console.WriteLine("--------------------------------------------");
                            Console.WriteLine("Välj en egenskap att ändra: ");

                            Console.ReadLine();
                        }
                    }
                }
            }
        }
    }
}

