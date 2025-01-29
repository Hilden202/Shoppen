using System;
using ConsoleShoppen.Models;
using System.Globalization;

namespace ConsoleShoppen.Data
{
    public class DeleteProduct
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
                    Console.WriteLine("|              Ta bort Produkt              |");
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
                    Console.WriteLine("Välj en produkt att ta bort: ");

                    if (!int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out int chosenProduct))
                    {
                        Console.WriteLine("Ogiltig inmatning, vänligen välj ett giltigt nummer.");
                        Thread.Sleep(2000);
                        Console.Clear();
                        continue;
                    }
                    if (chosenProduct == 0)
                    {
                        Console.Clear();
                        return;
                    }

                    // Kontrollera om valda numret motsvarar en produkt
                    if (chosenProduct < 1 || chosenProduct > allProducts.Count)
                    {
                        Console.WriteLine("Det finns ingen produkt med det numret. Försök igen.");
                        Thread.Sleep(2000);
                        Console.Clear();
                        continue;
                    }

                    // Hämta vald produkt
                    var selectedProduct = allProducts.ElementAtOrDefault(chosenProduct - 1);

                    if (selectedProduct != null)
                    {
                        // Bekräfta borttagning
                        Console.WriteLine("Är du säker på att du vill ta bort " + selectedProduct.Name + "? (Y/N): ");
                        var confirmDelete = Console.ReadKey(true).Key;

                        if (confirmDelete == ConsoleKey.Y)
                        {
                            myDb.Products.Remove(selectedProduct);
                            await myDb.SaveChangesAsync();
                            Console.WriteLine("Produkten " + selectedProduct.Name + " har tagits bort.");
                        }
                        else
                        {
                            Console.WriteLine("Borttagning avbruten.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ingen produkt hittades med det numret.");
                    }

                    Thread.Sleep(2000);
                    Console.Clear();
                    allProducts = myDb.Products.ToList();
                }
            }
        }
    }
}