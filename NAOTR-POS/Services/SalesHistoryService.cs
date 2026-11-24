
using System;
using System.Data;
using System.Data.SqlClient;
using NAOTR_POS.Data;
using NAOTR_POS.Helpers;

namespace NAOTR_POS.Services
{
    public class SalesHistoryService
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

        public DataTable GetHistory(
            string search = "",
            DateTime? fromDate = null,
            DateTime? toDate = null,
            string paymentMethod = "All",
            string status = "All")
        {
            EnsureAccess();

            if (fromDate.HasValue &&
                toDate.HasValue &&
                fromDate.Value.Date > toDate.Value.Date)
            {
                throw new Exception(
                    "Invalid date range.");
            }

            DataTable table = new DataTable();

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "dbo.usp_Sales_GetHistory", conn))
            {
                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@Search",
                    SqlDbType.NVarChar,
                    150).Value = search ?? "";

                cmd.Parameters.Add(
                    "@FromDate",
                    SqlDbType.Date).Value =
                        fromDate.HasValue
                        ? (object)fromDate.Value.Date
                        : DBNull.Value;

                cmd.Parameters.Add(
                    "@ToDate",
                    SqlDbType.Date).Value =
                        toDate.HasValue
                        ? (object)toDate.Value.Date
                        : DBNull.Value;

                cmd.Parameters.Add(
                    "@PaymentMethod",
                    SqlDbType.NVarChar,
                    20).Value = paymentMethod;

                cmd.Parameters.Add(
                    "@Status",
                    SqlDbType.NVarChar,
                    20).Value = status;

                using (SqlDataAdapter adapter =
                    new SqlDataAdapter(cmd))
                {
                    adapter.Fill(table);
                }
            }

            return table;
        }

        public void VoidOrder(
            int orderId,
            string reason)
        {
            EnsureAccess();

            reason = (reason ?? "").Trim();

            if (reason.Length == 0 ||
                reason.Length > 150)
            {
                throw new Exception(
                    "Enter a valid void reason.");
            }

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "dbo.usp_Sales_VoidOrder", conn))
            {
                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@OrderId",
                    SqlDbType.Int).Value = orderId;

                cmd.Parameters.Add(
                    "@UserId",
                    SqlDbType.Int).Value =
                        Session.CurrentUser.UserId;

                cmd.Parameters.Add(
                    "@Reason",
                    SqlDbType.NVarChar,
                    150).Value = reason;

                conn.Open();

                cmd.ExecuteNonQuery();
            }
        }
    }
}
