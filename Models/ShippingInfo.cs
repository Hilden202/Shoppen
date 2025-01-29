using System;
using System.Net;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ConsoleShoppen.Models
{
    public class ShippingInfo
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int CartId { get; set; }
        public string FullName { get; set; }
        public Address Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Address
    {
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string StreetAddress { get; set; }

        // Override ToString för att skriva ut adressen på ett meningsfullt sätt
        public override string ToString()
        {
            return StreetAddress + ", " + PostalCode + ", " + City;
        }

    }
}