using System.IO;
using System.Security.Claims;
using Services.DomainServices;
using Services.Domain;
using Services.Domain.Models.User.EditorModel;
using Web.AppCode;
using Services.EmailService;
using Services.PDFService;
using System;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.SqlClient;
using System.Data.Common;

namespace Web.Controllers
{
    public class LoginController : BaseController
    {
        private readonly UserDomainService _userDomainService;
        //private readonly COSHHPDFTemplateGeneratorService _coshhPDFService;
         



        #region Login Controller Constructor Related Method

        public LoginController(UserDomainService userDomainServiceContext)
        {
            //_coshhPDFService = coshhPDFService;
            _userDomainService = userDomainServiceContext;

            //string htmlTemplate = projectPDFService.GenerateRAMSProjectPDFTextFromTemplate(6, 2, environment);
            //string htmlTemplate = toolboxService.GenerateToolBoxTalkPDFTextFromTemplate(1, 2, environment);


        }
        #endregion

        #region Page Load Related Method


        public ActionResult Index()
        {
            ViewBag.ErrorClass = "";   
            string deleteMsg = MessageDisplayHelper.ErrorMessageSetOrDisplay(this, false, string.Empty);
            if (deleteMsg != "") {
                ViewBag.ErrorClass = "error-content";
            }
            ViewBag.LoginFailed = deleteMsg;
       
            return View();
        }
        #endregion

       

        #region Login Related Method
    

        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            
            User loginResult = null;
            long cookieUserId= LoggedInUserInfoFromCookie.UserIdInCookie;
            bool IsFromSignIn = true;

            if (loginModel.IsFromSignIn != null) {
                IsFromSignIn = loginModel.IsFromSignIn.Value;
            }       
            return DataActionViewService(
                () =>
                {
                    loginResult = _userDomainService.Login(loginModel.UserName, loginModel.Password, cookieUserId, loginModel.UserIP, IsFromSignIn);

                },
                () =>
                {                
                    string userIdRole;



                    //if role is user or stuff

                    if (loginResult == null) {
                        MessageDisplayHelper.ErrorMessageSetOrDisplay(this, true, "এই ব্যবহারকারীর তথ্যে সমস্যা আছে।");
                        return RedirectToAction("Index", "Login");
                    }
                    
                        userIdRole = loginResult.UserID + ":" + loginResult.Role;
                        LoggedInUserInfoFromCookie.UserIdRoleEncryptedInCookie = userIdRole;
                        LoggedInUserInfoFromCookie.UserLogoInCookie = loginResult.UserImagePath;
                        LoggedInUserInfoFromCookie.UserFirstNameInCookie = loginResult.FirstName;
                        LoggedInUserInfoFromCookie.UserLastNameInCookie = loginResult.LastName;
                        LoggedInUserInfoFromCookie.UserDisplayNameInCookie = loginResult.FirstName + " " + loginResult.LastName;
                        LoggedInUserInfoFromCookie.UserCompanyIdInCookie = 0;
                        LoggedInUserInfoFromCookie.PaidUserExpiredStatusInCookie = "NotExpired";
                        LoggedInUserInfoFromCookie.TrialUserExpiredStatusInCookie = "NotExpired";
                        LoggedInUserInfoFromCookie.UserCreatedDateInCookie = loginResult.CreatedDate.ToString();
                        LoggedInUserInfoFromCookie.UserRememberMeOptionsInCoockie = loginModel.RememberMe;
                        LoggedInUserInfoFromCookie.SelectedAssesRiskIdInCookie = "";
                        LoggedInUserInfoFromCookie.UserAreaIdInCookie = loginResult.AreaID ?? 0;
                        LoggedInUserInfoFromCookie.UserProjectIdInCookie = loginResult.ProjectID;
                        LoggedInUserInfoFromCookie.UserExpendIdInCookie = 0;

                        if (loginResult.Role == Services.Domain.Models.Role.Enums.Role.User.ToString())
                        {
                            return RedirectToAction("Index", "ProjectDashboard");
                        }
                        return RedirectToAction("Index", "Dashboard");
                    

                },
                () => View(nameof(Index)));
        }

        #endregion

      

        #region Forgot Password Related Method

        //POST: Login/ForgotPassword
        public ActionResult ForgotPassword()
        {
         

            return View();
        }



