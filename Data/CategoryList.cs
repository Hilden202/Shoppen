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
        public static void Run(ICollection<Category> allCategories, ICollection<Product> allProducts)
        {
            bool loop = true;

            while (loop)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("|              Kategori Lista               |");
                Console.WriteLine("--------------------------------------------");
                Console.ResetColor();

                var categoryList = allCategories
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
                else if (categoryChoice > 0)
                {
                    var selectedCategory = categoryList.ElementAtOrDefault(categoryChoice - 1);
                    if (selectedCategory != null)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("--------------------------------------------");
                        Console.WriteLine("|          Kategori: " + selectedCategory.Name.PadRight(18) + "    |");
                        Console.WriteLine("--------------------------------------------");
                        Console.ResetColor();

                        // Hämta produkterna som tillhör den valda kategorin
                        var productList = allProducts
                            .Where(p => p.Categories.Any(c => c.Id == selectedCategory.Id))
                            .OrderBy(p => p.Name)
                            .ToList();

                        Console.WriteLine("Totalt " + productList.Count + " produkter i listan.");
                        Console.WriteLine("--------------------------------------------");

                        int j = 1;
                        foreach (var product in selectedCategory.Products)
                        {
                            Console.WriteLine("[" + j + "] " + product.Name?.PadRight(32) + product.Price.GetValueOrDefault().ToString("0.##") + "kr");
                            j++;
                        }
                        Console.WriteLine("[0] Tillbaka");
                        Console.WriteLine("--------------------------------------------");
                        Console.WriteLine("Välj en produkt: ");

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

                        //var selectedProduct = productList.ElementAtOrDefault(productChoice - 1);
                        var selectedProduct = selectedCategory.Products.ElementAtOrDefault(productChoice - 1);

                        if (selectedProduct != null)
                        {
                            Console.WriteLine("--------------------------------------------");
                            Console.WriteLine("Produkt: " + selectedProduct.Name);
                            Console.WriteLine("Om produkten: " + selectedProduct.ProductInfo);
                            Console.WriteLine("Pris: " + selectedProduct.Price);
                            Console.WriteLine("--------------------------------------------");

                            Console.WriteLine("[1] Lägg till i varukorg");
                            Console.WriteLine("[0] Tillbaka ");
                        }
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
                            Helpers.AddToCart(selectedProduct); // Todo Fungerar inte att lägga till produkt som inte finns sen innan
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

