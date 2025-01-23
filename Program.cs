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

        // Ladda produktlistan asynkront vid start
        await ProductList.LoadProductListAsync();

        //CategoryProducts.Run();
        await Menus.StartMenu.SMenuAsync();
    }
}

