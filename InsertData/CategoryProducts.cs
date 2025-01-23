using System;
using ConsoleShoppen.Models;

namespace ConsoleShoppen.InsertData
{
    public class CategoryProducts
    {
        public static void Run()
        {
            using (var myDb = new MyDbContext())
            {

                var category1 = new Category { Name = "Elektronik" };
                var category2 = new Category { Name = "Gaming" };
                var category3 = new Category { Name = "Smartphones" };

                myDb.AddRange(

                        new Product
                        {
                            Name = "Gaming Laptop",
                            ProductInfo = "Prestanda och Stil i Ett",
                            Price = 1500,
                            Stock = 50,
                            Categories = new List<Category> { category2, category1 }
                        },
                        new Product
                        {
                            Name = "Kylskåp",
                            ProductInfo = "Energisnålt och Rymligt",
                            Price = 5000,
                            Stock = 15,
                            Categories = new List<Category> { category1 }
                        },
                        new Product
                        {
                            Name = "Gaming Musmatta",
                            ProductInfo = "Precision och Komfort",
                            Price = 300,
                            Stock = 40,
                            Categories = new List<Category> { category2 }
                        },
                        new Product
                        {
                            Name = "Mobil hållare till bilen",
                            ProductInfo = "Enkel och Praktisk",
                            Price = 150,
                            Stock = 75,
                            Categories = new List<Category> { category3 }
                        },
                        new Product
                        {
                            Name = "Iphone 20 Pro",
                            ProductInfo = "Innovativ Teknologi i Din Hand",
                            Price = 20000,
                            Stock = 10,
                            Categories = new List<Category> { category1, category3 }
                        });

                myDb.SaveChanges();

                // ---------------------------------------------------------------------

                // En uppdate. klar

                //// Hämta alla produkter från databasen
                //var products = myDb.Products.ToList();

                //// Uppdatera priset för varje produkt
                //foreach (var product in products)
                //{
                //    if (product.Name == "Gaming Laptop")
                //        product.Price = 15000;
                //    else if (product.Name == "Kylskåp")
                //        product.Price = 5000;
                //    else if (product.Name == "Gaming Musmatta")
                //        product.Price = 300;
                //    else if (product.Name == "Mobil hållare till bilen")
                //        product.Price = 150;
                //    else if (product.Name == "Iphone 20 Pro")
                //        product.Price = 20000;
                //}

                //myDb.SaveChanges();
            }
            }
        }
    }

