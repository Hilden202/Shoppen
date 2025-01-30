using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleShoppen.Models;
using ConsoleShoppen.Data;
using Microsoft.EntityFrameworkCore;
using WindowDemo;

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
                Console.WriteLine("┌-─-──────────────────────────────────────-┐");
                Console.WriteLine("│          Välj Topp 3 Produkter           │");
                Console.WriteLine("└-────────────────────────────────────────-┘");
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
                    return;
                }

                if (key >= ConsoleKey.D1 && key <= ConsoleKey.D9)
                {
                    int choice = key - ConsoleKey.D0;

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
                                Thread.Sleep(2000);
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
            using (var context = new MyDbContext())
            {
                // Hämta de valda produkterna från SelectedProduct
                top3Products = context.SelectedProducts
                    .Where(sp => sp.IsFeatured == true)
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
                        IsFeatured = selectedProduct.IsFeatured
                    });
                }

                context.SaveChanges();
            }
        }

        public static void ShowTop3Products()
        {

            Lowest.LowestPosition = 0; // Återställ LowestPosition till 0

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
                using (var context = new MyDbContext())
                {
                    // Lista för att lagra textinnehåll som ska visas i fönstren
                    var topText1 = new List<string>();
                    var topText2 = new List<string>();
                    var topText3 = new List<string>();

                    // Hämta produkterna och förbered innehållet för varje fönster
                    for (int i = 0; i < top3Products.Count; i++)
                    {
                        var selectedProduct = top3Products[i];
                        var product = context.Products.FirstOrDefault(p => p.Id == selectedProduct.ProductId);

                        if (product != null)
                        {
                            // Lägg till produktinformation till textlistorna
                            var productDetails = new List<string>
                    {
                        "Namn: " + product.Name,
                        "Pris: " + product.Price.Value.ToString("0.##") + " kr",
                        "Info: " + product.ProductInfo
                    };

                            if (i == 0) topText1.AddRange(productDetails);
                            if (i == 1) topText2.AddRange(productDetails);
                            if (i == 2) topText3.AddRange(productDetails);
                        }
                    }

                    // Standardmarginal mellan fönster
                    int margin = 5;

                    // Bredd för varje fönster (med fallback om listan är tom)
                    int windowWidth1 = topText1.Any() ? topText1.Max(line => line.Length) : 0;
                    int windowWidth2 = topText2.Any() ? topText2.Max(line => line.Length) : 0;
                    int windowWidth3 = topText3.Any() ? topText3.Max(line => line.Length) : 0;

                    // Startposition för fönster
                    int currentLeft = 0;

                    // Rita fönster 1 om det finns innehåll
                    if (topText1.Any())
                    {
                        var windowTop1 = new Window("Erbjudande 1", currentLeft, 10, topText1);
                        windowTop1.Draw();
                        currentLeft += windowWidth1 + margin;
                    }

                    // Rita fönster 2 om det finns innehåll
                    if (topText2.Any())
                    {
                        var windowTop2 = new Window("Erbjudande 2", currentLeft, 10, topText2);
                        windowTop2.Draw();
                        currentLeft += windowWidth2 + margin;
                    }

                    // Rita fönster 3 om det finns innehåll
                    if (topText3.Any())
                    {
                        var windowTop3 = new Window("Erbjudande 3", currentLeft, 10, topText3);
                        windowTop3.Draw();
                    }
                }
            }
        }
    }
}