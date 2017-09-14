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
    [Activity(Label = "Going Pro", MainLauncher = false, Icon = "@drawable/GoingPro_Icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Login : Activity
    {

        //used to use the token across forms
        public static AccessObject Token;
        //the sql connection used to connect to the KISD Database webdb\webdb
        public static SqlConnection conn = new SqlConnection(@"Data Source = webdb\webdb; Initial Catalog = MTSS_BadgePro; Integrated Security = False; User ID = mtss_admin; Password =KBhSIQXqZ8J^; Pooling = False");
        //used to send the signed in users names to the log in the studentac form
        public static string StaffFirst;
        public static string StaffLast;
        public static string StaffEmail;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //removes title
            Window.RequestFeature(WindowFeatures.NoTitle);
            //brings us to login page
            SetContentView(Resource.Layout.Login);
            //changes login page color
            FindViewById<LinearLayout>(Resource.Id.Login).SetBackgroundColor(Color.Argb(255, 0, 9, 26));

            //collects info from the login text boxes
            EditText Username = FindViewById<EditText>(Resource.Id.Username_Textbox);
            EditText Password = FindViewById<EditText>(Resource.Id.Password_Textbox);

            //assigns the login button
            Button button = FindViewById<Button>(Resource.Id.button1);
            //set button color
            button.SetTextColor(Color.White);

            //sets text box colors
            Username.SetBackgroundColor(Color.White);
            Username.SetTextColor(Color.Black);
            Password.SetBackgroundColor(Color.White);
            Password.SetTextColor(Color.Black);

            //login button
            button.Click += (sender1, e) =>
            {
                //the password grabbed from the DB
                string pass = null;
                try
                {
                    using (SqlConnection connection = new SqlConnection())
                    {
                        connection.ConnectionString = conn.ConnectionString;

                        connection.Open();

                        //gets the password the matches the username the user entered, if no match, "" is returned
                        SqlCommand getpassword = new SqlCommand("SELECT Login_PW FROM MTSS_LoginAccount WHERE Login_Name=@LN", connection);
                        getpassword.Parameters.AddWithValue("@LN", Username.Text);
                        //sets pass as the grabbed password
                        pass = Convert.ToString(getpassword.ExecuteScalar());
                    }
                }
                catch
                {
                    Toast.MakeText(this.ApplicationContext, "Could Not Connect", ToastLength.Long).Show();
                    return;
                }

                //checks if the passwords match and the query did not return ""
                if (pass == Password.Text && !string.IsNullOrEmpty(Password.Text) && pass != null)
                {
                    try
                    {

                        //Requests an access token from powerschool that we use for getting data;
                        Token = (AccessObject)MainActivity.MakeRequest(string.Format(@"http://powerschool.kentisd.org/oauth/access_token?grant_type=client_credentials"), "application/x-www-form-urlencoded;charset=UTF-8", "POST", "Basic ZWRlMjY4ZmMtOTM5Mi00Y2NkLTgxNjktNjk2ZjI0YmNjZTU2OmU5MDRlNzYwLTEzZjQtNDY5My1iYWM5LWIwZTMyYTJhM2Y3Ng==", true);

                        //saves the logged in users information
                        conn.Open();
                        SqlCommand SF = new SqlCommand("SELECT First_Name FROM MTSS_LoginAccount WHERE Login_Name=@ln", conn);
                        SF.Parameters.AddWithValue("@ln", Username.Text);
                        StaffFirst = SF.ExecuteScalar().ToString();

                        SqlCommand SL = new SqlCommand("SELECT Last_Name FROM MTSS_LoginAccount WHERE Login_Name=@ln", conn);
                        SL.Parameters.AddWithValue("@ln", Username.Text);
                        StaffLast = SL.ExecuteScalar().ToString();

                        SqlCommand SE = new SqlCommand("SELECT Allow_Email FROM MTSS_LoginAccount WHERE Login_Name=@ln", conn);
                        SE.Parameters.AddWithValue("@ln", Username.Text);
                        StaffEmail = SE.ExecuteScalar().ToString();
                        conn.Close();
                    }
                    catch
                    {
                        Toast.MakeText(this.ApplicationContext, "Could Not Connect", ToastLength.Long).Show();
                        return;
                    }
                    
                    // Create an intent allowing the program to change to a different page;
                    var MainPage = new Intent(this, typeof(MainActivity));
                    //Go to different page;
                    StartActivity(MainPage);
                }
                else
                {
                    //Pops up on screen to signify when user inputs incorrect password
                    Toast.MakeText(this, "Incorrect Password", ToastLength.Short).Show();
                }
            };
        }

        public override void OnBackPressed()
        {
            //exits app on back button pressed.
            MoveTaskToBack(true);
        }
    }
}