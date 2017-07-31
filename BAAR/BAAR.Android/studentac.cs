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
using System.Net;
using System.IO;
using Newtonsoft.Json;
using ZXing.Mobile;

namespace BAAR.Droid
{
    [Activity(Label = "student",ScreenOrientation = ScreenOrientation.Portrait)]
    public class studentac : Activity
    {
        public string studentname;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.RequestFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.student);





            var StudentID = Intent.Extras.GetString("StudentID");
            /*string Splitter = @";";
            string[] STInfo = Regex.Split(StudentID, Splitter);*/
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

            /*
            { 
            "name":"students",
            "record":[
                { 
                "name":"students",
                "tables":
                    { "students":
                        { "lastfirst":"Schroyer, Daniel R"}
                    } 
                }
            ],
            "@extensions":"activities,u_alerts,u_lea,s_mi_stu_gc_x,u_sped,u_health,c_studentlocator,u_studentsuserfields,u_wbl,u_section504,u_equipment,studentcorefields,u_students_extension,s_stu_ncea_x,s_stu_crdc_x,s_mi_stu_crdc_x,s_mi_stu_assess_x,u_intake"
            }


            */
        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://172.21.123.196/ws/schema/query/pqtest?");
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add(HttpRequestHeader.Authorization, string.Format("Bearer {0}", Login.Test.AccessToken));
                request.Accept = "application/json";


                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = "{\"scannedbarcode\": 12000}";

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        var content = reader.ReadToEnd();
                        if (string.IsNullOrWhiteSpace(content))
                        {
                            Console.Out.WriteLine("Response contained empty body...");
                        }
                        else
                        {
                            Console.WriteLine("Info Body: \r\n {0}", content);
                        }
                        content = content.Substring(content.IndexOf("lastfirst"));
                        content = content.Substring(content.IndexOf(":") + 2);
                        content = content.Remove(content.IndexOf('"'));
                        Console.WriteLine("CHECK " + content);
                        studentname = content;
                    }
                }

                

                var NewScreen = new Intent(this, typeof(studentac));
                ;
                /*string[] StudentInformation = Regex.Split(result.Text, Splitter);*/
                CreateStudentTicket(studentname, Convert.ToString(NewScreen.PutExtra("StudentID", result.Text)));
            };

            
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

    public class Test {

        [JsonProperty("record")]
        public string Record;
        [JsonProperty("tables")]
        public string Tables;
        [JsonProperty("Students")]
        public string Students;
        [JsonProperty("lastfirst")]
        public string Name;
    }

}