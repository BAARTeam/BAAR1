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
            Window.RequestFeature(WindowFeatures.NoTitle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            ImageButton button = FindViewById<ImageButton>(Resource.Id.scanButton);

            FindViewById<LinearLayout>(Resource.Id.main).SetBackgroundColor(Color.Argb(255,0,9,26));

            button.Click += (sender, e) =>
            {
                var NewScreen = new Intent(this, typeof(studentac));
                StartActivity(NewScreen);
            };

        }

        public static object MakeRequest(string RequestURL, string ContentType, string Method, string AuthHeader, bool ReturnAccessToken = false)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(RequestURL);
            request.ContentType = ContentType;
            request.Method = Method;
            request.Headers.Add(HttpRequestHeader.Authorization, AuthHeader);

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var content = reader.ReadToEnd();
                    if (string.IsNullOrWhiteSpace(content))
                    {
                        Console.Out.WriteLine("Response contained empty body...");
                    }
                    else
                    {
                        Console.Out.WriteLine("Response Body: \r\n {0}", content);
                    }

                    if (ReturnAccessToken)
                    {
                        AccessObject Token = JsonConvert.DeserializeObject<AccessObject>(content);
                        return Token;
                    }
                    return response;
                }
            }
        }
        public static object MakeRequest2(string Result)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://172.21.123.196/ws/schema/query/name?");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add(HttpRequestHeader.Authorization, string.Format("Bearer {0}", Login.Token.AccessToken));
            request.Accept = "application/json";


            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                //FIx this mess
                Console.WriteLine("Input String " + Result);
                double THing = Convert.ToDouble(Result);
                JsonPayload New = new JsonPayload();
                New.Number = THing;
                string Tests = (string)JsonConvert.SerializeObject(New);

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
                    if (string.IsNullOrWhiteSpace(content))
                    {
                        Console.Out.WriteLine("Response contained empty body...");
                    }
                    else
                    {
                        Console.WriteLine("Info Body: \r\n {0}", content);
                    }
                    return content;
                }
            }
        }
        public static object MakeRequest3(string QueryName,string StudentNumber)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(@"http://172.21.123.196/ws/schema/query/" +QueryName +"?");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add(HttpRequestHeader.Authorization, string.Format("Bearer {0}", Login.Token.AccessToken));
            request.Accept = "application/json";


            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                //FIx this mess
                Console.WriteLine("Input String " + StudentNumber);
                double THing = Convert.ToDouble(StudentNumber);
                JsonPayload New = new JsonPayload();
                New.Number = THing;
                string Tests = (string)JsonConvert.SerializeObject(New);

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
                    if (string.IsNullOrWhiteSpace(content))
                    {
                        Console.Out.WriteLine("Response contained empty body...");
                    }
                    else
                    {
                        Console.WriteLine("Info Body: \r\n {0}", content);
                    }
                    return content;
                }
            }
        }




    }
}



