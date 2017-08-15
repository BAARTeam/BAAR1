using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BAAR.Droid
{
    public class log
    {
        public log(string SF, string SL, string SN, double Num, string AType, string ALocation)
        {
            this.StaffF = SF;
            this.StaffL = SL;
            this.Student_Name = SN;
            this.Student_Number = Num;
            this.Action_Type = AType;
            this.Action_Location = ALocation;
        }
        private string StaffF { get; set; }

        private string StaffL { get; set; }

        private string Student_Name { get; set; }

        private double Student_Number { get; set; }

        private string Action_Type { get; set; }

        private string Action_Location { get; set; }

        public void exe(SqlCommand sql)
        {
            sql.Parameters.AddWithValue("@DT", DateTime.Now);

            sql.Parameters.AddWithValue("@SF", StaffF);

            sql.Parameters.AddWithValue("@SL", StaffL);

            sql.Parameters.AddWithValue("@SN", Student_Name);

            sql.Parameters.AddWithValue("@StN", Student_Number);

            sql.Parameters.AddWithValue("@ATi", DateTime.Today);

            sql.Parameters.AddWithValue("@AT", Action_Type);

            sql.Parameters.AddWithValue("@AL", Action_Location);

            sql.ExecuteNonQuery();
        }
    }
}