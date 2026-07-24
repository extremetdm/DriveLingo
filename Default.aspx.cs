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

        protected void btnQuickLearner_Click(object sender, EventArgs e)
        {
            var repo = AppStateRepository.GetCurrent();
            var user = repo.Users.FirstOrDefault(u => u.Role == "learner");
            if (user != null)
            {
                Session["CurrentUser"] = user;
                Response.Redirect("~/Dashboard");
            }
        }

        protected void btnQuickEducator_Click(object sender, EventArgs e)
        {
            var repo = AppStateRepository.GetCurrent();
            var user = repo.Users.FirstOrDefault(u => u.Role == "educator");
            if (user != null)
            {
                Session["CurrentUser"] = user;
                Response.Redirect("~/Educator.aspx");
            }
        }

        protected void btnQuickAdmin_Click(object sender, EventArgs e)
        {
            var repo = AppStateRepository.GetCurrent();
            var user = repo.Users.FirstOrDefault(u => u.Role == "admin");
            if (user != null)
            {
                Session["CurrentUser"] = user;
                Response.Redirect("~/Admin.aspx");
            }
        }
    }
}
