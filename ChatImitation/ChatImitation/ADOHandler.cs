using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ChatImitation
{
    /// <summary>
    /// A class for providing datastore functionality
    /// </summary>
	public static class ADOHandler
	{
        /// <summary>
        /// A method that creates table in database if it doesn't exist for storing data
        /// </summary>
		public static void CreateDatabase()
		{
            bool exists = true;
            const string sqlStatement = @"SELECT * From ChatTable";
            using (SqlConnection con = new SqlConnection
					(ConfigurationManager.ConnectionStrings["ChatConnection"].ConnectionString))
			{
				string sql = "CREATE TABLE ChatTable(Name VARCHAR(15), Message VARCHAR(500));";
				con.Open();
                try
                {
                    SqlCommand command = new SqlCommand(sqlStatement, con);
                    command.ExecuteNonQuery();
                    exists = true;
                }
                catch
                {
                    exists = false;
                }
                if (exists == false)
                {
                    SqlCommand cmd = new SqlCommand(sql, con);
                    SqlDataReader dr = cmd.ExecuteReader();
                }
			}
		}
        /// <summary>
        /// A method for adding a new data to the datastore
        /// </summary>
        /// <param name="name"></param>
        /// <param name="message"></param>
		public static void AddEntry(string name, string message)
		{
			using (SqlConnection con = new SqlConnection
					(ConfigurationManager.ConnectionStrings["ChatConnection"].ConnectionString))
			{
                SqlCommand cmd = new SqlCommand("dbo.AddEntry", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Message", message);
                con.Open();
                cmd.ExecuteNonQuery();
            }
		}
        /// <summary>
        /// A method that returns DB's rows as a list of strings
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllEntries()
        {
            List<string> data = new List<string>();
            using (SqlConnection con = new SqlConnection
                    (ConfigurationManager.ConnectionStrings["ChatConnection"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("dbo.GetData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    data.Add(ReadSingleRow((IDataRecord)reader));
                }
                reader.Close();
                return data;
            }
        }
        /// <summary>
        /// A method that reads a single line from SqlDataReader object
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        private static string ReadSingleRow(IDataRecord record)
        {
            return String.Format("{0}: {1}", record[0], record[1]);
        }

    }
}
