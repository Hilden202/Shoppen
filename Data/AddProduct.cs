using System;
using ConsoleShoppen.Models;

namespace ConsoleShoppen.Data
{
    public class AddProduct
    {
        public static void AddProductMenu()
        {
            using (var myDb = new MyDbContext())
            {
                bool loop = true;

                while (loop)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("--------------------------------------------");
                    Console.WriteLine("|           Lägg till Produkt              |");
                    Console.WriteLine("--------------------------------------------");
                    Console.ResetColor();

                    Console.WriteLine("[1] Lägg till en ny produkt");
                    Console.WriteLine("[0] Avbryt");

                    if (!int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out int nr))
                    {
                        Console.WriteLine("Ogiltig inmatning, vänligen välj ett giltigt nummer.");
                        Thread.Sleep(2000);
                        Console.Clear();
                        continue;
                    }
                    if (nr == 0)
                    {
                        loop = false;
                    }
                    if (nr == 1)
                    {
                        Product.AddNewProduct();
                        Thread.Sleep(200);
                        Console.Clear();
                        break;

                    }
                }

            }

        }
    }
}

