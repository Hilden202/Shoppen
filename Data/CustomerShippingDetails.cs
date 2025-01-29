using System;
namespace ConsoleShoppen.Models
{
    public class CustomerShippingDetails
    {
        public static ShippingInfo GetShippingInfo(int cartId)
        {
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Fyll i fraktinformation:");

            // obligatorisk
            Console.Write("*Fullständigt namn: ");
            string fullName = Console.ReadLine();
            while (string.IsNullOrEmpty(fullName))
            {
                Console.WriteLine("Fullständigt namn är obligatoriskt!");
                Console.Write("*Fullständigt namn: ");
                fullName = Console.ReadLine();
            }
            // valfritt --------------------------->
            Console.Write("Adress (gata): ");
            string streetAddress = Console.ReadLine();

            Console.Write("Postnummer: ");
            string postalCode = Console.ReadLine();

            Console.Write("Stad: ");
            string city = Console.ReadLine();

            Console.Write("Telefonnummer: ");
            string phoneNumber = Console.ReadLine();
            // <------------------------------------

            // obligatorisk
            Console.Write("*E-postadress: ");
            string email = Console.ReadLine();
            while (string.IsNullOrEmpty(email))
            {
                Console.WriteLine("E-postadress är obligatorisk!");
                Console.Write("*E-postadress: ");
                email = Console.ReadLine();
            }

            // Skapa och returnera ShippingInfo-objekt
            var shippingInfo = new ShippingInfo
            {
                FullName = fullName,
                Address = new Address
                {
                    StreetAddress = streetAddress,
                    PostalCode = postalCode,
                    City = city
                },
                PhoneNumber = phoneNumber,
                Email = email,
                CartId = cartId // Kopplar kundvagnens ID till fraktinformationen
            };

            return shippingInfo;
        }
    }
}