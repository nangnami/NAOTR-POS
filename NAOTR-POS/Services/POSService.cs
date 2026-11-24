
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NAOTR_POS.Data;
using NAOTR_POS.Models;
using NAOTR_POS.Helpers;

namespace NAOTR_POS.Services
{
    public class POSService
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

        // MAP DATABASE RECORD TO PRODUCT

        private Product MapProduct(SqlDataReader reader)
        {
            return new Product
            {
                ProductId = Convert.ToInt32(
                    reader["ProductId"]),

                Sku = Convert.ToString(
                    reader["Sku"]),

                Barcode = Convert.ToString(
                    reader["Barcode"]),

                Name = Convert.ToString(
                    reader["Name"]),

                CategoryId = Convert.ToInt32(
                    reader["CategoryId"]),

                CategoryName = Convert.ToString(
                    reader["CategoryName"]),

                Price = Convert.ToDecimal(
                    reader["Price"]),

                StockQty = Convert.ToInt32(
                    reader["StockQty"]),

                ReorderLevel = Convert.ToInt32(
                    reader["ReorderLevel"]),

                Image = reader["Image"] == DBNull.Value
                    ? null
                    : (byte[])reader["Image"],

                IsActive = true
            };
        }

        // GET POS PRODUCTS

        public List<Product> GetProducts(
            string search = "",
            int categoryId = 0)
        {
            EnsureAccess();

            List<Product> products =
                new List<Product>();

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "dbo.usp_POS_GetProducts", conn))
            {
                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@Search",
                    SqlDbType.NVarChar,
                    150).Value = search ?? "";

                cmd.Parameters.Add(
                    "@CategoryId",
                    SqlDbType.Int).Value = categoryId;

                conn.Open();

                using (SqlDataReader reader =
                    cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(
                            MapProduct(reader));
                    }
                }
            }

            return products;
        }

        // FIND PRODUCT USING EXACT BARCODE

        public Product GetProductByBarcode(
            string barcode)
        {
            EnsureAccess();

            if (string.IsNullOrWhiteSpace(barcode))
                return null;

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "dbo.usp_POS_GetByBarcode", conn))
            {
                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@Barcode",
                    SqlDbType.NVarChar,
                    50).Value = barcode.Trim();

                conn.Open();

                using (SqlDataReader reader =
                    cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapProduct(reader);
                    }
                }
            }

            return null;
        }
    }
}
