using System;
using ConsoleShoppen.Data;
using ConsoleShoppen.Models;

namespace ConsoleShoppen.Menus
{
    public class StartMenu
    {
        public static async Task SMenuAsync()
        {
            bool loop = true;

            while (loop)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("------------------------------------");
                Console.WriteLine("|     Välkommen till Elbutiken     |");
                Console.WriteLine("|       Allt inom Elektronik       |");
                Console.WriteLine("------------------------------------");
                Console.ResetColor();

                var menuNumbersSorted = Enum.GetValues(typeof(Models.StartMenu))
                          .Cast<int>()
                          .OrderBy(menuNumber => menuNumber == 0 ? int.MaxValue : menuNumber)
                          .ToList();

                foreach (int menuButton in menuNumbersSorted)
                {
                    Console.WriteLine("[" + menuButton + "] " + Enum.GetName(typeof(Models.StartMenu), menuButton).Replace('_', ' '));
                }

                Console.WriteLine();

                Top3Products.LoadTop3Products();
                Top3Products.ShowTop3Products();

                if (int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out int nr))
                {
                    switch ((Models.StartMenu)nr)
                    {
                        case Models.StartMenu.Besök_som_kund:
                            Console.Clear();
                            // Ladda produktlistan & categorilidtan när du loggar in som kund
                            // behövde läggas till även för att updatera om tex ny produkt skapats i samma körning.
                            await ProductList.LoadProductListAsync(); 
                            await CategoryList.LoadCategoryListAsync();
                            await CustomerMenu.CMenuAsync();
                            break;

                        case Models.StartMenu.Logga_in_som_Admin:
                            //Console.Clear();
                            if (AdminLogin.ShowLoginWindow() == true)
                            {
                            await AdminMenu.AMenuAsync();
                            }
                            else
                            {
                                Console.WriteLine("\n\n  Fel lösenord...");
                                Thread.Sleep(2000);
                            }
                            break;

                        case Models.StartMenu.Avsluta:
                            loop = false;
                            break;
                    }
                }
            }
        }
    }
}