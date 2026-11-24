
using System;
using System.Data;
using System.Data.SqlClient;
using NAOTR_POS.Data;
using NAOTR_POS.Helpers;

namespace NAOTR_POS.Services
{
    public class InventoryHistoryService
    {
        private void EnsureAccess()
        {
            if (!Session.IsInRole("Admin") &&
                !Session.IsInRole("Manager"))
            {
                throw new UnauthorizedAccessException(
                    "You do not have permission to view inventory history.");
            }
        }

        public DataTable GetHistory(
            string search = "",
            string movementType = "All",
            DateTime? fromDate = null,
            DateTime? toDate = null)
        {
            EnsureAccess();

            if (fromDate.HasValue &&
                toDate.HasValue &&
                fromDate.Value.Date > toDate.Value.Date)
            {
                throw new ArgumentException(
                    "Invalid date range.");
            }

            DataTable table = new DataTable();

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                using (SqlCommand cmd =
                    new SqlCommand(
                        "dbo.usp_Inventory_GetHistory",
                        conn))
                {
                    cmd.CommandType =
                        CommandType.StoredProcedure;

                    cmd.Parameters.Add(
                        "@Search",
                        SqlDbType.NVarChar,
                        150).Value =
                            search ?? "";

                    cmd.Parameters.Add(
                        "@MovementType",
                        SqlDbType.NVarChar,
                        20).Value =
                            movementType ?? "All";

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

                    conn.Open();

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(table);
                    }
                }
            }

            return table;
        }
    }
}
