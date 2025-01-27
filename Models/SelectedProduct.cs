using System;
namespace ConsoleShoppen.Models
{
    public class SelectedProduct
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public bool IsFeatured { get; set; } = false;
    }
}

