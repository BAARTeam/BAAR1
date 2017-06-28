﻿using System;
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
    [Activity(Label = "Login", MainLauncher = true, Icon = "@drawable/icon")]
    public class Login : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);

            ImageButton button = FindViewById<ImageButton>(Resource.Id.DebugButton);
            button.Click +=  (sender, e) => {

                var NewScreen = new Intent(this, typeof(MainActivity));
                StartActivity(NewScreen);
            };

        }
    }
}