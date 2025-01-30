using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ConsoleShoppen.Data;
using ConsoleShoppen.Models;
using System.Data.SqlClient;
using Dapper;

namespace ConsoleShoppen.Data
{
    public class OrderHistoryDapper
    {
        static string connString = "Server=tcp:patrikdb.database.windows.net,1433;Initial Catalog=patrikdb;"
       + "Persist Security Info=False;User ID=patrik;Password=!Q2w3e4r;MultipleActiveResultSets=False;Encrypt=True;"
       + "TrustServerCertificate=False;Connection Timeout=30;";


        public static List<OrderHistory> GetOrderHistory()
        {
            string sql = "SELECT " +
                             "c.Id AS OrderId, " +
                             "c.TotalPrice, " +
                             "c.DateCompleted AS OrderDate, " +
                             "c.Status AS OrderStatus, " +
                             "cp.Quantity, " +
                             "p.Name AS ProductName, " +
                             "p.Price AS ProductPrice, " +
                             "(cp.Quantity * p.Price) AS TotalProductPrice " +
                         "FROM " +
                             "Carts c " +
                         "INNER JOIN " +
                             "CartProducts cp ON c.Id = cp.CartId " +
                         "INNER JOIN " +
                             "Products p ON cp.ProductId = p.Id " +
                         "WHERE " +
                             "c.Status = 'Completed' " +
                         "ORDER BY " +
                             "c.DateAdded"; // Plockade bort DESC för det sorterade tvärtom mot vad sql gjorde.

            List<OrderHistory> orderHistory = new List<OrderHistory>();

            try
            {
                using (var connection = new SqlConnection(connString))
                {
                    orderHistory = connection.Query<OrderHistory>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ett fel inträffade: " + ex.Message);
                Console.ResetColor();
            }
            return orderHistory;
        }


        public static void OrderHistoryMenu()
        {

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("┌──────────────────────────────────────────────────────────────┐");
                Console.WriteLine("│                      Beställnings historik                   │");
                Console.WriteLine("└-────────────────────────────────────────────────────────────-┘");
                Console.ResetColor();

                var orders = GetOrderHistory();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("┌────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────┐");
                Console.WriteLine("│ Order ID   │ Totalpris   │ Beställningsdatum │ Orderstatus   │ Produkt                │ Antal    │ Produktpris   │ Totalt produktpris  │");
                Console.WriteLine("└────────────+─────────────+───────────────────+───────────────+────────────────────────+──────────+───────────────+─────────────────────┘");
                Console.ResetColor();

                foreach (var order in orders)
                {
                    Console.WriteLine("│" + order.OrderId.ToString().PadRight(12) +
                  "│" + order.TotalPrice.ToString("0 kr").PadRight(13) +
                  "│" + order.OrderDate.ToString("yyyy-MM-dd").PadRight(19) +
                  "│" + order.OrderStatus.PadRight(15) +
                  "│" + order.ProductName.PadRight(24) +
                  "│" + order.Quantity.ToString().PadRight(10) +
                  "│" + order.ProductPrice.ToString("0 kr").PadRight(15) +
                  "│" + order.TotalProductPrice.ToString("0 kr").PadRight(21) + "│");

                    Console.WriteLine("+────────────+─────────────+───────────────────+───────────────+────────────────────────+──────────+───────────────+─────────────────────+");
                }

                Console.WriteLine("[0] Tillbaka");
                Console.WriteLine("--------------------------------------------");

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


