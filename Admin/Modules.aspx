<%@ Page Title="DriveLingo | Admin Module & Quiz Manager" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Modules.aspx.cs" Inherits="DriveLingo.Admin.Modules" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .module-tab-btn {
            background: rgba(15, 23, 42, 0.6);
            border: 1px solid rgba(255, 255, 255, 0.1);
            color: var(--text-secondary);
            padding: 0.65rem 1.1rem;
            border-radius: var(--radius-sm);
            cursor: pointer;
            font-weight: 600;
            font-size: 0.88rem;
            transition: all 0.2s ease;
            display: inline-flex;
            align-items: center;
            gap: 0.5rem;
            text-decoration: none;
        }
        .module-tab-btn:hover {
            border-color: var(--primary);
            color: white;
            transform: translateY(-2px);
        }
        .module-tab-btn.active {
            background: linear-gradient(135deg, var(--primary), var(--secondary));
            color: white;
            border-color: transparent;
            box-shadow: 0 4px 12px rgba(99, 102, 241, 0.3);
        }
        .module-card-badge {
            display: inline-flex;
            align-items: center;
            gap: 0.4rem;
            padding: 0.3rem 0.75rem;
            border-radius: 20px;
            font-size: 0.8rem;
            font-weight: 700;
            background: rgba(99, 102, 241, 0.2);
            color: var(--primary);
            border: 1px solid rgba(99, 102, 241, 0.4);
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
                <h1 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">📁 Admin Dynamic Module & Quiz Manager</h1>
                <p style="color: var(--text-secondary); margin: 0;">Create, edit, and delete curriculum modules dynamically, and manage quizzes under each custom module.</p>
            </div>
        </div>
    </div>

    <!-- SECTION 1: DYNAMIC MODULE MANAGEMENT (ADD, EDIT, DELETE MODULES) -->
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
                    <label style="display: block; font-weight: 600; margin-bottom: 0.4rem; font-size: 0.85rem;">Module Description</label>
                    <asp:TextBox ID="txtModuleDescription" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" placeholder="Overview of topics and rules covered in this module..." style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.7); color: white;" />
                </div>

                <div style="display: flex; gap: 0.5rem; margin-top: 0.25rem;">
                    <asp:Button ID="btnAddModule" runat="server" Text="➕ Save Module" OnClick="btnAddModule_Click" CssClass="btn btn-primary" style="flex: 1; padding: 0.85rem; font-weight: 700;" />
                    <asp:Button ID="btnCancelModuleEdit" runat="server" Text="❌ Cancel Edit" OnClick="btnCancelModuleEdit_Click" Visible="false" CssClass="btn btn-secondary" style="padding: 0.85rem;" />
                </div>
            </div>
        </div>

        <!-- Right: Active Modules List Grid -->
        <div class="glass-card">
            <h2 style="font-family: var(--font-heading); margin-bottom: 1.25rem;">🗂️ Curriculum Modules</h2>
            <asp:GridView ID="gvModules" runat="server" AutoGenerateColumns="false" CssClass="data-table" DataKeyNames="Id" OnRowCommand="gvModules_RowCommand" EmptyDataText="No modules found. Create one using the form.">
                <Columns>
                    <asp:TemplateField HeaderText="Module" ItemStyle-Width="220px">
                        <ItemTemplate>
                            <span class="module-card-badge">
                                <span><%# Eval("Icon") %></span>
                                <span><%# Eval("Name") %></span>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="Description" HeaderText="Description" />

                    <asp:TemplateField HeaderText="Actions" ItemStyle-Width="140px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Button ID="btnEditMod" runat="server" Text="✏️ Edit" CommandName="EditModule" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-secondary btn-sm" />
                            <asp:Button ID="btnDeleteMod" runat="server" Text="🗑️" CommandName="DeleteModule" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-secondary btn-sm" OnClientClick="return confirm('Delete this module? Associated quizzes will be unlinked.');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- SECTION 2: QUIZZES MANAGEMENT (FALLS UNDER A DYNAMIC MODULE) -->
    <div class="glass-card" style="margin-bottom: 1.5rem;">
        <h2 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">✍️ Quizzes by Module</h2>
        <p style="color: var(--text-secondary); margin-bottom: 1.25rem; font-size: 0.9rem;">Filter or assign practice quizzes under any dynamic module.</p>
        
        <!-- Dynamic Module Filter Buttons -->
        <div style="display: flex; gap: 0.6rem; flex-wrap: wrap; align-items: center;">
            <asp:LinkButton ID="btnFilterAllModules" runat="server" OnClick="btnFilterAllModules_Click" CssClass="module-tab-btn active">
                <span>🌐</span> All Quizzes
            </asp:LinkButton>

            <asp:Repeater ID="rptModuleFilters" runat="server" OnItemCommand="rptModuleFilters_ItemCommand">
                <ItemTemplate>
                    <asp:LinkButton ID="btnFilterModule" runat="server" CommandName="FilterByModule" CommandArgument='<%# Eval("Name") %>' CssClass='<%# GetFilterTabClass(Eval("Name").ToString()) %>'>
                        <span><%# Eval("Icon") %></span>
                        <span><%# Eval("Name") %></span>
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>

    <div class="grid-2-col" style="margin-bottom: 2rem;">
        <!-- Left: Create / Edit Quiz Form -->
        <div class="glass-card">
            <asp:HiddenField ID="hfEditingQuizId" runat="server" Value="" />
            <h2 style="font-family: var(--font-heading); margin-bottom: 1.25rem;">
                <asp:Literal ID="litQuizFormTitle" runat="server" Text="➕ Add Quiz under Module" />
            </h2>

            <div style="display: flex; flex-direction: column; gap: 1.1rem;">
                <div>
                    <label style="display: block; font-weight: 600; margin-bottom: 0.4rem; font-size: 0.9rem;">Select Target Module</label>
                    <asp:DropDownList ID="ddlQuizModule" runat="server" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.7); color: white;" />
                </div>

                <div>
                    <label style="display: block; font-weight: 600; margin-bottom: 0.4rem; font-size: 0.9rem;">Quiz Title</label>
                    <asp:TextBox ID="txtQuizTitle" runat="server" CssClass="form-control" placeholder="e.g. Speed Limits Practice Test" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.7); color: white;" />
                </div>

                <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 1rem;">
                    <div>
                        <label style="display: block; font-weight: 600; margin-bottom: 0.4rem; font-size: 0.9rem;">Reward Points (Pts)</label>
                        <asp:TextBox ID="txtQuizRewardPoints" runat="server" Text="100" TextMode="Number" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.7); color: white;" />
                    </div>
                </div>

                <div style="display: flex; gap: 0.5rem; margin-top: 0.5rem;">
                    <asp:Button ID="btnAddQuiz" runat="server" Text="➕ Save Quiz" OnClick="btnAddQuiz_Click" CssClass="btn btn-primary" style="flex: 1; padding: 0.85rem; font-weight: 700;" />
                    <asp:Button ID="btnCancelQuizEdit" runat="server" Text="❌ Cancel Edit" OnClick="btnCancelQuizEdit_Click" Visible="false" CssClass="btn btn-secondary" style="padding: 0.85rem;" />
                </div>
            </div>
        </div>

        <!-- Right: Quizzes List Grid (Manage Questions button removed!) -->
        <div class="glass-card">
            <h2 style="font-family: var(--font-heading); margin-bottom: 1.25rem;">📝 Active Quizzes Grid</h2>
            <asp:GridView ID="gvQuizzes" runat="server" AutoGenerateColumns="false" CssClass="data-table" DataKeyNames="Id" OnRowCommand="gvQuizzes_RowCommand" EmptyDataText="No quizzes found under selected module.">
                <Columns>
                    <asp:TemplateField HeaderText="Module Section" ItemStyle-Width="180px">
                        <ItemTemplate>
                            <span class="module-card-badge">
                                <%# Eval("Category") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="Title" HeaderText="Quiz Title" />
                    
                    <asp:TemplateField HeaderText="Questions" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <span style="font-weight: 700; color: var(--primary);"><%# GetQuestionCount(Eval("Questions")) %> Qs</span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="RewardPoints" HeaderText="Reward" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />

                    <asp:TemplateField HeaderText="Actions" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Button ID="btnEditQz" runat="server" Text="✏️ Edit" CommandName="EditQuiz" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-secondary btn-sm" />
                            <asp:Button ID="btnDeleteQz" runat="server" Text="🗑️" CommandName="DeleteQuiz" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-secondary btn-sm" OnClientClick="return confirm('Delete this quiz?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
