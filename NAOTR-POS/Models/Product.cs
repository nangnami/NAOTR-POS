namespace NAOTR_POS.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Sku { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public int StockQty { get; set; }
        public int ReorderLevel { get; set; }
        public byte[] Image { get; set; }
        public bool IsActive { get; set; }
        public bool ChangeImage { get; set; }
    }
}