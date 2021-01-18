using System;
using System.Collections.Generic;
using Core.DomainService;
using Services.AppServices;
using Services.DataServices;
using Services.Domain;
using System.IO;
using System.Net.Http.Headers;
using Services.Domain.Models.User.EditorModel;
using Services.Domain.Models.User.Exceptions;
using System.Web;
using Services.HelperServices;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;

namespace Services.DomainServices
{
    public class UserDomainService : DomainService<User, long>
    {
        private readonly UserDataService _userDataService;
        public static Dictionary<string, int> dLoginFailed = new Dictionary<string, int>();
        public UserDomainService(UserDataService userDataService) : base(userDataService)
        {
            _userDataService = userDataService;
        }

      

        public User Login(string userName, string password,long cookieUserId,string userIp,bool isfromSingIn)
        {
            string encryptPassword = SimpleCryptService.Factory().Encrypt(password);
            var loginResult = _userDataService.Login(userName, encryptPassword, isfromSingIn);
       
            if (loginResult == null)
            {
                LoginFailureCountCheek(userIp);
                throw new InvalidUserSignupException(userName);
            } 
                     
            return loginResult;
        }

        public void LoginFailureCountCheek(string userIp)
        {
            // repeat offense? add to the number of attempts
            if (dLoginFailed.ContainsKey(userIp))
            {
                // increment number of failed attempts
                ++dLoginFailed[userIp];
            }
            else // first failed login attempt for given username
            {
                // first failed attempt, so initialize it
                dLoginFailed[userIp] = 1;
            }

            var falideCount = UserDomainService.dLoginFailed[userIp];

            if (falideCount >= 10)
            {
                string blockReason = "Login Failed 10 times";
             

            }

        }

        public User GetLoggedInUserInfoByUserID(long userId)
        {
            User loggedInUser = _userDataService.GetLoggedInUserInfoByUserID(userId);

            if (loggedInUser != null)
            {
                if (loggedInUser.UserImageCaption != null && loggedInUser.UserImagePath != null)
                {
                    loggedInUser.UserImageCaption = Path.GetFileNameWithoutExtension(loggedInUser.UserImageCaption);
                    loggedInUser.UserImagePath = loggedInUser.UserImagePath;

                    #region "This code does not return actual image"
                    //This code does not return actual image
                    //System.Threading.Tasks.Task<bool> isFileExist = AzureServices.AzureStorageService.IsFileExist(AzureServices.AzureBlobFolderName.USER_LOGO_FOLDER_NAME, loggedInUser.UserImageCaption);

                    //if (isFileExist.Result == false)
                    //{
                    //    loggedInUser.UserImageCaption = Path.GetFileNameWithoutExtension(Utilities.SiteSettings.DefaultLogoTitle);
                    //    loggedInUser.UserImagePath = Utilities.SiteSettings.DefaultLogo;
                    //}
                    //if (isFileExist.Result == true)
                    //{
                    //    loggedInUser.UserImageCaption = Path.GetFileNameWithoutExtension(loggedInUser.UserImageCaption);
                    //    loggedInUser.UserImagePath = loggedInUser.UserImagePath;
                    //}
                    #endregion
                }
                if (loggedInUser.UserImageCaption == null || loggedInUser.UserImagePath == null)
                {
                    loggedInUser.UserImageCaption = Path.GetFileNameWithoutExtension(Utilities.SiteSettings.DefaultLogoTitle);
                    loggedInUser.UserImagePath = Utilities.SiteSettings.DefaultLogo;
                }

            }

            return loggedInUser;
        }

        public User ForgotPassword(string emailAddress)
        {
          return  _userDataService.ForgotPassword(emailAddress);
        }


        #region profile Update Code
        public bool UpdatePersonalInformation(long loggedInUserId, PersonalInformation personalInfoNew)
        {
            return _userDataService.UpdatePersonalInformation(loggedInUserId, personalInfoNew);
        }

        public bool UpdatePassword(long loggedInUserId, UserPassword userPasswordNew)
        {
            return _userDataService.UpdatePassword(loggedInUserId, userPasswordNew);
        }
        #endregion


    }//end class


}