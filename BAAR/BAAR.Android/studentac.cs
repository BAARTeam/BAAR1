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

            var StudentID = Intent.Extras.GetString("StudentID");
            string Splitter = @";";
            string[] STInfo = Regex.Split(StudentID, Splitter);
            Button EmailButton = FindViewById<Button>(Resource.Id.EmailButton);
            EmailButton.Click += (sender, e) =>
            {
                SendEmail();
            };

            Button TicketButton = FindViewById<Button>(Resource.Id.AddTicket);

            MobileBarcodeScanner.Initialize(Application);
            TicketButton.Click += async (sender, e) =>
            {
                var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                var result = await scanner.Scan();

                var NewScreen = new Intent(this, typeof(studentac));
                NewScreen.PutExtra("StudentID", result.Text);
                string[] StudentInformation = Regex.Split(result.Text, Splitter);
                CreateStudentTicket(StudentInformation[0], StudentInformation[1]);
            };

            CreateStudentTicket(STInfo[0], STInfo[1]);
        }

        public void CreateStudentTicket(string Name, string Number)
        {

            Spinner BehaviourSpinner = new Spinner(this);
            var Behaviours = new List<string>() { "Showed Responsibility", "Showed Respect", "Demonstrated Initiative", "Was Safe", "Demonstrated Professionalism" };
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

            StudentIdNumber.Text = Name;
            StudentName.Text = Number;
            LinearLayout MainLayout = FindViewById<LinearLayout>(Resource.Id.TicketHolder);
            RelativeLayout RelLayout = new RelativeLayout(this);
            RelLayout.SetPadding(0, 5, 0, 0);
            MainLayout.AddView(RelLayout);

            var StudentImageParam = new RelativeLayout.LayoutParams(200, 200);
            StudentImageParam.AddRule(LayoutRules.AlignParentLeft);
            RelLayout.AddView(StudentImage, StudentImageParam);

            var StudentNameParam = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent,
    ViewGroup.LayoutParams.WrapContent);
            StudentNameParam.AddRule(LayoutRules.RightOf, StudentImage.Id);
            RelLayout.AddView(StudentName, StudentNameParam);

            var StudentIDNumber = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent,
    ViewGroup.LayoutParams.WrapContent);
            StudentIDNumber.AddRule(LayoutRules.RightOf, StudentImage.Id);
            StudentIDNumber.AddRule(LayoutRules.Below, StudentName.Id);
            RelLayout.AddView(StudentIdNumber, StudentIDNumber);

            var BehaviourParam = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent,
ViewGroup.LayoutParams.WrapContent);
            BehaviourParam.AddRule(LayoutRules.Below, StudentIdNumber.Id);
            RelLayout.AddView(BehaviourSpinner, BehaviourParam);


            var LocationParam = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent,
ViewGroup.LayoutParams.WrapContent);
            LocationParam.AddRule(LayoutRules.Below, BehaviourSpinner.Id);
            RelLayout.AddView(LocationSpinner, LocationParam);
        }

        private void SendEmail()
        {
            var Email = new Intent(Android.Content.Intent.ActionSend);
            Email.PutExtra(Android.Content.Intent.ExtraBcc, new string[] { "parent_guardian@gmail.com"});
            Email.PutExtra(Android.Content.Intent.ExtraSubject, "Testing " + DateTime.Today);
            Email.PutExtra(Android.Content.Intent.ExtraText, "Congratulations Your Kid has done something to grant you this email!");
            Email.SetType("message/rfc822");
            StartActivity(Email);
        }
    }
}