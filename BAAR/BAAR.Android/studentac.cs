using System;
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
using ZXing.Mobile;

namespace BAAR.Droid
{
    [Activity(Label = "student",ScreenOrientation = ScreenOrientation.Portrait)]
    public class studentac : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.RequestFeature(WindowFeatures.NoTitle);
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
            Button TicketButton = FindViewById<Button>(Resource.Id.AddTicket);
     

                MobileBarcodeScanner.Initialize(Application);
                TicketButton.Click += async (sender, e) => {

                    var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                    var result = await scanner.Scan();

                    var NewScreen = new Intent(this, typeof(studentac));
                    NewScreen.PutExtra("StudentID", result.Text);
                    string[] Test =Regex.Split(result.Text,Splitter);
                    CreateStudentTicket(Test[0],Test[1]);
            };

            CreateStudentTicket(STInfo[0], STInfo[1]);

        }

        public void CreateStudentTicket(string Name,string Number)
        {

            Spinner BehaviourSpinner = new Spinner(this);
            var Behaviours = new List<string>() { "Showed Responsibility", "Showed Respect", "Demonstrated Initiative","Was Safe","Demonstrated Professionalism" };
            var Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, Behaviours);
            BehaviourSpinner.Adapter = Adapter;
  

            Spinner LocationSpinner = new Spinner(this);
            var Locations = new List<string>() { "E-Wing", "Commons", "Main Office" };
            var LocationAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, Locations);
            LocationSpinner.Adapter = LocationAdapter;

            TextView StudentName = new TextView(this);
            TextView StudentIdNumber = new TextView(this);
            ImageView StudentImage = new ImageView(this);
            StudentImage.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.Icon));
            StudentName.Id = 2;
            StudentName.TextSize = 25;
            StudentIdNumber.TextSize = 25;
            StudentIdNumber.Id = 4;
            StudentImage.Id = 10;
            BehaviourSpinner.Id = 6;
            LocationSpinner.Id = 8;

            StudentIdNumber.Text = Name;//STInfo[0];
            StudentName.Text = Number;// STInfo[1];
            LinearLayout MainLayout = FindViewById<LinearLayout>(Resource.Id.TicketHolder);
            RelativeLayout Test = new RelativeLayout(this);
            Test.SetPadding(0,5,0,0);
            MainLayout.AddView(Test);

            var param1 = new RelativeLayout.LayoutParams(200, 200);
            param1.AddRule(LayoutRules.AlignParentLeft);
            Test.AddView(StudentImage, param1);

            var param = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent,
    ViewGroup.LayoutParams.WrapContent);
            param.AddRule(LayoutRules.RightOf, StudentImage.Id);
            Test.AddView(StudentName, param);
            var param2 = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent,
    ViewGroup.LayoutParams.WrapContent);
            param2.AddRule(LayoutRules.RightOf, StudentImage.Id);
            param2.AddRule(LayoutRules.Below, StudentName.Id);
            Test.AddView(StudentIdNumber, param2);

            var param3 = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent,
ViewGroup.LayoutParams.WrapContent);
            param3.AddRule(LayoutRules.Below, StudentIdNumber.Id);
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
            Email.PutExtra(Android.Content.Intent.ExtraBcc, new string[] { "dakotastickney@gmail.com"});
            Email.PutExtra(Android.Content.Intent.ExtraSubject, "Testing " + DateTime.Today);
            Email.PutExtra(Android.Content.Intent.ExtraText, "Congratulations Your Kid has done something to grant you this email!");
            Email.SetType("message/rfc822");
            StartActivity(Email);
        }
    }
}