using System;
using ConsoleShoppen.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace ConsoleShoppen.Data
{
    public class SearchProductDapper
    {
        static string connString = "Server=tcp:patrikdb.database.windows.net,1433;Initial Catalog=patrikdb;"
        + "Persist Security Info=False;User ID=patrik;Password=!Q2w3e4r;MultipleActiveResultSets=False;Encrypt=True;"
        + "TrustServerCertificate=False;Connection Timeout=30;";

        public static List<Models.Product> GetProductsByName(string searchText)
        {
            string sql = "SELECT * FROM Products WHERE Name LIKE '%' + @SearchText + '%'";
            List<Models.Product> products = new List<Models.Product>();

            using (var connection = new SqlConnection(connString))
            {
                products = connection.Query<Models.Product>(sql, new { SearchText = searchText }).ToList();
            }
            return products;
        }
        public static Models.Product GetProductById(int productId)
        {
            string sql = "SELECT * FROM Products WHERE Id = @ProductId";

            using (var connection = new SqlConnection(connString))
            {
                return connection.QuerySingleOrDefault<Models.Product>(sql, new { ProductId = productId });
            }

        }
        public static void ShearchProduct()
        {

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("|               Sök Produkt                |");
                Console.WriteLine("--------------------------------------------");
                Console.ResetColor();

                Console.WriteLine("[0] Tillbaka");
                Console.WriteLine("--------------------------------------------");
                Console.Write("Sök:");

                string input = string.Empty;

                while (true)
                {
                    var key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.Backspace)
                    {
                        if (input.Length > 0)
                        {
                            input = input.Substring(0, input.Length - 1);
                            Console.Write("\b \b");
                        }
                    }
                    else if (key.Key == ConsoleKey.D0)
                    {
                        Console.Clear();
                        return;
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine();
                        break;
                    }
                    else if (!char.IsControl(key.KeyChar))
                    {
                        input += key.KeyChar;
                        Console.Write(key.KeyChar);
                    }
                }
                // Ta bort denna om man vill att alla produkter ska visas vid null inmatning
                if (string.IsNullOrEmpty(input))
                {
                    Console.Clear();
                    continue;
                }

                var matchingProducts = Data.SearchProductDapper.GetProductsByName(input);

                if (matchingProducts.Count > 0)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("--------------------------------------------");
                    Console.WriteLine("|     Förslag baserat på din inmatning     |");
                    Console.WriteLine("--------------------------------------------");
                    Console.ResetColor();
                    int i = 1;
                    foreach (var product in matchingProducts)
                    {
                        Console.WriteLine("[" + i + "] " + product.Name?.PadRight(30) + product.Price.GetValueOrDefault().ToString("0.##") + "kr");
                        i++;
                    }
                    Console.WriteLine("[0] Tillbaka");
                    Console.WriteLine("--------------------------------------------");
                    Console.WriteLine("Välj en produkt: ");

                    if (!int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out int nr))
                    {
                        Console.WriteLine("Ogiltig inmatning, vänligen välj ett giltigt nummer.");
                        Thread.Sleep(2000);
                        Console.Clear();
                        continue;
                    }
                    if (nr == 0)
                    {
                        Console.Clear();
                        continue;
                    }
                    else if (nr > 0 && nr <= matchingProducts.Count)
                    {
                        var selectedProduct = Data.SearchProductDapper.GetProductById(matchingProducts[nr - 1].Id);

                        if (selectedProduct != null)
                        {
                            Console.WriteLine("--------------------------------------------");
                            Console.WriteLine("Produkt: " + selectedProduct.Name);
                            Console.WriteLine("Om produkten: " + selectedProduct.ProductInfo);
                            Console.WriteLine("Pris: " + selectedProduct.Price.GetValueOrDefault().ToString("0.##") + "kr");
                            Console.WriteLine("--------------------------------------------");

                            Console.WriteLine("[1] Lägg till i varukorg");
                            Console.WriteLine("[0] Tillbaka: ");

                            if (!int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out int nr2))
                            {
                                Console.WriteLine("Ogiltig inmatning, vänligen välj ett giltigt nummer.");
                                Thread.Sleep(2000);
                                Console.Clear();
                                continue;
                            }
                            if (nr2 == 0)
                            {
                                Console.Clear();
                                continue;
                            }
                            if (nr2 == 1)
                            {
                                // Lägg till i kundvagn --->
                                Helpers.AddToCart(selectedProduct);
                                Thread.Sleep(2000);
                                Console.Clear();
                            }
                            else
                            {
                                Console.WriteLine("Ogiltigt val, vänligen välj ett giltigt alternativ.");
                                Thread.Sleep(2000);
                                Console.Clear();
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Produkt med ID " + nr + " hittades inte.");
                        Thread.Sleep(2000);
                        Console.Clear();
                    }
                }

                }
            }
        }
    }