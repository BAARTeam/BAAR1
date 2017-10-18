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
                int Quote = StringToGet.IndexOf(Find) + Find.Length + 2;
                if (StringToGet[Quote]=='"')
                {
                    int Third = StringToGet.IndexOf('"', Quote+1);
                    string Testing = StringToGet.Substring(Quote+1, Third - Quote-1);
                    
                    return Testing;
                }
                else
                {
                    return null;
                }
            }
            return StringToGet;
        }
    }
}