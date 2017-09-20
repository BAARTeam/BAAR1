using System.Configuration;
using System.Data.SqlClient;
using System;
using BAAR.Droid;

namespace ProAPI
{
    /// This class is used to connect to sql server database  
    public class DBConnect
    {
        private static SqlConnection NewCon;
        private static string conStr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;  
        public static SqlConnection getConnection()
        {

            NewCon = new SqlConnection(conStr);
            return NewCon;
        }
        public DBConnect()
        {

        }
    }
}