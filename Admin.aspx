<%@ Page Title="DriveLingo | System Administration" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="DriveLingo.Admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <asp:Panel ID="pnlNotification" runat="server" Visible="false" CssClass="glass-card" style="margin-bottom: 1.5rem; border-left: 4px solid var(--success); padding: 1rem;">
    <asp:Literal ID="litNotificationText" runat="server" />
  </asp:Panel>

  <!-- PANEL 1: DASHBOARD -->
  <asp:Panel ID="pnlDashboard" runat="server">
    <div class="glass-card" style="margin-bottom: 2rem; display: flex; align-items: center; justify-content: space-between; flex-wrap: wrap; gap: 1rem;">
      <div>
        <h1 style="font-family: var(--font-heading); margin-bottom: 0.25rem;">👑 System Administration & Dashboard</h1>
        <p style="color: var(--text-secondary); margin: 0;">Monitor global databases, manage privileges, audit materials, and run system maintenance.</p>
      </div>
      <asp:Button ID="btnResetState" runat="server" Text="🔄 Reset App State & Demo Data" OnClick="btnResetState_Click" CssClass="btn btn-secondary btn-sm" OnClientClick="return confirm('Reset all users, quizzes, and attempts to initial mock state?');" />
    </div>

    <!-- Metrics Grid -->
    <div class="grid-3-col" style="margin-bottom: 2rem;">
      <div class="glass-card" style="display: flex; align-items: center; gap: 1.5rem;">
        <span style="font-size: 2.5rem;">👥</span>
        <div>
          <h3 style="font-size: 0.9rem; color: var(--text-secondary); margin: 0;">Registered Accounts</h3>
          <span style="font-size: 1.8rem; font-weight: 800; color: var(--primary);"><asp:Literal ID="litTotalUsers" runat="server" Text="0" /></span>
        </div>
      </div>

      <div class="glass-card" style="display: flex; align-items: center; gap: 1.5rem;">
        <span style="font-size: 2.5rem;">❓</span>
        <div>
          <h3 style="font-size: 0.9rem; color: var(--text-secondary); margin: 0;">Total Questions</h3>
          <span style="font-size: 1.8rem; font-weight: 800; color: var(--secondary);"><asp:Literal ID="litTotalQuestions" runat="server" Text="0" /></span>
        </div>
      </div>

      <div class="glass-card" style="display: flex; align-items: center; gap: 1.5rem;">
        <span style="font-size: 2.5rem;">📝</span>
        <div>
          <h3 style="font-size: 0.9rem; color: var(--text-secondary); margin: 0;">Completed Exams</h3>
          <span style="font-size: 1.8rem; font-weight: 800; color: var(--warning);"><asp:Literal ID="litTotalAttempts" runat="server" Text="0" /></span>
        </div>
      </div>
    </div>
  </asp:Panel>

  <!-- PANEL 2: CRUD USERS -->
  <asp:Panel ID="pnlUsers" runat="server" Visible="false">
    <div class="glass-card" style="margin-bottom: 2rem;">
      <h1 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">👥 CRUD User Accounts Manager</h1>
      <p style="color: var(--text-secondary); margin: 0;">Create new accounts, edit existing user profile details and role permissions, or delete accounts.</p>
    </div>

    <div class="grid-2-col">
      <!-- Create User Account Form -->
      <div class="glass-card">
        <h2 style="font-family: var(--font-heading); margin-bottom: 1.5rem;">➕ Create New User Account</h2>
        <div style="display: flex; flex-direction: column; gap: 1.25rem;">
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Full Name</label>
            <asp:TextBox ID="txtNewUserName" runat="server" CssClass="form-control" placeholder="Ahmad Zaki" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>

          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Email Address</label>
            <asp:TextBox ID="txtNewUserEmail" runat="server" CssClass="form-control" placeholder="zaki@drivelingo.com" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>

          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Password</label>
            <asp:TextBox ID="txtNewUserPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="••••••••" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>

          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Role Permission</label>
            <asp:DropDownList ID="ddlNewUserRole" runat="server" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;">
              <asp:ListItem Value="learner">🚘 Candidate / Learner</asp:ListItem>
              <asp:ListItem Value="educator">👨‍✈️ Driving Instructor / Educator</asp:ListItem>
              <asp:ListItem Value="admin">👑 System Administrator</asp:ListItem>
            </asp:DropDownList>
          </div>

          <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 1rem;">
            <div>
              <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.85rem;">Initial Points</label>
              <asp:TextBox ID="txtNewUserPoints" runat="server" Text="100" CssClass="form-control" style="width: 100%; padding: 0.6rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
            </div>
            <div>
              <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.85rem;">Initial Level</label>
              <asp:TextBox ID="txtNewUserLevel" runat="server" Text="1" CssClass="form-control" style="width: 100%; padding: 0.6rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
            </div>
          </div>

          <asp:Button ID="btnAddUserSubmit" runat="server" Text="➕ Create User Account" OnClick="btnAddUserSubmit_Click" CssClass="btn btn-primary" style="padding: 0.85rem; font-weight: 700; margin-top: 0.5rem;" />
        </div>
      </div>

      <!-- User Directory Grid with Edit / Update / Delete -->
      <div class="glass-card">
        <h2 style="font-family: var(--font-heading); margin-bottom: 1.5rem;">👥 System User Directory</h2>
        <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="false" CssClass="data-table" DataKeyNames="Id" 
          OnRowEditing="gvUsers_RowEditing" OnRowUpdating="gvUsers_RowUpdating" OnRowCancelingEdit="gvUsers_RowCancelingEdit" OnRowDeleting="gvUsers_RowDeleting">
          <Columns>
            <asp:BoundField DataField="Id" HeaderText="User ID" ReadOnly="true" ItemStyle-Width="80px" />
            <asp:BoundField DataField="Name" HeaderText="Full Name" />
            <asp:BoundField DataField="Email" HeaderText="Email Address" />
            
            <asp:TemplateField HeaderText="Role" ItemStyle-Width="110px">
              <ItemTemplate>
                <span class='<%# Eval("Role").ToString() == "admin" ? "badge badge-danger" : Eval("Role").ToString() == "educator" ? "badge badge-warning" : "badge badge-success" %>'>
                  <%# Eval("Role").ToString().ToUpper() %>
                </span>
              </ItemTemplate>
              <EditItemTemplate>
                <asp:DropDownList ID="ddlEditRole" runat="server" SelectedValue='<%# Bind("Role") %>' CssClass="form-control" style="padding: 0.3rem; font-size: 0.85rem; background: rgba(15, 23, 42, 0.9); color: white;">
                  <asp:ListItem Value="learner">LEARNER</asp:ListItem>
                  <asp:ListItem Value="educator">EDUCATOR</asp:ListItem>
                  <asp:ListItem Value="admin">ADMIN</asp:ListItem>
                </asp:DropDownList>
              </EditItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="Points" HeaderText="Points" ItemStyle-Width="70px" />
            <asp:BoundField DataField="Level" HeaderText="Lvl" ItemStyle-Width="50px" />

            <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" EditText="✏️ Edit" UpdateText="💾 Save" CancelText="❌ Cancel" DeleteText="🗑️ Delete" 
              ItemStyle-Width="140px" ItemStyle-HorizontalAlign="Right" ButtonType="Button" ControlStyle-CssClass="btn btn-secondary btn-sm" />
          </Columns>
        </asp:GridView>
      </div>
    </div>
  </asp:Panel>

  <!-- PANEL 3: CRUD LEARNING MATERIAL -->
  <asp:Panel ID="pnlMaterials" runat="server" Visible="false">
    <div class="glass-card" style="margin-bottom: 2rem;">
      <h1 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">📚 CRUD Learning Materials</h1>
      <p style="color: var(--text-secondary); margin: 0;">Author new JPJ study guides, update handbook categories, and manage PDF manual downloads.</p>
    </div>

    <div class="grid-2-col">
      <!-- Create Material Form -->
      <div class="glass-card">
        <h2 style="font-family: var(--font-heading); margin-bottom: 1rem;">➕ Add Study Guide Material</h2>
        <div style="display: flex; flex-direction: column; gap: 1rem;">
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Material Title</label>
            <asp:TextBox ID="txtMatTitle" runat="server" CssClass="form-control" placeholder="Malaysian Speed Limits" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Category</label>
            <asp:DropDownList ID="ddlMatCategory" runat="server" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;">
              <asp:ListItem Value="Rules & Safety">Rules & Safety</asp:ListItem>
              <asp:ListItem Value="Road Signs">Road Signs</asp:ListItem>
              <asp:ListItem Value="Vehicle Mechanics & Checks">Vehicle Mechanics & Checks</asp:ListItem>
            </asp:DropDownList>
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">🚦 Attach Road Sign Image (File Upload or Image Link)</label>
            <asp:FileUpload ID="fileMaterialImage" runat="server" CssClass="form-control" style="width: 100%; padding: 0.5rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white; margin-bottom: 0.5rem;" />
            <asp:TextBox ID="txtMaterialImageUrl" runat="server" CssClass="form-control" placeholder="Or select/paste road sign image path (e.g. uploads/speed_limit_110.svg)..." style="width: 100%; padding: 0.6rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">PDF Manual URL (optional)</label>
            <asp:TextBox ID="txtMatPdf" runat="server" CssClass="form-control" placeholder="https://www.jpj.gov.my/..." style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Guide Content</label>
            <asp:TextBox ID="txtMatContent" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" placeholder="Full guide text..." style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <asp:Button ID="btnAddMaterial" runat="server" Text="➕ Create Material" OnClick="btnAddMaterial_Click" CssClass="btn btn-primary" />
        </div>
      </div>

      <!-- Materials List Grid -->
      <div class="glass-card">
        <h2 style="font-family: var(--font-heading); margin-bottom: 1rem;">🗃️ Active Study Guides</h2>
        <asp:GridView ID="gvMaterials" runat="server" AutoGenerateColumns="false" CssClass="data-table" DataKeyNames="Id" OnRowDeleting="gvMaterials_RowDeleting">
          <Columns>
            <asp:BoundField DataField="Category" HeaderText="Category" ItemStyle-Width="120px" />
            <asp:BoundField DataField="Title" HeaderText="Title" />
            <asp:CommandField ShowDeleteButton="true" DeleteText="🗑️ Delete" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Center" ButtonType="Button" ControlStyle-CssClass="btn btn-secondary btn-sm" />
          </Columns>
        </asp:GridView>
      </div>
    </div>
  </asp:Panel>

  <!-- PANEL 4: SIMULATION CONTROLS -->
  <asp:Panel ID="pnlSimulation" runat="server" Visible="false">
    <div class="glass-card" style="margin-bottom: 2rem;">
      <h1 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">🏎️ Simulation Parameters & Rules</h1>
      <p style="color: var(--text-secondary); margin: 0;">Configure strict JPJ KPP01 examination criteria, passing threshold percentage, and timer duration.</p>
    </div>

    <div class="glass-card" style="max-width: 600px;">
      <h2 style="font-family: var(--font-heading); margin-bottom: 1.5rem;">Exam Simulation Settings</h2>
      <div style="display: flex; flex-direction: column; gap: 1.25rem;">
        <div>
          <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Passing Percentage Threshold (%)</label>
          <asp:TextBox ID="txtSimPassScore" runat="server" Text="84" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
        </div>
        <div>
          <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Simulation Time Limit (Minutes)</label>
          <asp:TextBox ID="txtSimTimeLimit" runat="server" Text="45" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
        </div>
        <div>
          <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Total Exam Questions</label>
          <asp:TextBox ID="txtSimTotalQuestions" runat="server" Text="50" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
        </div>
        <asp:Button ID="btnSaveSimSettings" runat="server" Text="💾 Save Simulation Rules" OnClick="btnSaveSimSettings_Click" CssClass="btn btn-primary" style="padding: 0.85rem; font-weight: 700;" />
      </div>
    </div>
  </asp:Panel>

  <!-- PANEL 5: CRUD STORE -->
  <asp:Panel ID="pnlStore" runat="server" Visible="false">
    <div class="glass-card" style="margin-bottom: 2rem;">
      <h1 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">🛒 CRUD Store Cosmetics & Pricing</h1>
      <p style="color: var(--text-secondary); margin: 0;">Add new avatar frames, badges, or visual themes and manage point prices.</p>
    </div>

    <div class="grid-2-col">
      <div class="glass-card">
        <h2 style="font-family: var(--font-heading); margin-bottom: 1rem;">➕ Create Store Item</h2>
        <div style="display: flex; flex-direction: column; gap: 1rem;">
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Item Title</label>
            <asp:TextBox ID="txtStoreTitle" runat="server" CssClass="form-control" placeholder="Border: Cyberpunk" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Icon Emoji</label>
            <asp:TextBox ID="txtStoreIcon" runat="server" Text="✨" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Price (Points)</label>
            <asp:TextBox ID="txtStorePrice" runat="server" Text="200" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Description</label>
            <asp:TextBox ID="txtStoreDesc" runat="server" CssClass="form-control" placeholder="Glowing profile border" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <asp:Button ID="btnAddStoreItem" runat="server" Text="➕ Create Store Item" OnClick="btnAddStoreItem_Click" CssClass="btn btn-primary" />
        </div>
      </div>

      <div class="glass-card">
        <h2 style="font-family: var(--font-heading); margin-bottom: 1rem;">🗃️ Active Store Catalog</h2>
        <asp:GridView ID="gvStore" runat="server" AutoGenerateColumns="false" CssClass="data-table" DataKeyNames="Id" OnRowDeleting="gvStore_RowDeleting">
          <Columns>
            <asp:BoundField DataField="Icon" HeaderText="Icon" ItemStyle-Width="50px" />
            <asp:BoundField DataField="Title" HeaderText="Title" />
            <asp:BoundField DataField="Price" HeaderText="Price (Pts)" ItemStyle-Width="100px" />
            <asp:CommandField ShowDeleteButton="true" DeleteText="🗑️ Delete" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Center" ButtonType="Button" ControlStyle-CssClass="btn btn-secondary btn-sm" />
          </Columns>
        </asp:GridView>
      </div>
    </div>
  </asp:Panel>

  <!-- PANEL 6: CRUD ACHIEVEMENTS -->
  <asp:Panel ID="pnlAchievements" runat="server" Visible="false">
    <div class="glass-card" style="margin-bottom: 2rem;">
      <h1 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">🏆 CRUD System Achievements</h1>
      <p style="color: var(--text-secondary); margin: 0;">Define achievement milestones and reward XP bonuses for candidate milestones.</p>
    </div>

    <div class="grid-2-col">
      <div class="glass-card">
        <h2 style="font-family: var(--font-heading); margin-bottom: 1rem;">➕ Create Achievement</h2>
        <div style="display: flex; flex-direction: column; gap: 1rem;">
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Achievement Title</label>
            <asp:TextBox ID="txtAchTitle" runat="server" CssClass="form-control" placeholder="Theory Expert" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Icon Emoji</label>
            <asp:TextBox ID="txtAchIcon" runat="server" Text="🏆" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">XP Bonus</label>
            <asp:TextBox ID="txtAchXp" runat="server" Text="100" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Description</label>
            <asp:TextBox ID="txtAchDesc" runat="server" CssClass="form-control" placeholder="Complete 5 JPJ practice tests" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <asp:Button ID="btnAddAch" runat="server" Text="➕ Create Achievement" OnClick="btnAddAch_Click" CssClass="btn btn-primary" />
        </div>
      </div>

      <div class="glass-card">
        <h2 style="font-family: var(--font-heading); margin-bottom: 1rem;">🗃️ Active Achievements</h2>
        <asp:GridView ID="gvAchievements" runat="server" AutoGenerateColumns="false" CssClass="data-table" DataKeyNames="Id" OnRowDeleting="gvAchievements_RowDeleting">
          <Columns>
            <asp:BoundField DataField="Icon" HeaderText="Icon" ItemStyle-Width="50px" />
            <asp:BoundField DataField="Title" HeaderText="Title" />
            <asp:BoundField DataField="XpBonus" HeaderText="XP Bonus" ItemStyle-Width="90px" />
            <asp:CommandField ShowDeleteButton="true" DeleteText="🗑️ Delete" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Center" ButtonType="Button" ControlStyle-CssClass="btn btn-secondary btn-sm" />
          </Columns>
        </asp:GridView>
      </div>
    </div>
  </asp:Panel>
</asp:Content>
