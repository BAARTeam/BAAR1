using System;
using Android.Content.PM;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Auth;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace BAAR.Droid
{
    [Activity(Label = "BAAR", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Login : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.RequestFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Login);

            ImageButton button = FindViewById<ImageButton>(Resource.Id.DebugButton);
            button.Click += (sender1, e) =>
            {
                AccessObject Test = (AccessObject)MainActivity.MakeRequest(string.Format(@"http://172.21.123.196/oauth/access_token?grant_type=client_credentials"), "application/x-www-form-urlencoded;charset=UTF-8", "POST", "Basic MTM2ZDZmNzEtYTYzOS00Nzc1LWIxMjktNDMwNWE4YjA4ZDZkOjQzNDFmMjNmLTczZTEtNGI4ZS1iMzNjLTVhMWQ5MTkyZDczNQ==", true);

                Console.WriteLine("Testing" + Test.AccessToken);
                var NewScreen = new Intent(this, typeof(MainActivity));
                StartActivity(NewScreen);
            };
        }
    }
}