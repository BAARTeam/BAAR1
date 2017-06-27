using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BAAR.Droid
{
    [Activity(Label = "student")]
    public class studentac : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.student);
            var STID = Intent.Extras.GetString("StudentID");
            TextView textv = FindViewById<TextView>(Resource.Id.stuName);
            textv.Text = STID; 
            
            // Create your application here
        }
    }
}