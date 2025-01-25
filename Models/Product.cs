using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using ConsoleShoppen.Data;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ConsoleShoppen.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ProductInfo { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }

        public List<CartProduct> CartProducts { get; set; } = new List<CartProduct>();

        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

        public static void AddStock(int productId)
        {
            using (var context = new MyDbContext())
            {
                Console.WriteLine("Ange antal att lägga till: ");
                if (int.TryParse(Console.ReadLine(), out int addStock))
                {
                    var product = context.Products.FirstOrDefault(p => p.Id == productId);
                    if (product != null)
                    {
                        product.Stock += addStock;
                        context.SaveChanges();
                        Console.WriteLine("Lager för " + product.Name + " har uppdaterats.");
                    }
                    else
                    {
                        Console.WriteLine("Produkt med det angivna ID:t hittades inte.");
                    }
                }
                else
                {
                    Console.WriteLine("Ogiltig inmatning. Ange ett giltigt antal.");
                }
            }
        }
        public static void UppdateProduct()
        {

        }
        public static void AddNewProduct()
        {
            using (var context = new MyDbContext())
            {
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("Ange namn på produkten: ");
                string addName = Console.ReadLine();
                if (addName == "0")
                {
                    return;
                }
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("Ange info på produkten: ");
                string addInfo = Console.ReadLine();
                if (addInfo == "0")
                {
                    return;
                }
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("Ange pris på produkten: ");
                string inputPrice = Console.ReadLine();
                if (inputPrice == "0")
                {
                    return;
                }
                decimal addPrice = 0;

                if (int.TryParse(inputPrice, out int intPrice))
                {
                    // Om det är ett heltal, konvertera till decimal
                    addPrice = intPrice;
                }
                else if (decimal.TryParse(inputPrice, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal decimalPrice))
                {
                    // Om det är ett decimalvärde
                    addPrice = decimalPrice;
                }
                else
                {
                    Console.WriteLine("Ogiltigt pris. Ange ett giltigt tal.");
                    Thread.Sleep(2000);
                    return;
                }
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("Ange saldo på produkten: ");
                string inputStock = Console.ReadLine();
                if (inputStock == "0")
                {
                    return;
                }

                int addStock = 0;

                if (!int.TryParse(inputStock, out addStock))
                {
                    Console.WriteLine("Ogiltigt antal. Ange ett giltigt heltal.");
                    return;
                }

                // Skapa ny produkt
                var newProduct = new Product()
                {
                    Name = addName,
                    ProductInfo = addInfo,
                    Price = addPrice,
                    Stock = addStock,
                };


                // Hämta alla kategorier från databasen
                var allCategories = context.Categories.OrderBy(c => c.Name).ToList();
                if (!allCategories.Any())
                {
                    Console.WriteLine("Det finns inga kategorier att välja från. Lägg till kategorier först.");
                    return;
                }


                // Lista för valda kategorier
                var selectedCategories = new List<Category>();

                bool continueSelection = true;
                while (continueSelection)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("--------------------------------------------");
                    Console.WriteLine("Välj kategori till produkten: ");
                    Console.WriteLine("--------------------------------------------");
                    Console.ResetColor();

                    int i = 1;
                    foreach (var category in allCategories)
                    {
                        string isSelected = selectedCategories.Contains(category) ? " (vald)" : "";
                        Console.WriteLine("[" + i + "] " + category.Name + isSelected);
                        i++;
                    }
                    Console.WriteLine("--------------------------------------------");
                    Console.WriteLine("[0] Avbryt");
                    Console.WriteLine("[S] Skapa produkten");
                    Console.WriteLine("--------------------------------------------");

                    // Vänta på tangenttryckning från användaren
                    var key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.D0)
                    {
                        return;
                    }

                    if (key == ConsoleKey.S) // Skapa produkten
                    {
                        if (!selectedCategories.Any())
                        {
                            Console.WriteLine("Ingen kategori har valts. Lägg till minst en kategori innan du skapar produkten.");
                            Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
                            Thread.Sleep(2000);
                            continue;
                        }

                        // Lägg till valda kategorier till produkten
                        foreach (var category in selectedCategories)
                        {
                            newProduct.Categories.Add(category);
                        }

                        context.Products.Add(newProduct);
                        context.SaveChanges();
                        Console.WriteLine("Produkten " + newProduct.Name + " har skapats.");
                        Thread.Sleep(2000);
                        return;
                    }

                    // Kontrollera om en kategori trycks
                    int selectedCategoryIndex = -1;
                    if (key >= ConsoleKey.D1 && key <= ConsoleKey.D9) // För kategorier 1-9
                    {
                        selectedCategoryIndex = key - ConsoleKey.D1;
                    }

                    if (selectedCategoryIndex >= 0 && selectedCategoryIndex < allCategories.Count)
                    {
                        var chosenCategory = allCategories[selectedCategoryIndex];

                        if (selectedCategories.Contains(chosenCategory))
                        {
                            selectedCategories.Remove(chosenCategory); // Ta bort kategori om den redan är vald
                            Console.WriteLine("Kategorin " + chosenCategory.Name + " har tagits bort.");
                        }
                        else
                        {
                            selectedCategories.Add(chosenCategory); // Lägg till kategori om den inte är vald
                            Console.WriteLine("Kategorin " + chosenCategory.Name + " har lagts till.");
                        }
                    }
                    Thread.Sleep(2000);
                }
            }
        }
    }
}