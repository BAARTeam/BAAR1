﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Android.Content.PM;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net.Mail;

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
            TextView TID = FindViewById<TextView>(Resource.Id.stuID);
            TextView TName = FindViewById<TextView>(Resource.Id.stuName);
            string Splitter = @";";
            string[] STInfo = Regex.Split(STID, Splitter);
            TID.Text = STInfo[0];
            TName.Text = STInfo[1];
            Button EmailButton = FindViewById<Button>(Resource.Id.EmailButton);
            EmailButton.Click += (sender, e) =>
            {
                SendEmail();
            };

            Spinner BehaviourSpinner;
            BehaviourSpinner = FindViewById<Spinner>(Resource.Id.Behaviour_Spinner);
            var Behaviours = new List<string>() { "Cleaned Up", "COmplimented", "Turned In Assignment" };
            var adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, Behaviours);
            BehaviourSpinner.Adapter = adapter;
            Spinner LocationSpinner;
            LocationSpinner = FindViewById<Spinner>(Resource.Id.Location_Spinner);
            var Locations = new List<string>() { "E-Wing", "Commons", "Main Office" };
            var adapter2 = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, Locations);
            LocationSpinner.Adapter = adapter2;

            Spinner TestSpin = new Spinner(this);
            TextView NewView = new TextView(this);

            TextView NewView2 = new TextView(this);
            NewView.Text = STInfo[0];
            NewView2.Text = STInfo[1];
            RelativeLayout Test = FindViewById<RelativeLayout>(Resource.Id.StudentTable);
            var param = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.FillParent,
    ViewGroup.LayoutParams.WrapContent);
            param.AddRule(LayoutRules.AlignParentTop);
            Test.AddView(NewView,param);
            var param2 = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent,
ViewGroup.LayoutParams.WrapContent);
            param.AddRule(LayoutRules.Below, NewView.Id);
            Test.AddView(NewView2,param2);
        }

        private void SendEmail()
        {

            MailMessage message = new System.Net.Mail.MailMessage();
            string toEmail = "dakotastickney@gmail.com";
            message.From = new MailAddress("dakotastickney@gmail.com");
            message.To.Add(toEmail);
            message.Subject = "Test Email";
            message.Body = "Congratulations Your Kid has done something to grant you this email!";
            message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            using (var Client = new SmtpClient("smtp.gmail.com",587))
            {
                // Client.SmtpServer = "MyMailServer";
                Client.EnableSsl = true;
                Client.UseDefaultCredentials = true;
                Client.Send(message);
            }

            Console.Write("Sending Email");
         //   var Email = new Intent(Android.Content.Intent.ActionSend);
            //Email.PutExtra(Android.Content.Intent.ExtraEmail, new string[] {"dakotastickney@gmail.com" });
           // Email.PutExtra(Android.Content.Intent.ExtraSubject,"Testing");
           // Email.PutExtra(Android.Content.Intent.ExtraText,"Congratulations Your Kid has done something to grant you this email!");
            //Email.SetType("message/rfc822");
           // StartActivity(Email);
        }
    }
}