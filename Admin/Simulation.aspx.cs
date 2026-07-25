using DriveLingo.Data;
using DriveLingo.Models;
using DriveLingo.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DriveLingo.Admin
{
    public partial class Simulation : AuthPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth(Database.Models.User.UserRole.Admin);

            if (!IsPostBack)
            {
                BindSimulationQuestions();

            }
        }

        // --- Simulation Question Bank CRUD Handlers ---
        private void BindSimulationQuestions()
        {
            var repo = AppStateRepository.GetCurrent();
            string selectedSec = ddlFilterSimSection.SelectedValue;

            if (selectedSec == "ALL")
            {
                gvSimQuestions.DataSource = repo.SimulationQuestions;
            }
            else
            {
                gvSimQuestions.DataSource = repo.SimulationQuestions.Where(q => q.Section == selectedSec).ToList();
            }
            gvSimQuestions.DataBind();
        }

        protected void ddlFilterSimSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSimulationQuestions();
        }

        protected void btnAddSimQuestion_Click(object sender, EventArgs e)
        {
            string qText = txtSimQuestionText.Text.Trim();
            string opt1 = txtSimOpt1.Text.Trim();
            string opt2 = txtSimOpt2.Text.Trim();
            string opt3 = txtSimOpt3.Text.Trim();
            string opt4 = txtSimOpt4.Text.Trim();

            if (string.IsNullOrEmpty(qText) || string.IsNullOrEmpty(opt1) || string.IsNullOrEmpty(opt2))
            {
                ShowNotification("Please provide question text and at least 2 options.");
                return;
            }

            var repo = AppStateRepository.GetCurrent();
            string editingId = hfEditingSimQuestionId.Value;

            var options = new List<string> { opt1, opt2 };
            if (!string.IsNullOrEmpty(opt3)) options.Add(opt3);
            if (!string.IsNullOrEmpty(opt4)) options.Add(opt4);

            int correctIdx = Convert.ToInt32(ddlSimCorrect.SelectedValue);
            if (correctIdx >= options.Count) correctIdx = 0;

            if (!string.IsNullOrEmpty(editingId))
            {
                // Update Existing Simulation Question
                var existingQ = repo.SimulationQuestions.FirstOrDefault(q => q.Id == editingId);
                if (existingQ != null)
                {
                    existingQ.Section = ddlSimSection.SelectedValue;
                    existingQ.Text = qText;
                    existingQ.Options = options;
                    existingQ.CorrectIndex = correctIdx;
                    existingQ.ImageUrl = txtSimImageUrl.Text.Trim();
                    existingQ.Explanation = txtSimExplanation.Text.Trim();

                    ShowNotification("Simulation Question updated successfully!");
                }
            }
            else
            {
                // Create New Simulation Question
                var newQ = new Question
                {
                    Id = "sim_q_" + Guid.NewGuid().ToString("N").Substring(0, 8),
                    Section = ddlSimSection.SelectedValue,
                    Text = qText,
                    Options = options,
                    CorrectIndex = correctIdx,
                    ImageUrl = txtSimImageUrl.Text.Trim(),
                    Explanation = txtSimExplanation.Text.Trim()
                };
                repo.SimulationQuestions.Add(newQ);
                ShowNotification("New Simulation Question added to bank!");
            }

            ResetSimForm();
            BindSimulationQuestions();
        }

        protected void btnCancelSimEdit_Click(object sender, EventArgs e)
        {
            ResetSimForm();
            ShowNotification("Edit cancelled.");
        }

        protected void gvSimQuestions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string qId = e.CommandArgument.ToString();
            var repo = AppStateRepository.GetCurrent();

            if (e.CommandName == "EditSimQuestion")
            {
                var q = repo.SimulationQuestions.FirstOrDefault(x => x.Id == qId);
                if (q != null)
                {
                    hfEditingSimQuestionId.Value = q.Id;
                    litSimFormTitle.Text = "✏️ Edit Simulation Question";
                    btnAddSimQuestion.Text = "💾 Save Changes";
                    btnCancelSimEdit.Visible = true;

                    if (ddlSimSection.Items.FindByValue(q.Section) != null)
                        ddlSimSection.SelectedValue = q.Section;

                    txtSimQuestionText.Text = q.Text;
                    txtSimOpt1.Text = q.Options.Count > 0 ? q.Options[0] : "";
                    txtSimOpt2.Text = q.Options.Count > 1 ? q.Options[1] : "";
                    txtSimOpt3.Text = q.Options.Count > 2 ? q.Options[2] : "";
                    txtSimOpt4.Text = q.Options.Count > 3 ? q.Options[3] : "";

                    if (ddlSimCorrect.Items.FindByValue(q.CorrectIndex.ToString()) != null)
                        ddlSimCorrect.SelectedValue = q.CorrectIndex.ToString();

                    txtSimImageUrl.Text = q.ImageUrl;
                    txtSimExplanation.Text = q.Explanation;

                    ShowNotification("Loaded simulation question into editor.");
                }
            }
            else if (e.CommandName == "DeleteSimQuestion")
            {
                var q = repo.SimulationQuestions.FirstOrDefault(x => x.Id == qId);
                if (q != null)
                {
                    repo.SimulationQuestions.Remove(q);
                    ResetSimForm();
                    BindSimulationQuestions();
                    ShowNotification("Simulation question deleted from question bank.");
                }
            }
        }

        protected void gvSimQuestions_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Handled in gvSimQuestions_RowCommand
        }

        private void ResetSimForm()
        {
            hfEditingSimQuestionId.Value = "";
            litSimFormTitle.Text = "➕ Add Simulation Question";
            btnAddSimQuestion.Text = "➕ Create Simulation Question";
            btnCancelSimEdit.Visible = false;

            txtSimQuestionText.Text = "";
            txtSimOpt1.Text = "";
            txtSimOpt2.Text = "";
            txtSimOpt3.Text = "";
            txtSimOpt4.Text = "";
            txtSimImageUrl.Text = "";
            txtSimExplanation.Text = "";
            ddlSimSection.SelectedIndex = 0;
            ddlSimCorrect.SelectedIndex = 0;
        }
        private void ShowNotification(string message)
        {
            pnlNotification.Visible = true;
            litNotificationText.Text = "✅ " + message;
        }
    }
}