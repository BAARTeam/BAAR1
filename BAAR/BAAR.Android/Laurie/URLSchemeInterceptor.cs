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
using Xamarin.Auth;

namespace BAAR
{
    [Activity(Label = "URLSchemeInterceptor",
        NoHistory =true, LaunchMode = Android.Content.PM.LaunchMode.SingleTop)]
    [IntentFilter (
        actions: new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataSchemes = new[] { "org.Kentisd.Powerschool.MTSS.redirectURL" },
        DataPath = "/oauth2redirect")]
    public class URLSchemeInterceptor : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            global::Android.Net.Uri uri_android = Intent.Data;

            //Convert Android.Net.Url to C#/netxf/BCL System.Uri - common API
            Uri uri_netfx = new Uri (uri_android.ToString());

            //load redirect_url Page for parsing
            
            AuthenticationState.Authenticator.onPageLoading(url_netfx);         

        }
    }
}