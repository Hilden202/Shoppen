using System;
using Microsoft.EntityFrameworkCore;

namespace ConsoleShoppen.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        // Ny egenskap för att dölja kategorier
        public bool IsHidden { get; set; } = false;

        public static void AddNewCategory()
        {
            while (true)
            {
                using (var context = new MyDbContext())
                {
                    Console.WriteLine("--------------------------------------------");
                    Console.WriteLine("Ange ny Kategori: ");
                    string addCategory = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(addCategory))
                    {
                        var newCategory = new Category
                        {
                            Name = addCategory
                        };
                        context.Categories.Add(newCategory);
                        context.SaveChanges();

                        Console.WriteLine("Ny kategori har lagts till: " + newCategory.Name);
                    }
                    else
                    {
                        Console.WriteLine("Ogiltig inmatning. Avbryter..");
                    }
                    break;
                }
            }
        }
    }
}