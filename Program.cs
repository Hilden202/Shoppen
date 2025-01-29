using ConsoleShoppen.Data;
using ConsoleShoppen.InsertData;
using ConsoleShoppen.Models;

namespace ConsoleShoppen;

class Program
{
    static async Task Main(string[] args)
    {
        //// Första gången bara!
        //CategoryProducts.Run();

        // Ladda produktlistan & categorilistan asynkront vid start
        await ProductList.LoadProductListAsync();
        await CategoryList.LoadCategoryListAsync();

        await Menus.StartMenu.SMenuAsync();
    }
}

