using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.AppCode
{
    public class UserAuthenticationEntity
    {
        public string LoggedInUserId { get; set; }
        public string[] UserRole { get; set; }
       
    }
}