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
    static class Utilities
    {
        //parses the json content for the strings needed
        public static string GetStringOut(this string StringToGet,string Find)
        {

            if (StringToGet.Contains(Find))
            {
                int Test = StringToGet.IndexOf(Find) + Find.Length + 3;
                int Third = StringToGet.IndexOf('"',Test);
                string Testing = StringToGet.Substring(Test,Third - Test);
                return Testing;
            }
            return StringToGet;
        }
    }
}