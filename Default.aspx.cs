using System;
using System.Linq;
using System.Web.UI;
using DriveLingo.Data;
using DriveLingo.Models;

namespace DriveLingo
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Ensure state repository is initialized
                AppStateRepository.GetCurrent();
            }
        }

        protected void btnContinueGuest_Click(object sender, EventArgs e)
        {
            var guestUser = new User
            {
                Id = "usr_guest",
                Email = "guest@drivelingo.com",
                Name = "Guest Candidate",
                Role = "guest",
                Avatar = "🚗",
                Points = 0,
                Level = 1,
                XP = 0,
                JoinedDate = DateTime.Now.ToString("yyyy-MM-dd")
            };

            Session["CurrentUser"] = guestUser;
            Session["IsGuestMode"] = true;
            Response.Redirect("~/Dashboard");
        }

        protected void btnQuickLearner_Click(object sender, EventArgs e)
        {
            var repo = AppStateRepository.GetCurrent();
            var user = repo.Users.FirstOrDefault(u => u.Role == "learner");
            if (user != null)
            {
                Session["CurrentUser"] = user;
                Session["IsGuestMode"] = false;
                Response.Redirect("~/Dashboard");
            }
        }

        protected void btnQuickEducator_Click(object sender, EventArgs e)
        {
            var repo = AppStateRepository.GetCurrent();
            var user = repo.Users.FirstOrDefault(u => u.Role == "educator" || u.Role == "instructor");
            if (user != null)
            {
                Session["CurrentUser"] = user;
                Session["IsGuestMode"] = false;
                Response.Redirect("~/Instructor");
            }
        }

        protected void btnQuickAdmin_Click(object sender, EventArgs e)
        {
            var repo = AppStateRepository.GetCurrent();
            var user = repo.Users.FirstOrDefault(u => u.Role == "admin");
            if (user != null)
            {
                Session["CurrentUser"] = user;
                Session["IsGuestMode"] = false;
                Response.Redirect("~/Admin");
            }
        }
    }
}
