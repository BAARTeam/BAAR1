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
using Newtonsoft.Json;

namespace BAAR.Droid
{
    public class AccessObject
    {
        [JsonProperty("access_token")]
        public string AccessToken;
        [JsonProperty("token_type")]
        public string Bearer;
        [JsonProperty("expires_in")]
        public string Expiration;
    }
}