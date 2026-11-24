
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using NAOTR_POS.Data;
using NAOTR_POS.Models;
using NAOTR_POS.Helpers;

namespace NAOTR_POS.Services
{
    public class CustomerService
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

        // GET ALL CUSTOMERS

        public DataTable GetCustomers(string search = "")
        {
            EnsureAccess();

            DataTable table = new DataTable();

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "dbo.usp_Customer_GetAll", conn))
            {
                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@Search",
                    SqlDbType.NVarChar,
                    150).Value = search ?? "";

                conn.Open();

                using (SqlDataAdapter adapter =
                    new SqlDataAdapter(cmd))
                {
                    adapter.Fill(table);
                }
            }

            return table;
        }

        // GET CUSTOMER BY ID

        public Customer GetCustomerById(int customerId)
        {
            EnsureAccess();

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "dbo.usp_Customer_GetById", conn))
            {
                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@CustomerId",
                    SqlDbType.Int).Value = customerId;

                conn.Open();

                using (SqlDataReader reader =
                    cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Customer
                        {
                            CustomerId = Convert.ToInt32(
                                reader["CustomerId"]),

                            FullName = Convert.ToString(
                                reader["FullName"]),

                            Phone = Convert.ToString(
                                reader["Phone"]),

                            Email = Convert.ToString(
                                reader["Email"]),

                            Address = Convert.ToString(
                                reader["Address"]),

                            CreatedAt = Convert.ToDateTime(
                                reader["CreatedAt"])
                        };
                    }
                }
            }

            return null;
        }

        // VALIDATION

        private void ValidateCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            if (string.IsNullOrWhiteSpace(
                customer.FullName))
            {
                throw new ArgumentException(
                    "Customer name is required.");
            }

            if (customer.FullName.Trim().Length > 100)
                throw new ArgumentException(
                    "Customer name is too long.");

            if (customer.Phone != null &&
                customer.Phone.Length > 30)
                throw new ArgumentException(
                    "Phone number is too long.");

            if (customer.Email != null &&
                customer.Email.Length > 120)
                throw new ArgumentException(
                    "Email is too long.");

            if (customer.Address != null &&
                customer.Address.Length > 250)
                throw new ArgumentException(
                    "Address is too long.");

            if (!string.IsNullOrWhiteSpace(
                customer.Email))
            {
                try
                {
                    MailAddress address =
                        new MailAddress(customer.Email.Trim());

                    if (!string.Equals(
                        address.Address,
                        customer.Email.Trim(),
                        StringComparison.OrdinalIgnoreCase))
                    {
                        throw new FormatException();
                    }
                }
                catch (FormatException)
                {
                    throw new ArgumentException(
                        "Please enter a valid email address.");
                }
            }
        }

        private object OptionalValue(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? (object)DBNull.Value
                : value.Trim();
        }

        private void AddParameters(
            SqlCommand cmd,
            Customer customer)
        {
            cmd.Parameters.Add(
                "@FullName",
                SqlDbType.NVarChar,
                100).Value = customer.FullName.Trim();

            cmd.Parameters.Add(
                "@Phone",
                SqlDbType.NVarChar,
                30).Value = OptionalValue(customer.Phone);

            cmd.Parameters.Add(
                "@Email",
                SqlDbType.NVarChar,
                120).Value = OptionalValue(customer.Email);

            cmd.Parameters.Add(
                "@Address",
                SqlDbType.NVarChar,
                250).Value = OptionalValue(customer.Address);
        }

        // INSERT CUSTOMER

        public int InsertCustomer(Customer customer)
        {
            EnsureAccess();
            ValidateCustomer(customer);

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "dbo.usp_Customer_Insert", conn))
            {
                cmd.CommandType =
                    CommandType.StoredProcedure;

                AddParameters(cmd, customer);

                conn.Open();

                return Convert.ToInt32(
                    cmd.ExecuteScalar());
            }
        }

        // UPDATE CUSTOMER

        public void UpdateCustomer(Customer customer)
        {
            EnsureAccess();
            ValidateCustomer(customer);

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "dbo.usp_Customer_Update", conn))
            {
                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@CustomerId",
                    SqlDbType.Int).Value =
                        customer.CustomerId;

                AddParameters(cmd, customer);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // DELETE CUSTOMER

        public void DeleteCustomer(int customerId)
        {
            EnsureAccess();

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "dbo.usp_Customer_Delete", conn))
            {
                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@CustomerId",
                    SqlDbType.Int).Value = customerId;

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // CUSTOMER LOOKUP FOR FUTURE POS MODULE

        public DataTable GetCustomerLookup()
        {
            if (!Session.IsInRole("Admin") &&
                !Session.IsInRole("Manager") &&
                !Session.IsInRole("Cashier"))
            {
                throw new UnauthorizedAccessException(
                    "Access denied.");
            }

            DataTable table = new DataTable();

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "dbo.usp_Customer_GetLookup", conn))
            {
                cmd.CommandType =
                    CommandType.StoredProcedure;

                conn.Open();

                using (SqlDataAdapter adapter =
                    new SqlDataAdapter(cmd))
                {
                    adapter.Fill(table);
                }
            }

            return table;
        }
    }
}
