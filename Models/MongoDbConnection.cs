using System;
using MongoDB.Driver;

namespace ConsoleShoppen.Models
{
    public class MongoDbConnection
    {
        private static MongoClient GetClient()
        {
            string connectionString = "mongodb+srv://patrikmdb:Q!w2e3r4@cluster0.7gmkp.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";

            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));

            var client = new MongoClient(settings);

            return client;
        }

        public static IMongoCollection<Models.ShippingInfo> GetShippingCollection()
        {
            var client = GetClient();
            var database = client.GetDatabase("OrdersDb");
            var shippingCollection = database.GetCollection<ShippingInfo>("ShippingDetails");
            return shippingCollection;
        }

        public static void SaveShippingInfoToMongoDb(ShippingInfo shippingInfo, string orderNumber)
        {
            // Skapa MongoDB-klient och databasanslutning
            var client = MongoDbConnection.GetClient();
            var database = client.GetDatabase("OrdersDb");
            var collection = database.GetCollection<ShippingInfo>("ShippingDetails");

            // Sätt ordernummer på shippingInfo
            shippingInfo.Id = orderNumber;

            // Spara shippingInfo i MongoDB
            collection.InsertOne(shippingInfo);

            Console.WriteLine("Fraktinformation har sparats.");
        }
        public static string GenerateOrderNumber()
        {
            return Guid.NewGuid().ToString();
        }
    }
}