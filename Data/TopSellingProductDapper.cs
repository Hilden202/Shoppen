using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ConsoleShoppen.Data;
using ConsoleShoppen.Models;
using System.Data.SqlClient;
using Dapper;
using System.Text.RegularExpressions;

namespace ConsoleShoppen.Data
{
    public class TopSellingProductDapper
    {
        static string connString = "Server=tcp:patrikdb.database.windows.net,1433;Initial Catalog=patrikdb;"
       + "Persist Security Info=False;User ID=patrik;Password=!Q2w3e4r;MultipleActiveResultSets=False;Encrypt=True;"
       + "TrustServerCertificate=False;Connection Timeout=30;";


        public static List<TopSellingProduct> GetTopSellingProduct()
        {
            string sql = @"
                SELECT 
                   p.Name AS ProductName,
                   SUM(cp.Quantity) AS [TotalQuantitySold]
                FROM 
                   Carts c
                INNER JOIN 
                   CartProducts cp ON c.Id = cp.CartId
                INNER JOIN 
                   Products p ON cp.ProductId = p.Id
                WHERE 
                   c.Status = 'Completed'
                GROUP BY 
                   p.Name
                ORDER BY 
                   [TotalQuantitySold] DESC";

            List<TopSellingProduct> topSellingProduct = new List<TopSellingProduct>();

            try
            {
                using (var connection = new SqlConnection(connString))
                {
                    topSellingProduct = connection.Query<TopSellingProduct>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ett fel inträffade: " + ex.Message);
                Console.ResetColor();
            }
            return topSellingProduct;
        }


        public static void TopSellingProductMenu()
        {

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("┌───────────────────────────────────────────────────────┐");
                Console.WriteLine("│                Bäst Säljande produkter                │");
                Console.WriteLine("└-─────────────────────────────────────────────────────-┘");
                Console.ResetColor();

                var topSeller = GetTopSellingProduct();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("┌───────────────────────────────────────────────────────┐");
                Console.WriteLine("│ Produkt                │ Totalt antal sålda produkter │");
                Console.WriteLine("└────────────────────────+──────────────────────────────┘");
                Console.ResetColor();

                foreach (var product in topSeller)
                {
                    Console.WriteLine("│" + product.ProductName.ToString().PadRight(24) +
                  "│" + product.TotalQuantitySold.ToString("0 kr").PadRight(30) + "│");

                    Console.WriteLine("+────────────────────────+──────────────────────────────+");
                }

                Console.WriteLine("[0] Tillbaka");

                if (!int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out int chosenProduct))
                {
                    Console.WriteLine("Ogiltig inmatning, vänligen välj ett giltigt nummer.");
                    Thread.Sleep(2000);
                    Console.Clear();
                    continue;
                }
                if (chosenProduct == 0)
                {
                    Console.Clear();
                    return;
                }

            }
        }
    }
}



