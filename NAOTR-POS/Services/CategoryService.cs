
using System;
using System.Data;
using System.Data.SqlClient;
using NAOTR_POS.Data;
using NAOTR_POS.Models;
using NAOTR_POS.Helpers;

namespace NAOTR_POS.Services
{
    public class CategoryService
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

        // EXISTING METHOD FOR CATEGORY COMBOBOX

        public DataTable GetCategories()
        {
            DataTable table = new DataTable();

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                @"SELECT CategoryId, Name
                  FROM dbo.Categories
                  ORDER BY Name", conn))
            using (SqlDataAdapter adapter =
                new SqlDataAdapter(cmd))
            {
                conn.Open();
                adapter.Fill(table);
            }

            return table;
        }

        // GET ALL CATEGORIES WITH PRODUCT COUNTS

        public DataTable GetAllCategories(
            string search = "")
        {
            EnsureAccess();

            DataTable table = new DataTable();

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "dbo.usp_Category_GetAll", conn))
            {
                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@Search",
                    SqlDbType.NVarChar,
                    100).Value = search ?? "";

                conn.Open();

                using (SqlDataAdapter adapter =
                    new SqlDataAdapter(cmd))
                {
                    adapter.Fill(table);
                }
            }

            return table;
        }

        // GET CATEGORY BY ID

        public Category GetCategoryById(int categoryId)
        {
            EnsureAccess();

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "dbo.usp_Category_GetById", conn))
            {
                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@CategoryId",
                    SqlDbType.Int).Value = categoryId;

                conn.Open();

                using (SqlDataReader reader =
                    cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Category
                        {
                            CategoryId = Convert.ToInt32(
                                reader["CategoryId"]),

                            Name = Convert.ToString(
                                reader["Name"]),

                            Description = Convert.ToString(
                                reader["Description"])
                        };
                    }
                }
            }

            return null;
        }

        // VALIDATE CATEGORY

        private void ValidateCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            if (string.IsNullOrWhiteSpace(category.Name))
                throw new ArgumentException(
                    "Category name is required.");

            if (category.Name.Trim().Length > 100)
                throw new ArgumentException(
                    "Category name is too long.");

            if (category.Description != null &&
                category.Description.Length > 200)
                throw new ArgumentException(
                    "Description is too long.");
        }

        // COMMON PARAMETERS

        private void AddParameters(
            SqlCommand cmd,
            Category category)
        {
            cmd.Parameters.Add(
                "@Name",
                SqlDbType.NVarChar,
                100).Value = category.Name.Trim();

            cmd.Parameters.Add(
                "@Description",
                SqlDbType.NVarChar,
                200).Value =
                    string.IsNullOrWhiteSpace(
                        category.Description)
                    ? (object)DBNull.Value
                    : category.Description.Trim();
        }

        // INSERT CATEGORY

        public int InsertCategory(Category category)
        {
            EnsureAccess();
            ValidateCategory(category);

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "dbo.usp_Category_Insert", conn))
            {
                cmd.CommandType =
                    CommandType.StoredProcedure;

                AddParameters(cmd, category);

                conn.Open();

                return Convert.ToInt32(
                    cmd.ExecuteScalar());
            }
        }

        // UPDATE CATEGORY

        public void UpdateCategory(Category category)
        {
            EnsureAccess();
            ValidateCategory(category);

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "dbo.usp_Category_Update", conn))
            {
                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@CategoryId",
                    SqlDbType.Int).Value =
                        category.CategoryId;

                AddParameters(cmd, category);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // DELETE CATEGORY

        public void DeleteCategory(int categoryId)
        {
            EnsureAccess();

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "dbo.usp_Category_Delete", conn))
            {
                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@CategoryId",
                    SqlDbType.Int).Value = categoryId;

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
