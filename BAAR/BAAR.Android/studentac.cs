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
using Android.Graphics;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace BAAR.Droid
{
    [Activity(Label = "student", ScreenOrientation = ScreenOrientation.Portrait)]



    public class studentac : Activity
    {
        public Dictionary<int, Tuple<Spinner, Spinner>> LayoutSpinner = new Dictionary<int, Tuple<Spinner, Spinner>>();
        public List<string> EmailNames = new List<string>();
        public string studentname;
        public string EmailAddress;
        private int NumberOfTickets;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.RequestFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.student);

            FindViewById<LinearLayout>(Resource.Id.Root).SetBackgroundColor(Color.Argb(255, 0, 9, 26));

            BarcodeScanReturn Returned = await StartBarcodeScanner();
            Console.WriteLine("Yellow Submarine " + Returned.StudentName);
            string[] Name = SplitName(Returned.StudentName);

            Console.WriteLine(Name);
            EmailNames.Add(Name[0]);
            CreateStudentTicket(Name[0] + " " + Name[1], Returned.StudentNumber.ToString());


            Button EmailButton = FindViewById<Button>(Resource.Id.EmailButton);
            EmailButton.Click += (sender, e) =>
            {
               

                for (int i = 0; i < NumberOfTickets; i++)
                {
                    string EmailBehaviour = LayoutSpinner[i + 1].Item1.SelectedItem.ToString();
                    string EmailLocation = LayoutSpinner[i + 1].Item2.SelectedItem.ToString();
                    string EmailName = EmailNames[i];
                    EmailInfo NewEmail = new EmailInfo(EmailName,Returned.PrimaryEmailAddress,Returned.SecondaryAddress,Returned.StudentAddress,"Commons","Safety");
                    BackgroundEmail(NewEmail);
                }



                SqlCommand Insert = new SqlCommand("INSERT INTO MTSS_ActionLog VALUES (@DT, @SF, @SL, @SN, @StN, @ATi, @AT, @AL)", Login.conn);
                Login.conn.Open();
                log log = new log("TEST", "TEsT", EmailNames[0], Convert.ToDouble(Returned.StudentNumber.ToString()), LayoutSpinner[1].Item1.SelectedItem.ToString(), LayoutSpinner[1].Item2.SelectedItem.ToString());
                log.exe(Insert);
                Login.conn.Close();
                Toast.MakeText(this, "Email Sent", ToastLength.Long).Show();
                Intent MainPage = new Intent(this, typeof(MainActivity));
                StartActivity(MainPage);
            };

            Button TicketButton = FindViewById<Button>(Resource.Id.AddTicket);

            TicketButton.Click += async (sender, e) =>
            {
                try
                {
                    BarcodeScanReturn Thing = await StartBarcodeScanner();

                    string content = (string)MainActivity.MakeRequest3("data", Thing.StudentNumber.ToString());
                    content = content.GetStringOut("lastfirst");
                    studentname = content;
                    string[] SecondaryName = SplitName(studentname);
                    EmailNames.Add(SecondaryName[0]);
                    CreateStudentTicket(SecondaryName[0] + " " + SecondaryName[1], Thing.StudentNumber.ToString());


                }
                catch
                {
                    Toast.MakeText(this, "Invalid Barcode Scanned", ToastLength.Long).Show();
                    Console.WriteLine("Woah Something Went Wrong When Scanning Barcode either that is not a valid barcode or there is no connection.");
                }
            };
        }

        private string[] SplitName(string ToSplit)
        {
            string[] SplitName = ToSplit.Split(',');
            string[] SpaceSplit = SplitName[1].Split(' ');
            return new string[] { SpaceSplit[1], SplitName[0] };
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
            StudentImage.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.pbutton));
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
            RelLayout.SetPadding(0, 10, 0, 0);
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
            LayoutSpinner.Add(NumberOfTickets, new Tuple<Spinner, Spinner>(BehaviourSpinner, LocationSpinner));
        }

        public void BackgroundEmail(EmailInfo Email)
        {
            var fromAddress = new MailAddress("GoingPro@kentisd.org", "Going Pro");
            var toAddress = new MailAddress(Email.PrimaryAddress, "Thing");
            const string fromPassword = AccountPassword;
            string subject = " " + Email.Name + " was positively recognized today at Kent ISD!";
            string body = "“A staff member at Kent ISD secondary campus schools recognized " + Email.Name + " for " + Email.Action + " in the " + Email.Location + " today!” \n “This recognition comes with our campus initiative, “Going Pro at Kent ISD”, which is preparing students to be college and career ready by focusing on positive behaviors.\n Be professional.Be Respectful.Be Responsible.Demonstrate Initiative.Be Safe.” \n “Please make sure to congratulate " + Email.Name + " tonight!”" + "Sincerely <a href=\"mailto:dakotastickney@gmail.com?GoingPro\" target=\"_top\">LaurieFernandez</a>";

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
        public async Task<BarcodeScanReturn> StartBarcodeScanner()
        {
            MobileBarcodeScanner.Initialize(Application);

            var scanner = new ZXing.Mobile.MobileBarcodeScanner();
            var result = await scanner.Scan();
            string Contra = (string)MainActivity.MakeRequest3("data",result.ToString());

            Console.WriteLine("Returned Data " + Contra);
            string Name = Contra.GetStringOut("lastfirst");
            Console.WriteLine("Name " + Name);
            string Email1 = Contra.GetStringOut("guardianemail");
            string Email2 = Contra.GetStringOut("guardianemail_2");
            string Email3 = Contra.GetStringOut("stud_email");
            return new BarcodeScanReturn(Name,result.ToString(),Email1,Email2,Email3);
        }
    } 

    public class JsonPayload
    {
        [JsonProperty("scannedbarcode")]
        public double Number;
    }

    public class BarcodeScanReturn
    {
        public string StudentName;
        public string StudentNumber;
        public string PrimaryEmailAddress;
        public string SecondaryAddress;
        public string StudentAddress;

        public BarcodeScanReturn(string StuName, string StuNumber,string PrimEAdd,string SecEAdd,string StuAdd)
        {
            this.StudentName = StuName;
            this.StudentNumber = StuNumber;
            this.PrimaryEmailAddress = PrimEAdd;
            this.SecondaryAddress = SecEAdd;
            this.StudentAddress = StuAdd;
        }
    }

    public class EmailInfo
    {
        public string PrimaryAddress;
        public string SecondaryAddress;
        public string StudentAddress;
        public string Location;
        public string Action;
        public string Name;

        public EmailInfo(string StuName, string PrimAdd,string SecAdd,string StuAdd,string Loc,string Act)
        {
            this.Name = StuName;
            this.PrimaryAddress = PrimAdd;
            this.SecondaryAddress = SecAdd;
            this.StudentAddress = StuAdd;
            this.Location = Loc;
            this.Action = Act;
        }
    }
}

 