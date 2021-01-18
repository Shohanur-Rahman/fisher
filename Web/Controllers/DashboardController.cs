using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Services.Domain;
using Services.DomainServices;
using Web.AppCode;
using Services.ApplicationEntity;
using Services.ApplicationServices;
using Services.Domain.Models.User.EditorModel;
using System.Security.Claims;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using Services.Helper;
using Services.Domain.Models.User;
using Services.DomainServices.Company;
using Services.ApplicationEntity.Company;
using Services.ApplicationServices.Company;

namespace Web.Controllers
{

    [Authorize]
    public class DashboardController : BaseController
    {

        #region "Declare Domain Service's objects Needed for Super admin"
        private readonly CompanyUserDomainService _companyUserDomainService;
        private readonly UserDomainService _userDomainService;

        #endregion

        #region "Application Service object and Application Entity object For Super admin"
        protected UserDashbordAppEntity userDashbordAppEntity;
        protected CompanyUserAppEntity companyUserAppEntityInstance;
        protected CompanyUserApplicationService companyUserApplicationService;
        #endregion

        #region "Constructor"
        public DashboardController(CompanyUserDomainService contextUser, UserDomainService contextUserDomainService)
        {
            _companyUserDomainService = contextUser;

            //Application Entity: Initialize and assign domain objects in Application Entity for company user
            companyUserAppEntityInstance = new CompanyUserAppEntity();

            companyUserAppEntityInstance.CompanyUserDomainService = _companyUserDomainService;

            //Application Service: Initialize Application Service object for company user Module
            companyUserApplicationService = new CompanyUserApplicationService(companyUserAppEntityInstance);

            _userDomainService = contextUserDomainService;


            //Application Entity: Initialize and assign domain objects in Application Entity for Super admin Module
            userDashbordAppEntity = new UserDashbordAppEntity();

            userDashbordAppEntity.UserDomainService = _userDomainService;

        }
        #endregion

        #region Page Load Related Method


        public ActionResult Index()
        {
            //================================== DO NOT DELETE ====================================
            //This code is just for show purporse. How to get value from cookie.
            //Also How to get display message and display it in view


            //bool isUser = HttpContext.User.IsInRole(Services.Domain.Models.Role.Enums.Role.User.ToString());

            //string displayName = LoggedInUserInfoFromCookie.GetLoggedInUserDisplayName(HttpContext.User);
            //string userName = LoggedInUserInfoFromCookie.GetLoggedInUserName(HttpContext.User);
            //long userId = LoggedInUserInfoFromCookie.GetLoggedInUserId(HttpContext.User);
            //long comapnyId = LoggedInUserInfoFromCookie.GetLoggedInUserCompanyId(HttpContext.User);
            //string roleName = LoggedInUserInfoFromCookie.GetLoggedInUserRole(HttpContext.User);

            //string message = MessageDisplayHelper.InformationMessageSetOrDisplay(this, false, string.Empty);
            //ViewBag.InformationMessage = message;

            //========================================================================================

            //===== PDF Merge Test With 2 PDF ==============================
            //List<byte[]> projectPDFAndExtraPDFBytes = new List<byte[]>();
            //byte[] pdf1 = System.IO.File.ReadAllBytes(@"C:\Users\ParonPC\Desktop\pdf\ice.pdf");
            //byte[] pdf2 = System.IO.File.ReadAllBytes(@"C:\Users\ParonPC\Desktop\pdf\ice.pdf");

            //projectPDFAndExtraPDFBytes.Add(pdf1);
            //projectPDFAndExtraPDFBytes.Add(pdf2);

            ////Get the pdf file bytes after merging all the pdf documents
            //byte[] mergePDFBytes = PDFMerger.PDFFileMergeHelper.MergeFiles(projectPDFAndExtraPDFBytes);

            //return File(mergePDFBytes, "application/pdf", "test2.pdf");
            //==================================================================

            string loggedUserImagePath = LoggedInUserInfoFromCookie.UserLogoInCookie;
            string loggedUserName = LoggedInUserInfoFromCookie.UserDisplayNameInCookie;
            LoggedInUserInfoFromCookie.LoggedInUserLogo = loggedUserImagePath;

            if (loggedUserName.Length > 20)
            {
                string FirstName = LoggedInUserInfoFromCookie.UserFirstNameInCookie;
                string LastName = LoggedInUserInfoFromCookie.UserLastNameInCookie;
                //ViewBag.UserNameWithNewLine = string.Format("{0} {1} {2}", FirstName, Environment.NewLine, LastName);
                LoggedInUserInfoFromCookie.LoggedInUserDisplayName = string.Format("{0} {1} {2}", FirstName, Environment.NewLine, LastName);
            }
            else
            { LoggedInUserInfoFromCookie.LoggedInUserDisplayName = loggedUserName; }

            //===============================================================

            //For Checking User Profile Document Settings Table
            long userId = LoggedInUserInfoFromCookie.UserIdInCookie;
            long companyId = LoggedInUserInfoFromCookie.UserCompanyIdInCookie;
            string userRole = LoggedInUserInfoFromCookie.UserRoleInCookie;

            SPAdminDashBoardOveriew sellingReportList = companyUserApplicationService.GetAdminDashBoardOverview();
            ViewBag.TotalFishSell = 0;
            ViewBag.TotalFeedBuy = 0;
            ViewBag.TotalFeedDistribute = 0;
            ViewBag.TotalRecord = 0;

            if (sellingReportList != null) {
                ViewBag.TotalFishSell = sellingReportList.TotalFishSell;
                ViewBag.TotalFeedBuy = sellingReportList.TotalFeedBuy;
                ViewBag.TotalFeedDistribute = sellingReportList.TotalFeedDistribute;
                ViewBag.TotalRecord = sellingReportList.TotalRecord;
            }
            return View();
        }

        #endregion

        #region Get Logged In User Image And Name Related Method

        //POST: POST By Ajax call Get Logged In User Image And Name
        public ActionResult GetLoggedInUserImageAndName()
        {

            string loggedUserImagePath = "";
            string loggedUserName = LoggedInUserInfoFromCookie.LoggedInUserDisplayName;

            if (loggedUserName.Length > 20)
            {
                string FirstName = LoggedInUserInfoFromCookie.UserFirstNameInCookie;
                string LastName = LoggedInUserInfoFromCookie.UserLastNameInCookie;

                loggedUserName = string.Format("{0} {1} {2}", FirstName, Environment.NewLine, LastName);

            }

            return Json(new { loggedUserName = loggedUserName, loggedUserImagePath = loggedUserImagePath });
        }
        #endregion
  

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

        public ActionResult GetAdminDashBoardFishSellingReport()
        {
            IList<SPFishSellingChartReport> sellingReportList = companyUserApplicationService.GetAdminDashBoardFishSellingReport();

            return Json(sellingReportList, JsonRequestBehavior.AllowGet);
        }
        
    }

}