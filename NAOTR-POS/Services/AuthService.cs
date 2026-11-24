
using System;
using System.Data;
using System.Data.SqlClient;
using NAOTR_POS.Data;
using NAOTR_POS.Helpers;
using NAOTR_POS.Models;

namespace NAOTR_POS.Services
{
    public class AuthService
    {
        public User Login(string username, string password)
        {
            User user = null;

            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (SqlCommand cmd =
                    new SqlCommand("dbo.usp_Login", conn))
                {
                    cmd.CommandType =
                        CommandType.StoredProcedure;

                    cmd.Parameters.Add(
                        "@Username",
                        SqlDbType.NVarChar, 50).Value = username;

                    using (SqlDataReader reader =
                        cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;

                        bool isActive =
                            Convert.ToBoolean(reader["IsActive"]);

                        if (!isActive)
                            return null;

                        string storedHash =
                            reader["PasswordHash"].ToString();

                        string salt =
                            reader["Salt"].ToString();

                        bool validPassword =
                            PasswordHelper.VerifyPassword(
                                password,
                                salt,
                                storedHash);

                        if (!validPassword)
                            return null;

                        user = new User
                        {
                            UserId = Convert.ToInt32(
                                reader["UserId"]),

                            Username =
                                reader["Username"].ToString(),

                            FullName =
                                reader["FullName"].ToString(),

                            RoleId = Convert.ToInt32(
                                reader["RoleId"]),

                            RoleName =
                                reader["RoleName"].ToString(),

                            IsActive = isActive
                        };
                    }
                }

                using (SqlCommand cmd =
                    new SqlCommand(
                        "dbo.usp_User_TouchLogin", conn))
                {
                    cmd.CommandType =
                        CommandType.StoredProcedure;

                    cmd.Parameters.Add(
                        "@UserId",
                        SqlDbType.Int).Value = user.UserId;

                    cmd.ExecuteNonQuery();
                }
            }

            return user;
        }
    }
}