        //POST: Login/ForgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(LoginModel loginModel)
        {
            User usr = null;

            return DataActionViewService(
                () =>
                {
                    if (loginModel != null)
                    {
                       usr = _userDomainService.ForgotPassword(loginModel.EmailAddress);
                    }
                },
                () =>
                {
                    if (usr != null)
                    {
                        string firstName = usr.FirstName;
                        string lastName = usr.LastName;
                        string emailAddress = usr.EmailAddress;
                        long userId = usr.UserID;
                        string dynamicUserGuidId = usr.UserRegistartionGuid;
                        string password = usr.Password;

                        string emailTemplatePath = HttpContext.Server.MapPath(@"~/EmailTemplate/New_ForgotPassword.html");
                        string emailBody = Emailer.GetEmailHtmlTemplate(emailTemplatePath);

                        emailBody = emailBody.Replace("###FirstName###", firstName);
                        emailBody = emailBody.Replace("###LastName###", lastName);
                        emailBody = emailBody.Replace("###dynamic_guid###", dynamicUserGuidId);
                        emailBody = emailBody.Replace("###UserID###", userId.ToString());
                        emailBody = emailBody.Replace("###EmailAddress###", emailAddress);

                        //Email template dynamic link and other image set
                        emailBody = emailBody.Replace("###New-RAMs-Logo###", Services.Utilities.SiteSettings.RAMSLogoForEmail);
                        emailBody = emailBody.Replace("###Login-Link###", Services.Utilities.SiteSettings.SiteLoginURL);
                        emailBody = emailBody.Replace("###RiskAssessment-Image###", Services.Utilities.SiteSettings.RiskAssessmentForEmail);
                        emailBody = emailBody.Replace("###About-Link###", Services.Utilities.SiteSettings.SiteAboutURL);
                        emailBody = emailBody.Replace("###FactionHSG###", Services.Utilities.SiteSettings.FactionSiteURL);
                        emailBody = emailBody.Replace("###RAMS-Site-URL###", Services.Utilities.SiteSettings.SiteURL);
                        emailBody = emailBody.Replace("###Privacy-Link###", Services.Utilities.SiteSettings.SitePrivacyURL);
                        emailBody = emailBody.Replace("###Terms-Link###", Services.Utilities.SiteSettings.SiteTermsURL);
                        emailBody = emailBody.Replace("###Contact-Us-Link###", Services.Utilities.SiteSettings.SiteContactUsURL);
                        emailBody = emailBody.Replace("###Faqs-Link###", Services.Utilities.SiteSettings.SiteFAQSURL);
                        emailBody = emailBody.Replace("###Tutorials-Link###", Services.Utilities.SiteSettings.SiteTutorialsURL);
                        emailBody = emailBody.Replace("###Facebook-Link###", Services.Utilities.SiteSettings.SiteFacebookURL);
                        emailBody = emailBody.Replace("###Facebook-Image###", Services.Utilities.SiteSettings.FacebookLogoForEmail);
                        emailBody = emailBody.Replace("###Twitter-Link###", Services.Utilities.SiteSettings.SiteTwitterURL);
                        emailBody = emailBody.Replace("###Twitter-Image###", Services.Utilities.SiteSettings.TwitterLogoForEmail);
                        emailBody = emailBody.Replace("###Linkedin-Link###", Services.Utilities.SiteSettings.SiteLinkedinURL);
                        emailBody = emailBody.Replace("###Linkeden-Image###", Services.Utilities.SiteSettings.LinkedinLogoForEmail);

                        //Emailer.SendMail(emailAddress, "Forgotten Your Password?", emailBody, null, null, null, null);

                        #region "Old Code For Password And Simple Email Template"

                        //string emailTemplatePath = Path.Combine(_hostingEnvironment.WebRootPath, @"EmailTemplate\ForgotPassword.html");
                        //string emailBody = Emailer.GetEmailHtmlTemplate(emailTemplatePath);

                        //emailBody = emailBody.Replace("###FirstName###", firstName);
                        //emailBody = emailBody.Replace("###LastName###", lastName);
                        //emailBody = emailBody.Replace("###Password###", password);

                        //Emailer.SendMail(emailAddress, "RAMS User Password Recovery", emailBody, null, null, null, null);

                        #endregion

                        return RedirectToAction("Index", "Login");
                    }
                    return RedirectToAction("Index", "SignUp");

                },
                () => View(nameof(Index)));
        }

        #endregion


    }
}