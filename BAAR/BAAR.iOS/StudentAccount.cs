using System;
using System.Collections;
using System.Collections.Generic;
using UIKit;
using System.Net;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Threading.Tasks;
using ToastIOS;
using BAAR.iOS;
using CoreAnimation;
using System.Drawing;
using System.IO;
using System.Threading;
using Foundation;

namespace BAAR.iOS
{
    public partial class StudentAccount : UIViewController
    {
        public StudentAccount(IntPtr handle) : base(handle)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }
        public UIDropDown OpenDropDown= null;

        public Dictionary<int, List<UIDropDown>> LayoutSpinner = new Dictionary<int, List<UIDropDown>>();

        public Dictionary<int, string[]> BLL = new Dictionary<int, string[]>()
        {
            {0, new string[] { "Front Entry", "Hallways", "Office", "Classrooms", "Breakroom", "Bus/Parking Lot" } },
            {1, new string[] { "Testing Center", "Hallways", "Office / Counselor", "Working Classroom", "Parking Lot" } },
            {2, new string[] { "Parking Lot", "Hallways","Office", "Classrooms","Commons"} },
            {3, new string[] { "Hallways", "Classrooms", "Parking Lot", "Main Office", "Vending Machines", "Collaboration Areas / Learning Pods"} },
        };
        public List<BarcodeScanReturn> AllReturned = new List<BarcodeScanReturn>();
        private int NumberOfTickets = 0;
        public async override void ViewDidLoad()
        {
            base.ViewDidLoad();
            foreach (UIView view in TicketHolder.Subviews)
            {
                view.RemoveFromSuperview();
            }
            NumberOfTickets = 0;
            AllReturned.Clear();
            try
            {
                BarcodeScanReturn Returned = await StartBarcodeScanner();
                string[] Name = SplitName(Returned.StudentName);
                Returned.FirstName = Name[0];
                Returned.LastName = Name[1];
                TicketHolder.WidthAnchor.ConstraintEqualTo(UIScreen.MainScreen.Bounds.Width).Active = true;
                Scroll.AddSubview(TicketHolder);
                Scroll.ContentSize = TicketHolder.Frame.Size;
                CreateStudentTicket(Name[0] + " " + Name[1], Returned.StudentNumber, NumberOfTickets);
            }
            catch
            {
                Console.WriteLine("Something went Wrong");
            }

            SubmitButton.TouchUpInside += (sender, e) =>
            {
                for (int i = 0; i < NumberOfTickets; i++)
                {
                    string EmailBehaviour = LayoutSpinner[i][0].Selected;
                    string EmailBuildingLocation = LayoutSpinner[i][1].Selected;
                    string EmailLocation = LayoutSpinner[i][2].Selected;
                    string EmailName = AllReturned[i].FirstName.ToString();
                    Thread EmailThread = new Thread(new ThreadStart(new EmailInfo(AllReturned[i].StudentName, AllReturned[i].PrimaryEmailAddress, AllReturned[i].SecondaryAddress, AllReturned[i].StudentAddress, EmailLocation, EmailBehaviour).BackgroundEmail));
                    EmailThread.Start();

                    var thisinfo = JsonConvert.SerializeObject(new
                    {
                        LogDateTime = DateTime.Now,
                        District = "KentISD",
                        Building = EmailBuildingLocation,
                        Student_ID = AllReturned[i].StudentNumber,
                        Student_First_Name = AllReturned[i].FirstName,
                        Student_Last_Name = AllReturned[i].LastName,
                        Behavior = EmailBehaviour,
                        Behavior_Location = EmailLocation,
                        Staff_Login_ID = ViewController.StaffUserName
                    });


                    // POST a JSON string
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://goingpro.azurewebsites.net/api/Behavior_Log");
                    request.Method = "POST";
                    request.ContentType = @"application/json";
                    request.Accept = @"application/json";

                    var dataStream = new StreamWriter(request.GetRequestStream());

                    using (dataStream)
                    {
                        dataStream.Write(thisinfo);
                        dataStream.Flush();
                        dataStream.Close();
                    }

                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        if (response.StatusCode != HttpStatusCode.Created)
                        {
                            Console.WriteLine("Error fetching data.  Server returned status code " + response.StatusCode);
                        }
                        else
                        {
                            Console.WriteLine("Information Successfully saved");
                        }
                    }
                }
                Toast.MakeText("Email Sent").Show();
            };



