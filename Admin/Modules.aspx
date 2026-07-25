<%@ Page Title="DriveLingo | Admin Module & Reward Points Manager" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Modules.aspx.cs" Inherits="DriveLingo.Admin.Modules" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .module-card-badge {
            display: inline-flex;
            align-items: center;
            gap: 0.4rem;
            padding: 0.35rem 0.8rem;
            border-radius: 20px;
            font-size: 0.85rem;
            font-weight: 700;
            background: rgba(99, 102, 241, 0.2);
            color: var(--primary);
            border: 1px solid rgba(99, 102, 241, 0.4);
        }
        .pts-badge {
            display: inline-block;
            padding: 0.25rem 0.65rem;
            border-radius: 12px;
            font-size: 0.8rem;
            font-weight: 800;
            background: rgba(245, 158, 11, 0.2);
            color: var(--warning);
            border: 1px solid rgba(245, 158, 11, 0.4);
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Notification Banner -->
    <asp:Panel ID="pnlNotification" runat="server" Visible="false" CssClass="glass-card" style="margin-bottom: 1.5rem; border-left: 4px solid var(--success); padding: 1rem;">
        <asp:Literal ID="litNotificationText" runat="server" />
    </asp:Panel>

    <!-- Header Panel -->
    <div class="glass-card" style="margin-bottom: 2rem;">
        <div style="display: flex; justify-content: space-between; align-items: center; flex-wrap: wrap; gap: 1rem;">
            <div>
                <h1 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">📁 Admin Curriculum Module & Reward Points Manager</h1>
                <p style="color: var(--text-secondary); margin: 0;">Manage curriculum modules dynamically and configure the Reward Points awarded per question for candidate practice tests.</p>
            </div>
        </div>
    </div>

    <!-- DYNAMIC MODULE CRUD MANAGEMENT (WITH REWARD POINTS PER QUESTION) -->
    <div class="grid-2-col" style="margin-bottom: 2.5rem;">
        <!-- Left: Add / Edit Module Form -->
        <div class="glass-card">
            <asp:HiddenField ID="hfEditingModuleId" runat="server" Value="" />
            <h2 style="font-family: var(--font-heading); margin-bottom: 1.25rem;">
                <asp:Literal ID="litModuleFormTitle" runat="server" Text="➕ Create New Curriculum Module" />
            </h2>

            <div style="display: flex; flex-direction: column; gap: 1.1rem;">
                <div style="display: grid; grid-template-columns: 1fr 3fr; gap: 0.75rem;">
                    <div>
                        <label style="display: block; font-weight: 600; margin-bottom: 0.4rem; font-size: 0.85rem;">Icon Emoji</label>
                        <asp:TextBox ID="txtModuleIcon" runat="server" Text="📁" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.7); color: white; text-align: center; font-size: 1.2rem;" />
                    </div>
                    <div>
                        <label style="display: block; font-weight: 600; margin-bottom: 0.4rem; font-size: 0.85rem;">Module Name</label>
                        <asp:TextBox ID="txtModuleName" runat="server" CssClass="form-control" placeholder="e.g. Section A - Road Signs" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.7); color: white;" />
                    </div>
                </div>

                <div>
                    <label style="display: block; font-weight: 600; margin-bottom: 0.4rem; font-size: 0.85rem;">🪙 Reward Points Per Question (Set by Admin)</label>
                    <asp:TextBox ID="txtRewardPointsPerQuestion" runat="server" Text="20" TextMode="Number" CssClass="form-control" placeholder="20" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.7); color: white;" />
                    <span style="font-size: 0.8rem; color: var(--text-secondary); margin-top: 0.2rem; display: block;">Educators' quizzes under this module will award this amount of points per correct question.</span>
                </div>

                <div>
                    <label style="display: block; font-weight: 600; margin-bottom: 0.4rem; font-size: 0.85rem;">Module Description</label>
                    <asp:TextBox ID="txtModuleDescription" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" placeholder="Overview of topics and regulations covered in this module..." style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.7); color: white;" />
                </div>

                <div style="display: flex; gap: 0.5rem; margin-top: 0.25rem;">
                    <asp:Button ID="btnAddModule" runat="server" Text="➕ Save Module" OnClick="btnAddModule_Click" CssClass="btn btn-primary" style="flex: 1; padding: 0.85rem; font-weight: 700;" />
                    <asp:Button ID="btnCancelModuleEdit" runat="server" Text="❌ Cancel Edit" OnClick="btnCancelModuleEdit_Click" Visible="false" CssClass="btn btn-secondary" style="padding: 0.85rem;" />
                </div>
            </div>
        </div>

        <!-- Right: Active Modules List Grid -->
        <div class="glass-card">
            <h2 style="font-family: var(--font-heading); margin-bottom: 1.25rem;">🗂️ Active Curriculum Modules</h2>
            <asp:GridView ID="gvModules" runat="server" AutoGenerateColumns="false" CssClass="data-table" DataKeyNames="Id" OnRowCommand="gvModules_RowCommand" EmptyDataText="No modules found. Create one using the form.">
                <Columns>
                    <asp:TemplateField HeaderText="Module" ItemStyle-Width="200px">
                        <ItemTemplate>
                            <span class="module-card-badge">
                                <span><%# Eval("Icon") %></span>
                                <span><%# Eval("Name") %></span>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Reward Rate" ItemStyle-Width="110px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <span class="pts-badge">
                                🪙 <%# Eval("RewardPointsPerQuestion") %> Pts/Q
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="Description" HeaderText="Description" />

                    <asp:TemplateField HeaderText="Actions" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Button ID="btnEditMod" runat="server" Text="✏️ Edit" CommandName="EditModule" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-secondary btn-sm" />
                            <asp:Button ID="btnDeleteMod" runat="server" Text="🗑️" CommandName="DeleteModule" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-secondary btn-sm" OnClientClick="return confirm('Delete this module?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
