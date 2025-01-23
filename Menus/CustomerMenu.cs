using System;
using ConsoleShoppen.Data;
using ConsoleShoppen.Models;

namespace ConsoleShoppen.Menus
{
    public class CustomerMenu
    {
        public static async Task CMenuAsync()
        {

            bool loop = true;

            while (loop)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("------------------------------------");
                Console.WriteLine("|             Kund meny            |");
                Console.WriteLine("------------------------------------");
                Console.ResetColor();

                var menuNumbersSorted = Enum.GetValues(typeof(Models.CustomerMenu))
                          .Cast<int>()
                          .OrderBy(menuNumber => menuNumber == 0 ? int.MaxValue : menuNumber)
                          .ToList();

                foreach (int menuButton in menuNumbersSorted)
                {
                    Console.WriteLine("[" + menuButton + "] " + Enum.GetName(typeof(Models.CustomerMenu), menuButton).Replace('_', ' '));
                }


                if (int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out int nr))
                {
                    switch ((Models.CustomerMenu)nr)
                    {
                        case Models.CustomerMenu.Startsida:
                            Console.Clear();
                            await StartMenu.SMenuAsync();

                            break;

                        case Models.CustomerMenu.Shoppen:
                            Console.Clear();
                            await ShopMenu.SMenuAsync();

                            break;

                        case Models.CustomerMenu.Varukorgen:
                            Console.Clear();
                            Order.OrderCheckout();
                            break;

                        case Models.CustomerMenu.Logga_ut:
                            loop = false;
                            break;
                    }
                }
            }
        }
    }
}