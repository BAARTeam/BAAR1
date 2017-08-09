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
using System.Net.Mail;
using ZXing.Mobile;

namespace BAAR.Droid
{
    [Activity(Label = "student",ScreenOrientation = ScreenOrientation.Portrait)]
    public class studentac : Activity
    {
        public Dictionary<int,Tuple<Spinner,Spinner>> LayoutSpinner = new Dictionary<int, Tuple<Spinner, Spinner>>();
        public List<string> EmailNames = new List<string>();
        public string studentname;
        private int NumberOfTickets;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.RequestFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.student);
            MobileBarcodeScanner.Initialize(Application);

            var scanner = new ZXing.Mobile.MobileBarcodeScanner();
            var result = await scanner.Scan();
            string Contra = (string)MainActivity.MakeRequest2(result.ToString());
            Contra = Contra.Substring(Contra.IndexOf("lastfirst"));
            Contra = Contra.Substring(Contra.IndexOf(":") + 2);
            Contra = Contra.Remove(Contra.IndexOf('"'));
            string[] SplitName1 = Contra.Split(',');
            string[] SpaceSplit1 = SplitName1[1].Split(' ');
            EmailNames.Add(SpaceSplit1[1]);
            Console.WriteLine("CHECK " + Contra);
            CreateStudentTicket(SpaceSplit1[1] + " " + SplitName1[0], result.ToString());
            MobileBarcodeScanner.Uninitialize(Application);

            Button EmailButton = FindViewById<Button>(Resource.Id.EmailButton);
            EmailButton.Click += (sender, e) =>
            {
                Console.WriteLine("Email Number " + NumberOfTickets);
                Console.WriteLine("EmailNames Count " + EmailNames.Count);
                for (int i = 0; i < NumberOfTickets; i++)
                {
                   string EmailBehaviour = LayoutSpinner[i + 1].Item1.SelectedItem.ToString();
                   string EmailLocation = LayoutSpinner[i + 1].Item2.SelectedItem.ToString();
                   string EmailName = EmailNames[i];
                   BackgroundEmail(EmailName,EmailLocation,EmailBehaviour);
                  // SendEmail( EmailName,  EmailLocation,  EmailBehaviour);
                  // Console.WriteLine(i);
                }
                Toast.MakeText(this, "Email Sent", ToastLength.Long).Show();

                Intent MainPage = new Intent(this,typeof(MainActivity));
                StartActivity(MainPage);
            };

            Button TicketButton = FindViewById<Button>(Resource.Id.AddTicket);

            TicketButton.Click += async (sender, e) =>
            {
                try
                {
                    MobileBarcodeScanner.Initialize(Application);
                    var scanner1 = new ZXing.Mobile.MobileBarcodeScanner();
                    var result1 = await scanner.Scan();

                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://172.21.123.196/ws/schema/query/pqtest?");
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.Headers.Add(HttpRequestHeader.Authorization, string.Format("Bearer {0}", Login.Token.AccessToken));
                    request.Accept = "application/json";

                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        int ScannerResult = Convert.ToInt16(result1.ToString());
                        JsonPayload Payload = new JsonPayload();
                        Payload.Number = ScannerResult;
                        string JsonString = (string)JsonConvert.SerializeObject(Payload);

                        streamWriter.Write(JsonString);
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

                    string[] SplitName = studentname.Split(',');
                    string[] SpaceSplit = SplitName[1].Split(' ');
                    EmailNames.Add(SpaceSplit[1]);

                    CreateStudentTicket(SpaceSplit[1] + " " + SplitName[0], result1.ToString());
                }
                catch
                {
                    Toast.MakeText(this,"Invalid Barcode Scanned",ToastLength.Long).Show(); 
                    Console.WriteLine("Woah Something Went Wrong When Scanning Barcode either that is not a valid barcode or there is no connection.");
                }
            };
        }
       const string AccountPassword = "Fopo7082";
        public void CreateStudentTicket(string Number, string Name)
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
            NumberOfTickets++;
            LayoutSpinner.Add(NumberOfTickets,new Tuple<Spinner,Spinner>(BehaviourSpinner,LocationSpinner));
        }

        public void BackgroundEmail(string Name, string Location, string Behaviours)
        {
            var fromAddress = new MailAddress("GoingPro@kentisd.org", "Going Pro");
            var toAddress = new MailAddress("dakotastickney@gmail.com", "Jacoby");
            const string fromPassword = AccountPassword;
            string subject = " "+ Name+ " was positively recognized today at Kent ISD!";
            string body = "“A staff member at Kent ISD secondary campus schools recognized "+Name +" for "+Behaviours+" in the "+Location+" today!” \n “This recognition comes with our campus initiative, “Going Pro at Kent ISD”, which is preparing students to be college and career ready by focusing on positive behaviors.\n Be professional.Be Respectful.Be Responsible.Demonstrate Initiative.Be Safe.” \n “Please make sure to congratulate "+Name+" tonight!”" + "Sincerely <a href=\"mailto:dakotastickney@gmail.com?GoingPro\" target=\"_top\">LaurieFernandez</a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.office365.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                IsBodyHtml = true,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }

    public class JsonPayload
    {
      [JsonProperty("scannedbarcode")]
       public int Number;
    }
}