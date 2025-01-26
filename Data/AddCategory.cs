using System;
using ConsoleShoppen.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleShoppen.Data
{
    public class AddCategory
    {
        public static void AddCategoryMenu()
        {
            using (var myDb = new MyDbContext())
            {
                var allCategories = myDb.Categories.OrderBy(c => c.Id).ToList();

                bool loop = true;

                while (loop)
                {

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("--------------------------------------------");
                    Console.WriteLine("|         Lägg till Produktkategori        |");
                    Console.WriteLine("--------------------------------------------");
                    Console.ResetColor();

                    Console.WriteLine("Totalt " + allCategories.Count + " kategorier i listan.");
                    Console.WriteLine("--------------------------------------------");
                    // Hämta alla kategorier från databasen
                    int i = 1;
                    foreach (var category in allCategories)
                    {
                        string isHidden = category.IsHidden ? " (Dold)" : "";
                        Console.WriteLine("[" + i + "] " + category.Name.PadRight(30) + isHidden);
                        i++;
                    }
                    Console.WriteLine("[" + i + "] Lägg till en ny Kategori: ");
                    Console.WriteLine("[0] Tillbaka");
                    Console.WriteLine("--------------------------------------------");
                    Console.WriteLine("Välj kategori att visa/dölja");

                    if (!int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out int newCategory))
                    {
                        Console.WriteLine("Ogiltig inmatning, vänligen välj ett giltigt nummer.");
                        Thread.Sleep(2000);
                        Console.Clear();
                        continue;
                    }
                    if (newCategory == 0)
                    {
                        loop = false;
                    }
                    if (newCategory == i)
                    {
                        Category.AddNewCategory();
                        Thread.Sleep(200);
                        Console.Clear();
                        continue;

                    }
                    else if (newCategory > 0 && newCategory < i)
                    {
                        var selectedCategory = allCategories.FirstOrDefault(c => c.Id == newCategory);

                        if (selectedCategory != null)
                        {
                            selectedCategory.IsHidden = !selectedCategory.IsHidden; // Toggla värdet mellan visa/dold
                            myDb.Categories.Update(selectedCategory);
                            myDb.SaveChanges();
                            Console.WriteLine("Kategorin " + selectedCategory.Name + " har ändrats.");
                            Thread.Sleep(2000);
                            Console.Clear();
                        }

                    }
                }

            }

        }
    }
}


