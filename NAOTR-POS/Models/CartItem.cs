
namespace NAOTR_POS.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }

        public string Sku { get; set; }

        public string Barcode { get; set; }

        public string ProductName { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public int AvailableStock { get; set; }

        public decimal LineTotal
        {
            get
            {
                return UnitPrice * Quantity;
            }
        }
    }
}
