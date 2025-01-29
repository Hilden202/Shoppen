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
        Besök_som_kund = 1,
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
        Produkt_hantering,
        Statistik,
        Välj_topp_3_produkt,
        Redigera_beställnings_information,
        Logga_ut = 0
    }
    public enum ProductEditMenu
    {
        Lägg_till_produkt = 1,
        Lägg_till_produktkategori,
        Ändra_Produkt,
        Ta_bort_produkt,
        Tillbaka = 0
    }
    public enum StatisticsMenu
    {
        Beställningshistorik = 1,
        Bäst_Säljande_produkter,
        Tillbaka = 0
    }
}