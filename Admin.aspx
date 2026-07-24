<%@ Page Title="DriveLingo | System Administration" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="DriveLingo.Administrator" %>

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
      <!-- Create / Edit User Account Form -->
      <div class="glass-card">
        <asp:HiddenField ID="hfEditingUserId" runat="server" Value="" />
        <h2 style="font-family: var(--font-heading); margin-bottom: 1.5rem;">
          <asp:Literal ID="litUserFormTitle" runat="server" Text="➕ Create New User Account" />
        </h2>
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
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Password (leave blank when editing to keep unchanged)</label>
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
              <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.85rem;">Points</label>
              <asp:TextBox ID="txtNewUserPoints" runat="server" Text="100" CssClass="form-control" style="width: 100%; padding: 0.6rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
            </div>
            <div>
              <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.85rem;">Level</label>
              <asp:TextBox ID="txtNewUserLevel" runat="server" Text="1" CssClass="form-control" style="width: 100%; padding: 0.6rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
            </div>
          </div>

          <div style="display: flex; gap: 0.5rem;">
            <asp:Button ID="btnAddUserSubmit" runat="server" Text="➕ Create User Account" OnClick="btnAddUserSubmit_Click" CssClass="btn btn-primary" style="flex: 1; padding: 0.85rem; font-weight: 700; margin-top: 0.5rem;" />
            <asp:Button ID="btnCancelUserEdit" runat="server" Text="❌ Cancel Edit" OnClick="btnCancelUserEdit_Click" Visible="false" CssClass="btn btn-secondary" style="padding: 0.85rem; margin-top: 0.5rem;" />
          </div>
        </div>
      </div>

      <!-- User Directory Grid with Edit / Delete -->
      <div class="glass-card">
        <h2 style="font-family: var(--font-heading); margin-bottom: 1.5rem;">👥 System User Directory</h2>
        <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="false" CssClass="data-table" DataKeyNames="Id" 
          OnRowCommand="gvUsers_RowCommand" OnRowDeleting="gvUsers_RowDeleting" EmptyDataText="No users found.">
          <Columns>
            <asp:BoundField DataField="Id" HeaderText="User ID" ReadOnly="true" ItemStyle-Width="80px" />
            <asp:BoundField DataField="Name" HeaderText="Full Name" />
            <asp:BoundField DataField="Email" HeaderText="Email Address" />
            
            <asp:TemplateField HeaderText="Role" ItemStyle-Width="120px">
              <ItemTemplate>
                <span class='<%# Eval("Role").ToString() == "admin" ? "badge badge-danger" : Eval("Role").ToString() == "educator" ? "badge badge-warning" : "badge badge-success" %>'>
                  <%# Eval("Role").ToString() == "admin" ? "👑 ADMIN" : Eval("Role").ToString() == "educator" ? "👨‍✈️ EDUCATOR" : "🚘 LEARNER" %>
                </span>
              </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="Points" HeaderText="Points" ItemStyle-Width="70px" />
            <asp:BoundField DataField="Level" HeaderText="Lvl" ItemStyle-Width="50px" />

            <asp:TemplateField HeaderText="Actions" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Right">
              <ItemTemplate>
                <asp:Button ID="btnEditUser" runat="server" Text="✏️ Edit" CommandName="EditUser" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-secondary btn-sm" />
                <asp:Button ID="btnDeleteUser" runat="server" Text="🗑️" CommandName="DeleteUser" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-secondary btn-sm" OnClientClick="return confirm('Delete this user account?');" />
              </ItemTemplate>
            </asp:TemplateField>
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
      <!-- Create / Edit Material Form -->
      <div class="glass-card">
        <asp:HiddenField ID="hfEditingMaterialId" runat="server" Value="" />
        <h2 style="font-family: var(--font-heading); margin-bottom: 1rem;">
          <asp:Literal ID="litMaterialFormTitle" runat="server" Text="➕ Add Study Guide Material" />
        </h2>
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
          <div style="display: flex; gap: 0.5rem;">
            <asp:Button ID="btnAddMaterial" runat="server" Text="➕ Create Material" OnClick="btnAddMaterial_Click" CssClass="btn btn-primary" style="flex: 1;" />
            <asp:Button ID="btnCancelMaterialEdit" runat="server" Text="❌ Cancel Edit" OnClick="btnCancelMaterialEdit_Click" Visible="false" CssClass="btn btn-secondary" />
          </div>
        </div>
      </div>

      <!-- Materials List Grid -->
      <div class="glass-card">
        <h2 style="font-family: var(--font-heading); margin-bottom: 1rem;">🗃️ Active Study Guides</h2>
        <asp:GridView ID="gvMaterials" runat="server" AutoGenerateColumns="false" CssClass="data-table" DataKeyNames="Id" OnRowCommand="gvMaterials_RowCommand" OnRowDeleting="gvMaterials_RowDeleting" EmptyDataText="No materials found.">
          <Columns>
            <asp:BoundField DataField="Category" HeaderText="Category" ItemStyle-Width="120px" />
            <asp:BoundField DataField="Title" HeaderText="Title" />
            <asp:TemplateField HeaderText="Actions" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Right">
              <ItemTemplate>
                <asp:Button ID="btnEditMat" runat="server" Text="✏️ Edit" CommandName="EditMaterial" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-secondary btn-sm" />
                <asp:Button ID="btnDeleteMat" runat="server" Text="🗑️" CommandName="DeleteMaterial" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-secondary btn-sm" OnClientClick="return confirm('Delete this study material?');" />
              </ItemTemplate>
            </asp:TemplateField>
          </Columns>
        </asp:GridView>
      </div>
    </div>
  </asp:Panel>

  <!-- PANEL 4: CRUD SIMULATION QUESTION BANK -->
  <asp:Panel ID="pnlSimulation" runat="server" Visible="false">
    <div class="glass-card" style="margin-bottom: 2rem;">
      <h1 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">🏎️ CRUD Simulation Question Bank</h1>
      <p style="color: var(--text-secondary); margin: 0;">Add, edit, or remove questions from the 180+ question pool across Color Blindness, Section A, Section B, and Section C.</p>
    </div>

    <div class="grid-2-col">
      <!-- Question Form Card -->
      <div class="glass-card">
        <asp:HiddenField ID="hfEditingSimQuestionId" runat="server" Value="" />
        <h2 style="font-family: var(--font-heading); margin-bottom: 1rem;">
          <asp:Literal ID="litSimFormTitle" runat="server" Text="➕ Add Simulation Question" />
        </h2>
        <div style="display: flex; flex-direction: column; gap: 1rem;">
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Target Test Section</label>
            <asp:DropDownList ID="ddlSimSection" runat="server" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;">
              <asp:ListItem Value="ColorBlindness">👁️ Color Blindness Test (8 Qs)</asp:ListItem>
              <asp:ListItem Value="SectionA">🛑 Section A - Road Signs (21 Qs)</asp:ListItem>
              <asp:ListItem Value="SectionB">🛣️ Section B - Rules of the Road (35 Qs)</asp:ListItem>
              <asp:ListItem Value="SectionC">⚠️ Section C - KEJARA & Safety (14 Qs)</asp:ListItem>
            </asp:DropDownList>
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Question Text</label>
            <asp:TextBox ID="txtSimQuestionText" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control" placeholder="Enter question description..." style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Option 1 (Index 0)</label>
            <asp:TextBox ID="txtSimOpt1" runat="server" CssClass="form-control" placeholder="Choice 1" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Option 2 (Index 1)</label>
            <asp:TextBox ID="txtSimOpt2" runat="server" CssClass="form-control" placeholder="Choice 2" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Option 3 (Index 2)</label>
            <asp:TextBox ID="txtSimOpt3" runat="server" CssClass="form-control" placeholder="Choice 3" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Option 4 (Index 3)</label>
            <asp:TextBox ID="txtSimOpt4" runat="server" CssClass="form-control" placeholder="Choice 4" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Correct Answer Index</label>
            <asp:DropDownList ID="ddlSimCorrect" runat="server" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;">
              <asp:ListItem Value="0">Option 1 (Index 0)</asp:ListItem>
              <asp:ListItem Value="1">Option 2 (Index 1)</asp:ListItem>
              <asp:ListItem Value="2">Option 3 (Index 2)</asp:ListItem>
              <asp:ListItem Value="3">Option 4 (Index 3)</asp:ListItem>
            </asp:DropDownList>
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Image Visual URL (optional)</label>
            <asp:TextBox ID="txtSimImageUrl" runat="server" CssClass="form-control" placeholder="uploads/ishihara_12.svg" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div>
            <label style="display: block; font-weight: 600; margin-bottom: 0.5rem; font-size: 0.9rem;">Explanation</label>
            <asp:TextBox ID="txtSimExplanation" runat="server" CssClass="form-control" placeholder="Official answer reasoning..." style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
          </div>
          <div style="display: flex; gap: 0.5rem;">
            <asp:Button ID="btnAddSimQuestion" runat="server" Text="➕ Create Simulation Question" OnClick="btnAddSimQuestion_Click" CssClass="btn btn-primary" style="flex: 1;" />
            <asp:Button ID="btnCancelSimEdit" runat="server" Text="❌ Cancel Edit" OnClick="btnCancelSimEdit_Click" Visible="false" CssClass="btn btn-secondary" />
          </div>
        </div>
      </div>

      <!-- Simulation Question Bank Grid Card -->
      <div class="glass-card">
        <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 1rem; flex-wrap: wrap; gap: 0.5rem;">
          <h2 style="font-family: var(--font-heading); margin: 0;">🗃️ Simulation Question Pool</h2>
          <asp:DropDownList ID="ddlFilterSimSection" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFilterSimSection_SelectedIndexChanged" CssClass="form-control" style="padding: 0.5rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white; font-size: 0.85rem;">
            <asp:ListItem Value="ALL">All Sections</asp:ListItem>
            <asp:ListItem Value="ColorBlindness">👁️ Color Blindness</asp:ListItem>
            <asp:ListItem Value="SectionA">🛑 Section A - Signs</asp:ListItem>
            <asp:ListItem Value="SectionB">🛣️ Section B - Rules</asp:ListItem>
            <asp:ListItem Value="SectionC">⚠️ Section C - KEJARA</asp:ListItem>
          </asp:DropDownList>
        </div>

        <asp:GridView ID="gvSimQuestions" runat="server" AutoGenerateColumns="false" CssClass="data-table" DataKeyNames="Id" OnRowCommand="gvSimQuestions_RowCommand" OnRowDeleting="gvSimQuestions_RowDeleting" EmptyDataText="No simulation questions found.">
          <Columns>
            <asp:BoundField DataField="Section" HeaderText="Section" ItemStyle-Width="120px" />
            <asp:TemplateField HeaderText="Question">
              <ItemTemplate>
                <div style="font-weight: 600;"><%# Eval("Text").ToString().Length > 60 ? Eval("Text").ToString().Substring(0, 60) + "..." : Eval("Text") %></div>
              </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Actions" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Right">
              <ItemTemplate>
                <asp:Button ID="btnEditSimQ" runat="server" Text="✏️ Edit" CommandName="EditSimQuestion" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-secondary btn-sm" />
                <asp:Button ID="btnDeleteSimQ" runat="server" Text="🗑️" CommandName="DeleteSimQuestion" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-secondary btn-sm" OnClientClick="return confirm('Delete this simulation question?');" />
              </ItemTemplate>
            </asp:TemplateField>
          </Columns>
        </asp:GridView>
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
        <asp:HiddenField ID="hfEditingStoreItemId" runat="server" Value="" />
        <h2 style="font-family: var(--font-heading); margin-bottom: 1rem;">
          <asp:Literal ID="litStoreFormTitle" runat="server" Text="➕ Create Store Item" />
        </h2>
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
          <div style="display: flex; gap: 0.5rem;">
            <asp:Button ID="btnAddStoreItem" runat="server" Text="➕ Create Store Item" OnClick="btnAddStoreItem_Click" CssClass="btn btn-primary" style="flex: 1;" />
            <asp:Button ID="btnCancelStoreEdit" runat="server" Text="❌ Cancel Edit" OnClick="btnCancelStoreEdit_Click" Visible="false" CssClass="btn btn-secondary" />
          </div>
        </div>
      </div>

      <div class="glass-card">
        <h2 style="font-family: var(--font-heading); margin-bottom: 1rem;">🗃️ Active Store Catalog</h2>
        <asp:GridView ID="gvStore" runat="server" AutoGenerateColumns="false" CssClass="data-table" DataKeyNames="Id" OnRowCommand="gvStore_RowCommand" OnRowDeleting="gvStore_RowDeleting" EmptyDataText="No store items found.">
          <Columns>
            <asp:BoundField DataField="Icon" HeaderText="Icon" ItemStyle-Width="50px" />
            <asp:BoundField DataField="Title" HeaderText="Title" />
            <asp:BoundField DataField="Price" HeaderText="Price (Pts)" ItemStyle-Width="100px" />
            <asp:TemplateField HeaderText="Actions" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Right">
              <ItemTemplate>
                <asp:Button ID="btnEditStore" runat="server" Text="✏️ Edit" CommandName="EditStoreItem" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-secondary btn-sm" />
                <asp:Button ID="btnDeleteStore" runat="server" Text="🗑️" CommandName="DeleteStoreItem" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-secondary btn-sm" OnClientClick="return confirm('Delete this store item?');" />
              </ItemTemplate>
            </asp:TemplateField>
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
        <asp:HiddenField ID="hfEditingAchId" runat="server" Value="" />
        <h2 style="font-family: var(--font-heading); margin-bottom: 1rem;">
          <asp:Literal ID="litAchFormTitle" runat="server" Text="➕ Create Achievement" />
        </h2>
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
          <div style="display: flex; gap: 0.5rem;">
            <asp:Button ID="btnAddAch" runat="server" Text="➕ Create Achievement" OnClick="btnAddAch_Click" CssClass="btn btn-primary" style="flex: 1;" />
            <asp:Button ID="btnCancelAchEdit" runat="server" Text="❌ Cancel Edit" OnClick="btnCancelAchEdit_Click" Visible="false" CssClass="btn btn-secondary" />
          </div>
        </div>
      </div>

      <div class="glass-card">
        <h2 style="font-family: var(--font-heading); margin-bottom: 1rem;">🗃️ Active Achievements</h2>
        <asp:GridView ID="gvAchievements" runat="server" AutoGenerateColumns="false" CssClass="data-table" DataKeyNames="Id" OnRowCommand="gvAchievements_RowCommand" OnRowDeleting="gvAchievements_RowDeleting" EmptyDataText="No achievements found.">
          <Columns>
            <asp:BoundField DataField="Icon" HeaderText="Icon" ItemStyle-Width="50px" />
            <asp:BoundField DataField="Title" HeaderText="Title" />
            <asp:BoundField DataField="XpBonus" HeaderText="XP Bonus" ItemStyle-Width="90px" />
            <asp:TemplateField HeaderText="Actions" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Right">
              <ItemTemplate>
                <asp:Button ID="btnEditAch" runat="server" Text="✏️ Edit" CommandName="EditAchievement" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-secondary btn-sm" />
                <asp:Button ID="btnDeleteAch" runat="server" Text="🗑️" CommandName="DeleteAchievement" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-secondary btn-sm" OnClientClick="return confirm('Delete this achievement?');" />
              </ItemTemplate>
            </asp:TemplateField>
          </Columns>
        </asp:GridView>
      </div>
    </div>
  </asp:Panel>
</asp:Content>
