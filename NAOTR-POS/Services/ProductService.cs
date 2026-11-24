
using System;
using System.Data;
using System.Data.SqlClient;
using NAOTR_POS.Data;
using NAOTR_POS.Models;
using NAOTR_POS.Helpers;

namespace NAOTR_POS.Services
{
    public class ProductService
    {
        private void EnsureAccess()
        {
            if (!Session.IsInRole("Admin") &&
                !Session.IsInRole("Manager"))
            {
                throw new UnauthorizedAccessException(
                    "You do not have inventory access.");
            }
        }

        // READ AND SEARCH PRODUCTS
        public DataTable GetProducts(
            string search = "",
            int categoryId = 0)
        {
            EnsureAccess();

            DataTable table = new DataTable();

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                string sql = @"
                    SELECT
                        p.ProductId,
                        p.Sku,
                        p.Barcode,
                        p.Name,
                        p.CategoryId,
                        c.Name AS CategoryName,
                        p.Price,
                        p.Cost,
                        p.StockQty,
                        p.ReorderLevel,
                        CASE
                            WHEN p.StockQty = 0
                                THEN 'Out of Stock'
                            WHEN p.StockQty <= p.ReorderLevel
                                THEN 'Low Stock'
                            ELSE 'In Stock'
                        END AS StockStatus
                    FROM dbo.Products p
                    INNER JOIN dbo.Categories c
                        ON p.CategoryId = c.CategoryId
                    WHERE p.IsActive = 1
                    AND (
                        @Search = ''
                        OR p.Name LIKE '%' + @Search + '%'
                        OR p.Sku LIKE '%' + @Search + '%'
                        OR p.Barcode LIKE '%' + @Search + '%'
                    )
                    AND (
                        @CategoryId = 0
                        OR p.CategoryId = @CategoryId
                    )
                    ORDER BY p.Name";

                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(
                        "@Search",
                        SqlDbType.NVarChar, 150)
                        .Value = search ?? "";

                    cmd.Parameters.Add(
                        "@CategoryId",
                        SqlDbType.Int)
                        .Value = categoryId;

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(table);
                    }
                }
            }

            return table;
        }

        // GET SINGLE PRODUCT
        public Product GetProductById(int productId)
        {
            EnsureAccess();

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"
                    SELECT *
                    FROM dbo.Products
                    WHERE ProductId = @ProductId";

                using (SqlCommand cmd =
                    new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(
                        "@ProductId",
                        SqlDbType.Int)
                        .Value = productId;

                    using (SqlDataReader reader =
                        cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;

                        return new Product
                        {
                            ProductId =
                                Convert.ToInt32(
                                    reader["ProductId"]),

                            Sku =
                                reader["Sku"].ToString(),

                            Barcode =
                                reader["Barcode"].ToString(),

                            Name =
                                reader["Name"].ToString(),

                            CategoryId =
                                Convert.ToInt32(
                                    reader["CategoryId"]),

                            Price =
                                Convert.ToDecimal(
                                    reader["Price"]),

                            Cost =
                                Convert.ToDecimal(
                                    reader["Cost"]),

                            StockQty =
                                Convert.ToInt32(
                                    reader["StockQty"]),

                            ReorderLevel =
                                Convert.ToInt32(
                                    reader["ReorderLevel"]),

                            IsActive =
                                Convert.ToBoolean(
                                    reader["IsActive"]),

                            Image =
                                reader["Image"] == DBNull.Value
                                ? null
                                : (byte[])reader["Image"]
                        };
                    }
                }
            }
        }

        // COMMON PRODUCT PARAMETERS
        private void AddProductParameters(
            SqlCommand cmd, Product product)
        {
            cmd.Parameters.Add(
                "@Sku", SqlDbType.NVarChar, 30)
                .Value = product.Sku;

            cmd.Parameters.Add(
                "@Barcode", SqlDbType.NVarChar, 50)
                .Value = string.IsNullOrWhiteSpace(product.Barcode)
                    ? (object)DBNull.Value
                    : product.Barcode;

            cmd.Parameters.Add(
                "@Name", SqlDbType.NVarChar, 150)
                .Value = product.Name;

            cmd.Parameters.Add(
                "@CategoryId", SqlDbType.Int)
                .Value = product.CategoryId;

            SqlParameter price = cmd.Parameters.Add(
                "@Price", SqlDbType.Decimal);

            price.Precision = 18;
            price.Scale = 2;
            price.Value = product.Price;

            SqlParameter cost = cmd.Parameters.Add(
                "@Cost", SqlDbType.Decimal);

            cost.Precision = 18;
            cost.Scale = 2;
            cost.Value = product.Cost;

            cmd.Parameters.Add(
                "@ReorderLevel", SqlDbType.Int)
                .Value = product.ReorderLevel;

            cmd.Parameters.Add(
                "@Image", SqlDbType.VarBinary, -1)
                .Value = (object)product.Image
                    ?? DBNull.Value;
        }

        // CREATE PRODUCT
        public int InsertProduct(Product product)
        {
            EnsureAccess();

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (SqlCommand cmd =
                    new SqlCommand(
                        "dbo.usp_Product_Insert", conn))
                {
                    cmd.CommandType =
                        CommandType.StoredProcedure;

                    AddProductParameters(cmd, product);

                    // Automatically generated by SQL Server.
                    cmd.Parameters.RemoveAt("@Sku");
                    cmd.Parameters.RemoveAt("@Barcode");

                    cmd.Parameters.Add(
                        "@StockQty", SqlDbType.Int)
                        .Value = product.StockQty;

                    cmd.Parameters.Add(
                        "@UserId", SqlDbType.Int)
                        .Value = Session.CurrentUser.UserId;

                    return Convert.ToInt32(
                        cmd.ExecuteScalar());
                }
            }
        }

        // UPDATE PRODUCT
        public void UpdateProduct(Product product)
        {
            EnsureAccess();

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (SqlCommand cmd =
                    new SqlCommand(
                        "dbo.usp_Product_Update", conn))
                {
                    cmd.CommandType =
                        CommandType.StoredProcedure;

                    AddProductParameters(cmd, product);

                    cmd.Parameters.Add(
                        "@ProductId", SqlDbType.Int)
                        .Value = product.ProductId;

                    cmd.Parameters.Add(
                        "@IsActive", SqlDbType.Bit)
                        .Value = product.IsActive;

                    cmd.Parameters.Add(
                        "@ChangeImage", SqlDbType.Bit)
                        .Value = product.ChangeImage;

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // SOFT DELETE PRODUCT
        public void DeleteProduct(int productId)
        {
            EnsureAccess();

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (SqlCommand cmd =
                    new SqlCommand(
                        "dbo.usp_Product_Delete", conn))
                {
                    cmd.CommandType =
                        CommandType.StoredProcedure;

                    cmd.Parameters.Add(
                        "@ProductId", SqlDbType.Int)
                        .Value = productId;

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // STOCK ADJUSTMENT
        public int AdjustStock(
            int productId,
            int changeQty,
            string reason)
        {
            EnsureAccess();

            if (changeQty == 0)
                throw new ArgumentException(
                    "Quantity change cannot be zero.");

            if (string.IsNullOrWhiteSpace(reason) ||
                reason.Length > 200)
                throw new ArgumentException(
                    "Enter a reason of up to 200 characters.");

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (SqlCommand cmd =
                    new SqlCommand(
                        "dbo.usp_Inventory_Adjust", conn))
                {
                    cmd.CommandType =
                        CommandType.StoredProcedure;

                    cmd.Parameters.Add(
                        "@ProductId", SqlDbType.Int)
                        .Value = productId;

                    cmd.Parameters.Add(
                        "@ChangeQty", SqlDbType.Int)
                        .Value = changeQty;

                    cmd.Parameters.Add(
                        "@Reason", SqlDbType.NVarChar, 200)
                        .Value = reason;

                    cmd.Parameters.Add(
                        "@UserId", SqlDbType.Int)
                        .Value = Session.CurrentUser.UserId;

                    return Convert.ToInt32(
                        cmd.ExecuteScalar());
                }
            }
        }
    }
}
