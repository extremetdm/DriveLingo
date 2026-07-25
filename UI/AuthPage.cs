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
        protected Database.Models.User CurrentUser
        {
            get
            {
                var dbUser = HttpContext.Current?.Items["CurrentUser"] as Database.Models.User;
                if (dbUser != null) return dbUser;

                var sessionUser = Session?["CurrentUser"] as DriveLingo.Models.User;
                if (sessionUser != null)
                {
                    var role = Database.Models.User.UserRole.Learner;
                    if (sessionUser.Role == "admin") role = Database.Models.User.UserRole.Admin;
                    else if (sessionUser.Role == "educator" || sessionUser.Role == "instructor") role = Database.Models.User.UserRole.Instructor;
                    else if (sessionUser.Role == "guest") role = Database.Models.User.UserRole.Guest;

                    return new Database.Models.User
                    {
                        Id = 9999,
                        Username = sessionUser.Name,
                        Email = sessionUser.Email,
                        Role = role,
                        Points = sessionUser.Points,
                        XP = sessionUser.XP
                    };
                }

                return null;
            }
        }

        protected bool IsGuest => (Session?["IsGuestMode"] != null && (bool)Session["IsGuestMode"]) || (CurrentUser != null && CurrentUser.Role == Database.Models.User.UserRole.Guest);

        protected bool IsLoggedIn => CurrentUser != null;

        protected void RequireAuth()
        {
            RequireAuth(null);
        }

        protected void RequireAuth(Database.Models.User.UserRole? role)
        {
            if (role == Database.Models.User.UserRole.Admin || role == Database.Models.User.UserRole.Instructor)
            {
                if (IsGuest || CurrentUser == null || CurrentUser.Role != role)
                {
                    Response.Redirect("~/Login.aspx", true);
                    return;
                }
            }

            if (!IsLoggedIn)
            {
                Response.Redirect("~/Login.aspx", true);
            }
        }
    }
}