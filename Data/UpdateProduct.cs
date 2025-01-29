using System;
using System.Globalization;
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

                    if (!int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out int chosenProduct))
                    {
                        Console.WriteLine("Ogiltig inmatning, vänligen välj ett giltigt nummer.");
                        Thread.Sleep(2000);
                        Console.Clear();
                        continue;
                    }
                    if (chosenProduct == 0)
                    {
                        loop = false;
                        Console.Clear();
                        continue;
                    }
                    // Kontrollera om det valda numret motsvarar en produkt i listan
                    if (chosenProduct < 1 || chosenProduct > allProducts.Count)
                    {
                        Console.WriteLine("Det finns ingen produkt med det numret. Försök igen.");
                        Thread.Sleep(2000);
                        Console.Clear();
                        continue;
                    }
                    else if (chosenProduct > 0)
                    {
                        var selectedProduct = allProducts.Where(p => p.Id == chosenProduct).FirstOrDefault();

                        if (selectedProduct != null)
                        {

                            Console.WriteLine("--------------------------------------------");
                            Console.WriteLine("[1] Namn: " + selectedProduct.Name);
                            Console.WriteLine("[2] Info: " + selectedProduct.ProductInfo);
                            Console.WriteLine("[3] Pris: " + selectedProduct.Price);
                            // Visa alla kategorier som är kopplade till produkten
                            var categoryNames = string.Join(", ", selectedProduct.Categories.Select(c => c.Name));
                            Console.WriteLine("[4] Kategorier: " + (string.IsNullOrEmpty(categoryNames) ? "Inga kategorier" : categoryNames));
                            Console.WriteLine("[0] Tillbaka ");
                            Console.WriteLine("--------------------------------------------");
                            Console.WriteLine("Välj en egenskap att ändra: ");

                            if (!int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out int changeProp))
                            {
                                Console.WriteLine("Ogiltig inmatning, vänligen välj ett giltigt nummer.");
                                Thread.Sleep(2000);
                                Console.Clear();
                                continue;
                            }
                            switch (changeProp)
                            {
                                case 0:
                                    Console.Clear();
                                    continue;

                                case 1: // Ändra namn
                                    Console.WriteLine("Ange nytt namn för produkten (nuvarande: " + selectedProduct.Name + "): ");
                                    string newName = Console.ReadLine();
                                    if (!string.IsNullOrWhiteSpace(newName))
                                    {
                                        selectedProduct.Name = newName;
                                        myDb.Products.Update(selectedProduct);
                                        await myDb.SaveChangesAsync();
                                        Console.WriteLine("Namnet har uppdaterats till: " + selectedProduct.Name);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Ogiltig inmatning. Namnet ändrades inte.");
                                    }
                                    break;

                                case 2: // Ändra info
                                    Console.WriteLine("Ange ny kort info för produkten (nuvarande: " + selectedProduct.ProductInfo + "): ");
                                    string newInfo = Console.ReadLine();

                                    if (!string.IsNullOrWhiteSpace(newInfo))
                                    {
                                        selectedProduct.ProductInfo = newInfo;
                                        myDb.Products.Update(selectedProduct);
                                        await myDb.SaveChangesAsync();
                                        Console.WriteLine("Produktinfo har uppdaterats till: " + selectedProduct.ProductInfo);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Ogiltig inmatning. Info ändrades inte.");
                                    }
                                    break;

                                case 3: // Ändra pris
                                    Console.WriteLine("Ange nytt pris för produkten (nuvarande: " + selectedProduct.Price + "): ");
                                    string priceInput = Console.ReadLine();

                                    if (decimal.TryParse(priceInput, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal newPrice) && newPrice >= 0)
                                    {
                                        selectedProduct.Price = newPrice;
                                        myDb.Products.Update(selectedProduct);
                                        await myDb.SaveChangesAsync();
                                        Console.WriteLine("Priset har uppdaterats till: " + selectedProduct.Price);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Ogiltigt pris. Inget ändrades.");
                                    }
                                    break;

                                default:
                                    Console.WriteLine("Ogiltigt val.");
                                    break;
                            }
                            Thread.Sleep(2000);
                            Console.Clear();
                            allProducts = myDb.Products.ToList();
                        }
                        else
                        {
                            Console.WriteLine("Ingen produkt hittades med det numret.");
                            Thread.Sleep(2000);
                            Console.Clear();
                        }
                    }
                }
            }
        }
    }
}