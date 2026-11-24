
namespace NAOTR_POS.Models
{
    public class ReportSummary
    {
        public decimal TotalRevenue { get; set; }

        public int TotalTransactions { get; set; }

        public int LowStockCount { get; set; }

        public decimal AverageOrderValue
        {
            get
            {
                return TotalTransactions == 0
                    ? 0
                    : TotalRevenue / TotalTransactions;
            }
        }
    }
}
