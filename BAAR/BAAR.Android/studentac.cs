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

namespace BAAR.Droid
{
    [Activity(Label = "student",ScreenOrientation = ScreenOrientation.Portrait)]
    public class studentac : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            
            SetContentView(Resource.Layout.student);
            var STID = Intent.Extras.GetString("StudentID");
            TextView tev = FindViewById<TextView>(Resource.Id.stuName);
            string Splitter = @";";
            string[] STInfo = Regex.Split(STID, Splitter);
            tev.Text = STInfo[0];
            Button EmailButton = FindViewById<Button>(Resource.Id.EmailButton);
            EmailButton.Click +=  delegate
            {
                SendEmail();
            };
            
            // Create your application here
        }

        private void SendEmail()
        {
            var Email = new Intent(Android.Content.Intent.ActionSend);
            Email.PutExtra(Android.Content.Intent.ExtraEmail, new string[] {"dakotastickney@gmail.com" });
            Email.PutExtra(Android.Content.Intent.ExtraText,"Congratulations Your Kid has done something to grant you this email!");
            Email.SetType("message/rfc822");
        }
    }
}