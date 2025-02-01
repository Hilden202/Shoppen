using System;
using ConsoleShoppen.Data;
using ConsoleShoppen.Models;

namespace ConsoleShoppen.Menus
{
    public class DataStatisticsMenu
    {
        public static async Task StatMenuAsync()
        {
            bool loop = true;

            while (loop)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("┌-─-──────────────────────────────-┐");
                Console.WriteLine("│         Statistik Querys         │");
                Console.WriteLine("└-────────────────────────────────-┘");
                Console.ResetColor();

                var menuNumbersSorted = Enum.GetValues(typeof(Models.StatisticsMenu))
                          .Cast<int>()
                          .OrderBy(menuNumber => menuNumber == 0 ? int.MaxValue : menuNumber)
                          .ToList();

                foreach (int menuButton in menuNumbersSorted)
                {
                    Console.ResetColor();
                    switch ((Models.StatisticsMenu)menuButton)
                    {
                        case Models.StatisticsMenu.Beställningshistorik:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        case Models.StatisticsMenu.Bäst_Säljande_produkter:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        case Models.StatisticsMenu.Tillbaka:
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                    }

                    Console.WriteLine("[" + menuButton + "] " + Enum.GetName(typeof(Models.StatisticsMenu), menuButton).Replace('_', ' '));
                }

                if (int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out int nr))
                {
                    switch ((Models.StatisticsMenu)nr)
                    {
                        case Models.StatisticsMenu.Beställningshistorik:
                            Console.Clear();
                            OrderHistoryDapper.OrderHistoryMenu();
                            break;

                        case Models.StatisticsMenu.Bäst_Säljande_produkter:
                            Console.Clear();
                            TopSellingProductDapper.TopSellingProductMenu();
                            break;

                        case Models.StatisticsMenu.Tillbaka:
                            loop = false;
                            break;
                    }
                }
            }
        }
    }
}