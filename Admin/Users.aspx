<%@ Page Title="DriveLingo | User Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="DriveLingo.Admin.Users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <asp:Panel ID="pnlNotification" runat="server" Visible="false" CssClass="glass-card" style="margin-bottom: 1.5rem; border-left: 4px solid var(--success); padding: 1rem;">
    <asp:Literal ID="litNotificationText" runat="server" />
  </asp:Panel>

  <!-- PANEL 2: CRUD USERS -->
  <asp:Panel ID="pnlUsers" runat="server">
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
              <asp:ListItem Value='Learner'>🚘 Candidate / Learner</asp:ListItem>
              <asp:ListItem Value="Instructor">👨‍✈️ Driving Instructor / Educator</asp:ListItem>
              <asp:ListItem Value="Admin">👑 System Administrator</asp:ListItem>
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
          OnRowCommand="gvUsers_RowCommand" EmptyDataText="No users found.">
          <Columns>
            <asp:BoundField DataField="Id" HeaderText="User ID" ReadOnly="true" ItemStyle-Width="80px" />
            <asp:BoundField DataField="Username" HeaderText="Username" />
            <asp:BoundField DataField="Email" HeaderText="Email Address" />
            
            <asp:TemplateField HeaderText="Role" ItemStyle-Width="120px">
              <ItemTemplate>
                  <asp:PlaceHolder runat="server" Visible='<%# (DriveLingo.Database.Models.User.UserRole) Eval("Role") == DriveLingo.Database.Models.User.UserRole.Admin %>'>
                      <span class="badge badge-danger">👑 ADMIN</span>
                  </asp:PlaceHolder>
                  <asp:PlaceHolder runat="server" Visible='<%# (DriveLingo.Database.Models.User.UserRole) Eval("Role") == DriveLingo.Database.Models.User.UserRole.Instructor %>'>
                    <span class="badge badge-warning">👨‍✈️ INSTRUCTOR</span>
                </asp:PlaceHolder>
                  <asp:PlaceHolder runat="server" Visible='<%# (DriveLingo.Database.Models.User.UserRole) Eval("Role") == DriveLingo.Database.Models.User.UserRole.Learner %>'>
                    <span class="badge badge-success">🚘 LEARNER</span>
                </asp:PlaceHolder>
              </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="Points" HeaderText="Points" ItemStyle-Width="70px" />
            <asp:BoundField DataField="CurrentLevel" HeaderText="Lvl" ItemStyle-Width="50px" />

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
</asp:Content>
