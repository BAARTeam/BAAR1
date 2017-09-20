using System;
using System.Data;
using System.Data.SqlClient;

namespace ProAPI
{
    public class SAPI : ISAPI

    {
        SqlConnection dbConnection;
        public SAPI()
        {
            dbConnection = DBConnect.getConnection();
        }
        public void CreateNewAccount(string Name, string userName, string password, string PhoneNumber, string CNIC)
        {
            if (dbConnection.State.ToString() == "Closed")
            {
                dbConnection.Open();
            }

            //string query = "SELECT INTO UserDetails VALUES ('" + Name + "','" + userName + "','" + password + "','" + PhoneNumber + "','" + CNIC + "');";
            SqlCommand command = new SqlCommand("SELECT Login_PW FROM MTSS_LoginAccount WHERE Login_Name=@LN", dbConnection);
            command.Parameters.AddWithValue("@LN", userName);

            //SqlCommand command = new SqlCommand(query, dbConnection);
            command.ExecuteNonQuery();


            dbConnection.Close();
        }
    }
}