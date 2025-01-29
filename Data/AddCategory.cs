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
                    Console.WriteLine("[0] Tillbaka");
                    Console.WriteLine("--------------------------------------------");
                    Console.WriteLine("[k] Lägg till en ny Kategori: ");
                    Console.WriteLine("--------------------------------------------");
                    Console.WriteLine("Välj kategori att visa/dölja");

                    var key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.K)
                    {
                        Category.AddNewCategory();
                        Thread.Sleep(2000);
                        Console.Clear();
                        continue;
                    }
                    if (key == ConsoleKey.D0)
                    {
                        loop = false;
                        continue;
                    }
                    if (key >= ConsoleKey.D1 && key <= ConsoleKey.D9)
                    {
                        int newCategory = (int)(key - ConsoleKey.D0);

                        if (newCategory <= allCategories.Count)
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
}