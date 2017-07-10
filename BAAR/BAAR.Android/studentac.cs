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
          //  TextView TID = FindViewById<TextView>(Resource.Id.stuID);
           // TextView TName = FindViewById<TextView>(Resource.Id.stuName);
            string Splitter = @";";
            string[] STInfo = Regex.Split(STID, Splitter);
           // TID.Text = STInfo[0];
           // TName.Text = STInfo[1];
            Button EmailButton = FindViewById<Button>(Resource.Id.EmailButton);
            EmailButton.Click += (sender, e) =>
            {
                SendEmail();
            };

            Spinner BehaviourSpinner = new Spinner(this);
            var Behaviours = new List<string>() { "Cleaned Up", "Complimented", "Turned In Assignment" };
            var Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, Behaviours);
            BehaviourSpinner.Adapter = Adapter;

            Spinner LocationSpinner = new Spinner(this);
            var Locations = new List<string>() { "E-Wing", "Commons", "Main Office" };
            var LocationAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, Locations);
            LocationSpinner.Adapter = LocationAdapter;

            TextView StudentName = new TextView(this);
            TextView StudentIdNumber = new TextView(this);
            ImageView StudentImage = new ImageView(this);
            StudentImage.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.Icon));
            StudentName.Id = 2;
            StudentIdNumber.Id = 4;
            StudentImage.Id = 10;
            BehaviourSpinner.Id = 6;
            LocationSpinner.Id = 8;

            StudentIdNumber.Text = STInfo[0];
            StudentName.Text = STInfo[1];
            RelativeLayout Test = FindViewById<RelativeLayout>(Resource.Id.StudentTable);


            var param1 = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.FillParent,
    ViewGroup.LayoutParams.WrapContent);
            param1.AddRule(LayoutRules.AlignLeft);
            Test.AddView(StudentImage,param1);

            var param = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent,
    ViewGroup.LayoutParams.WrapContent);
            param.AddRule(LayoutRules.Below,StudentImage.Id);
            Test.AddView(StudentName,param);
            var param2 = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent,
    ViewGroup.LayoutParams.WrapContent);
            param2.AddRule(LayoutRules.Below, StudentName.Id);
            Test.AddView(StudentIdNumber,param2);

            var param3 = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent,
ViewGroup.LayoutParams.WrapContent);
            param3.AddRule(LayoutRules.Below,StudentIdNumber.Id);
            Test.AddView(BehaviourSpinner, param3);

            var param4 = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent,
ViewGroup.LayoutParams.WrapContent);
            param4.AddRule(LayoutRules.Below, BehaviourSpinner.Id);
            Test.AddView(LocationSpinner, param4);
        }

        private void SendEmail()
        {
            //MailMessage message = new System.Net.Mail.MailMessage();
            //string toEmail = "dakotastickney@gmail.com";
            //message.From = new MailAddress("dakotastickney@gmail.com");
            //message.To.Add(toEmail);
            //message.Subject = "Test Email";
            //message.Body = "Congratulations Your Kid has done something to grant you this email!";
            //message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            //using (var Client = new SmtpClient("smtp.gmail.com",587))
            //{
            //    // Client.SmtpServer = "MyMailServer";
            //    Client.EnableSsl = true;
            //    Client.UseDefaultCredentials = true;
            //    Client.Send(message);
            //}

            //Console.Write("Sending Email");
            var Email = new Intent(Android.Content.Intent.ActionSend);
            Email.PutExtra(Android.Content.Intent.ExtraEmail, new string[] { "dakotastickney@gmail.com" });
            Email.PutExtra(Android.Content.Intent.ExtraSubject, "Testing");
            Email.PutExtra(Android.Content.Intent.ExtraText, "Congratulations Your Kid has done something to grant you this email!");
            Email.SetType("message/rfc822");
            StartActivity(Email);
        }
    }
}