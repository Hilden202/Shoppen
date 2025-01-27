using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleShoppen.Models;
using ConsoleShoppen.Data;
using Microsoft.EntityFrameworkCore;

namespace ConsoleShoppen.Data
{
    public class Top3Products
    {
        private static List<SelectedProduct> top3Products = new List<SelectedProduct>();

        public static void SetTop3Products(List<Product> availableProducts)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("|         Välj Topp 3 Produkter            |");
                Console.WriteLine("--------------------------------------------");
                Console.ResetColor();

                // Ladda de tre valda produkterna från databasen om de inte finns i den statiska listan
                LoadTop3Products();

                // Skapa en lista av ProductIds från SelectedProduct för att jämföra
                var selectedProductIds = top3Products.Select(p => p.ProductId).ToList();

                // Skriv ut alla produkter och visa om de är valda eller inte
                int i = 1;
                foreach (var product in availableProducts)
                {
                    string isSelected = selectedProductIds.Contains(product.Id) ? " (vald)" : ""; // Kontrollera om produktens Id finns i listan med valda produkter
                    Console.WriteLine("[" + i + "] " + product.Name.PadRight(33) + isSelected);
                    i++;
                }

                Console.WriteLine("[0] Avbryt");
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("Välj produkt att lägga till eller ta bort:");

                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.D0)
                {
                    // Avbryt och återvänd till föregående meny
                    return;
                }

                // Hantera val av produkt direkt (om det är ett nummer mellan 1 och tillgängliga produkter)
                if (key >= ConsoleKey.D1 && key <= ConsoleKey.D9)
                {
                    int choice = key - ConsoleKey.D0;  // Om tangenttrycket är mellan 1-9, hämta det som ett val

                    if (choice >= 1 && choice <= availableProducts.Count)
                    {
                        var selectedProduct = availableProducts[choice - 1];

                        // Om produkten redan är vald, ta bort den från listan, annars lägg till den
                        if (selectedProductIds.Contains(selectedProduct.Id))
                        {
                            // Ta bort från top3Products
                            var selected = top3Products.FirstOrDefault(p => p.ProductId == selectedProduct.Id);
                            if (selected != null)
                            {
                                top3Products.Remove(selected);
                                Console.WriteLine(selectedProduct.Name + " har tagits bort från topp 3.");
                            }
                        }
                        else
                        {
                            if (top3Products.Count < 3)
                            {
                                // Lägg till i top3Products
                                top3Products.Add(new SelectedProduct { ProductId = selectedProduct.Id, IsFeatured = true });
                                Console.WriteLine(selectedProduct.Name + " har lagts till i topp 3.");
                            }
                            else
                            {
                                Console.WriteLine("Du kan bara välja 3 produkter.");
                            }
                        }

                        // Uppdatera databasen med valda produkter
                        UpdateSelectedProductsInDb();
                    }
                }
            }
        }

        public static void LoadTop3Products()
        {
            // Ladda de valda produkterna från databasen
            using (var context = new MyDbContext())
            {
                // Hämta de valda produkterna från SelectedProduct
                top3Products = context.SelectedProducts
                    .Where(sp => sp.IsFeatured == true) // Endast de som är "valda" (IsFeature = true)
                    .ToList();
            }
        }

        public static void UpdateSelectedProductsInDb()
        {
            using (var context = new MyDbContext())
            {
                // Ta bort alla tidigare valda produkter från databasen
                var existingSelectedProducts = context.SelectedProducts.ToList();
                context.SelectedProducts.RemoveRange(existingSelectedProducts);

                // Lägg till de nya valda produkterna
                foreach (var selectedProduct in top3Products)
                {
                    context.SelectedProducts.Add(new SelectedProduct
                    {
                        ProductId = selectedProduct.ProductId,
                        IsFeatured = selectedProduct.IsFeatured // Märk som vald
                    });
                }

                // Spara ändringarna i databasen
                context.SaveChanges();
            }
        }
        // Funktion för att visa de tre topprodukterna på startsidan
        public static void ShowTop3Products()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Våra Utvalda Favoriter:");
            Console.ResetColor();

            // Kontrollera om det finns valda produkter
            if (top3Products.Count == 0)
            {
                Console.WriteLine("Inga topprodukter valda ännu.");
            }
            else
            {
                // Här definieras kolumnerna som ska visas för varje produkt
                int columnWidth = 30;  // Bredden på varje produktkolumn
                int margin = 3;        // Avstånd mellan kolumnerna

                // Beräkna var produkterna ska börja skrivas ut
                int startX = Console.CursorLeft;
                int startY = Console.CursorTop;
                // Skapa en instans av DbContext för att hämta produkter
                using (var context = new MyDbContext())
                {
                    // Skriv ut de tre produkterna bredvid varandra
                    for (int i = 0; i < top3Products.Count; i++)
                    {
                        var selectedProduct = top3Products[i];

                        // Hämta den relaterade produkten
                        var product = context.Products.FirstOrDefault(p => p.Id == selectedProduct.ProductId);
                        if (product != null)
                        {
                            // Beräkna varje kolumns startposition (för att skriva bredvid varandra)
                            int columnStartX = startX + (columnWidth + margin) * i;

                            // Inramad produkt med detaljer
                            WriteProductInFrame(columnStartX, startY, product);
                        }
                    }
                }
            }
        }

        // Hjälpmetod för att skriva ut en produkt inramad
        private static void WriteProductInFrame(int x, int y, Product product)
        {
            Console.SetCursorPosition(x, y);
            Console.WriteLine("+-------------------------------+");
            Console.SetCursorPosition(x, y + 1);
            Console.WriteLine("| " + product.Name.PadRight(29) + "|");
            Console.SetCursorPosition(x, y + 2);
            Console.WriteLine("| Pris: " + product.Price.Value.ToString("0.##").PadLeft(8) + " kr |");
            Console.SetCursorPosition(x, y + 3);
            Console.WriteLine("| Info: " + product.ProductInfo.PadRight(22) + "|");
            Console.SetCursorPosition(x, y + 4);
            Console.WriteLine("+-------------------------------+");
        }
    }
}