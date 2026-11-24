
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace NAOTR_POS.Data
{
    public static class DatabaseHelper
    {
        private static readonly string connectionString =
            ConfigurationManager
                .ConnectionStrings["POSConnection"]
                .ConnectionString;

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static bool TestConnection()
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                return conn.State == ConnectionState.Open;
            }
        }
    }
}
