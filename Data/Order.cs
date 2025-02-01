using System;
using ConsoleShoppen.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleShoppen.Data
{
    public class Order
    {
        public static void OrderCheckout(ShippingService shippingService)
        {
            using (var context = new MyDbContext())
            {
                Cart activeCart = Cart.GetActiveCart();

                if (activeCart == null)
                {
                    Console.WriteLine("Det finns ingen aktiv varukorg.");
                    return;
                }

                while (true)
                {
                    ShowCart(activeCart);

                    Console.WriteLine("Välj produkt att ändra/Ta bort: ");
                    Console.WriteLine("Tryck [P] för att slutföra köp: ");

                    var key = Console.ReadKey(true).Key;

                    if (key >= ConsoleKey.D0 && key <= ConsoleKey.D9)
                    {
                        // Försök att konvertera till int och hantera produktval
                        if (!int.TryParse(key.ToString().Substring(1), out int nr))
                        {
                            Console.WriteLine("Ogiltig inmatning, vänligen välj en giltig inmatning.");
                            Thread.Sleep(2000);
                            Console.Clear();
                            continue;
                        }
                        if (nr == 0)
                        {
                            break;
                        }
                        else if (nr > 0 && nr <= activeCart.CartProducts.Count)
                        {
                            var selectedCartProduct = activeCart.CartProducts[nr - 1]; // Hämta det valda CartProduct

                            if (selectedCartProduct != null)
                            {
                                EditProduct(activeCart, context, selectedCartProduct);
                            }
                        }
                    }
                    else if (key == ConsoleKey.P)
                    {
                        // Hämta kundens aktuella varukorg
                        var currentCartId = activeCart.Id;

                        // Koppla ordernummer med shippingInfo.Id
                        var orderNumber = Models.MongoDbConnection.GenerateOrderNumber(); // Skapa ett unikt ordernummer
                        Console.WriteLine("--------------------------------------------");

                        // Fråga användaren om fraktalternativ
                        Console.WriteLine("Välj fraktalternativ:");
                        Console.WriteLine("[1] Standard frakt (50 kr)");
                        Console.WriteLine("[2] Express frakt (100 kr)");
                        var shippingChoice = Console.ReadKey(true).KeyChar;

                        // Använd en metod för att få fraktkostnaden baserat på användarens val
                        decimal shippingCost = shippingService.GetShippingCost(shippingChoice);

                        // Hämta fraktinformation och koppla den till CartId
                        var shippingInfo = CustomerShippingDetails.GetShippingInfo(currentCartId);

                        Console.Clear();

                        // Granska och godkänn
                        ShowCartWithShipping(activeCart, shippingCost);

                        Console.WriteLine("--------------------------------------------");
                        // Be användaren att godkänna eller avbryta
                        Console.WriteLine("Tryck [1] för att godkänna eller [0] för att avbryta.");

                        var confirmationChoice = Console.ReadKey(true).KeyChar;

                        if (confirmationChoice == '1')
                        {
                            // Spara shippingInfo till MongoDB
                            Models.MongoDbConnection.SaveShippingInfoToMongoDb(shippingInfo, orderNumber);

                            // Slutför köpet
                            Cart.CompleteCart(currentCartId, shippingCost);
                            Cart.CreateNewCart();
                            Console.WriteLine("Köp har slutförts.");
                            Thread.Sleep(2000);
                        }
                        else if (confirmationChoice == '0')
                        {
                            // Om användaren avbryter
                            Console.WriteLine("Köp har avbrutits.");
                            Thread.Sleep(2000);
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Ogiltig inmatning, vänligen välj ett giltigt alternativ.");
                        Thread.Sleep(2000);
                        Console.Clear();
                    }
                }
            }
        }
        public static void ShowCart(Cart activeCart)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("┌-─-──────────────────────────────────────-┐");
            Console.WriteLine("│                 Varukorg                 │");
            Console.WriteLine("└-────────────────────────────────────────-┘");
            Console.ResetColor();

            int totalQuantity = activeCart.GetTotalQuantity(); // Total kvantitet av produkter

            Console.WriteLine("Totalt " + totalQuantity + " produkter i listan.");
            Console.WriteLine("--------------------------------------------");


            int i = 1;
            foreach (var cartProduct in activeCart.CartProducts)
            {
                Console.WriteLine("[" + i + "] " + cartProduct.Product.Name.PadRight(32) + "Antal: " + cartProduct.Quantity);
                i++;
            }
            Console.WriteLine("[0] Tillbaka");
            Console.WriteLine("--------------------------------------------");

            var cartList = activeCart.CartProducts
                          .OrderBy(p => p.Product.Id)
                          .ToList();

            // Beräkna totalpriset för alla produkter i varukorgen baserat på kvantitet och pris
            decimal totalSum = cartList.Sum(p => p.Quantity * p.Product.Price.GetValueOrDefault());

            // Lägg till moms
            decimal totalWithTax = totalSum * (1 + activeCart.GetMoms());

            // Skriv ut totalpris inklusive moms
            Console.WriteLine("Totalt pris exkl. moms: ".PadRight(36) + totalSum.ToString("0.##") + " kr");
            Console.WriteLine("Moms (25%): ".PadRight(36) + (totalWithTax - totalSum).ToString("0.##") + " kr");
            Console.WriteLine("Totalt pris inkl. moms: ".PadRight(36) + totalWithTax.ToString("0.##") + " kr");
            Console.WriteLine("--------------------------------------------");
        }

        public static void ShowCartWithShipping(Cart activeCart, decimal shippingCost)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("┌-─-──────────────────────────────────────-┐");
            Console.WriteLine("│           Granska beställning            │");
            Console.WriteLine("└-────────────────────────────────────────-┘");
            Console.ResetColor();

            int totalQuantity = activeCart.GetTotalQuantity(); // Total kvantitet av produkter

            Console.WriteLine("Totalt " + totalQuantity + " produkter i listan.");
            Console.WriteLine("--------------------------------------------");

            int i = 1;
            foreach (var cartProduct in activeCart.CartProducts)
            {
                Console.WriteLine("[" + i + "] " + cartProduct.Product.Name.PadRight(32) + "Antal: " + cartProduct.Quantity);
                i++;
            }
            Console.WriteLine("[0] Tillbaka");
            Console.WriteLine("--------------------------------------------");

            var cartList = activeCart.CartProducts
                                      .OrderBy(p => p.Product.Id)
                                      .ToList();

            // Beräkna totalpriset för alla produkter i varukorgen baserat på kvantitet och pris
            decimal totalSum = cartList.Sum(p => p.Quantity * p.Product.Price.GetValueOrDefault());

            // Lägg till moms
            decimal totalWithTax = totalSum * (1 + activeCart.GetMoms());

            // Skriv ut totalpris inklusive moms
            Console.WriteLine("Totalt pris exkl. moms: ".PadRight(36) + totalSum.ToString("0.##") + " kr");
            Console.WriteLine("Moms (25%): ".PadRight(36) + (totalWithTax - totalSum).ToString("0.##") + " kr");
            Console.WriteLine("Totalt pris inkl. moms: ".PadRight(36) + totalWithTax.ToString("0.##") + " kr");

            // Lägg till fraktkostnaden
            decimal totalCostWithShipping = totalWithTax + shippingCost;
            Console.WriteLine("Fraktkostnad: ".PadRight(36) + shippingCost.ToString("0.##") + " kr");

            // Total kostnad inklusive frakt
            Console.WriteLine("Total kostnad inklusive frakt: ".PadRight(36) + totalCostWithShipping.ToString("0.##") + " kr");

        }

        public static void EditProduct(Cart activeCart, MyDbContext context, CartProduct selectedCartProduct) // todo pågår utredning nedan..
        {
            bool editingProduct = true;

            while (editingProduct)
            {
                Console.Clear();
                ShowCart(activeCart);

                decimal totalProductSum = selectedCartProduct.Quantity * selectedCartProduct.Product.Price.GetValueOrDefault();

                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("Produkt: " + selectedCartProduct.Product.Name);
                Console.WriteLine("Om produkten: " + selectedCartProduct.Product.ProductInfo);
                Console.WriteLine("Antal: " + selectedCartProduct.Quantity);
                Console.WriteLine("Summa: " + totalProductSum.ToString("0.##") + "kr");
                Console.WriteLine("--------------------------------------------");

                Console.WriteLine("[1] Lägg till produkt: ");
                Console.WriteLine("[2] Ta bort produkt: ");
                Console.WriteLine("[0] Tillbaka: ");

                if (!int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out int nr))
                {
                    Console.WriteLine("Ogiltig inmatning, vänligen välj ett giltigt nummer.");
                    Thread.Sleep(2000);
                    Console.Clear();
                    continue;
                }
                // todo live updatering för av kundvagnen syns inte i sql när man kör pluss minus produkt..
                switch (nr)
                {
                    case 1: // Lägg till produkt
                        selectedCartProduct.Quantity++;

                        var productToUpdate = context.CartProducts.FirstOrDefault(cp => cp.Id == selectedCartProduct.Id);
                        if (productToUpdate != null)
                        {
                            // Uppdatera kvantiteten i databasen
                            productToUpdate.Quantity = selectedCartProduct.Quantity;
                            context.SaveChanges();
                        }
                        break;

                    case 2: // Ta bort produkt
                        selectedCartProduct.Quantity--;

                        var productToUpdate2 = context.CartProducts.FirstOrDefault(cp => cp.Id == selectedCartProduct.Id);
                        if (productToUpdate2 != null)
                        {
                            if (selectedCartProduct.Quantity > 0)
                            {
                                // Uppdatera kvantiteten
                                productToUpdate2.Quantity = selectedCartProduct.Quantity;
                                context.SaveChanges();
                            }
                            else
                            {
                                // Ta bort produkten från kundvagnen
                                activeCart.CartProducts.Remove(selectedCartProduct);
                                context.CartProducts.Remove(productToUpdate2);
                                context.SaveChanges();

                                editingProduct = false;
                                Console.Clear();
                            }
                        }
                        break;

                    case 0:
                        editingProduct = false;
                        Console.Clear();
                        break;

                    default:
                        Console.WriteLine("Ogiltig inmatning, vänligen välj ett giltigt alternativ.");
                        Thread.Sleep(2000);
                        break;
                }
            }
        }

        //public static void EditProduct(Cart activeCart, MyDbContext context, CartProduct selectedCartProduct) //todo visar fortfarande 1 produkt fast den är borta.
        //{
        //    bool editingProduct = true;

        //    while (editingProduct)
        //    {
        //        Console.Clear();

        //        // Ladda om activeCart från databasen så den uppdateras live
        //        activeCart = context.Carts.Include(c => c.CartProducts)
        //                                  .ThenInclude(cp => cp.Product)
        //                                  .FirstOrDefault(c => c.Id == activeCart.Id);

        //        ShowCart(activeCart);

        //        decimal totalProductSum = selectedCartProduct.Quantity * selectedCartProduct.Product.Price.GetValueOrDefault();

        //        Console.WriteLine("--------------------------------------------");
        //        Console.WriteLine("Produkt: " + selectedCartProduct.Product.Name);
        //        Console.WriteLine("Om produkten: " + selectedCartProduct.Product.ProductInfo);
        //        Console.WriteLine("Antal: " + selectedCartProduct.Quantity);
        //        Console.WriteLine("Summa: " + totalProductSum.ToString("0.##") + "kr");
        //        Console.WriteLine("--------------------------------------------");

        //        Console.WriteLine("[1] Lägg till produkt: ");
        //        Console.WriteLine("[2] Ta bort produkt: ");
        //        Console.WriteLine("[0] Tillbaka: ");

        //        if (!int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out int nr))
        //        {
        //            Console.WriteLine("Ogiltig inmatning, vänligen välj ett giltigt nummer.");
        //            Thread.Sleep(2000);
        //            Console.Clear();
        //            continue;
        //        }
        //        switch (nr)
        //        {
        //            case 1: // Lägg till produkt
        //                {
        //                    var cart = context.Carts.Include(c => c.CartProducts)
        //                                            .ThenInclude(cp => cp.Product)
        //                                            .FirstOrDefault(c => c.Id == activeCart.Id);

        //                    if (cart != null)
        //                    {
        //                        var productToUpdate = cart.CartProducts.FirstOrDefault(cp => cp.Id == selectedCartProduct.Id);
        //                        if (productToUpdate != null)
        //                        {
        //                            productToUpdate.Quantity++;
        //                            cart.TotalPrice += selectedCartProduct.Product.Price.GetValueOrDefault();
        //                            context.SaveChanges();

        //                            // Hämta den uppdaterade versionen av produkten
        //                            selectedCartProduct = activeCart.CartProducts.FirstOrDefault(cp => cp.Id == selectedCartProduct.Id);
        //                        }
        //                    }
        //                    Console.Clear();
        //                    break;
        //                }

        //            case 2: // Ta bort produkt
        //                {
        //                    var cart = context.Carts.Include(c => c.CartProducts)
        //                                            .ThenInclude(cp => cp.Product)
        //                                            .FirstOrDefault(c => c.Id == activeCart.Id);

        //                    if (cart != null)
        //                    {
        //                        var productToUpdate = cart.CartProducts.FirstOrDefault(cp => cp.Id == selectedCartProduct.Id);
        //                        if (productToUpdate != null)
        //                        {
        //                            cart.TotalPrice -= selectedCartProduct.Product.Price.GetValueOrDefault();
        //                            if (productToUpdate.Quantity > 1)
        //                            {
        //                                productToUpdate.Quantity--;
        //                            }
        //                            else
        //                            {
        //                                cart.CartProducts.Remove(productToUpdate);
        //                                context.CartProducts.Remove(productToUpdate);
        //                            }
        //                            context.SaveChanges();
        //                        }

        //                        // Uppdatera activeCart efter ändringar
        //                        activeCart = context.Carts.Include(c => c.CartProducts)
        //                                                  .FirstOrDefault(c => c.Id == activeCart.Id);

        //                        // Om varukorgen nu är tom, avsluta redigeringsloopen
        //                        if (activeCart == null || !activeCart.CartProducts.Any())
        //                        {
        //                            Console.Clear();
        //                            Console.WriteLine("Varukorgen är tom!");
        //                            Thread.Sleep(2000);
        //                            continue;
        //                            //ShopMenu;
        //                            //editingProduct = false;

        //                        }
        //                        else
        //                        {
        //                            // Hämta uppdaterad produkt (om den fortfarande finns kvar)
        //                            selectedCartProduct = activeCart.CartProducts.FirstOrDefault(cp => cp.Id == selectedCartProduct.Id);
        //                        }
        //                    }
        //                    Console.Clear();
        //                    break;
        //                }

        //            case 0:
        //                editingProduct = false;
        //                Console.Clear();
        //                break;

        //            default:
        //                Console.WriteLine("Ogiltig inmatning, vänligen välj ett giltigt alternativ.");
        //                Thread.Sleep(2000);
        //                break;
        //        }
        //    }
        //}
    }
}