            TicketButton.TouchUpInside += async delegate
            {
                Console.WriteLine("Pushed");
                try
                {
                    BarcodeScanReturn Returned = await StartBarcodeScanner();
                    string[] SecondaryName = SplitName(Returned.StudentName);
                    Returned.FirstName = SecondaryName[0];
                    Returned.LastName = SecondaryName[1];
                    CreateStudentTicket(SecondaryName[0] + " " + SecondaryName[1], Returned.StudentNumber, NumberOfTickets);
                }
                catch
                {
                    Toast.MakeText("Invalid Barcode Scanned").Show();
                    Console.WriteLine("Woah Something Went Wrong When Scanning Barcode either that is not a valid barcode or there is no connection.");
                }

            };
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            UITouch Touch = touches.AnyObject as UITouch;
            if (OpenDropDown != null)
            {
                for (int i = 0; i < OpenDropDown.Items.Count; i++)
                {
                    if (OpenDropDown.Items[i].Frame.Contains(Touch.GetPreciseLocation(View)))
                    {
                        Console.WriteLine("TouchedInside");
                    }
                    else
                    {
                        for (int k = 0; k < OpenDropDown.Items.Count; k++)
                        {
                            OpenDropDown.Items[k].Hidden = true;
                        }
                        OpenDropDown.Scrolled.UserInteractionEnabled = true;
                    }
                }
            }
            Console.WriteLine("Touch");
        }
        private string[] SplitName(string ToSplit)
        {
            string[] SplitName = ToSplit.Split(',');
            string[] SpaceSplit = SplitName[1].Split(' ');
            return new string[] { SpaceSplit[1], SplitName[0] };
        }
        const string AccountPassword = "Fopo7082";
        public void CreateStudentTicket(string Name, string Number, int TicketNumber)
        {

            TicketHolder.TranslatesAutoresizingMaskIntoConstraints = false;

            UIView Ticket = new UIView();
            Ticket.BackgroundColor = UIColor.FromRGBA(21, 21, 30, 255);
            Ticket.HeightAnchor.ConstraintEqualTo(160).Active = true;
            Ticket.WidthAnchor.ConstraintEqualTo(UIScreen.MainScreen.Bounds.Width).Active = true;

            UILabel StudentName = new UILabel();
        //    StudentName.LineBreakMode = UILineBreakMode.Clip;
            StudentName.Text = Name;
            StudentName.TextColor = UIColor.White;
            StudentName.Font.WithSize(40);



            Ticket.AddSubview(StudentName);

            UILabel StudentNumber = new UILabel();
            StudentNumber.Text = Number;
            StudentNumber.TextColor = UIColor.White;
            StudentNumber.Font.WithSize(40);

            Ticket.AddSubview(StudentNumber);

            TicketHolder.AddArrangedSubview(Ticket);

            UIImageView ImageView = new UIImageView();
            ImageView.BackgroundColor = UIColor.Black;
            ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            ImageView.Image = UIImage.FromBundle("GoingPro_Icon.png");


            ImageView.Frame = new CoreGraphics.CGRect(15, 15, 50, 50);
            StudentName.Frame = new RectangleF(75, 18, 500, 20);
            StudentNumber.Frame = new RectangleF(75, 36, 90, 20);
            Ticket.AddSubview(ImageView);

            UIDropDown DropDown = new UIDropDown(Ticket, View, new List<string>()
            {
                "Showing Responsibility",
                "Showing Respect",
                "Demonstrating Initiative",
                "Being Safe",
                "Demonstrating Professionalism"
            }, new CoreGraphics.CGRect(25, 85, TicketHolder.Bounds.Width - 65, 30), Scroll, TicketNumber,this);


            UIDropDown DropDown2 = new UIDropDown(Ticket, View, new List<string>()
            {
                "KTC",
                "MySchool",
                "KCTC",
                "KIH",
            }, new CoreGraphics.CGRect(25, 120, 100, 30), Scroll, TicketNumber,this);


            UIDropDown DropDown3 = new UIDropDown(Ticket, View, new List<string>()
            {
                "Classrooms",
                "Parking Lot",
                "Vending Machines",
                "Commons",
            }, new CoreGraphics.CGRect(182, 120, 100, 30), Scroll, TicketNumber,this);

            DropDown2.OptionSelected += (e) =>
            {
                Console.WriteLine("Here " + e);
                DropDown3.Options = BLL[e].ToList();
                DropDown3.GenerateNewOptions();
                return null;
            };
            LayoutSpinner.Add(TicketNumber, new List<UIDropDown>() { DropDown, DropDown2, DropDown3 });
            NumberOfTickets += 1;
        }
        public class ExamplePickerViewModel : UIPickerViewModel
        {
            private List<string> _myItems;
            protected int selectedIndex = 0;

            public ExamplePickerViewModel(List<string> items)
            {
                _myItems = items;
            }

            public string SelectedItem
            {
                get { return _myItems[selectedIndex]; }
            }

            public override nint GetComponentCount(UIPickerView picker)
            {
                return 1;
            }

            public override nint GetRowsInComponent(UIPickerView picker, nint component)
            {
                return _myItems.Count;
            }

            public override string GetTitle(UIPickerView picker, nint row, nint component)
            {
                return _myItems[(int)row];
            }

            public override void Selected(UIPickerView picker, nint row, nint component)
            {
                selectedIndex = (int)row;
            }
        }
        public async Task<BarcodeScanReturn> StartBarcodeScanner()
        {
            var scanner = new ZXing.Mobile.MobileBarcodeScanner();
            var result = await scanner.Scan();

            try
            {
                string[] Results = result.ToString().Split('*');
                Console.WriteLine("Returned Data " + Results[1]);
                string Contra = (string)ViewController.MakeRequest3("data", Results[1]);
                string Name = Contra.GetStringOut("lastfirst");
                string Email1 = Contra.GetStringOut("guardianemail");
                string Email2 = Contra.GetStringOut("guardianemail_2");
                string Email3 = Contra.GetStringOut("stud_email");
                BarcodeScanReturn Student = new BarcodeScanReturn(Name, Results[1], Email1, Email2, Email3);
                AllReturned.Add(Student);
                return Student;

            }
            catch
            {
                string[] Results = result.ToString().Split(' ');
                BarcodeScanReturn Staff = new BarcodeScanReturn((Results[2] + ", " + Results[1]),"", null, null, null);
                AllReturned.Add(Staff);
                return Staff;
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

            public object FirstName { get; internal set; }
            public object LastName { get; internal set; }

            public BarcodeScanReturn(string StuName, string StuNumber, string PrimEAdd, string SecEAdd, string StuAdd)
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

            public EmailInfo(string StuName, string PrimAdd, string SecAdd, string StuAdd, string Loc, string Act)
            {
                this.Name = StuName;
                this.PrimaryAddress = PrimAdd;
                this.SecondaryAddress = SecAdd;
                this.StudentAddress = StuAdd;
                this.Location = Loc;
                this.Action = Act;
            }
            const string oysd8fh376sdflsdfo8 = "Fopo7082";

            public void BackgroundEmail()
            {
                try
                {
                    var fromAddress = new MailAddress("GoingPro@kentisd.org", "Going Pro");
                    string subject = " " + this.Name + " was positively recognized today at Kent ISD!";
                    string body = String.Format("A staff member at Kent ISD secondary campus schools recognized " + this.Name + " for " + this.Action + " in the " + this.Location + " today! \n This recognition comes with our campus initiative, �Going Pro at Kent ISD�, which is preparing students to be college and career ready by focusing on positive behaviors.\n Be Professional. Be Respectful. Be Responsible. Demonstrate Initiative. Be Safe. \n Please make sure to congratulate " + this.Name + " tonight!" + "\n Sincerely <a href=\"mailto:{1}?GoingPro\" target=\"_top\">{0}</a>", ViewController.StaffFirst + " " + ViewController.StaffLast, ViewController.StaffEmail);

                    const string ejkrj9858 = oysd8fh376sdflsdfo8;
                    if (PrimaryAddress != null)
                    {
                        var toAddress = new MailAddress(this.PrimaryAddress, this.PrimaryAddress);

                        var smtp = new SmtpClient
                        {
                            Host = "smtp.outlook.com",
                            Port = 587,
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(fromAddress.Address, ejkrj9858)
                        };

                        using (var GuardianEmail = new MailMessage(fromAddress, toAddress)
                        {
                            Subject = subject,
                            IsBodyHtml = true,
                            Body = body
                        })
                        {
                            if (GuardianEmail != null)
                            {
                                GuardianEmail.CC.Add(ViewController.StaffEmail);
                                smtp.Send(GuardianEmail);
                            }
                        }
                    }
                    var smtp2 = new SmtpClient
                    {
                        Host = "smtp.outlook.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, ejkrj9858),
                        
                        
                    };
                    MailMessage New = new MailMessage();
                    
                    if (this.SecondaryAddress != null)
                    {
                        using (var GuardianEmail2 = new MailMessage(fromAddress, new MailAddress(this.SecondaryAddress))
                        {
                            Subject = subject,
                            IsBodyHtml = true,
                            Body = body,


                        })
                        {
                            if (GuardianEmail2 != null)
                            {
                                smtp2.Send(GuardianEmail2);
                            }
                        }
                    }

                    if (this.StudentAddress != null)
                    {

                        var smtp3 = new SmtpClient
                        {
                            Host = "smtp.outlook.com",
                            Port = 587,
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(fromAddress.Address, ejkrj9858)
                        };
                        using (var StudentEmail = new MailMessage(fromAddress, new MailAddress(this.StudentAddress))
                        {
                            Subject = "Congratulations on being positively recognized today at Kent ISD",
                            IsBodyHtml = true,
                            Body = "A staff member at Kent ISD secondary campus schools recognized you for being respectful in the commons today! This recognition comes with our campus initiative, �Going Pro at Kent ISD�, which is preparing students to be college and career ready by focusing on positive behaviors. Be Professional. Be Respectful. Be Responsible. Demonstrate Initiative. Be Safe. Congratulations on demonstrating professional behavior today!"
                        })
                        {
                            if (StudentEmail != null)
                            {
                                smtp3.Send(StudentEmail);
                            }
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Error when sending emails. Probably not connected to the internet.");
                }
            }
        }
    }
}