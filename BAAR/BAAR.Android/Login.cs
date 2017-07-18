using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Android.Content.PM;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Auth;

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
            button.Click +=  (sender, e) => {
               // var activity = this.BaseContext as Activity;
                // Get our button from the layout resource,
                // and attach an event to it
                // var Test = new OAuth2Request("GET");
                var authenticator = new OAuth2Authenticator(
                      "679b09c5-2498-45e3-a6b4-929516a7d732",
                      "read",
                      new Uri("https://www.facebook.com"),
                      new Uri("https://www.google.com"));
                authenticator.Completed += (sender1, args) =>
                {
                    Console.WriteLine("This is definately working!!");
                    if (args.IsAuthenticated)
                    {
                        var account = args.Account;
                        AlertDialog.Builder builder1 = new AlertDialog.Builder(this);
                        builder1.SetMessage(args.Account.Username);
                        AlertDialog alert11 = builder1.Create();
                        alert11.Show();
                        Console.WriteLine(account.Username);
                        // Do success work 
                    }
                    else
                    {
                        // The user cancelled 

                    }
                };
                    Console.Write("This is definately Not Working");
                    this.StartActivity(authenticator.GetUI(this));
              //  var NewScreen = new Intent(this, typeof(MainActivity));
               // StartActivity(NewScreen);
            };

        }

    }
}