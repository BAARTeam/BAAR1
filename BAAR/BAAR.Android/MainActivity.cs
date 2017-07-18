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
            MobileBarcodeScanner.Initialize(Application);
            ImageButton button = FindViewById<ImageButton>(Resource.Id.scanButton);
            button.Click += async (sender, e) =>
            {

                var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                var result = await scanner.Scan();

                var NewScreen = new Intent(this, typeof(studentac));
                NewScreen.PutExtra("StudentID", result.Text);
                StartActivity(NewScreen);
            };
          
           }

    public class Test : FormAuthenticator
        {
            public override Task<Account> SignInAsync(CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    } }


