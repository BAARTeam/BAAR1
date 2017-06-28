using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            tev.Text = STID; 
            
            // Create your application here
        }
    }
}