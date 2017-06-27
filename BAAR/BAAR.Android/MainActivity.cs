using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using ZXing.Mobile;
//using Xamarin.Auth;

namespace BAAR.Droid
{
	[Activity (Label = "BAAR.Android", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			ImageButton button = FindViewById<ImageButton> (Resource.Id.scanButton);
            

            /*var authenticator = new OAuth2Authenticator(
                clientId,
                null,
                Constants.Scope,
                new Uri(Constants.AuthorizeUrl),
                new Uri(redirectUri),
                new Uri(Constants.AccessTokenUrl),
                null,
                true);*/


            MobileBarcodeScanner.Initialize(Application);
            button.Click += async (sender, e) => {

                var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                var result = await scanner.Scan();

                var NewScreen = new Intent(this,typeof(studentac));
                NewScreen.PutExtra("StudentID", result.Text);
                StartActivity(NewScreen);
            };
        }
	}
}


