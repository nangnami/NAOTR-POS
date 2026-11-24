
using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;

using NAOTR_POS.Data;
using NAOTR_POS.Helpers;
using NAOTR_POS.Models;

namespace NAOTR_POS.Services
{
    public class UserService
    {
        // ================= ACCESS CONTROL =================

        private void EnsureAccess()
        {
            if (!Session.IsInRole("Admin"))
            {
                throw new UnauthorizedAccessException(
                    "Only administrators can manage users.");
            }
        }

        // ================= GET ROLES =================

        public DataTable GetRoles()
        {
            EnsureAccess();

            DataTable table = new DataTable();

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())

            using (SqlCommand cmd =
                new SqlCommand("dbo.usp_Role_GetAll", conn))
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

        // ================= GET USERS =================

        public DataTable GetUsers(
            string search = "",
            int roleId = 0,
            string status = "All")
        {
            EnsureAccess();

            DataTable table = new DataTable();

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())

            using (SqlCommand cmd =
                new SqlCommand("dbo.usp_User_GetAll", conn))
            {
                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@Search", SqlDbType.NVarChar, 100)
                    .Value = search ?? "";

                cmd.Parameters.Add(
                    "@RoleId", SqlDbType.Int)
                    .Value = roleId;

                cmd.Parameters.Add(
                    "@Status", SqlDbType.NVarChar, 20)
                    .Value = status;

                using (SqlDataAdapter adapter =
                    new SqlDataAdapter(cmd))
                {
                    adapter.Fill(table);
                }
            }

            return table;
        }

        // ================= GET USER BY ID =================

        public UserAccount GetUserById(int userId)
        {
            EnsureAccess();

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())

