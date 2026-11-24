
using System;

namespace NAOTR_POS.Models
{
    public class CheckoutResult
    {
        public int OrderId { get; set; }

        public string OrderNumber { get; set; }

        public DateTime OrderDate { get; set; }

        public string CustomerName { get; set; }

        public string CashierName { get; set; }

        public decimal Subtotal { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal Total { get; set; }

        public string PaymentMethod { get; set; }

        public decimal AmountPaid { get; set; }

        public decimal ChangeDue { get; set; }

        public string Status { get; set; }
    }
}
