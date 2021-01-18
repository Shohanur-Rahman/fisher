using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Web.AppCode;

namespace Web.Controllers.Project
{
    public class ProjectDashboardController : Controller
    {
        // GET: ProjectDashboard
        public ActionResult Index()
        {
            return View();
        }
        #region Logout Related Method
        public ActionResult Logout()
        {

            if (User.Identity.IsAuthenticated)
            {
                long userId = LoggedInUserInfoFromCookie.UserIdInCookie;
                //  _userDomainService.UpdateCurrentUserLoggedInInfo(userId);

                var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    authCookie.Expires = DateTime.Today.AddDays(-1);
                }

                Response.Cookies.Add(authCookie);

                FormsAuthentication.SignOut();

            }

            List<string> cookieKeyNameList = new List<string>();
            foreach (var key in HttpContext.Request.Cookies.Keys)
            {
                cookieKeyNameList.Add(key.ToString());
            }

            if (cookieKeyNameList != null && cookieKeyNameList.Count > 0)
            {
                foreach (string cookieKey in cookieKeyNameList)
                {
                    HttpCookie cookie = new HttpCookie(cookieKey);
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie);
                }
            }



            return RedirectToAction("Index", "Login");
        }

        #endregion
    }
}