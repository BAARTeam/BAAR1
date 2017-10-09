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
using System.Security.Cryptography;

namespace BAAR.Droid
{
    [Activity(Label = "Going Pro", MainLauncher = true, Icon = "@drawable/GoingPro_Icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Login : Activity
    {
        //used to use the token across forms
        public static AccessObject Token;
        //used to send the signed in users names to the log in the studentac form
        public static string StaffFirst;
        public static string StaffLast;
        public static string StaffEmail;
        public static string StaffUserName;

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
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://goingpro.azurewebsites.net/api/Logins?loginid=" + Username.Text);
                    request.Method = "Get";
                    //request.ContentType = @"application/json";
                    request.Accept = @"application/json";

                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    Stream stream = response.GetResponseStream();
                    StreamReader getjson = new StreamReader(stream);

                    String getcontent = getjson.ReadToEnd();

                    using (response)
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            Console.WriteLine("Error fetching data.  Server returned status code " + response.StatusCode);
                        }
                        else
                        {
                            //unhash password and check against TBPassword.Text
                            LoginInfo thisinfo = JsonConvert.DeserializeObject<LoginInfo>(getcontent);
                            if (!checkpassword(thisinfo.Login_Password, Password.Text))
                            {
                                Toast.MakeText(this, "Wrong Password", ToastLength.Long).Show();
                            }
                            else
                            {
                                //Requests an access token from powerschool that we use for getting data;
                                Token = (AccessObject)MainActivity.MakeRequest(string.Format(@"http://powerschool.kentisd.org/oauth/access_token?grant_type=client_credentials"), "application/x-www-form-urlencoded;charset=UTF-8", "POST", "Basic ZWRlMjY4ZmMtOTM5Mi00Y2NkLTgxNjktNjk2ZjI0YmNjZTU2OmU5MDRlNzYwLTEzZjQtNDY5My1iYWM5LWIwZTMyYTJhM2Y3Ng==", true);
                                Toast.MakeText(this,"Login Successful",ToastLength.Long).Show();

                                StaffFirst = thisinfo.First_Name;
                                StaffLast = thisinfo.Last_Name;
                                StaffEmail = thisinfo.Email;
                                StaffUserName = Username.Text;
                                // Create an intent allowing the program to change to a different page;
                                var MainPage = new Intent(this, typeof(MainActivity));
                                //Go to different page;
                                StartActivity(MainPage);
                            }
                        }
                    }
                }
                catch
                {
                    Toast.MakeText(this.ApplicationContext, "Could not Connect to Service (Perhaps Wifi or Powerschool is down.)", ToastLength.Long).Show();
                    return;
                }
            };
        }

        public bool checkpassword(string hashpw, string PW)
        {
            //Extract the bytes
            byte[] hashBytes = Convert.FromBase64String(hashpw);
            //Get the salt
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            //Compute the hash on the password the user entered
            var pbkdf2 = new Rfc2898DeriveBytes(PW, salt, 10000);
            byte[] hashentered = pbkdf2.GetBytes(20);
            //Compare the hashentered to the hash retrieved from db
            bool PWAuthorized = true;
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hashentered[i])
                {
                    PWAuthorized = false;
                }
            }
            return PWAuthorized;
        }

        public override void OnBackPressed()
        {
            //exits app on back button pressed.
            MoveTaskToBack(true);
        }
    }

    public class LoginInfo
    {
        public long login_db_id { get; set; }
        public string Distric { get; set; }
        public string Building { get; set; }
        public string Login_ID { get; set; }
        public string Login_Password { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }

        public LoginInfo(long dbid, string Dis, string bldg, string ID, string PW, string First, string Last, string email)
        {
            this.login_db_id = dbid;
            this.Distric = Dis;
            this.Building = bldg;
            this.Login_ID = ID;
            this.Login_Password = PW;
            this.First_Name = First;
            this.Last_Name = Last;
            this.Email = email;
        }



    }

    public class BehaviorLogInfo
    {
        public long log_db_id { get; set; }
        public DateTime LogDateTime { get; set; }
        public string District { get; set; }
        public string Building { get; set; }
        public string Student_ID { get; set; }
        public string Student_First_Name { get; set; }
        public string Student_Last_Name { get; set; }
        public string Behavior { get; set; }
        public string Behavior_Location { get; set; }
        public string Staff_Login_ID { get; set; }

        public BehaviorLogInfo(long dbid, DateTime dt, string Dist, string bldg, string studid, string First, string Last, string behav, string behavLoc, string staffid)
        {
            this.log_db_id = dbid;
            this.LogDateTime = dt;
            this.District = Dist;
            this.Building = bldg;
            this.Student_ID = studid;
            this.Student_First_Name = First;
            this.Student_Last_Name = Last;
            this.Behavior = behav;
            this.Behavior_Location = behavLoc;
            this.Staff_Login_ID = staffid;
        }
    }

}