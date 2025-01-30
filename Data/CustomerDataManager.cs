using ConsoleShoppen.Data;
using ConsoleShoppen.Models;
using MongoDB.Driver;

public static class CustomerDataManager
{
    public static void EditOrderDetails()
    {
        bool loop = true;

        while (loop)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("┌-─-─────────────────────────────────-┐");
            Console.WriteLine("│        Redigera Beställning         │");
            Console.WriteLine("└-───────────────────────────────────-┘");
            Console.ResetColor();

            // Hämta alla ordrar från MongoDB
            var orders = MongoDbConnection.GetShippingCollection().Find(FilterDefinition<ShippingInfo>.Empty).ToList();

            if (orders.Any())
            {
                foreach (var order in orders)
                {
                    Console.WriteLine("Beställning: " + order.CartId + ", Kund: " + order.FullName);
                }
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("[0] Tillbaka");
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("Välj en beställning att ändra:");

                string? input = Console.ReadLine();

                if (input == "0")
                {
                    loop = false;
                    break;
                }

                if (int.TryParse(input, out int orderId))
                {
                    var selectedOrder = orders.FirstOrDefault(o => o.CartId == orderId);

                    if (selectedOrder != null)
                    {
                        Console.WriteLine("---------------------------------------");
                        Console.WriteLine("Redigera information för beställning " + selectedOrder.CartId + ":");
                        Console.WriteLine("---------------------------------------");
                        Console.WriteLine("[1] Namn: " + selectedOrder.FullName);
                        Console.WriteLine("[2] Adress: " + selectedOrder.Address);
                        Console.WriteLine("[3] Tel: " + selectedOrder.PhoneNumber);
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
                            case '3':
                                Console.WriteLine("Ange nytt telefonnummer:");
                                selectedOrder.PhoneNumber = Console.ReadLine();
                                SaveChangesToDatabase(selectedOrder);
                                Console.WriteLine("Telefonnumret uppdaterat till: " + selectedOrder.PhoneNumber);
                                Thread.Sleep(2000);
                                break;
                            default:
                                Console.WriteLine("Ogiltigt alternativ. Vänligen välj ett alternativ");
                                break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Beställningsnummret hittades inte. Försök igen.");
                    Thread.Sleep(2000);
                }
            }
            else
            {
                Console.WriteLine("Inga beställningar finns i systemet.");
                Thread.Sleep(2000);
                loop = false;
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
            .Set(o => o.Address, order.Address)
            .Set(o => o.PhoneNumber, order.PhoneNumber);

        shippingCollection.UpdateOne(filter, update);
    }
}