using System;
using System.Collections.Generic;
using ConsoleShoppen.Data;
using ConsoleShoppen.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleShoppen.Menus
{
    public class ShopMenu
    {
        public static async Task SMenuAsync()
        {
            bool loop = true;

            while (loop)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("------------------------------------");
                Console.WriteLine("|             Shoppen              |");
                Console.WriteLine("------------------------------------");
                Console.ResetColor();

                var menuNumbersSorted = Enum.GetValues(typeof(Models.ShopMenu))
                          .Cast<int>()
                          .OrderBy(menuNumber => menuNumber == 0 ? int.MaxValue : menuNumber)
                          .ToList();

                foreach (int menuButton in menuNumbersSorted)
                {
                    Console.WriteLine("[" + menuButton + "] " + Enum.GetName(typeof(Models.ShopMenu), menuButton).Replace('_', ' '));
                }

                if (int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out int nr))
                {
                    switch ((Models.ShopMenu)nr)
                    {
                        case Models.ShopMenu.Sök:
                            Console.Clear();
                            Data.SearchProductDapper.ShearchProduct();
                            break;

                        case Models.ShopMenu.Kategori:
                            Console.Clear();
                            await CategoryList.RunAsync();
                            break;

                        case Models.ShopMenu.Produkt_lista:
                            Console.Clear();
                            await ProductList.RunAsync();
                            break;

                        case Models.ShopMenu.Tillbaka:
                            loop = false;
                            break;
                    }
                }
            }
        }
    }
}