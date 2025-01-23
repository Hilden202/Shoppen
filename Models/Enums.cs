using System;
namespace ConsoleShoppen.Models
{
    //public enum UserType
    //{
    //    Kund = 1,
    //    Admin = 2,
    //}
    public enum StartMenu
    {
        Logga_in_som_kund = 1,
        Logga_in_som_Admin,
        Avsluta = 0
    }
    public enum CustomerMenu
    {
        Startsida = 1,
        Shoppen,
        Varukorgen,
        Logga_ut = 0
    }

    public enum ShopMenu
    {
        Sök = 1,
        Kategori,
        Produkt_lista,
        Tillbaka = 0
    }

    public enum AdminMenu
    {
        Startsida = 1,
        Visa_lagersaldo,
        Lägg_till_produkt,
        Ändra_Produkt,
        Ta_bort_produkt,
        Beställningshistorik,
        Bäst_Säljande_produkter,
        Välj_topp_produkt,
        Logga_ut = 0
    }
}

