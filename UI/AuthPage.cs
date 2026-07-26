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
        /// Retrieves the current authenticated or session User.
        /// </summary>
        /// 
        protected Database.Models.User CurrentUser
        {
            get
            {
                return HttpContext.Current?.Items["CurrentUser"] as Database.Models.User;
            }
        }

        protected bool IsGuest => (CurrentUser == null || CurrentUser.Role == Database.Models.User.UserRole.Guest);

        protected bool IsLoggedIn => !IsGuest;

        protected void RequireAuth()
        {
            RequireAuth(null);
        }

        protected void RequireAuth(Database.Models.User.UserRole? role)
        {
            if (!IsLoggedIn)
            {
                Response.Redirect("~/Login.aspx", true);
                return;
            }

            if (role == Database.Models.User.UserRole.Admin || role == Database.Models.User.UserRole.Instructor)
            {
                if (CurrentUser.Role != role)
                {
                    Response.Redirect("~/Login.aspx", true);
                    return;
                }
            }
        }
    }
}