using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Services.Domain;
using Services.DomainServices;
using Web.AppCode;
using Services.Domain.Models.User.EditorModel;
using System.Security.Claims;
using System.Collections.Generic;
using Services.AppServices;
using System;
using System.Web;
using System.Web.Mvc;
using System.Security;

using Services.EmailService;


namespace Web.Controllers
{
    [Authorize]
    public class ProfileController : BaseController
    {
        private readonly UserDomainService _userDomainService;


        #region Controller Constructor
        public ProfileController(UserDomainService userDomainServiceContext)
        {
            _userDomainService = userDomainServiceContext;
     
        }
        #endregion

        #region "HTML String For User Image"

        string newUserImageHtmlString = "<img src='###ImageURL###' id='divUplodedUserImg' class='img-responsive dummy_divUplodedUserImg' alt='User Logo' title='###ImageCaption###'>";

        #endregion

        #region Bind data for update

 
        public ActionResult Index(string chkRedirect)
        {

            UserProfileModel userProfileModel = new UserProfileModel();
          

            userProfileModel.PersonalInfo = new PersonalInformation();
            userProfileModel.UserPassword = new UserPassword();
            long userId = LoggedInUserInfoFromCookie.UserIdInCookie;
            long companyId = LoggedInUserInfoFromCookie.UserCompanyIdInCookie;
            string userRole = LoggedInUserInfoFromCookie.UserRoleInCookie;
            User loggedInUserData = null;

            string passMessage = MessageDisplayHelper.ErrorMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.PasswordInformationMessage = passMessage;
            string infoMessage = MessageDisplayHelper.InformationMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.InformationMessage = infoMessage;

            return DataActionViewService(
                () =>
                {
                    loggedInUserData = _userDomainService.GetLoggedInUserInfoByUserID(userId);
        
                },
                () =>
                {
                    if (loggedInUserData != null)
                    {
                      
                        //set Personal Information Value
                        userProfileModel.PersonalInfo.UserId = userId;
                        userProfileModel.PersonalInfo.FirstName = loggedInUserData.FirstName;
                        userProfileModel.PersonalInfo.LastName = loggedInUserData.LastName;
                        userProfileModel.PersonalInfo.EmailAddress = loggedInUserData.EmailAddress;
                        userProfileModel.PersonalInfo.Position = loggedInUserData.Position;
                        userProfileModel.PersonalInfo.UserImageCaption = loggedInUserData.UserImageCaption;
                        userProfileModel.PersonalInfo.UserImagePath = loggedInUserData.UserImagePath;
                        userProfileModel.PersonalInfo.UserCreatedBy = loggedInUserData.UserCreatedBy;
                        userProfileModel.PersonalInfo.IsImageUploadedByUser = loggedInUserData.IsImageUploadedByUser;
                        userProfileModel.PersonalInfo.IsTrialUser = loggedInUserData.IsTrialUser;
                        //Set Password object value
                        userProfileModel.UserPassword.UserId = userId;

                        //userProfileModel.SupportLink = Services.Utilities.SiteSettings.SiteSupportURL;
                        //User Document Settings Related Code
     
                        if (chkRedirect != null && chkRedirect == "fromAddDocument")
                        {
                            ViewBag.RedirectCheck = "fromAddDocument";
                        }
                        return View(userProfileModel);
                     

                    }

                    return View();

                },
                () => View(nameof(Index)));


        }

        #endregion

        #region update user profile information related code

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserProfileInformationUpdate(long? userId, PersonalInformation personalInfo)
        {

            bool isUpdateSuccess = false;
            long loggedInUserId = LoggedInUserInfoFromCookie.UserIdInCookie;

            return DataActionViewService(
              () =>
             {
                 isUpdateSuccess = _userDomainService.UpdatePersonalInformation(loggedInUserId, personalInfo);
             },
            () =>
            {
                if (isUpdateSuccess == true)

                {

                    LoggedInUserInfoFromCookie.UserFirstNameInCookie = personalInfo.FirstName;
                    LoggedInUserInfoFromCookie.UserLastNameInCookie = personalInfo.LastName;
                    LoggedInUserInfoFromCookie.UserDisplayNameInCookie = personalInfo.FirstName + " " + personalInfo.LastName;
                    string loggedUserName = LoggedInUserInfoFromCookie.UserDisplayNameInCookie;
                    if (loggedUserName.Length > 20)
                    {
                        string FirstName = LoggedInUserInfoFromCookie.UserFirstNameInCookie;
                        string LastName = LoggedInUserInfoFromCookie.UserLastNameInCookie;
                        //ViewBag.UserNameWithNewLine = string.Format("{0} {1} {2}", FirstName, Environment.NewLine, LastName);
                        LoggedInUserInfoFromCookie.LoggedInUserDisplayName = string.Format("{0} {1} {2}", FirstName, Environment.NewLine, LastName);
                    }
                    else
                    { LoggedInUserInfoFromCookie.LoggedInUserDisplayName = loggedUserName; }
                    MessageDisplayHelper.InformationMessageSetOrDisplay(this, true, "The personal information has been updated.");

                    return RedirectToAction("Index", "Profile");
                }

                return RedirectToAction("Index");
            },
            () => View(nameof(Index)));

        }

        #endregion

        #region update user password change related code

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserPasswordUpdate(long? userId, UserPassword userPassword)
        {

            bool isUpdateSuccess = false;
            long loggedInUserId = LoggedInUserInfoFromCookie.UserIdInCookie;

            return DataActionViewService(
                () =>
                {
                   isUpdateSuccess = _userDomainService.UpdatePassword(loggedInUserId, userPassword);

                },
                () =>
                {
                    if (isUpdateSuccess == true)
                    {
                        MessageDisplayHelper.InformationMessageSetOrDisplay(this, true, "The password has been updated.");
                        return RedirectToAction("Index", "Profile");
                    }
                    MessageDisplayHelper.ErrorMessageSetOrDisplay(this, true, "The current password does not match.");
                    return RedirectToAction("Index", "Profile");
                },
                () => View(nameof(Index)));
        }

        #endregion


        #region "For User Imgae Upload" 

        public ActionResult UserImageUpload(List<HttpPostedFileBase> files)
        {
            Guid fileNameInGuid;

            string imagePath = "";
            string actualFileName = "";
            long userId = LoggedInUserInfoFromCookie.UserIdInCookie;

            return DataActionViewService(
                () =>
                {
                    if (files != null)
                    {
                        foreach (var file in files)
                        {
                            fileNameInGuid = Guid.NewGuid();
                            actualFileName = Path.GetFileNameWithoutExtension(file.FileName);
                            //imagePath = _userDomainService.UserImageUpload(file, fileNameInGuid, userId);

                            break;
                        }
                    }
                },
                () =>
                {
                    if (imagePath != null || imagePath != "")
                    {
                        LoggedInUserInfoFromCookie.UserLogoInCookie = imagePath;
                        LoggedInUserInfoFromCookie.LoggedInUserLogo = imagePath;
                       // string imageURL = AzureStorageURL.BLOB_URL + imagePath;
                        string imageCaption = actualFileName;
                        var imageURL = "";
                        newUserImageHtmlString = newUserImageHtmlString.Replace("###ImageURL###", imageURL);
                        newUserImageHtmlString = newUserImageHtmlString.Replace("###ImageCaption###", imageCaption);

                        return Json(new { DynamicHtml = newUserImageHtmlString, UploadImageURL = imageURL });

                    }
                    return View(nameof(Index));

                },
                () => View(nameof(Index)));

        }

        #endregion


      

    }
}
