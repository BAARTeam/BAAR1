using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAAR.iOS
{
    static class Utilities
    {
        //parses the json content for the strings needed
        public static string GetStringOut(this string StringToGet, string Find)
        {

            if (StringToGet.Contains(Find))
            {
                int Quote = StringToGet.IndexOf(Find) + Find.Length + 2;
                if (StringToGet[Quote] == '"')
                {
                    int Third = StringToGet.IndexOf('"', Quote + 1);
                    string Testing = StringToGet.Substring(Quote + 1, Third - Quote - 1);

                    return Testing;
                }
                else
                {
                    return "WHOPS";
                }
            }
            return StringToGet;
        }
    }
}