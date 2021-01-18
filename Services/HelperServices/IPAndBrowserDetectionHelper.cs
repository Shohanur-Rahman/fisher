using System;
using System.Web;

namespace Services.Helper
{
    public class IPAndBrowserDetectionHelper
    {
  

        public static string GetClientIPAddress()
        {
            string clientIP = "";
            try
            {
                string ipAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                string remoteIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                if (ipAddress != null)
                {
                    clientIP = ipAddress.Split(':')[0];
                }
                else if (remoteIpAddress != null)
                {
                    clientIP = remoteIpAddress;
                }
            }
            catch (Exception err)
            {
                clientIP = "Not recorded " + err;
            }
            return clientIP;
        }

        public static string GetClientUserAgent()
        {
            string returnVal = "";
            try
            {
                returnVal = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
            }
            catch (Exception err)
            {
                returnVal = "Not recorded " + err;
            }
            return returnVal;
        }

        public static string GetClientBrowserName()
        {
            string returnVal = "";
            try
            {
                returnVal = HttpContext.Current.Request.Browser["Browser"];
            }
            catch (Exception err)
            {
                returnVal = "Not recorded " + err;
            }
            return returnVal;
        }

        public static string GetClientBrowserVersion()
        {
            string returnVal = "";
            try
            {
                returnVal = HttpContext.Current.Request.Browser["Version"];
            }
            catch (Exception err)
            {
                returnVal = "Not recorded " + err;
            }
            return returnVal;
        }

    }
}