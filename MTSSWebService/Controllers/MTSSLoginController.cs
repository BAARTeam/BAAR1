using MTSSWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MTSSWebService.Controllers
{
    public class MTSSLoginController : ApiController
    {
        MTSSDataModel[] logindata;
        public IEnumerable<MTSSDataModel> GetAllMTSSLogin()
        {
            return logindata;
        }

        public IHttpActionResult GetMTSSLogin(string loginID)
        {
            var loginperson = logindata.FirstOrDefault((p) => p.logonname == loginID);
            if (loginperson == null)
            {
                return NotFound();
            }
            return Ok(loginperson);
        }
    }
}
