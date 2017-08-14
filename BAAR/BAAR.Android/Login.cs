using System;
using Android.Content.PM;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Xamarin.Auth;
using System.Json;
using System.Data.SqlClient;
using Android.Graphics;

namespace BAAR.Droid
{
    [Activity(Label = "Going Pro", MainLauncher = true, Icon = "@drawable/GoingPro_Icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Login : Activity
    {
        public static AccessObject Token;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.RequestFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Login);
            FindViewById<LinearLayout>(Resource.Id.Login).SetBackgroundColor(Color.Argb(255, 0, 9, 26));

            SqlConnection conn = new SqlConnection(@"Data Source = webdb\webdb; Initial Catalog = MTSS_BadgePro; Integrated Security = False; User ID = mtss_admin; Password =KBhSIQXqZ8J^; Pooling = False");

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = conn.ConnectionString;

                connection.Open();
                SqlCommand sel = new SqlCommand("SELECT Login_PW FROM MTSS_LoginAccount WHERE Login_Name = 'jacobsalinas'",connection);
                string printthis= Convert.ToString(sel.ExecuteScalar());
                Console.WriteLine("yellow " + printthis);
                Console.WriteLine("State: {0}", connection.State);
                Console.WriteLine("ConnectionString: {0}",
                    connection.ConnectionString);
            }


            Button button = FindViewById<Button>(Resource.Id.button1);

            EditText Username = FindViewById<EditText>(Resource.Id.Username_Textbox);
            EditText Password = FindViewById<EditText>(Resource.Id.Password_Textbox);

            Username.SetBackgroundColor(Color.White);
            Username.SetTextColor(Color.Black);
            Password.SetBackgroundColor(Color.White);
            Password.SetTextColor(Color.Black);


            button.Click += (sender1, e) =>
            {
                //Requests an access token from powerschool that we use for getting data;
                Token = (AccessObject)MainActivity.MakeRequest(string.Format(@"http://172.21.123.196/oauth/access_token?grant_type=client_credentials"), "application/x-www-form-urlencoded;charset=UTF-8", "POST", "Basic ZThmMmViNjYtNDcwYy00YjZkLTlhYjItMDQ4OWM5NGJlNDEwOjJmY2U2MmY3LWVlZDMtNDAzYi04NWNhLWRjY2E5OTFjMGI2Nw==", true);

                // Create an intent allowing the program to change to a different page;
                var MainPage = new Intent(this, typeof(MainActivity));
                //Go to different page;
                StartActivity(MainPage);
            };
        }
    }
}