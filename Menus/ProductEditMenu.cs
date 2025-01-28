using System;
using ConsoleShoppen.Data;
using ConsoleShoppen.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleShoppen.Menus
{
    public class ProductEditMenu
    {
        public static async Task PMenuAsync()
        {
            ICollection<Product> allProducts;
            ICollection<Category> allCategories;

            using (var myDb = new MyDbContext())
            {
                // Inkludera relaterade kategorier
                allProducts = myDb.Products
                                  .Include(p => p.Categories)
                                  .ToList();

                allCategories = myDb.Categories.ToList();
            }

            bool loop = true;

            while (loop)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("------------------------------------");
                Console.WriteLine("|     Produkt redigerings meny      |");
                Console.WriteLine("------------------------------------");
                Console.ResetColor();

                var menuNumbersSorted = Enum.GetValues(typeof(Models.ProductEditMenu))
                          .Cast<int>()
                          .OrderBy(menuNumber => menuNumber == 0 ? int.MaxValue : menuNumber)
                          .ToList();

                foreach (int menuButton in menuNumbersSorted)
                {
                    Console.ResetColor();
                    switch ((Models.ProductEditMenu)menuButton)
                    {
                        case Models.ProductEditMenu.Lägg_till_produkt:
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case Models.ProductEditMenu.Lägg_till_produktkategori:
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            break;
                        case Models.ProductEditMenu.Ändra_Produkt:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        case Models.ProductEditMenu.Ta_bort_produkt:
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case Models.ProductEditMenu.Tillbaka:
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                    }

                    Console.WriteLine("[" + menuButton + "] " + Enum.GetName(typeof(Models.ProductEditMenu), menuButton).Replace('_', ' '));
                    Console.WriteLine();
                }

                if (int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out int nr))
                {
                    switch ((Models.ProductEditMenu)nr)
                    {

                        case Models.ProductEditMenu.Lägg_till_produkt:
                            Console.Clear();
                            AddProduct.AddProductMenu();
                            break;

                        case Models.ProductEditMenu.Lägg_till_produktkategori:
                            Console.Clear();
                            AddCategory.AddCategoryMenu();
                            break;

                        case Models.ProductEditMenu.Ändra_Produkt:
                            Console.Clear();
                            await UpdateProduct.RunAsync(allProducts);
                            break;

                        case Models.ProductEditMenu.Ta_bort_produkt:
                            Console.Clear();
                            await DeleteProduct.RunAsync(allProducts);
                            break;

                        case Models.ProductEditMenu.Tillbaka:
                            loop = false;
                            break;
                    }
                }
            }
        }
    }
}