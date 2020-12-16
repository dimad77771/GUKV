using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

namespace GUKV.Common
{
    public static class CommonUtils
    {
        /// <summary>
        /// Attempts to connect to the GUKV database
        /// </summary>
        /// <returns>Created connection object, or NULL if connection could not be established</returns>
        public static SqlConnection ConnectToDatabase()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["GUKVConnectionString"].ConnectionString;

            if (connectionString != null && connectionString.Length > 0)
            {
                SqlConnection connection = new SqlConnection(connectionString);

                try
                {
                    connection.Open();

                    return connection;
                }
                catch
                {
                    //
                }
            }

            return null;
        }


        public static SqlConnection ConnectToDatabase2016()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["GUKV2016ConnectionString"].ConnectionString;

            if (connectionString != null && connectionString.Length > 0)
            {
                SqlConnection connection = new SqlConnection(connectionString);

                try
                {
                    connection.Open();

                    return connection;
                }
                catch
                {
                    //
                }
            }

            return null;
        }
    }
}
