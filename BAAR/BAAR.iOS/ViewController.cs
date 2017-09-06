using System;
using System.Data.SqlClient;
using UIKit;

namespace BAAR.iOS
{

    public partial class ViewController : UIViewController
	{
		int count = 1;
        public static SqlConnection conn = new SqlConnection(@"Data Source = webdb\webdb; Initial Catalog = MTSS_BadgePro; Integrated Security = False; User ID = mtss_admin; Password =KBhSIQXqZ8J^; Pooling = False");

		public ViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            Login.AccessibilityIdentifier = "myButton";
            Login.TouchUpInside += delegate
            {//the password grabbed from the DB
                Console.WriteLine("What is this");
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
                        Console.WriteLine(pass);
                    }
                }
                catch
                {
                    // Toast.MakeText(this.ApplicationContext, "Could Not Connect", ToastLength.Long).Show();
                    //  return;
                }

                //checks if the passwords match and the query did not return ""
                if (pass == Password.Text && !string.IsNullOrEmpty(Password.Text) && pass != null)
                {
                    try
                    {

                        //Requests an access token from powerschool that we use for getting data;
                        //    Token = (AccessObject)MainActivity.MakeRequest(string.Format(@"http://172.21.123.196/oauth/access_token?grant_type=client_credentials"), "application/x-www-form-urlencoded;charset=UTF-8", "POST", "Basic ZThmMmViNjYtNDcwYy00YjZkLTlhYjItMDQ4OWM5NGJlNDEwOjJmY2U2MmY3LWVlZDMtNDAzYi04NWNhLWRjY2E5OTFjMGI2Nw==", true);

                        //saves the logged in users information
                        conn.Open();
                        SqlCommand SF = new SqlCommand("SELECT First_Name FROM MTSS_LoginAccount WHERE Login_Name=@ln", conn);
                        SF.Parameters.AddWithValue("@ln", Username.Text);
                        // StaffFirst = SF.ExecuteScalar().ToString();

                        SqlCommand SL = new SqlCommand("SELECT Last_Name FROM MTSS_LoginAccount WHERE Login_Name=@ln", conn);
                        SL.Parameters.AddWithValue("@ln", Username.Text);
                        //  StaffLast = SL.ExecuteScalar().ToString();

                        SqlCommand SE = new SqlCommand("SELECT Allow_Email FROM MTSS_LoginAccount WHERE Login_Name=@ln", conn);
                        SE.Parameters.AddWithValue("@ln", Username.Text);
                        //  StaffEmail = SE.ExecuteScalar().ToString();
                        conn.Close();
                    }
                    catch
                    {
                        // Toast.MakeText(this.ApplicationContext, "Could Not Connect", ToastLength.Long).Show();
                        return;
                    }

                    // Create an intent allowing the program to change to a different page;
                    // var MainPage = new Intent(this, typeof(MainActivity));
                    //Go to different page;
                    // StartActivity(MainPage);
                    Login.SetTitle("Login Successful", UIControlState.Normal);
                   // var _settingsStoryboard = UIStoryboard.FromName("ButtonPage", null);
                   // var initialViewController = _settingsStoryboard.InstantiateInitialViewController() as UIViewController;
                  //  initialViewController.LoadView();
                }
                else
                {
                    //Pops up on screen to signify when user inputs incorrect password
                    // Toast.MakeText(this, "Incorrect Password", ToastLength.Short).Show();
                }
            };
        }

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

