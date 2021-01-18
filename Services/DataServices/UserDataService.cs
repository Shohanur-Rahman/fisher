using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Core.DataService;
using Services.DataContext;
using Services.Domain;
using Services.Domain.Models.User.EditorModel;
using Services.AppServices;
using Services.RandomPasswordService;
using Services.Domain.Models.User.Exceptions;
using System.Data.Entity;
using Services.HelperServices;
using Services.Helper;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Services.Domain.Company.Models.CompanyAdmin;
using Services.Domain.Company.SPModels;
using Services.Utilities;

namespace Services.DataServices
{
    public class UserDataService : EntityContextDataService<User>
    {
        public UserDataService(AppDbContext dbContext) : base(dbContext)
        {
            DbContext.Database.CommandTimeout = SiteSettings.DbTimeOut;
        }

       

        public User Login(string userName, string encryptPassword, bool isfromSingIn)
        {
            try
            {
                if (isfromSingIn == true)
                {
                    return DbContext.Set<User>().FirstOrDefault(x =>
                   (x.UserName == userName || x.EmailAddress == userName) && x.Password == encryptPassword &&
                   x.IsActivated == true);
                }
                else
                {
                    return DbContext.Set<User>().FirstOrDefault(x =>
                   (x.UserName == userName || x.EmailAddress == userName) && x.Password == encryptPassword);
                }
            }
            catch (Exception ex) {
                throw ex;
            }
            
        }

        public User ForgotPassword(string emailAddress)
        {
            User user = null;

            if (emailAddress != null)
            {
                User usr = DbContext.Set<User>().Where(x => x.EmailAddress == emailAddress).FirstOrDefault();

                if (usr != null)
                {
                    string newRandomPassword = RandomPassword.Generate(7, 7);
                    string encryptRandomPass = SimpleCryptService.Factory().Encrypt(newRandomPassword);

                    usr.Password = encryptRandomPass;
                    usr.ConfirmPassword = encryptRandomPass;
                    usr.IsTemporaryPassword = true;

                    DbContext.SaveChanges();

                    User userInfo = new User();

                    userInfo.FirstName = usr.FirstName;
                    userInfo.LastName = usr.LastName;
                    userInfo.Password = newRandomPassword;
                    userInfo.EmailAddress = usr.EmailAddress;
                    userInfo.UserID = usr.UserID;
                    userInfo.UserRegistartionGuid = usr.UserRegistartionGuid;

                    return userInfo;

                }
            }

            return user;
        }


        public User GetLoggedInUserInfoByUserID(long userId)
        {
            return DbContext.Set<User>().Where(x => x.UserID == userId).FirstOrDefault();
        }


        public bool UpdatePersonalInformation(long loggedInUserId, PersonalInformation personalInfoNew)
        {
            User oldUserData = DbContext.Set<User>().Where(x => x.UserID == personalInfoNew.UserId).FirstOrDefault();

            if (oldUserData != null)
            {
                oldUserData.FirstName = personalInfoNew.FirstName;
                oldUserData.LastName = personalInfoNew.LastName;
                oldUserData.EmailAddress = personalInfoNew.EmailAddress;
                oldUserData.Position = personalInfoNew.Position;
                oldUserData.EditedDate = DateTime.Now;
                oldUserData.EditedBy = loggedInUserId;
                oldUserData.EditedIP = IPAndBrowserDetectionHelper.GetClientIPAddress();
            }

            //DbContext.Update<User>(oldUserData);
            int count = DbContext.SaveChanges();

            return count > 0 ? true : false;
        }

        public bool UpdatePassword(long loggedInUserId, UserPassword userPasswordNew)
        {
            bool isValidPassword = false;
            User oldUserData = DbContext.Set<User>().Where(x => x.UserID == userPasswordNew.UserId).FirstOrDefault();

            string decryptPass = SimpleCryptService.Factory().Decrypt(oldUserData.Password);


            if (decryptPass == userPasswordNew.CurrentPassword)
            {
                string encryptNewPass = SimpleCryptService.Factory().Encrypt(userPasswordNew.Password);
                string encryptCofirmNewPass = SimpleCryptService.Factory().Encrypt(userPasswordNew.ConfirmPassword);

                if (oldUserData != null)
                {
                    oldUserData.Password = encryptNewPass;
                    oldUserData.ConfirmPassword = encryptCofirmNewPass;
                    oldUserData.EditedDate = DateTime.Now;
                    oldUserData.EditedBy = loggedInUserId;
                    oldUserData.EditedIP = IPAndBrowserDetectionHelper.GetClientIPAddress();
                }

                isValidPassword = true;
            }

            if (isValidPassword == false)
            {


            }

            int count = DbContext.SaveChanges();
            return count > 0 ? true : false;
        }


    }
}