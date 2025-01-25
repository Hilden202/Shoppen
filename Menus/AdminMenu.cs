using System;
using ConsoleShoppen.Data;
using ConsoleShoppen.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleShoppen.Menus
{
    public class AdminMenu
    {
        public static async Task AMenuAsync()
        {
            ICollection<Product> allProducts;
            ICollection<Category> allCategories;

            using (var myDb = new MyDbContext())
            {
                allProducts = myDb.Products.ToList();
                allCategories = myDb.Categories.ToList();
            }

            bool loop = true;

            while (loop)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("------------------------------------");
                Console.WriteLine("|            Admin meny             |");
                Console.WriteLine("------------------------------------");
                Console.ResetColor();

                var menuNumbersSorted = Enum.GetValues(typeof(Models.AdminMenu))
                          .Cast<int>()
                          .OrderBy(menuNumber => menuNumber == 0 ? int.MaxValue : menuNumber)
                          .ToList();

                foreach (int menuButton in menuNumbersSorted)
                {
                    Console.WriteLine("[" + menuButton + "] " + Enum.GetName(typeof(Models.AdminMenu), menuButton).Replace('_', ' '));
                }


                if (int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out int nr))
                {
                    switch ((Models.AdminMenu)nr)
                    {
                        case Models.AdminMenu.Startsida:
                            Console.Clear();
                            await StartMenu.SMenuAsync();

                            break;

                        case Models.AdminMenu.Visa_lagersaldo:
                            Console.Clear();
                            await StockBalance.RunAsync(allProducts);
                            break;

                        case Models.AdminMenu.Lägg_till_produkt:
                            Console.Clear();
                            AddProduct.AddProductMenu();
                            break;

                        case Models.AdminMenu.Ändra_Produkt:
                            Console.Clear();
                            await UpdateProduct.RunAsync(allProducts);
                            break;

                        case Models.AdminMenu.Ta_bort_produkt:
                            Console.Clear();
                            // Todo
                            break;

                        case Models.AdminMenu.Beställningshistorik:
                            Console.Clear();
                            // Todo
                            break;

                        case Models.AdminMenu.Bäst_Säljande_produkter:
                            Console.Clear();
                            // Todo
                            break;

                        case Models.AdminMenu.Välj_topp_produkt:
                            Console.Clear();
                            // Todo
                            break;

                        case Models.AdminMenu.Logga_ut:
                            loop = false;
                            break;
                    }
                }
            }
        }
    }
}