using System;
namespace ConsoleShoppen.Models
{
    public class ShippingService
    {
        private const decimal StandardShippingCost = 50.00m;  // Standard fraktkostnad
        private const decimal ExpressShippingCost = 100.00m;  // Express fraktkostnad

        // Metod som returnerar fraktkostnaden baserat på användarens val
        public decimal GetShippingCost(char shippingChoice)
        {
            decimal shippingCost;

            switch (shippingChoice)
            {
                case '1':
                    shippingCost = StandardShippingCost; // Standard frakt
                    break;
                case '2':
                    shippingCost = ExpressShippingCost; // Express frakt
                    break;
                default:
                    Console.WriteLine("Ogiltigt val, standard frakt används.");
                    shippingCost = StandardShippingCost;
                    break;
            }

            return shippingCost;
        }
    }
}

