using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            UnityConfig.RegisterComponents();   //Added Unity MVC  

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
        }

        protected void FormsAuthentication_OnAuthenticate(Object sender, FormsAuthenticationEventArgs args)
         {
            HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null || string.IsNullOrWhiteSpace(authCookie.Value))
                return;

            string loggedInUserID = "";
            
            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            loggedInUserID = authTicket.Name.Split(':')[0];
            string role = authTicket.Name.Split(':')[1];
            string[] loggedInUserRoles = { role };


            Context.User = new GenericPrincipal(new GenericIdentity("LoggedInUserIdWithRole"), loggedInUserRoles);
        }
    }
}
