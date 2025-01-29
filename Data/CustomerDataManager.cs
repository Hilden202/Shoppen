using ConsoleShoppen.Data;
using ConsoleShoppen.Models;
using MongoDB.Driver;

public static class CustomerDataManager
{
    //// Hämta alla fraktinformation baserat på ordernummer
    //public static void ManageOrderDetails(string orderNumber)
    //{
    //    var shippingCollection = MongoDbConnection.GetShippingCollection();

    //    var orders = shippingCollection.Find(order => order.Id == orderNumber).ToList();

    //    if (orders.Any())
    //    {
    //        int index = 1;
    //        Console.WriteLine("Här är alla ordrar som matchar ordernumret:");
    //        foreach (var order in orders)
    //        {
    //            Console.WriteLine(index + "." + order.CartId + "-" + order.FullName + "," + order.Address);
    //            index++;
    //        }

    //        // Låt admin välja vilken order de vill ändra
    //        Console.WriteLine("Välj en order:");
    //        int orderIndex = int.Parse(Console.ReadLine()) - 1;

    //        if (orderIndex >= 0 && orderIndex < orders.Count)
    //        {
    //            var selectedOrder = orders[orderIndex];
    //            EditOrderDetails(selectedOrder);
    //        }
    //        else
    //        {
    //            Console.WriteLine("Ogiltigt val.");
    //        }
    //    }
    //    else
    //    {
    //        Console.WriteLine("Inga ordrar hittades för detta ordernummer.");
    //    }
    //}

    // Låt admin uppdatera fraktinformationen
    public static void EditOrderDetails()
    {
        bool loop = true;

        while (loop)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("|            Redigera Order           |");
            Console.WriteLine("---------------------------------------");
            Console.ResetColor();

            // Hämta alla ordrar från MongoDB
            var orders = MongoDbConnection.GetShippingCollection().Find(FilterDefinition<ShippingInfo>.Empty).ToList();

            if (orders.Any())
            {
                int i = 1;
                foreach (var order in orders)
                {
                    Console.WriteLine("[" + i + "] Order: " + order.CartId + ", Kund: " + order.FullName);
                    i++;
                }
                Console.WriteLine("[0] Tillbaka");
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("Välj en order att ändra:");

                if (!int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out int selectedOrderIndex))
                {
                    Console.WriteLine("Ogiltig inmatning, vänligen välj ett giltigt nummer.");
                    Thread.Sleep(2000);
                    Console.Clear();
                    continue;
                }

                if (selectedOrderIndex == 0)
                {
                    loop = false;
                    break;
                }

                else if (selectedOrderIndex > 0 && selectedOrderIndex <= orders.Count) // Kontrollera att valet är inom intervallet
                {

                    var selectedOrder = orders[selectedOrderIndex - 1]; // Hämta den valda ordern (indexen är 0-baserade)

                    Console.WriteLine("---------------------------------------");
                    Console.WriteLine("Redigera information för order " + selectedOrder.CartId + ":");
                    Console.WriteLine("---------------------------------------");
                    Console.WriteLine("[1] Namn: " + selectedOrder.FullName);
                    Console.WriteLine("[2] Adress: " + selectedOrder.Address);
                    Console.WriteLine("[0] Avbryt");
                    Console.WriteLine("---------------------------------------");

                    Console.WriteLine("Välj ett alternativ: ");
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    switch (key.KeyChar)
                    {
                        case '1':
                            Console.WriteLine("Ange nytt namn:");
                            selectedOrder.FullName = Console.ReadLine();
                            SaveChangesToDatabase(selectedOrder);
                            Console.WriteLine("Namn uppdaterat till: " + selectedOrder.FullName);
                            Thread.Sleep(2000);
                            break;
                        case '2':
                            Console.WriteLine("Redigera adress:");
                            Console.WriteLine("---------------------------------------");
                            Console.WriteLine("[1] Gata: " + selectedOrder.Address.StreetAddress);
                            Console.WriteLine("[2] Stad: " + selectedOrder.Address.City);
                            Console.WriteLine("[3] Postnummer: " + selectedOrder.Address.PostalCode);
                            Console.WriteLine("[0] Avbryt");
                            Console.WriteLine("---------------------------------------");

                            ConsoleKeyInfo key2 = Console.ReadKey(true);

                            switch (key2.KeyChar)
                            {
                                case '1':
                                    Console.WriteLine("Ange ny gataadress:");
                                    selectedOrder.Address.StreetAddress = Console.ReadLine();
                                    SaveChangesToDatabase(selectedOrder);
                                    Console.WriteLine("Gatuadress uppdaterat till: " + selectedOrder.Address.StreetAddress);
                                    Thread.Sleep(2000);
                                    break;
                                case '2':
                                    Console.WriteLine("Ange ny stad:");
                                    selectedOrder.Address.City = Console.ReadLine();
                                    SaveChangesToDatabase(selectedOrder);
                                    Console.WriteLine("Stad uppdaterat till: " + selectedOrder.Address.City);
                                    Thread.Sleep(2000);
                                    break;
                                case '3':
                                    Console.WriteLine("Ange nytt postnummer:");
                                    selectedOrder.Address.PostalCode = Console.ReadLine();
                                    SaveChangesToDatabase(selectedOrder);
                                    Console.WriteLine("Postnummer uppdaterat till: " + selectedOrder.Address.PostalCode);
                                    Thread.Sleep(2000);
                                    break;
                                case '0':
                                    break;
                                default:
                                    Console.WriteLine("Ogiltigt alternativ, vänligen välj ett giltigt nummer.");
                                    break;
                            }
                            break;
                        default:
                            Console.WriteLine("Ogiltigt alternativ. Vänligen välj ett alternativ");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Ogiltigt val, vänligen välj ett giltigt indexnummer.");
                }
            }
            else
            {
                Console.WriteLine("Inga ordrar finns i systemet.");
            }
        }
    }

    // Metod för att spara ändringarna i databasen
    public static void SaveChangesToDatabase(ShippingInfo order)
    {
        var shippingCollection = MongoDbConnection.GetShippingCollection();
        var filter = Builders<ShippingInfo>.Filter.Eq(o => o.Id, order.Id);
        var update = Builders<ShippingInfo>.Update
            .Set(o => o.FullName, order.FullName)
            .Set(o => o.Address, order.Address);

        shippingCollection.UpdateOne(filter, update);
    }
}