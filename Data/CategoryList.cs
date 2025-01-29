using System;
using ConsoleShoppen.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;  // För att använda Task

namespace ConsoleShoppen.Data
{
    public class CategoryList
    {
        private static List<Category> categoryListAsync = new List<Category>();

        public static async Task LoadCategoryListAsync()
        {
            using (var myDb = new MyDbContext())
            {
                categoryListAsync = await myDb.Categories.ToListAsync();
            }
        }

        public static async Task RunAsync()
        {
            using (var myDb = new MyDbContext())
            {

                bool loop = true;

                while (loop)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("--------------------------------------------");
                    Console.WriteLine("|             Kategori Lista               |");
                    Console.WriteLine("--------------------------------------------");
                    Console.ResetColor();

                    var categoryList = categoryListAsync
                        .Where(c => !c.IsHidden)
                        .OrderBy(c => c.Name)
                        .ToList();

                    Console.WriteLine("Totalt " + categoryList.Count + " kategorier i listan.");
                    Console.WriteLine("--------------------------------------------");

                    int i = 1;
                    foreach (var category in categoryList)
                    {
                        Console.WriteLine("[" + i + "] " + "Kategori: " + category.Name);
                        i++;

                    }
                    Console.WriteLine("[0] Tillbaka");
                    Console.WriteLine("--------------------------------------------");
                    Console.WriteLine("Välj en kategori: ");

                    if (!int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out int categoryChoice))
                    {
                        Console.WriteLine("Ogiltig inmatning, vänligen välj ett giltigt nummer.");
                        Thread.Sleep(2000);
                        Console.Clear();
                        continue;
                    }

                    if (categoryChoice == 0)
                    {
                        loop = false;
                        break;
                    }

                    else if (categoryChoice > 0 && categoryChoice <= categoryList.Count) // Kontrollera intervallet
                    {
                        var selectedCategory = categoryList[categoryChoice - 1]; // Hämta vald kategori

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("--------------------------------------------");
                        Console.WriteLine("|         Kategori: " + selectedCategory.Name.PadRight(19) + "    |");
                        Console.WriteLine("--------------------------------------------");
                        Console.ResetColor();

                        // Hämta produkterna som tillhör den valda kategorin
                        var productList = await myDb.Products
                            .Where(p => p.Categories.Any(c => c.Id == selectedCategory.Id))
                            .OrderBy(p => p.Name)
                            .ToListAsync();

                        Console.WriteLine("Totalt " + productList.Count + " produkter i listan.");
                        Console.WriteLine("--------------------------------------------");

                        int j = 1;
                        foreach (var product in productList)
                        {
                            Console.WriteLine("[" + j + "] " + product.Name?.PadRight(32) + product.Price.GetValueOrDefault().ToString("0.##") + "kr");
                            j++;
                        }
                        Console.WriteLine("[0] Tillbaka");
                        Console.WriteLine("--------------------------------------------");
                        Console.WriteLine("Välj en produkt: ");

                        // Hantera användarens val
                        if (!int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out int productChoice))
                        {
                            Console.WriteLine("Ogiltig inmatning, vänligen välj ett giltigt nummer.");
                            Thread.Sleep(2000);
                            Console.Clear();
                            continue;
                        }

                        if (productChoice == 0)
                        {
                            Console.Clear();
                            continue;
                        }
                        if (productChoice - 1 < 0 || productChoice - 1 >= productList.Count)
                        {
                            Console.WriteLine("Ogiltigt val. Vänligen välj en giltig produkt.");
                            Thread.Sleep(2000);
                            Console.Clear();
                            continue;
                        }

                        else if (productChoice > 0 && productChoice <= productList.Count) // Kontrollera intervallet
                        {
                            var selectedProduct = productList[productChoice - 1]; // Hämta vald produkt

                            Console.WriteLine("--------------------------------------------");
                            Console.WriteLine("Produkt: " + selectedProduct.Name);
                            Console.WriteLine("Om produkten: " + selectedProduct.ProductInfo);
                            Console.WriteLine("Pris: " + selectedProduct.Price);
                            Console.WriteLine("--------------------------------------------");

                            Console.WriteLine("[1] Lägg till i varukorg");
                            Console.WriteLine("[0] Tillbaka");

                            if (!int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out int addProduct))
                            {
                                Console.WriteLine("Ogiltig inmatning, vänligen välj ett giltigt nummer.");
                                Thread.Sleep(2000);
                                Console.Clear();
                                continue;
                            }
                            if (addProduct == 0)
                            {
                                Console.Clear();
                                continue;
                            }
                            else if (addProduct == 1)
                            {
                                // Lägg till i kundvagn --->
                                Cart.AddToCart(selectedProduct);
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
                            Console.WriteLine("Ogiltigt val, vänligen välj ett giltigt alternativ.");
                            Thread.Sleep(2000);
                            Console.Clear();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Vald kategori finns inte.");
                        Thread.Sleep(2000);
                        Console.Clear();
                    }
                }
            }
        }
    }
}

