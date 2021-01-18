using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class errorController : Controller
    {
        //404 page not found
        public ActionResult PageNotFound()
        {
            ViewBag.PreviousPageURL = Request.UrlReferrer.ToString();
            return View("~/Views/ErrorPage/PageNotFound.cshtml");
        }

        //404 page not found
        public ActionResult pageerror()
        {
            return View("~/Views/ErrorPage/pageerror.cshtml");
        }
    }
}