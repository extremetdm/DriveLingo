using DriveLingo.Services;
using System;
using System.Linq;
using System.Web.UI;

namespace DriveLingo
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnContinueGuest_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Dashboard");
        }

        protected void btnQuickLearner_Click(object sender, EventArgs e)
        {
            var user = AuthService.Login("learner@drivelingo.com", "learner", true);
            Response.Redirect("~/Dashboard");
        }

        protected void btnQuickEducator_Click(object sender, EventArgs e)
        {
            var user = AuthService.Login("instructor@drivelingo.com", "instructor", true);
            Response.Redirect("~/Instructor");
        }

        protected void btnQuickAdmin_Click(object sender, EventArgs e)
        {
            var user = AuthService.Login("admin@drivelingo.com", "admin", true);
            Response.Redirect("~/Admin");
        }
    }
}