            using (SqlCommand cmd =
                new SqlCommand("dbo.usp_User_GetById", conn))
            {
                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@UserId", SqlDbType.Int)
                    .Value = userId;

                conn.Open();

                using (SqlDataReader reader =
                    cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    return new UserAccount
                    {
                        UserId = Convert.ToInt32(
                            reader["UserId"]),

                        Username = reader["Username"]
                            .ToString(),

                        FullName = reader["FullName"]
                            .ToString(),

                        RoleId = Convert.ToInt32(
                            reader["RoleId"]),

                        RoleName = reader["RoleName"]
                            .ToString(),

                        IsActive = Convert.ToBoolean(
                            reader["IsActive"]),

                        CreatedAt = Convert.ToDateTime(
                            reader["CreatedAt"]),

                        LastLoginAt =
                            reader["LastLoginAt"] == DBNull.Value
                            ? (DateTime?)null
                            : Convert.ToDateTime(
                                reader["LastLoginAt"])
                    };
                }
            }
        }

        // ================= GENERATE SALT =================

        private string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];

            using (RandomNumberGenerator rng =
                RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            return BitConverter
                .ToString(saltBytes)
                .Replace("-", "");
        }

        // ================= VALIDATE PASSWORD =================

        private void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException(
                    "Password is required.");
            }

            if (password.Length < 12)
            {
                throw new ArgumentException(
                    "Password must contain at least 12 characters.");
            }

            if (password.Length > 128)
            {
                throw new ArgumentException(
                    "Password cannot exceed 128 characters.");
            }
        }

        // ================= VALIDATE USER =================

        private void ValidateUser(UserAccount user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (string.IsNullOrWhiteSpace(user.Username))
            {
                throw new ArgumentException(
                    "Username is required.");
            }

            if (user.Username.Trim().Length > 50)
            {
                throw new ArgumentException(
                    "Username cannot exceed 50 characters.");
            }

            if (string.IsNullOrWhiteSpace(user.FullName))
            {
                throw new ArgumentException(
                    "Full name is required.");
            }

            if (user.FullName.Trim().Length > 100)
            {
                throw new ArgumentException(
                    "Full name cannot exceed 100 characters.");
            }

            if (user.RoleId <= 0)
            {
                throw new ArgumentException(
                    "Please select a valid role.");
            }
        }

        // ================= INSERT USER =================

        public int InsertUser(
            UserAccount user,
            string password)
        {
            EnsureAccess();

            ValidateUser(user);
            ValidatePassword(password);

            string salt = GenerateSalt();

            string hash =
                PasswordHelper.HashPassword(
                    password, salt);

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())

            using (SqlCommand cmd =
                new SqlCommand("dbo.usp_User_Insert", conn))
            {
                cmd.CommandType =
                    CommandType.StoredProcedure;

                AddUserParameters(cmd, user);

                cmd.Parameters.Add(
                    "@PasswordHash", SqlDbType.VarChar, 64)
                    .Value = hash;

                cmd.Parameters.Add(
                    "@Salt", SqlDbType.VarChar, 32)
                    .Value = salt;

                cmd.Parameters.Add(
                    "@ActorId", SqlDbType.Int)
                    .Value = Session.CurrentUser.UserId;

                conn.Open();

                return Convert.ToInt32(
                    cmd.ExecuteScalar());
            }
        }

        // ================= UPDATE USER =================

        public void UpdateUser(UserAccount user)
        {
            EnsureAccess();

            ValidateUser(user);

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())

            using (SqlCommand cmd =
                new SqlCommand("dbo.usp_User_Update", conn))
            {
                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@UserId", SqlDbType.Int)
                    .Value = user.UserId;

                AddUserParameters(cmd, user);

                cmd.Parameters.Add(
                    "@ActorId", SqlDbType.Int)
                    .Value = Session.CurrentUser.UserId;

                conn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        // ================= COMMON PARAMETERS =================

        private void AddUserParameters(
            SqlCommand cmd,
            UserAccount user)
        {
            cmd.Parameters.Add(
                "@Username", SqlDbType.NVarChar, 50)
                .Value = user.Username.Trim();

            cmd.Parameters.Add(
                "@FullName", SqlDbType.NVarChar, 100)
                .Value = user.FullName.Trim();

            cmd.Parameters.Add(
                "@RoleId", SqlDbType.Int)
                .Value = user.RoleId;
        }

        // ================= SET ACCOUNT STATUS =================

        public void SetStatus(
            int userId,
            bool isActive)
        {
            EnsureAccess();

            if (userId == Session.CurrentUser.UserId &&
                !isActive)
            {
                throw new InvalidOperationException(
                    "You cannot deactivate your own account.");
            }

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())

            using (SqlCommand cmd =
                new SqlCommand("dbo.usp_User_SetStatus", conn))
            {
                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@UserId", SqlDbType.Int)
                    .Value = userId;

                cmd.Parameters.Add(
                    "@IsActive", SqlDbType.Bit)
                    .Value = isActive;

                cmd.Parameters.Add(
                    "@ActorId", SqlDbType.Int)
                    .Value = Session.CurrentUser.UserId;

                conn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        // ================= RESET PASSWORD =================

        public void ResetPassword(
            int userId,
            string newPassword)
        {
            EnsureAccess();

            ValidatePassword(newPassword);

            string salt = GenerateSalt();

            string hash =
                PasswordHelper.HashPassword(
                    newPassword, salt);

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())

            using (SqlCommand cmd =
                new SqlCommand(
                    "dbo.usp_User_ResetPassword", conn))
            {
                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@UserId", SqlDbType.Int)
                    .Value = userId;

                cmd.Parameters.Add(
                    "@PasswordHash", SqlDbType.VarChar, 64)
                    .Value = hash;

                cmd.Parameters.Add(
                    "@Salt", SqlDbType.VarChar, 32)
                    .Value = salt;

                cmd.Parameters.Add(
                    "@ActorId", SqlDbType.Int)
                    .Value = Session.CurrentUser.UserId;

                conn.Open();

                cmd.ExecuteNonQuery();
            }
        }
    }
}
