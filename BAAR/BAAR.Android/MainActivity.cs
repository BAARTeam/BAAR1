using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using Android.OS;
using ZXing.Mobile;
using Xamarin.Auth;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Android.Graphics;

namespace BAAR.Droid
{
    [Activity(Label = "BarcodeScanner", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            //removes title
            Window.RequestFeature(WindowFeatures.NoTitle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            //assigns button to the scan image button
            ImageButton button = FindViewById<ImageButton>(Resource.Id.scanButton);

            //sets main activity color
            FindViewById<LinearLayout>(Resource.Id.main).SetBackgroundColor(Color.Argb(255,0,9,26));

            //scan button click
            button.Click += (sender, e) =>
            {
                //button animation for click
                button.ScaleX = ((float)(.9));
                button.ScaleY = ((float)(.9));

                //opens the studentac page with tickets
                var NewScreen = new Intent(this, typeof(studentac));
                StartActivity(NewScreen);
            };

        }

        //function for the REST API Calls (Access Token)
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
        //REST API Calls for PowerQuery
        public static object MakeRequest3(string QueryName,string StudentNumber)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(@"http://172.21.123.196/ws/schema/query/" +QueryName +"?");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add(HttpRequestHeader.Authorization, string.Format("Bearer {0}", Login.Token.AccessToken));
            request.Accept = "application/json";


            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                double StudNum = Convert.ToDouble(StudentNumber);
                JsonPayload StudentNum = new JsonPayload();
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
}



