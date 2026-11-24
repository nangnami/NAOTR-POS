
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;
using NAOTR_POS.Data;
using NAOTR_POS.Helpers;
using NAOTR_POS.Models;

namespace NAOTR_POS.Services
{
    public class CheckoutService
    {
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

        // ================= GET CUSTOMERS =================

        public DataTable GetCustomers()
        {
            EnsureAccess();

            DataTable table = new DataTable();

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "dbo.usp_POS_GetCustomers", conn))
            {
                cmd.CommandType =
                    CommandType.StoredProcedure;

                using (SqlDataAdapter adapter =
                    new SqlDataAdapter(cmd))
                {
                    adapter.Fill(table);
                }
            }

            return table;
        }

        // ================= CREATE ORDER =================

        public int CreateOrder(
            List<CartItem> items,
            int? customerId,
            string paymentMethod,
            decimal discount,
            decimal taxRate,
            decimal amountPaid)
        {
            EnsureAccess();

            if (items == null || items.Count == 0)
                throw new Exception("The cart is empty.");

            if (items.Any(x => x.Quantity <= 0))
                throw new Exception("Invalid quantity.");

            if (paymentMethod != "Cash" &&
                paymentMethod != "Card" &&
                paymentMethod != "Mobile")
            {
                throw new Exception(
                    "Invalid payment method.");
            }

            decimal subtotal =
                items.Sum(x => x.LineTotal);

            if (discount < 0 || discount > subtotal)
                throw new Exception("Invalid discount.");

            if (taxRate < 0 || taxRate > 100)
                throw new Exception("Invalid tax rate.");

            decimal tax = Math.Round(
                (subtotal - discount) * taxRate / 100M,
                2,
                MidpointRounding.AwayFromZero);

            decimal total = subtotal - discount + tax;

            if (amountPaid < total)
                throw new Exception(
                    "Insufficient payment.");

            if (paymentMethod != "Cash" &&
                amountPaid != total)
            {
                throw new Exception(
                    "Non-cash payments must match the total.");
            }

            XElement itemsXml = new XElement(
                "Items",
                items.Select(item =>
                    new XElement(
                        "Item",
                        new XAttribute(
                            "ProductId",
                            item.ProductId),
                        new XAttribute(
                            "Quantity",
                            item.Quantity))));

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "dbo.usp_Order_Create", conn))
            {
                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@UserId",
                    SqlDbType.Int).Value =
                        Session.CurrentUser.UserId;

                cmd.Parameters.Add(
                    "@CustomerId",
                    SqlDbType.Int).Value =
                        customerId.HasValue
                            ? (object)customerId.Value
                            : DBNull.Value;

                cmd.Parameters.Add(
                    "@PaymentMethod",
                    SqlDbType.NVarChar,
                    20).Value = paymentMethod;

                AddDecimalParameter(
                    cmd, "@DiscountAmount",
                    discount, 18, 2);

                AddDecimalParameter(
                    cmd, "@TaxRate",
                    taxRate, 5, 2);

                AddDecimalParameter(
                    cmd, "@AmountPaid",
                    amountPaid, 18, 2);

                cmd.Parameters.Add(
                    "@ItemsXml",
                    SqlDbType.Xml).Value =
                        itemsXml.ToString();

                conn.Open();

                // Keep prices stable between validation
                // and the existing order procedure.

                using (SqlTransaction transaction =
                    conn.BeginTransaction(
                        IsolationLevel.Serializable))
                {
                    cmd.Transaction = transaction;

                    foreach (CartItem item in
                        items.OrderBy(x => x.ProductId))
                    {
                        using (SqlCommand check =
                            new SqlCommand(
                                @"SELECT Price, StockQty
                                  FROM dbo.Products
                                  WITH (UPDLOCK, HOLDLOCK)
                                  WHERE ProductId = @Id
                                  AND IsActive = 1;",
                                conn, transaction))
                        {
                            check.Parameters.Add(
                                "@Id",
                                SqlDbType.Int).Value =
                                    item.ProductId;

                            using (SqlDataReader reader =
                                check.ExecuteReader())
                            {
                                if (!reader.Read())
                                    throw new Exception(
                                        item.ProductName +
                                        " is unavailable.");

                                decimal currentPrice =
                                    Convert.ToDecimal(
                                        reader["Price"]);

                                int currentStock =
                                    Convert.ToInt32(
                                        reader["StockQty"]);

                                if (currentPrice !=
                                    item.UnitPrice)
                                {
                                    throw new Exception(
                                        "Price changed for " +
                                        item.ProductName +
                                        ". Cancel checkout and " +
                                        "add the product again.");
                                }

                                if (currentStock <
                                    item.Quantity)
                                {
                                    throw new Exception(
                                        "Insufficient stock for " +
                                        item.ProductName);
                                }
                            }
                        }
                    }

                    int orderId = Convert.ToInt32(
                        cmd.ExecuteScalar());

                    transaction.Commit();

                    return orderId;
                }
            }
        }

        // ================= GET COMPLETED ORDER =================

        public CheckoutResult GetOrder(int orderId)
        {
            EnsureAccess();

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "dbo.usp_POS_GetOrder", conn))
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
                    if (!reader.Read())
                        return null;

                    return new CheckoutResult
                    {
                        OrderId = Convert.ToInt32(
                            reader["OrderId"]),

                        OrderNumber = Convert.ToString(
                            reader["OrderNumber"]),

                        OrderDate = Convert.ToDateTime(
                            reader["OrderDate"]),

                        CustomerName = Convert.ToString(
                            reader["CustomerName"]),

                        CashierName = Convert.ToString(
                            reader["CashierName"]),

                        Subtotal = Convert.ToDecimal(
                            reader["Subtotal"]),

                        DiscountAmount = Convert.ToDecimal(
                            reader["DiscountAmount"]),

                        TaxAmount = Convert.ToDecimal(
                            reader["TaxAmount"]),

                        Total = Convert.ToDecimal(
                            reader["Total"]),

                        PaymentMethod = Convert.ToString(
                            reader["PaymentMethod"]),

                        AmountPaid = Convert.ToDecimal(
                            reader["AmountPaid"]),

                        ChangeDue = Convert.ToDecimal(
                            reader["ChangeDue"]),

                        Status = Convert.ToString(
                            reader["Status"])
                    };
                }
            }
        }

        // ================= DECIMAL PARAMETER =================

        private void AddDecimalParameter(
            SqlCommand cmd,
            string name,
            decimal value,
            byte precision,
            byte scale)
        {
            SqlParameter parameter =
                cmd.Parameters.Add(
                    name,
                    SqlDbType.Decimal);

            parameter.Precision = precision;
            parameter.Scale = scale;
            parameter.Value = value;
        }
    }
}
