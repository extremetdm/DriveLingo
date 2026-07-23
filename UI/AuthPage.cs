using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace DriveLingo.UI
{
    public abstract class AuthPage : Page
    {
        /// <summary>
        /// Retrieves the current authenticated User loaded during Application_PostAuthenticateRequest.
        /// </summary>
        protected Database.Models.User CurrentUser
        {
            get
            {
                return HttpContext.Current?.Items["CurrentUser"] as Database.Models.User;
            }
        }

        /// <summary>
        /// Convenience check to verify if a user is currently logged in.
        /// </summary>
        protected bool IsLoggedIn => CurrentUser != null;

        protected void RequireAuth()
        {
            RequireAuth(null);
        }

        /// <summary>
        /// Helper method to enforce authentication on pages that require login.
        /// </summary>
        protected void RequireAuth(Database.Models.User.UserRole? role)
        {
            bool shouldRedirect = !IsLoggedIn;
            shouldRedirect |= role != null && CurrentUser.Role == role;

            if (shouldRedirect)
            {
                Response.Redirect("~/Login.aspx", true);
            }
        }
    }
}