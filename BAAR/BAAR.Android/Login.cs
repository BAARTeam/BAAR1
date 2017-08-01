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
using System.Json;

namespace BAAR.Droid
{
    [Activity(Label = "GoingPro", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Login : Activity
    {
        public static AccessObject Test;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.RequestFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Login);

            ImageButton button = FindViewById<ImageButton>(Resource.Id.DebugButton);
            button.Click += (sender1, e) =>
            {

                Test = (AccessObject)MainActivity.MakeRequest(string.Format(@"http://172.21.123.196/oauth/access_token?grant_type=client_credentials"), "application/x-www-form-urlencoded;charset=UTF-8", "POST", "Basic M2VmOGZlMWQtNmVhNC00N2ZlLTljMDItN2VmYWUzMGEwOGJkOjdmZWVmZGZkLTA1MzEtNGI1NC04NGQ5LTMzY2UwZDc3NTAxYw==", true);

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://172.21.123.196/ws/schema/query/pqgemail");
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add(HttpRequestHeader.Authorization, string.Format("Bearer {0}", Login.Test.AccessToken));
                request.Accept = "application/json";


                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = "{\"scannedbarcode\": 12000}";

                    streamWriter.Write(json);
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
                        content = content.Substring(content.IndexOf("guardianemail"));
                        content = content.Substring(content.IndexOf(":") + 2);
                        content = content.Remove(content.IndexOf('"'));
                        content = content.Remove(content.IndexOf('}'));
                        Console.WriteLine("email here " + content);
                    }
                }

                var NewScreen = new Intent(this, typeof(MainActivity));
                StartActivity(NewScreen);
            };
        }
    }
}