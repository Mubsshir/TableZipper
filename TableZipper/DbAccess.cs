using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace TableZipper
{
    class DbAccess
    {
        private static string ConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        public static DataTable Get_table(int skip, int fetchNext)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("USP_GET_MeterData", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@offset", skip);
                    cmd.Parameters.AddWithValue("@viewcount", fetchNext);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.ReadKey();
            }
            return dt;
        }
    }
}