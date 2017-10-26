using System;
using System.Data.SqlClient;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using UIKit;
using ToastIOS;
using System.Security.Cryptography;

namespace BAAR.iOS
{

    public partial class ViewController : UIViewController
    {

        public static AccessObject Token;
        public static string StaffFirst;
        public static string StaffLast;
        public static string StaffEmail;
        public static string StaffUserName;
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            Login.AccessibilityIdentifier = "myButton";
            Login.TouchUpInside += delegate
            {
               try
                {
                    Password.ResignFirstResponder();
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://goingpro.azurewebsites.net/api/Logins?loginid=" + UserNameTextField.Text);
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
                                Console.WriteLine("Wrong!");
                                Toast.MakeText("Password Incorrect").Show();
                            }
                            else
                            {
                                //Requests an access token from powerschool that we use for getting data;
                                Token = (AccessObject)MakeRequest(string.Format(@"http://powerschool.kentisd.org/oauth/access_token?grant_type=client_credentials"), "application/x-www-form-urlencoded;charset=UTF-8", "POST", "Basic ZWRlMjY4ZmMtOTM5Mi00Y2NkLTgxNjktNjk2ZjI0YmNjZTU2OmU5MDRlNzYwLTEzZjQtNDY5My1iYWM5LWIwZTMyYTJhM2Y3Ng==", true);

                                StaffFirst = thisinfo.First_Name;
                                StaffLast = thisinfo.Last_Name;
                                StaffEmail = thisinfo.Email;
                                StaffUserName = UserNameTextField.Text;

                                Toast.MakeText("Login Successful").Show();
                                PerformSegue("GoToButtonPage",this.Self);
                            }
                        }
                    }
                }
               catch
                {
                    Toast.MakeText("Could Not Connect To Service (Perhaps Powerschool or Wi-Fi is down.)").Show();
                    return;
                }
            };
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            return UIInterfaceOrientationMask.Portrait;
        }

        public override UIInterfaceOrientation PreferredInterfaceOrientationForPresentation()
        {
            return UIInterfaceOrientation.Portrait;
        }
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
        public override bool ShouldAutorotate()
        {
            return false;
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

        public static object MakeRequest(string RequestURL, string ContentType, string Method, string AuthHeader, bool ReturnAccessToken = false)
        {
            //builds request
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(RequestURL);
            request.ContentType = ContentType;
            request.Method = Method;
            //passes in clientid+secret
            request.Headers.Add(HttpRequestHeader.Authorization, AuthHeader);

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                //reads response
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var content = reader.ReadToEnd();

                    if (ReturnAccessToken)
                    {
                        AccessObject Token = JsonConvert.DeserializeObject<AccessObject>(content);
                        return Token;
                    }
                    return response;
                }
            }
        }

        public static object MakeRequest3(string QueryName, string StudentNumber)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(@"http://powerschool.kentisd.org/ws/schema/query/" + QueryName + "?");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add(HttpRequestHeader.Authorization, string.Format("Bearer {0}", Token.AccessToken));
            request.Accept = "application/json";


            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                double StudNum = Convert.ToDouble(StudentNumber);
                StudentAccount.JsonPayload StudentNum = new StudentAccount.JsonPayload();
                StudentNum.Number = StudNum;
                string Tests = (string)JsonConvert.SerializeObject(StudentNum);

                streamWriter.Write(Tests);
                streamWriter.Flush();
                streamWriter.Close();
            }

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var content = reader.ReadToEnd();

                    return content;
                }
            }

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

