using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace Services.Utilities
{
    public class SiteSettings
    {

        public static string BaseUrl
        {
            get
            {
                string orginalUrl = HttpContext.Current.Request.Url.AbsoluteUri;
                string partUrl = "";

                if (HttpContext.Current.Request.Url.Query.Length > 0)
                {
                    orginalUrl = orginalUrl.Replace(HttpContext.Current.Request.Url.Query, String.Empty);
                }

                if (HttpContext.Current.Request.ApplicationPath == "/")
                {
                    partUrl = "";
                }
                else
                {
                    partUrl = HttpContext.Current.Request.ApplicationPath;
                }

                return orginalUrl.Replace(HttpContext.Current.Request.Url.AbsolutePath, String.Empty) + partUrl + "/";

            }

        }

        public static string ApplicationLogo
        {
            get
            {
                return "rams-logo/rams-logo.png";
            }

        }

        public static string DefaultLogo
        {
            get
            {
                return "";
            }

        }
        public static string RAMSLogoForEmail
        {
            get
            {
                return "";
            }

        }
        public static string RiskAssessmentForEmail
        {
            get
            {
                return "";
            }

        }

        
            

        public static string DefaultLogoTitle
        {
            get
            {
                return "rams-logo.png";
            }

        }

        public static string SiteURL
        {
            
            get
            {
                return @"http://localhost:51385/";
                //return @"http://nobintraders.com/";
                //return @"http://ramsapp.azurewebsites.net/";                

            }
        }

        public static string SiteLoginURL
        {

            get
            {
                return SiteURL + "Login";
            }
        }

        public static string SiteCompanyLoginURL
        {

            get
            {
                return SiteURL + "CompanyLogin";
            }
        }

        public static string SiteSupportURL
        {

            get
            {
                return SiteURL + "CustomerSupport";
            }
        }

        

        public static string SiteAboutURL
        {

            get
            {
                return "https://www.rams-app.co.uk/about/";
            }
        }

        public static string FactionSiteURL
        {

            get
            {
                return "https://www.factionhsg.co.uk/";
            }
        }

        public static string SitePrivacyURL
        {

            get
            {
                return "https://www.rams-app.co.uk/privacy-policy";
            }
        }

        public static string SiteTermsURL
        {

            get
            {
                return "https://www.rams-app.co.uk/terms-of-business";
            }
        }

        public static string SiteContactUsURL
        {

            get
            {
                return "https://www.rams-app.co.uk/contact-us";
            }
        }

        public static string SiteFAQSURL
        {

            get
            {
                return "https://www.rams-app.co.uk/faqs";
            }
        }

        public static string SiteTutorialsURL
        {

            get
            {
                return SiteURL + "Tutorials";
            }
        }

        public static string SiteFacebookURL
        {

            get
            {
                return "https://www.facebook.com/ramsapp/";
            }
        }

        public static string FacebookLogoForEmail
        {

            get
            {
                return "";
            }
        }

        public static string SiteTwitterURL
        {

            get
            {
                return "https://twitter.com/rams_app";
            }
        }

        public static string TwitterLogoForEmail
        {

            get
            {
                return "";
            }
        }

        public static string SiteLinkedinURL
        {

            get
            {
                return "https://www.linkedin.com/company/18112592/";
            }
        }

        public static string LinkedinLogoForEmail
        {

            get
            {
                return"";
            }
        }

        // this is used for js and css file to force the user’s browser to clear its cache 
        public static string UploadVersion
        {
            get
            {
                //return "1.8";

                return WebConfigurationManager.AppSettings["UploadVersion"];
            }

        }

        public static int DbTimeOut
        {
            get
            {
                string stringDbTimeOut = WebConfigurationManager.AppSettings["TimeOutValue"];
                int timeout = Convert.ToInt32(stringDbTimeOut);
                return timeout;
            }

        }

    }


}
