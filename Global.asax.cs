using DriveLingo.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace DriveLingo
{
    public partial class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated && Context.User != null)
            {
                if (Session != null && Session["CurrentUser"] is User)
                {
                    User currentUser = Session["CurrentUser"] as User;

                    string[] roles = new[] { currentUser.Role.ToString() };

                    Context.User = new GenericPrincipal(Context.User.Identity, roles);
                }
            }
        }
    }
}