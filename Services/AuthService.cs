using DriveLingo.Database;
using DriveLingo.Database.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace DriveLingo.Services
{
    public static class AuthService
    {
        public static User Login(string email, string password, bool rememberMe)
        {
            using (var db = new AppDbContext())
            {
                var user = db.Users.Where(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();

                if (user == null) return null;

                if (!BCrypt.Net.BCrypt.Verify(password, user.Password)) return null;

                IssueAuthCookie(user, rememberMe);

                return user;
            }
        }
        private static void IssueAuthCookie(User user)
        {
            IssueAuthCookie(user, true);
        }

        private static void IssueAuthCookie(User user, bool rememberMe)
        {
            var ticket = new FormsAuthenticationTicket(
                1,
                user.Id.ToString(),
                DateTime.Now,
                DateTime.Now.AddMinutes(2880),
                rememberMe,
                user.Role.ToString(),
                FormsAuthentication.FormsCookiePath
            );

            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,
                Secure = FormsAuthentication.RequireSSL
            };

            if (rememberMe)
            {
                cookie.Expires = ticket.Expiration;
            }

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static void AuthenticateRequest(HttpContext context)
        {
            if (context == null) return;

            string path = context.Request.AppRelativeCurrentExecutionFilePath;
            if (path.EndsWith(".css") || path.EndsWith(".js") || path.EndsWith(".png") || path.EndsWith(".jpg") || path.EndsWith(".ico"))
            {
                return;
            }

            HttpCookie authCookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null || string.IsNullOrEmpty(authCookie.Value)) return;

            try
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                if (ticket == null || ticket.Expired) return;

                int userId;
                if (!int.TryParse(ticket.Name, out userId)) return;

                using (var db = new AppDbContext())
                {
                    var user = db.Users.Find(userId);
                    if (user == null)
                    {
                        FormsAuthentication.SignOut();
                        return;
                    }

                    string[] roles = new[] { user.Role.ToString() };
                    var identity = new GenericIdentity(user.Username ?? "Guest Candidate");
                    context.User = new GenericPrincipal(identity, roles);

                    RefreshCurrentUser(context, db, user);
                }
            }
            catch
            {
                FormsAuthentication.SignOut();
            }
        }

        public static void RefreshCurrentUser(AppDbContext db, User user)
        {
            if (HttpContext.Current != null)
            {
                RefreshCurrentUser(HttpContext.Current, db, user);
            }
        }

        public static void RefreshCurrentUser(HttpContext context, AppDbContext db, User user)
        {
            context.Items["CurrentUser"] = user;
            context.Items["EquippedItems"] = user.ShopRedemptions
                .Where(r => r.IsEquiped)
                .Select(r => r.Item)
                .ToList();
        }

        public static void Logout(HttpContext context)
        {
            FormsAuthentication.SignOut();
            
            context.Session?.Clear();
            context.Session?.Abandon();

            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "")
            {
                Expires = DateTime.Now.AddDays(-1),
                HttpOnly = true,
                Secure = FormsAuthentication.RequireSSL
            };

            context.Response.Cookies.Add(authCookie);

            FormsAuthentication.RedirectToLoginPage();
        }

        public static User CreateGuest()
        {
            var guest = new User
            {
                Role = User.UserRole.Guest
            };

            using (var db = new AppDbContext())
            {
                db.Users.Add(guest);
                db.SaveChanges();

                IssueAuthCookie(guest, false);
                RefreshCurrentUser(db, guest);
            }

            return guest;
        }

        public static ServiceStatusOutput Register(string username, string password, string email)
        {
            return Register(username, password, email, null);
        }

        public static ServiceStatusOutput Register(string username, string password, string email, int? guestUserId)
        {
            using (var db = new AppDbContext())
            {
                var sameUsernameUser = db.Users
                    .Where(u => u.Username == username)
                    .FirstOrDefault();
                if (sameUsernameUser != null)
                {
                    return ServiceStatusOutput.error("Username has been taken.");
                }

                var sameEmailUser = db.Users
                    .Where(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();
                if (sameEmailUser != null)
                {
                    return ServiceStatusOutput.error("Email has been taken.");
                }

                User guest = null;

                if (guestUserId != null)
                {
                    guest = db.Users.Find(guestUserId);
                }

                if (guest == null)
                {
                    guest = new User();
                    db.Users.Add(guest);
                }

                guest.Role = User.UserRole.Learner;
                guest.Username = username;
                guest.Password = BCrypt.Net.BCrypt.HashPassword(password);
                guest.Email = email;
                guest.RegisteredAt = DateTime.Now;

                db.SaveChanges();

                IssueAuthCookie(guest);
                
                return ServiceStatusOutput.success("Register Successful");
            }
        }
    }
}