using System;
using System.Diagnostics;
using ConsoleShoppen.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleShoppen.Data
{
    public class ProductList
    {
        private static List<Product> productListAsync = new List<Product>();

        public static async Task LoadProductListAsync()
        {
            using (var myDb = new MyDbContext())
            {
                productListAsync = await myDb.Products.ToListAsync();
            }
        }

        public static async Task RunAsync()
        {
            using (var myDb = new MyDbContext())
            {
                //// bara för att visa skillnaden--->
                //Stopwatch sw = new Stopwatch();

                //sw.Start();
                //var productList = myDb.Products.ToList();
                //sw.Stop();
                //Console.WriteLine("Tid att ladda produkter vanligt: " + sw.ElapsedMilliseconds + " ms");

                //sw.Restart();
                //var productListAsync = await myDb.Products.ToListAsync();
                //sw.Stop();
                //Console.WriteLine("Tid att ladda produkter asyncront: " + sw.ElapsedMilliseconds + " ms");
                //// <--------------------------------

                bool loop = true;

                while (loop)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("--------------------------------------------");
                    Console.WriteLine("|              Produkt Lista               |");
                    Console.WriteLine("--------------------------------------------");
                    Console.ResetColor();

                    var productList = productListAsync
                                     .OrderBy(p => p.Name)
                                     .ToList();

                    Console.WriteLine("Totalt " + productList.Count + " produkter i listan.");
                    Console.WriteLine("--------------------------------------------");

                    int i = 1;
                    foreach (var product in productListAsync)
                    {
                        Console.WriteLine("[" + i + "] " + product.Name?.PadRight(32) + product.Price.GetValueOrDefault().ToString("0.##") + "kr");
                        i++;
                    }

                    Console.WriteLine("[0] Tillbaka");
                    Console.WriteLine("--------------------------------------------");
                    Console.WriteLine("Välj en produkt: ");

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
                        var selectedProduct = productListAsync.Where(p => p.Id == nr).FirstOrDefault();

                        if (selectedProduct != null)
                        {
                            Console.WriteLine("--------------------------------------------");
                            Console.WriteLine("Produkt: " + selectedProduct.Name);
                            Console.WriteLine("Om produkten: " + selectedProduct.ProductInfo);
                            Console.WriteLine("Pris: " + selectedProduct.Price);
                            Console.WriteLine("--------------------------------------------");


                            Console.WriteLine("[1] Lägg till i varukorg");
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
                                // Lägg till i kundvagn --->
                                Helpers.AddToCart(selectedProduct);
                                Thread.Sleep(2000);
                                Console.Clear();
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
}