using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Security;
//using Microsoft.AspNetCore.Authorization;
using System.Web.Mvc;
using Services.Domain;
using Services.DomainServices;
using System.Security.Claims;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Http;

using System.Web;
using Services.Domain.UserBilling;

namespace Web.AppCode
{
    public class LoggedInUserInfoFromCookie 
    {

        public static string LoggedInUserLogo;
        public static string LoggedInUserDisplayName;

        public static string UserIdRoleEncryptedInCookie
        {
            set
            {
                var authTicket = new FormsAuthenticationTicket(value, true, 7200);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                authCookie.Expires = DateTime.Now.AddMinutes(7200);
                HttpContext.Current.Response.Cookies.Add(authCookie);
            }
        }

        public static string UserRoleInCookie
        {

            get
            {
                HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie == null || string.IsNullOrWhiteSpace(authCookie.Value))
                    return null;

                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                string role = authTicket.Name.Split(':')[1];

                if (String.IsNullOrEmpty(role) == false)
                    return role;

                return null;
            }

        }

        public static long UserIdInCookie
        {
            get
            {
                HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie == null || string.IsNullOrWhiteSpace(authCookie.Value))
                    return -1;

                string loggedInUserID = "";

                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                loggedInUserID = authTicket.Name.Split(':')[0];
                return long.Parse(loggedInUserID);
            }
          
        }

        
        public static string UserLogoInCookie
        {
        

            get
            {

                string loggedInUserLogo = HttpContext.Current.Request.Cookies[Services.Domain.Models.User.Enums.UserCookieData.UserOrCompanyLogo.ToString()].Value;
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(loggedInUserLogo);
                return authTicket.Name;
            }
            set
            {
                var authTicket = new FormsAuthenticationTicket(value, true, 7200);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                var authCookie = new HttpCookie(Services.Domain.Models.User.Enums.UserCookieData.UserOrCompanyLogo.ToString(), encryptedTicket);
               // HttpCookie cookie = new HttpCookie(Services.Domain.Models.User.Enums.UserCookieData.UserOrCompanyLogo.ToString(), encryptedTicket);
                authCookie.Expires = DateTime.Now.AddDays(365);
                HttpContext.Current.Response.Cookies.Add(authCookie);
            }
        }

        public static long UserCompanyIdInCookie
        {
            get
            {
                string companyId = HttpContext.Current.Request.Cookies[Services.Domain.Models.User.Enums.UserCookieData.CompanyId.ToString()].Value;
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(companyId);           
                return Convert.ToInt64(authTicket.Name);
            }
            set
            {   
                 var authTicket = new FormsAuthenticationTicket(value.ToString(), true, 7200);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                var authCookie = new HttpCookie(Services.Domain.Models.User.Enums.UserCookieData.CompanyId.ToString(), encryptedTicket);               
                authCookie.Expires = DateTime.Now.AddDays(365);
                HttpContext.Current.Response.Cookies.Add(authCookie);
            }
        }


        public static string UserCompanyNameInCookie
        {
            get
            {
                string CompanyName = HttpContext.Current.Request.Cookies[Services.Domain.Models.User.Enums.UserCookieData.CompanyName.ToString()].Value;
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(CompanyName);
                return authTicket.Name;
            }
            set
            {
                var authTicket = new FormsAuthenticationTicket(value.ToString(), true, 7200);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                var authCookie = new HttpCookie(Services.Domain.Models.User.Enums.UserCookieData.CompanyName.ToString(), encryptedTicket);
                authCookie.Expires = DateTime.Now.AddDays(365);
                HttpContext.Current.Response.Cookies.Add(authCookie);
            }
        }

        //User First Name
        public static string UserFirstNameInCookie
        {
            get
            {
                string firstName = HttpContext.Current.Request.Cookies[Services.Domain.Models.User.Enums.UserCookieData.UserFirstName.ToString()].Value;
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(firstName);
                return authTicket.Name;     
            }
            set
            {

                var authTicket = new FormsAuthenticationTicket(value, true, 7200);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                var authCookie = new HttpCookie(Services.Domain.Models.User.Enums.UserCookieData.UserFirstName.ToString(), encryptedTicket);
                authCookie.Expires = DateTime.Now.AddDays(365);
                HttpContext.Current.Response.Cookies.Add(authCookie);

                //HttpCookie cookie = new HttpCookie(Services.Domain.Models.User.Enums.UserCookieData.UserFirstName.ToString());
                //cookie.Value = value.ToString();
                //cookie.Expires = DateTime.Now.AddDays(365);
                //HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public static string UserLastNameInCookie
        {
            get
            {
                string lastName = HttpContext.Current.Request.Cookies[Services.Domain.Models.User.Enums.UserCookieData.UserLastName.ToString()].Value;
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(lastName);
                return authTicket.Name;
            }
            set
            {
                //HttpCookie cookie = new HttpCookie(Services.Domain.Models.User.Enums.UserCookieData.UserLastName.ToString());
                //cookie.Value = value.ToString();
                //cookie.Expires = DateTime.Now.AddDays(365);
                //HttpContext.Current.Response.Cookies.Add(cookie);

                var authTicket = new FormsAuthenticationTicket(value, true, 7200);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                var authCookie = new HttpCookie(Services.Domain.Models.User.Enums.UserCookieData.UserLastName.ToString(), encryptedTicket);
                authCookie.Expires = DateTime.Now.AddDays(365);
                HttpContext.Current.Response.Cookies.Add(authCookie);
            }
        }

        public static string UserDisplayNameInCookie
        {
            get
            {
                string displayName = HttpContext.Current.Request.Cookies[Services.Domain.Models.User.Enums.UserCookieData.DisplayName.ToString()].Value;
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(displayName);
                return authTicket.Name;
            }
            set
            {
                //HttpCookie cookie = new HttpCookie(Services.Domain.Models.User.Enums.UserCookieData.DisplayName.ToString());
                //cookie.Value = value.ToString();
                //cookie.Expires = DateTime.Now.AddDays(365);
                //HttpContext.Current.Response.Cookies.Add(cookie);

                var authTicket = new FormsAuthenticationTicket(value, true, 7200);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                var authCookie = new HttpCookie(Services.Domain.Models.User.Enums.UserCookieData.DisplayName.ToString(), encryptedTicket);
                authCookie.Expires = DateTime.Now.AddDays(365);
                HttpContext.Current.Response.Cookies.Add(authCookie);
            }
        }

        public static string PaidUserExpiredStatusInCookie
        {
            get
            {
                string paidUserStatus = HttpContext.Current.Request.Cookies[Services.Domain.Models.User.Enums.UserCookieData.PaidUserStatus.ToString()].Value;
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(paidUserStatus);
                return authTicket.Name;
            }
            set
            {
                //HttpCookie cookie = new HttpCookie(Services.Domain.Models.User.Enums.UserCookieData.PaidUserStatus.ToString());
                //cookie.Value = value.ToString();
                //cookie.Expires = DateTime.Now.AddDays(365);
                //HttpContext.Current.Response.Cookies.Add(cookie);


                var authTicket = new FormsAuthenticationTicket(value, true, 7200);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                var authCookie = new HttpCookie(Services.Domain.Models.User.Enums.UserCookieData.PaidUserStatus.ToString(), encryptedTicket);
                authCookie.Expires = DateTime.Now.AddDays(365);
                HttpContext.Current.Response.Cookies.Add(authCookie);
            }
        }

        public static string TrialUserExpiredStatusInCookie
        {
            get
            {
                string trialUserStatus = HttpContext.Current.Request.Cookies[Services.Domain.Models.User.Enums.UserCookieData.TrialUserStatus.ToString()].Value;
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(trialUserStatus);
                return authTicket.Name;
            }
            set
            {
                //HttpCookie cookie = new HttpCookie(Services.Domain.Models.User.Enums.UserCookieData.TrialUserStatus.ToString());
                //cookie.Value = value.ToString();
                //cookie.Expires = DateTime.Now.AddDays(365);
                //HttpContext.Current.Response.Cookies.Add(cookie);

                var authTicket = new FormsAuthenticationTicket(value, true, 7200);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                var authCookie = new HttpCookie(Services.Domain.Models.User.Enums.UserCookieData.TrialUserStatus.ToString(), encryptedTicket);
                authCookie.Expires = DateTime.Now.AddDays(365);
                HttpContext.Current.Response.Cookies.Add(authCookie);
            }
        }


        public static bool UserRememberMeOptionsInCoockie
        {
          
           set
            {
                //HttpCookie cookieRemeberMe = new HttpCookie(Services.Domain.Models.User.Enums.UserCookieData.RememberMePassword.ToString());
                //cookieRemeberMe.Expires = DateTime.Now.AddDays(365);
                //string rememberMe = "false";
                //if (value == true)
                //{
                //    rememberMe = "true";
                //}
                //cookieRemeberMe.Value = rememberMe;
                //HttpContext.Current.Response.Cookies.Add(cookieRemeberMe);

                string rememberMe = "false";
                if (value == true)
                {
                    rememberMe = "true";
                }
                var authTicket = new FormsAuthenticationTicket(rememberMe, true, 7200);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                var authCookie = new HttpCookie(Services.Domain.Models.User.Enums.UserCookieData.RememberMePassword.ToString(), encryptedTicket);
                authCookie.Expires = DateTime.Now.AddDays(365);
                HttpContext.Current.Response.Cookies.Add(authCookie);
            }
        }

        

        public static string SelectedAssesRiskIdInCookie
        {
            get
            {
                string selectedAssesRiskId = HttpContext.Current.Request.Cookies[Services.Domain.Models.User.Enums.UserCookieData.SelectedAssesRiskId.ToString()].Value;
                SelectedAssesRiskIdInCookie="";
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(selectedAssesRiskId);
                return authTicket.Name;
            }
            set
            {
                //HttpCookie cookie = new HttpCookie(Services.Domain.Models.User.Enums.UserCookieData.SelectedAssesRiskId.ToString());
                //cookie.Expires = DateTime.Now.AddDays(365);
                //cookie.Value = value;
                //HttpContext.Current.Response.Cookies.Add(cookie);

                var authTicket = new FormsAuthenticationTicket(value, true, 7200);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                var authCookie = new HttpCookie(Services.Domain.Models.User.Enums.UserCookieData.SelectedAssesRiskId.ToString(), encryptedTicket);
                authCookie.Expires = DateTime.Now.AddDays(365);
                HttpContext.Current.Response.Cookies.Add(authCookie);
            }
        }

        public static string UserPackageNameInCookie
        {
            get
            {
                string packageName = HttpContext.Current.Request.Cookies[Services.Domain.Models.User.Enums.UserCookieData.PackageName.ToString()].Value;
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(packageName);
                return authTicket.Name;
            }
            set
            {
                //HttpCookie cookie = new HttpCookie(Services.Domain.Models.User.Enums.UserCookieData.PackageName.ToString());
                //cookie.Expires = DateTime.Now.AddDays(365);
                //cookie.Value = value;
                //HttpContext.Current.Response.Cookies.Add(cookie);

                var authTicket = new FormsAuthenticationTicket(value, true, 7200);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                var authCookie = new HttpCookie(Services.Domain.Models.User.Enums.UserCookieData.PackageName.ToString(), encryptedTicket);
                authCookie.Expires = DateTime.Now.AddDays(365);
                HttpContext.Current.Response.Cookies.Add(authCookie);
            }
        }


        public static string UserCreatedDateInCookie
        {
            get
            {
                string createdDate = HttpContext.Current.Request.Cookies[Services.Domain.Models.User.Enums.UserCookieData.UserCreatedDate.ToString()].Value;
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(createdDate);
                return authTicket.Name;
            }
            set
            {
                //HttpCookie cookie = new HttpCookie(Services.Domain.Models.User.Enums.UserCookieData.UserCreatedDate.ToString());
                //cookie.Expires = DateTime.Now.AddDays(365);
                //cookie.Value = value.ToString();
                //HttpContext.Current.Response.Cookies.Add(cookie);

                var authTicket = new FormsAuthenticationTicket(value, true, 7200);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                var authCookie = new HttpCookie(Services.Domain.Models.User.Enums.UserCookieData.UserCreatedDate.ToString(), encryptedTicket);
                authCookie.Expires = DateTime.Now.AddDays(365);
                HttpContext.Current.Response.Cookies.Add(authCookie);
            }
        }


        public static string LegislationModalStatus
        {
            get
            {
                string status = HttpContext.Current.Request.Cookies[Services.Domain.Models.User.Enums.UserCookieData.LegislationModalStatus.ToString()].Value;
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(status);
                return authTicket.Name;
            }
            set
            {
                //HttpCookie cookie = new HttpCookie(Services.Domain.Models.User.Enums.UserCookieData.LegislationModalStatus.ToString());
                //cookie.Value = value.ToString();
                //cookie.Expires = DateTime.Now.AddDays(365);
                //HttpContext.Current.Response.Cookies.Add(cookie);

                var authTicket = new FormsAuthenticationTicket(value, true, 7200);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                var authCookie = new HttpCookie(Services.Domain.Models.User.Enums.UserCookieData.LegislationModalStatus.ToString(), encryptedTicket);
                authCookie.Expires = DateTime.Now.AddDays(365);
                HttpContext.Current.Response.Cookies.Add(authCookie);
            }
        }



        public static int UserAreaIdInCookie
        {
            get
            {
                string userAreaId = HttpContext.Current.Request.Cookies[Services.Domain.Models.User.Enums.UserCookieData.UserAreaId.ToString()].Value;
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(userAreaId);
                return Convert.ToInt32(authTicket.Name);
            }
            set
            {
                var authTicket = new FormsAuthenticationTicket(value.ToString(), true, 7200);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                var authCookie = new HttpCookie(Services.Domain.Models.User.Enums.UserCookieData.UserAreaId.ToString(), encryptedTicket);
                authCookie.Expires = DateTime.Now.AddDays(365);
                HttpContext.Current.Response.Cookies.Add(authCookie);
            }
        }

        public static int UserExpendIdInCookie
        {
            get
            {
                string userAreaId = HttpContext.Current.Request.Cookies[Services.Domain.Models.User.Enums.UserCookieData.UserExpendId.ToString()].Value;
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(userAreaId);
                return Convert.ToInt32(authTicket.Name);
            }
            set
            {
                var authTicket = new FormsAuthenticationTicket(value.ToString(), true, 7200);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                var authCookie = new HttpCookie(Services.Domain.Models.User.Enums.UserCookieData.UserExpendId.ToString(), encryptedTicket);
                authCookie.Expires = DateTime.Now.AddDays(365);
                HttpContext.Current.Response.Cookies.Add(authCookie);
            }
        }


        public static long UserProjectIdInCookie
        {
            get
            {
                string userProjectId = HttpContext.Current.Request.Cookies[Services.Domain.Models.User.Enums.UserCookieData.UserProjectId.ToString()].Value;
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(userProjectId);
                return Convert.ToInt32(authTicket.Name);
            }
            set
            {
                var authTicket = new FormsAuthenticationTicket(value.ToString(), true, 7200);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                var authCookie = new HttpCookie(Services.Domain.Models.User.Enums.UserCookieData.UserProjectId.ToString(), encryptedTicket);
                authCookie.Expires = DateTime.Now.AddDays(365);
                HttpContext.Current.Response.Cookies.Add(authCookie);
            }
        }


    }//end class
}
