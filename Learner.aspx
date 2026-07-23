<%@ Page Title="DriveLingo | Learner Portal" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Learner.aspx.cs" Inherits="DriveLingo.Learner" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <!-- Notification Banner -->
  <asp:Panel ID="pnlNotification" runat="server" Visible="false" CssClass="glass-card" style="margin-bottom: 1.5rem; border-left: 4px solid var(--success); padding: 1rem;">
    <asp:Literal ID="litNotificationText" runat="server" />
  </asp:Panel>

  <!-- TAB 1: DASHBOARD -->
  <asp:Panel ID="pnlDashboard" runat="server">
    <div class="grid-3-col" style="margin-bottom: 2rem;">
      <div class="glass-card" style="display: flex; align-items: center; gap: 1.5rem;">
        <span style="font-size: 2.5rem;">🎯</span>
        <div>
          <h3 style="font-size: 0.9rem; color: var(--text-secondary); margin: 0;">Exam Readiness</h3>
          <span style="font-size: 1.8rem; font-weight: 800; color: var(--success);"><asp:Literal ID="litPassRate" runat="server" Text="100%" /></span>
        </div>
      </div>

      <div class="glass-card" style="display: flex; align-items: center; gap: 1.5rem;">
        <span style="font-size: 2.5rem;">⭐</span>
        <div>
          <h3 style="font-size: 0.9rem; color: var(--text-secondary); margin: 0;">Current Level</h3>
          <span style="font-size: 1.8rem; font-weight: 800; color: var(--primary);">Level <asp:Literal ID="litLevel" runat="server" Text="2" /></span>
        </div>
      </div>

      <div class="glass-card" style="display: flex; align-items: center; gap: 1.5rem;">
        <span style="font-size: 2.5rem;">🪙</span>
        <div>
          <h3 style="font-size: 0.9rem; color: var(--text-secondary); margin: 0;">Reward Points</h3>
          <span style="font-size: 1.8rem; font-weight: 800; color: var(--warning);"><asp:Literal ID="litPoints" runat="server" Text="350" /> Pts</span>
        </div>
      </div>
    </div>

    <!-- Recent Exam Attempts -->
    <div class="glass-card">
      <h2 style="font-family: var(--font-heading); margin-bottom: 1.5rem;">📝 Recent Quiz Attempts</h2>
      <asp:GridView ID="gvAttempts" runat="server" AutoGenerateColumns="false" CssClass="data-table" EmptyDataText="No exam attempts recorded yet. Launch the simulator to take your first test!">
        <Columns>
          <asp:BoundField DataField="QuizTitle" HeaderText="Practice Test" />
          <asp:BoundField DataField="Score" HeaderText="Score" ItemStyle-Width="80px" />
          <asp:BoundField DataField="Percentage" HeaderText="Percentage" DataFormatString="{0}%" ItemStyle-Width="100px" />
          <asp:TemplateField HeaderText="Result" ItemStyle-Width="100px">
            <ItemTemplate>
              <span class='<%# Convert.ToBoolean(Eval("Passed")) ? "badge badge-success" : "badge badge-danger" %>'>
                <%# Convert.ToBoolean(Eval("Passed")) ? "PASS 🟢" : "FAIL 🔴" %>
              </span>
            </ItemTemplate>
          </asp:TemplateField>
          <asp:BoundField DataField="CompletedAt" HeaderText="Date" ItemStyle-Width="140px" />
        </Columns>
      </asp:GridView>
    </div>
  </asp:Panel>

  <!-- TAB 2: EXAM SIMULATOR -->
  <asp:Panel ID="pnlExam" runat="server" Visible="false">
    <!-- Quiz Selector Panel -->
    <asp:Panel ID="pnlQuizList" runat="server">
      <div class="glass-card" style="margin-bottom: 2rem;">
        <h2 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">📋 Available JPJ Practice Quizzes</h2>
        <p style="color: var(--text-secondary); margin: 0;">Select a topic to launch an interactive 100% simulated test module.</p>
      </div>

      <div class="grid-2-col">
        <asp:Repeater ID="rptQuizzes" runat="server" OnItemCommand="rptQuizzes_ItemCommand">
          <ItemTemplate>
            <div class="glass-card" style="display: flex; flex-direction: column; justify-content: space-between;">
              <div>
                <span class="badge" style="background: rgba(99, 102, 241, 0.2); color: var(--primary); margin-bottom: 0.75rem; display: inline-block;">
                  <%# Eval("Category") %>
                </span>
                <h3 style="font-family: var(--font-heading); margin-bottom: 0.5rem;"><%# Eval("Title") %></h3>
                <p style="color: var(--text-secondary); font-size: 0.9rem;">
                  Reward: <strong><%# Eval("RewardPoints") %> Pts</strong> | Questions: <strong><%# DataBinder.Eval(Container.DataItem, "Questions.Count") %></strong>
                </p>
              </div>
              <asp:Button ID="btnStartQuiz" runat="server" Text="🚀 Start Practice Exam" CommandName="StartQuiz" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-primary" style="margin-top: 1rem; width: 100%;" />
            </div>
          </ItemTemplate>
        </asp:Repeater>
      </div>
    </asp:Panel>

    <!-- Active Exam Interface -->
    <asp:Panel ID="pnlActiveExam" runat="server" Visible="false" CssClass="glass-card">
      <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 2rem; padding-bottom: 1rem; border-bottom: 1px solid rgba(255,255,255,0.1);">
        <h2 style="font-family: var(--font-heading); margin: 0;">
          <asp:Literal ID="litExamTitle" runat="server" />
        </h2>
        <span class="badge" style="background: rgba(16, 185, 129, 0.2); color: var(--success); font-weight: 700; font-size: 1rem;">
          ⏱️ Timed Mode Active
        </span>
      </div>

      <asp:Repeater ID="rptQuestions" runat="server" OnItemDataBound="rptQuestions_ItemDataBound">
        <ItemTemplate>
          <div style="margin-bottom: 2rem; padding: 1.5rem; background: rgba(15, 23, 42, 0.4); border-radius: var(--radius-md); border: 1px solid rgba(255,255,255,0.05);">
            <h4 style="font-size: 1.1rem; margin-bottom: 1rem; line-height: 1.5;">
              Question <%# Container.ItemIndex + 1 %>: <%# Eval("Text") %>
            </h4>

            <%# Eval("ImageUrl") != null && !string.IsNullOrEmpty(Eval("ImageUrl").ToString()) ? "<div style='text-align: center; margin: 1rem 0; padding: 1rem; background: rgba(0,0,0,0.3); border-radius: var(--radius-md); border: 1px solid rgba(255,255,255,0.08);'><img src='" + Eval("ImageUrl") + "' alt='Road Sign Visual' style='max-height: 200px; max-width: 100%; border-radius: 8px; box-shadow: 0 4px 12px rgba(0,0,0,0.5);' /><span style='display: block; margin-top: 0.5rem; font-size: 0.8rem; color: var(--text-secondary); font-weight: 600;'>🚦 Road Sign Reference Visual</span></div>" : "" %>

            <asp:HiddenField ID="hfQuestionId" runat="server" Value='<%# Eval("Id") %>' />
            
            <asp:RadioButtonList ID="rblOptions" runat="server" CssClass="radio-options" style="width: 100%;">
            </asp:RadioButtonList>
          </div>
        </ItemTemplate>
      </asp:Repeater>

      <div style="display: flex; justify-content: flex-end; gap: 1rem;">
        <asp:Button ID="btnCancelExam" runat="server" Text="Cancel" OnClick="btnCancelExam_Click" CssClass="btn btn-secondary" />
        <asp:Button ID="btnSubmitExam" runat="server" Text="Submit & Grade Exam" OnClick="btnSubmitExam_Click" CssClass="btn btn-primary" style="padding: 0.75rem 2rem; font-weight: 700;" />
      </div>
    </asp:Panel>

    <!-- Exam Score Results Panel -->
    <asp:Panel ID="pnlExamResult" runat="server" Visible="false" CssClass="glass-card" style="text-align: center; padding: 3rem 1.5rem;">
      <span style="font-size: 4rem; display: block; margin-bottom: 1rem;">
        <asp:Literal ID="litResultIcon" runat="server" Text="🎉" />
      </span>
      <h2 style="font-size: 2.2rem; font-family: var(--font-heading); margin-bottom: 0.5rem;">
        <asp:Literal ID="litResultHeader" runat="server" Text="Test Passed!" />
      </h2>
      <p style="color: var(--text-secondary); font-size: 1.1rem; margin-bottom: 1.5rem;">
        Score: <strong style="color: var(--primary);"><asp:Literal ID="litResultScore" runat="server" /></strong> | 
        Percentage: <strong><asp:Literal ID="litResultPercentage" runat="server" />%</strong>
      </p>

      <asp:Panel ID="pnlResultBonus" runat="server" CssClass="badge" style="background: rgba(245, 158, 11, 0.2); color: var(--warning); font-size: 1.1rem; padding: 0.75rem 1.5rem; margin-bottom: 2rem; display: inline-block;">
        🏆 Awarded +<asp:Literal ID="litAwardedPoints" runat="server" /> Points & +<asp:Literal ID="litAwardedXP" runat="server" /> XP!
      </asp:Panel>

      <div style="display: flex; justify-content: center; gap: 1rem; margin-top: 1rem;">
        <asp:Button ID="btnBackToQuizzes" runat="server" Text="Back to Quiz Directory" OnClick="btnBackToQuizzes_Click" CssClass="btn btn-primary" />
      </div>
    </asp:Panel>
  </asp:Panel>

  <!-- TAB 3: STUDY MATERIALS -->
  <asp:Panel ID="pnlMaterials" runat="server" Visible="false">
    <div class="glass-card" style="margin-bottom: 2rem;">
      <h2 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">📚 Malaysian JPJ Theory Study Guides</h2>
      <p style="color: var(--text-secondary); margin: 0;">Official KPP01 handbook guides, traffic signs diagrams, and RPK/RSM checklists.</p>
    </div>

    <div class="grid-3-col">
      <asp:Repeater ID="rptMaterials" runat="server" OnItemCommand="rptMaterials_ItemCommand">
        <ItemTemplate>
          <div class="glass-card" style="display: flex; flex-direction: column; justify-content: space-between;">
            <div>
              <span class="badge" style="background: rgba(16, 185, 129, 0.2); color: var(--success); margin-bottom: 0.75rem; display: inline-block;">
                <%# Eval("Category") %>
              </span>
              <h3 style="font-family: var(--font-heading); margin-bottom: 0.5rem;"><%# Eval("Title") %></h3>

              <%# Eval("ImageUrl") != null && !string.IsNullOrEmpty(Eval("ImageUrl").ToString()) ? "<div style='text-align: center; margin: 0.75rem 0; padding: 0.75rem; background: rgba(0,0,0,0.3); border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.05);'><img src='" + Eval("ImageUrl") + "' alt='Road Sign Visual' style='max-height: 120px; max-width: 100%; border-radius: 6px;' /></div>" : "" %>

              <p style="color: var(--text-secondary); font-size: 0.85rem; margin-bottom: 1rem;">
                ⏱️ <%# Eval("ReadTime") %>
              </p>
              <p style="color: var(--text-secondary); font-size: 0.95rem; line-height: 1.5;">
                <%# Eval("Content").ToString().Length > 120 ? Eval("Content").ToString().Substring(0, 120) + "..." : Eval("Content") %>
              </p>
            </div>
            <div style="margin-top: 1.5rem; display: flex; gap: 0.5rem;">
              <asp:Button ID="btnReadMaterial" runat="server" Text="📖 Read Guide" CommandName="ReadMaterial" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-primary btn-sm" style="flex: 1;" />
              <%# !string.IsNullOrEmpty(Eval("PdfUrl").ToString()) ? "<a href='" + Eval("PdfUrl") + "' target='_blank' class='btn btn-secondary btn-sm'>📄 PDF</a>" : "" %>
            </div>
          </div>
        </ItemTemplate>
      </asp:Repeater>
    </div>
  </asp:Panel>

  <!-- TAB 4: STORE -->
  <asp:Panel ID="pnlStore" runat="server" Visible="false">
    <div class="glass-card" style="margin-bottom: 2rem;">
      <h2 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">🛒 Candidate Points Marketplace</h2>
      <p style="color: var(--text-secondary); margin: 0;">Redeem your hard-earned quiz points for profile borders, badges, and exclusive visual themes.</p>
    </div>

    <div class="grid-3-col">
      <asp:Repeater ID="rptStore" runat="server" OnItemCommand="rptStore_ItemCommand" OnItemDataBound="rptStore_ItemDataBound">
        <ItemTemplate>
          <div class="glass-card" style="text-align: center; display: flex; flex-direction: column; justify-content: space-between;">
            <div>
              <span style="font-size: 3.5rem; display: block; margin-bottom: 0.5rem;"><%# Eval("Icon") %></span>
              <h3 style="font-family: var(--font-heading); margin-bottom: 0.25rem;"><%# Eval("Name") %></h3>
              <span class="badge" style="background: rgba(245, 158, 11, 0.2); color: var(--warning); margin-bottom: 0.75rem; display: inline-block;">
                🪙 <%# Eval("Cost") %> Points
              </span>
              <p style="color: var(--text-secondary); font-size: 0.9rem;"><%# Eval("Description") %></p>
            </div>
            
            <asp:Panel ID="pnlStoreAction" runat="server" style="margin-top: 1.5rem;">
              <!-- If unowned -->
              <asp:Button ID="btnBuyItem" runat="server" Text="Redeem Item" CommandName="BuyItem" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-primary" style="width: 100%;" />
              <!-- If owned -->              
              <asp:Label ID="lblOwnedItem" runat="server" Text="Owned ✔" CssClass="badge badge-success" style="font-size: 1rem; padding: 0.6rem 1.2rem; display: block; width: 100%;" Visible="false" />
            </asp:Panel>
          </div>
        </ItemTemplate>
      </asp:Repeater>
    </div>
  </asp:Panel>

  <!-- TAB 5: FORUM -->
  <asp:Panel ID="pnlForum" runat="server" Visible="false">
    <div class="glass-card" style="margin-bottom: 2rem; display: flex; justify-content: space-between; align-items: center; flex-wrap: wrap; gap: 1rem;">
      <div>
        <h2 style="font-family: var(--font-heading); margin-bottom: 0.25rem;">💬 Candidate Community Q&A</h2>
        <p style="color: var(--text-secondary); margin: 0;">Ask questions about JPJ rules and get answers from fellow candidates and JPJ Educators.</p>
      </div>
      <asp:Button ID="btnToggleNewQuestion" runat="server" Text="➕ Ask a Question" OnClick="btnToggleNewQuestion_Click" CssClass="btn btn-primary" />
    </div>

    <!-- New Question Form -->
    <asp:Panel ID="pnlNewQuestionForm" runat="server" Visible="false" CssClass="glass-card" style="margin-bottom: 2rem;">
      <h3 style="font-family: var(--font-heading); margin-bottom: 1rem;">Post New Question to Community</h3>
      <div style="display: flex; flex-direction: column; gap: 1rem;">
        <asp:TextBox ID="txtForumTitle" runat="server" CssClass="form-control" placeholder="Question Title (e.g. Speed limit on highways)" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
        <asp:DropDownList ID="ddlForumCategory" runat="server" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;">
          <asp:ListItem Value="Rules & Safety">Rules & Safety</asp:ListItem>
          <asp:ListItem Value="Road Signs">Road Signs</asp:ListItem>
          <asp:ListItem Value="Vehicle Checks">Vehicle Checks</asp:ListItem>
        </asp:DropDownList>
        <asp:TextBox ID="txtForumContent" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" placeholder="Detailed description of your question..." style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
        <asp:Button ID="btnPostQuestion" runat="server" Text="Post Question" OnClick="btnPostQuestion_Click" CssClass="btn btn-primary" style="align-self: flex-end;" />
      </div>
    </asp:Panel>

    <!-- Forum Threads Repeater -->
    <div style="display: flex; flex-direction: column; gap: 1.5rem;">
      <asp:Repeater ID="rptForum" runat="server" OnItemCommand="rptForum_ItemCommand" OnItemDataBound="rptForum_ItemDataBound">
        <ItemTemplate>
          <div class="glass-card">
            <div style="display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 0.75rem;">
              <div>
                <span class="badge" style="background: rgba(99, 102, 241, 0.2); color: var(--primary); margin-bottom: 0.5rem; display: inline-block;"><%# Eval("Category") %></span>
                <h3 style="font-family: var(--font-heading); margin: 0;"><%# Eval("Title") %></h3>
              </div>
              <span style="font-size: 0.85rem; color: var(--text-secondary);"><%# Eval("DatePosted") %></span>
            </div>

            <p style="color: var(--text-secondary); line-height: 1.6; margin-bottom: 1.5rem;"><%# Eval("Content") %></p>

            <div style="display: flex; align-items: center; justify-content: space-between; background: rgba(15, 23, 42, 0.4); padding: 0.75rem 1rem; border-radius: var(--radius-sm);">
              <div style="display: flex; align-items: center; gap: 0.5rem;">
                <span><%# Eval("AuthorAvatar") %></span>
                <span style="font-weight: 600; font-size: 0.9rem;"><%# Eval("AuthorName") %></span>
                <span class="badge" style="font-size: 0.75rem;"><%# Eval("AuthorRole") %></span>
              </div>

              <asp:LinkButton ID="btnUpvote" runat="server" CommandName="Upvote" CommandArgument='<%# Eval("Id") %>' style="color: var(--warning); font-weight: 700; text-decoration: none;">
                👍 <%# Eval("Upvotes") %> Upvotes
              </asp:LinkButton>
            </div>

            <!-- Thread Replies -->
            <asp:Repeater ID="rptReplies" runat="server">
              <HeaderTemplate>
                <div style="margin-top: 1rem; padding-left: 1.5rem; border-left: 2px solid rgba(255,255,255,0.1);">
              </HeaderTemplate>
              <ItemTemplate>
                <div style="margin-bottom: 1rem; padding: 0.75rem; background: <%# Convert.ToBoolean(Eval("IsEducatorAnswer")) ? "rgba(16, 185, 129, 0.1)" : "rgba(15, 23, 42, 0.3)" %>; border-radius: var(--radius-sm); border: <%# Convert.ToBoolean(Eval("IsEducatorAnswer")) ? "1px solid var(--success)" : "none" %>;">
                  <div style="display: flex; justify-content: space-between; margin-bottom: 0.25rem;">
                    <span style="font-weight: 700; font-size: 0.85rem; color: <%# Convert.ToBoolean(Eval("IsEducatorAnswer")) ? "var(--success)" : "inherit" %>;">
                      <%# Eval("AuthorAvatar") %> <%# Eval("AuthorName") %> <%# Convert.ToBoolean(Eval("IsEducatorAnswer")) ? "✔ (Educator Verified Answer)" : "" %>
                    </span>
                    <span style="font-size: 0.75rem; color: var(--text-secondary);"><%# Eval("DatePosted") %></span>
                  </div>
                  <p style="margin: 0; font-size: 0.9rem; line-height: 1.4;"><%# Eval("Content") %></p>
                </div>
              </ItemTemplate>
              <FooterTemplate>
                </div>
              </FooterTemplate>
            </asp:Repeater>

            <!-- Learner Candidate Comment Reply Box -->
            <div style="margin-top: 1rem; display: flex; gap: 0.5rem;">
              <asp:TextBox ID="txtCandidateReply" runat="server" CssClass="form-control" placeholder="Write a comment under this thread..." style="flex: 1; padding: 0.5rem; font-size: 0.85rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
              <asp:Button ID="btnPostReply" runat="server" Text="Reply" CommandName="ReplyThread" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-secondary btn-sm" />
            </div>
          </div>
        </ItemTemplate>
      </asp:Repeater>
    </div>
  </asp:Panel>

  <!-- TAB 6: SIMULATION -->
  <asp:Panel ID="pnlSimulation" runat="server" Visible="false">
    <div class="glass-card" style="margin-bottom: 2rem; text-align: center; padding: 3rem 1.5rem;">
      <span style="font-size: 4rem; display: block; margin-bottom: 1rem;">🏎️</span>
      <h2 style="font-size: 2.2rem; font-family: var(--font-heading); margin-bottom: 0.5rem;">Official JPJ KPP01 Exam Simulator</h2>
      <p style="color: var(--text-secondary); max-width: 600px; margin: 0 auto 2rem auto; line-height: 1.6;">
        50 Questions | 45 Minutes Time Limit | Passing Score: 42/50 (84%). Full strict examination simulation mode.
      </p>
      <asp:Button ID="btnStartFullSim" runat="server" Text="🚀 Launch Full JPJ Exam Simulation" OnClick="btnStartFullSim_Click" CssClass="btn btn-primary" style="font-size: 1.1rem; padding: 0.85rem 2rem; font-weight: 700;" />
    </div>
  </asp:Panel>

  <!-- TAB 7: ACHIEVEMENTS -->
  <asp:Panel ID="pnlAchievements" runat="server" Visible="false">
    <div class="glass-card" style="margin-bottom: 2rem;">
      <h2 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">🏆 Badges & Achievements Tracker</h2>
      <p style="color: var(--text-secondary); margin: 0;">Unlock milestones by taking practice exams, scoring high marks, and studying JPJ materials.</p>
    </div>

    <div class="grid-3-col">
      <asp:Repeater ID="rptAchievements" runat="server" OnItemDataBound="rptAchievements_ItemDataBound">
        <ItemTemplate>
          <div class="glass-card" style="text-align: center;">
            <span style="font-size: 3.5rem; display: block; margin-bottom: 0.5rem;"><%# Eval("Icon") %></span>
            <h3 style="font-family: var(--font-heading); margin-bottom: 0.25rem;"><%# Eval("Title") %></h3>
            <p style="color: var(--text-secondary); font-size: 0.9rem; margin-bottom: 1rem;"><%# Eval("Description") %></p>
            <asp:Label ID="lblAchievementStatus" runat="server" CssClass="badge" />
          </div>
        </ItemTemplate>
      </asp:Repeater>
    </div>
  </asp:Panel>
</asp:Content>
