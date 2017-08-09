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
        public static string GetStringOut(this string StringToGet,string Find)
        {

            if (StringToGet.Contains(Find))
            {
                int Test = StringToGet.IndexOf(Find) + Find.Length + 3;
                int Third = StringToGet.IndexOf('"',Test);
                string Testing = StringToGet.Substring(Test,Third - Test);
                return Testing;
            }

           // StringToGet = StringToGet.Substring(StringToGet.IndexOf(Find));
           // StringToGet = StringToGet.Substring(StringToGet.IndexOf(":") + 2);

           // StringToGet = StringToGet.Remove(StringToGet.IndexOf('"'));
           // StringToGet = StringToGet.Remove(StringToGet.IndexOf('}'));

            return StringToGet;
        }
    }
}