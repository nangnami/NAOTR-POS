
using System;

namespace NAOTR_POS.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        public string FullName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public DateTime CreatedAt { get; set; }

        public int OrderCount { get; set; }

        public decimal TotalSpent { get; set; }
    }
}
