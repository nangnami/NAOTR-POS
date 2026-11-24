
using System;
using System.Data;
using System.Data.SqlClient;
using NAOTR_POS.Data;
using NAOTR_POS.Helpers;
using NAOTR_POS.Models;

namespace NAOTR_POS.Services
{
    public class ReportService
    {
        private void EnsureAccess()
        {
            if (!Session.IsInRole("Admin") &&
                !Session.IsInRole("Manager"))
            {
                throw new UnauthorizedAccessException(
                    "Access denied.");
            }
        }

        // ================= EXECUTE REPORT =================

        private DataTable ExecuteReport(
            string procedure,
            DateTime? from = null,
            DateTime? to = null,
            int? top = null)
        {
            EnsureAccess();

            if (from.HasValue && to.HasValue &&
                from.Value.Date > to.Value.Date)
            {
                throw new ArgumentException(
                    "Invalid date range.");
            }

            DataTable table = new DataTable();

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())

            using (SqlCommand cmd =
                new SqlCommand(procedure, conn))
            {
                cmd.CommandType =
                    CommandType.StoredProcedure;

                if (from.HasValue)
                {
                    cmd.Parameters.Add(
                        "@From",
                        SqlDbType.DateTime).Value =
                            from.Value.Date;
                }

                if (to.HasValue)
                {
                    cmd.Parameters.Add(
                        "@To",
                        SqlDbType.DateTime).Value =
                            to.Value.Date;
                }

                if (top.HasValue)
                {
                    cmd.Parameters.Add(
                        "@Top",
                        SqlDbType.Int).Value =
                            top.Value;
                }

                using (SqlDataAdapter adapter =
                    new SqlDataAdapter(cmd))
                {
                    adapter.Fill(table);
                }
            }

            return table;
        }

        // ================= DAILY SALES =================

        public DataTable GetDailySales(
            DateTime from,
            DateTime to)
        {
            return ExecuteReport(
                "dbo.usp_Report_DailySales",
                from,
                to);
        }

        // ================= TOP PRODUCTS =================

        public DataTable GetTopProducts(
            DateTime from,
            DateTime to,
            int top = 10)
        {
            return ExecuteReport(
                "dbo.usp_Report_TopProducts",
                from,
                to,
                top);
        }

        // ================= SALES BY CATEGORY =================

        public DataTable GetSalesByCategory(
            DateTime from,
            DateTime to)
        {
            return ExecuteReport(
                "dbo.usp_Report_SalesByCategory",
                from,
                to);
        }

        // ================= PAYMENT METHODS =================

        public DataTable GetPaymentMethods(
            DateTime from,
            DateTime to)
        {
            return ExecuteReport(
                "dbo.usp_Report_PaymentMethods",
                from,
                to);
        }

        // ================= LOW STOCK =================

        public DataTable GetLowStock()
        {
            return ExecuteReport(
                "dbo.usp_Report_LowStock");
        }

        // ================= REPORT SUMMARY =================

        public ReportSummary GetSummary(
            DateTime from,
            DateTime to)
        {
            EnsureAccess();

            DataTable dailySales =
                GetDailySales(from, to);

            DataTable lowStock =
                GetLowStock();

            ReportSummary summary =
                new ReportSummary();

            foreach (DataRow row in dailySales.Rows)
            {
                summary.TotalRevenue +=
                    Convert.ToDecimal(row["Revenue"]);

                summary.TotalTransactions +=
                    Convert.ToInt32(row["OrderCount"]);
            }

            summary.LowStockCount =
                lowStock.Rows.Count;

            return summary;
        }
    }
}
