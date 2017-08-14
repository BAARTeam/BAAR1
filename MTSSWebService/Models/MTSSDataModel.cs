using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MTSSWebService.Models
{
    public class MTSSDataModel
    {
        public string Login_Name { get; set; }
        public string Login_PW { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Allow_Email { get; set; }
        public string KISD_Location { get; set; }
        public DateTime Change_Date { get; set; }
    }
}