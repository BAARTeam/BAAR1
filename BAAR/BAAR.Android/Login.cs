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

namespace BAAR.Droid
{
    [Activity(Label = "High Five", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Login : Activity
    {
        public static AccessObject Token;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.RequestFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Login);


            ImageButton button = FindViewById<ImageButton>(Resource.Id.DebugButton);
            button.Click += (sender1, e) =>
            {
                Token = (AccessObject)MainActivity.MakeRequest(string.Format(@"http://172.21.123.196/oauth/access_token?grant_type=client_credentials"), "application/x-www-form-urlencoded;charset=UTF-8", "POST", "Basic M2VmOGZlMWQtNmVhNC00N2ZlLTljMDItN2VmYWUzMGEwOGJkOjdmZWVmZGZkLTA1MzEtNGI1NC04NGQ5LTMzY2UwZDc3NTAxYw==", true);

                // Create an intent allowing the program to change to a different page;
                var MainPage = new Intent(this, typeof(MainActivity));
                //Go to different page;
                StartActivity(MainPage);
            };
        }
    }
}