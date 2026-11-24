
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NAOTR_POS.Data;
using NAOTR_POS.Helpers;
using NAOTR_POS.Models;

namespace NAOTR_POS.Services
{
    public class ReceiptService
    {
        private readonly CheckoutService checkoutService =
            new CheckoutService();

        private void EnsureAccess()
        {
            if (!Session.IsInRole("Admin") &&
                !Session.IsInRole("Manager") &&
                !Session.IsInRole("Cashier"))
            {
                throw new UnauthorizedAccessException(
                    "Access denied.");
            }
        }

        // ================= ORDER INFORMATION =================

        public CheckoutResult GetOrder(int orderId)
        {
            EnsureAccess();

            CheckoutResult order =
                checkoutService.GetOrder(orderId);

            if (order == null)
            {
                throw new Exception(
                    "Transaction not found.");
            }

            return order;
        }

        // ================= RECEIPT ITEMS =================

        public List<ReceiptItem> GetReceiptItems(
            int orderId)
        {
            EnsureAccess();

            List<ReceiptItem> items =
                new List<ReceiptItem>();

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "dbo.usp_Receipt_GetItems", conn))
            {
                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@OrderId",
                    SqlDbType.Int).Value = orderId;

                conn.Open();

                using (SqlDataReader reader =
                    cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(new ReceiptItem
                        {
                            OrderDetailId =
                                Convert.ToInt32(
                                    reader["OrderDetailId"]),

                            ProductId =
                                Convert.ToInt32(
                                    reader["ProductId"]),

                            Sku =
                                Convert.ToString(
                                    reader["Sku"]),

                            ProductName =
                                Convert.ToString(
                                    reader["ProductName"]),

                            Quantity =
                                Convert.ToInt32(
                                    reader["Quantity"]),

                            UnitPrice =
                                Convert.ToDecimal(
                                    reader["UnitPrice"]),

                            LineTotal =
                                Convert.ToDecimal(
                                    reader["LineTotal"])
                        });
                    }
                }
            }

            return items;
        }
    }
}
