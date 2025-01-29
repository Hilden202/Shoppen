using System;
using ConsoleShoppen.Data;
using ConsoleShoppen.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace ConsoleShoppen.Menus
{
    public class AdminMenu
    {
        public static async Task AMenuAsync()
        {
            ICollection<Product> allProducts;

            using (var myDb = new MyDbContext())
            {
                // Inkludera relaterade kategorier
                allProducts = myDb.Products
                                  .Include(p => p.Categories)
                                  .ToList();
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
                    Console.ResetColor();
                    switch ((Models.AdminMenu)menuButton)
                    {
                        case Models.AdminMenu.Startsida:
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                        case Models.AdminMenu.Visa_lagersaldo:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        case Models.AdminMenu.Produkt_hantering:
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                        case Models.AdminMenu.Statistik:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        case Models.AdminMenu.Välj_topp_3_produkt:
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                        case Models.AdminMenu.Redigera_order_information:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        case Models.AdminMenu.Logga_ut:
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                    }

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

                        case Models.AdminMenu.Produkt_hantering:
                            Console.Clear();
                            await ProductEditMenu.PMenuAsync();
                            break;

                        case Models.AdminMenu.Statistik:
                            Console.Clear();
                            await DataStatisticsMenu.StatMenuAsync();
                            break;

                        case Models.AdminMenu.Välj_topp_3_produkt:
                            Console.Clear();
                            Top3Products.SetTop3Products(allProducts.ToList());
                            break;
                        case Models.AdminMenu.Redigera_order_information:
                            Console.Clear();
                            //Console.ForegroundColor = ConsoleColor.Yellow;
                            //Console.WriteLine("------------------------------------");
                            //Console.WriteLine("|          Redigera Order          |");
                            //Console.WriteLine("------------------------------------");
                            //Console.ResetColor();

                            //// Hämta alla ordrar från MongoDB
                            //var orders = MongoDbConnection.GetShippingCollection().Find(FilterDefinition<ShippingInfo>.Empty).ToList();

                            //if (orders.Any())
                            //{
                            //    int i = 1;
                            //    foreach (var order in orders)
                            //    {
                            //        Console.WriteLine("[" + i + "] Ordernummer: " + order.CartId + ", Kund: " + order.FullName);
                            //        i++;
                            //    }

                            //    Console.WriteLine("Välj en order att ändra:");
                            //    Console.WriteLine("------------------------------------");

                            //    int selectedOrderIndex = 0;

                            //    // Hämta input och försäkra oss om att den är ett giltigt index
                            //    if (int.TryParse(Console.ReadLine(), out selectedOrderIndex) && selectedOrderIndex > 0 && selectedOrderIndex <= orders.Count)
                            //    {
                            //        var selectedOrder = orders[selectedOrderIndex - 1]; // Hämta den valda ordern (indexen är 0-baserade)
                            //        CustomerDataManager.EditOrderDetails(selectedOrder); // Uppdatera detaljer för vald order
                            //    }
                            //    else
                            //    {
                            //        Console.WriteLine("Ogiltigt val, vänligen välj ett giltigt indexnummer.");
                            //    }
                            //}
                            //else
                            //{
                            //    Console.WriteLine("Inga ordrar finns i systemet.");
                            //}
                            CustomerDataManager.EditOrderDetails();
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