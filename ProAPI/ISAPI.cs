using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProAPI
{
    public interface ISAPI
    {
        void CreateNewAccount(string Name, string userName, string password, string PhoneNumber, string CNIC);
    }
}