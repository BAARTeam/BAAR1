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
                /* // var activity = this.BaseContext as Activity;
                  // Get our button from the layout resource,
                  // and attach an event to it
                  // var Test = new OAuth2Request("GET");
                      Console.WriteLine("This is definately working!!");
                  UriBuilder Thing = new UriBuilder("172.21.123.196/public");
                  var authenticator = new OAuth2Authenticator(
                        "679b09c5-2498-45e3-a6b4-929516a7d732",
                        "read",
                        Thing.Uri,
                        new Uri("http://172.21.123.196/guardian/bulletin2.html"));


                  authenticator.Completed += (sender, args) =>
                  {
                      if (args.IsAuthenticated)
                      {
                          var account = args.Account;
                          Console.WriteLine("YOooooo");

                          // Do success work 
                      }
                      else
                      {
                          // The user cancelled 
                          Console.WriteLine("YOooooo");
                      }
                  };
                  authenticator.OnError("Blasphemy");
                      Console.Write("This is definately Not Working");
                      this.StartActivity(authenticator.GetUI(this));*/


                //var rxcui = "grant_type=client_credentials";
                //youre trying to pass the body as seen above but cant figure out how to get it in the request.
                //perhaps you need to just put in in the url

                AccessObject Test = (AccessObject)MainActivity.MakeRequest(string.Format(@"http://172.21.123.196/oauth/access_token?grant_type=client_credentials"), "application/x-www-form-urlencoded;charset=UTF-8", "POST", "Basic MTM2ZDZmNzEtYTYzOS00Nzc1LWIxMjktNDMwNWE4YjA4ZDZkOjQzNDFmMjNmLTczZTEtNGI4ZS1iMzNjLTVhMWQ5MTkyZDczNQ==", true);

                Console.WriteLine("Testing" + Test.AccessToken);
                var NewScreen = new Intent(this, typeof(MainActivity));
                StartActivity(NewScreen);
            };
        }
    }
}