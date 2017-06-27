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
<<<<<<< HEAD
// is a push test
=======
//this is a push 
>>>>>>> 66e91577fcb78d3b295e4c457a1de81a8895a32a
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
            TextView tev = FindViewById<TextView>(Resource.Id.stuName);
            tev.Text = STID; 
            
            // Create your application here
        }
    }
}