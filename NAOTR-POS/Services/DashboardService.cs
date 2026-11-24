using System;
using System.Data;
using System.Data.SqlClient;
using NAOTR_POS.Data;
using NAOTR_POS.Models;

namespace NAOTR_POS.Services
{
    public class DashboardService
    {
        public DashboardMetrics GetMetrics()
        {
            DashboardMetrics metrics = new DashboardMetrics();

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(
                    "dbo.usp_Report_Dashboard", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            metrics.TodaySales =
                                Convert.ToDecimal(reader["TodaySales"]);

                            metrics.TodayOrders =
                                Convert.ToInt32(reader["TodayOrders"]);

                            metrics.LowStockCount =
                                Convert.ToInt32(reader["LowStockCount"]);

                            metrics.ActiveProducts =
                                Convert.ToInt32(reader["ActiveProducts"]);
                        }
                    }
                }
            }

            return metrics;
        }

        public DataTable GetLowStockProducts()
        {
            DataTable table = new DataTable();

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(
                    "dbo.usp_Report_LowStock", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

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