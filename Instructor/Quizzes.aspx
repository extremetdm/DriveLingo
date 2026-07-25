<%@ Page Title="DriveLingo | Educator Quiz Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Quizzes.aspx.cs" Inherits="DriveLingo.Instructor.Quizzes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .module-badge {
            display: inline-flex;
            align-items: center;
            gap: 0.4rem;
            padding: 0.25rem 0.65rem;
            border-radius: 15px;
            font-size: 0.78rem;
            font-weight: 700;
            background: rgba(99, 102, 241, 0.2);
            color: var(--primary);
            border: 1px solid rgba(99, 102, 241, 0.4);
        }
        .pts-rate-badge {
            display: inline-block;
            padding: 0.25rem 0.6rem;
            border-radius: 12px;
            font-size: 0.75rem;
            font-weight: 700;
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
        <h1 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">✍️ Educator Quiz & Question Bank Manager</h1>
        <p style="color: var(--text-secondary); margin: 0;">Select a curriculum module section, create practice quizzes, and author practice questions. Reward points per question are configured by System Administration.</p>
    </div>

    <!-- SECTION 1: EDUCATOR CREATE / EDIT QUIZ UNDER MODULE SECTION -->
    <div class="grid-2-col" style="margin-bottom: 2.5rem;">
        <!-- Left: Quiz Creation Form -->
        <div class="glass-card">
            <asp:HiddenField ID="hfEditingQuizId" runat="server" Value="" />
            <h2 style="font-family: var(--font-heading); margin-bottom: 1.25rem;">
                <asp:Literal ID="litQuizFormTitle" runat="server" Text="➕ Create Quiz under Curriculum Module" />
            </h2>

            <div style="display: flex; flex-direction: column; gap: 1.1rem;">
                <div>
                    <label style="display: block; font-weight: 600; margin-bottom: 0.4rem; font-size: 0.9rem;">1. Select Module Section</label>
                    <asp:DropDownList ID="ddlQuizModuleSection" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlQuizModuleSection_SelectedIndexChanged" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.7); color: white;" />
                    <div style="margin-top: 0.3rem;">
                        <asp:Literal ID="litModuleRewardRateInfo" runat="server" />
                    </div>
                </div>

                <div>
                    <label style="display: block; font-weight: 600; margin-bottom: 0.4rem; font-size: 0.9rem;">2. Quiz Title</label>
                    <asp:TextBox ID="txtQuizTitle" runat="server" CssClass="form-control" placeholder="e.g. Prohibitory Road Signs Practice Test" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.7); color: white;" />
                </div>

                <div style="display: flex; gap: 0.5rem; margin-top: 0.5rem;">
                    <asp:Button ID="btnAddQuiz" runat="server" Text="➕ Save Quiz" OnClick="btnAddQuiz_Click" CssClass="btn btn-primary" style="flex: 1; padding: 0.85rem; font-weight: 700;" />
                    <asp:Button ID="btnCancelQuizEdit" runat="server" Text="❌ Cancel Edit" OnClick="btnCancelQuizEdit_Click" Visible="false" CssClass="btn btn-secondary" style="padding: 0.85rem;" />
                </div>
            </div>
        </div>

        <!-- Right: Educator Quizzes List Grid -->
        <div class="glass-card">
            <h2 style="font-family: var(--font-heading); margin-bottom: 1.25rem;">📝 Educator Quizzes</h2>
            <asp:GridView ID="gvQuizzes" runat="server" AutoGenerateColumns="false" CssClass="data-table" DataKeyNames="Id" OnRowCommand="gvQuizzes_RowCommand" EmptyDataText="No quizzes found. Select a module section and create one.">
                <Columns>
                    <asp:TemplateField HeaderText="Module Section" ItemStyle-Width="180px">
                        <ItemTemplate>
                            <span class="module-badge">
                                <%# Eval("Module.Name") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="Title" HeaderText="Quiz Title" />
                    
                    <asp:TemplateField HeaderText="Questions" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <span style="font-weight: 700; color: var(--primary);"><%# GetQuestionCount(Eval("Questions")) %> Qs</span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Actions" ItemStyle-Width="180px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Button ID="btnManageQs" runat="server" Text="❓ Qs" CommandName="ManageQuestions" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-primary btn-sm" Title="Manage Questions" />
                            <asp:Button ID="btnEditQz" runat="server" Text="✏️" CommandName="EditQuiz" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-secondary btn-sm" />
                            <asp:Button ID="btnDeleteQz" runat="server" Text="🗑️" CommandName="DeleteQuiz" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-secondary btn-sm" OnClientClick="return confirm('Delete this quiz and its questions?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- SECTION 2: QUESTION BANK MANAGEMENT FOR SELECTED QUIZ -->
    <asp:Panel ID="pnlQuestionBank" runat="server" Visible="false" CssClass="glass-card" style="margin-top: 2rem;">
        <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 1.5rem; padding-bottom: 1rem; border-bottom: 1px solid rgba(255,255,255,0.1);">
            <div>
                <h2 style="font-family: var(--font-heading); margin: 0 0 0.25rem 0;">
                    ❓ Questions for: <asp:Literal ID="litActiveQuizTitle" runat="server" />
                </h2>
                <p style="color: var(--text-secondary); margin: 0; font-size: 0.9rem;">
                    Module Section: <asp:Literal ID="litActiveQuizModule" runat="server" />
                </p>
            </div>
            <asp:Button ID="btnCloseQuestionBank" runat="server" Text="✖ Close Questions Panel" OnClick="btnCloseQuestionBank_Click" CssClass="btn btn-secondary btn-sm" />
        </div>

        <div class="grid-2-col">
            <!-- Add / Edit Question Form -->
            <div style="background: rgba(15, 23, 42, 0.4); padding: 1.25rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.08);">
                <asp:HiddenField ID="hfEditingQuestionId" runat="server" Value="" />
                <h3 style="font-family: var(--font-heading); margin-bottom: 1rem;">
                    <asp:Literal ID="litQuestionFormTitle" runat="server" Text="➕ Add New Question" />
                </h3>

                <div style="display: flex; flex-direction: column; gap: 1rem;">
                    <div>
                        <label style="display: block; font-weight: 600; margin-bottom: 0.4rem; font-size: 0.85rem;">Question Prompt Text</label>
                        <asp:TextBox ID="txtQuestionText" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control" placeholder="What does this road sign indicate?" style="width: 100%; padding: 0.6rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.7); color: white;" />
                    </div>

                    <div>
                        <label style="display: block; font-weight: 600; margin-bottom: 0.4rem; font-size: 0.85rem;">Attach Question Image (File Upload or URL)</label>
                        <asp:FileUpload ID="fileQuestionImage" runat="server" CssClass="form-control" style="width: 100%; padding: 0.4rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.7); color: white; margin-bottom: 0.4rem;" />
                        <asp:TextBox ID="txtQuestionImageUrl" runat="server" CssClass="form-control" placeholder="uploads/no_entry.svg or image link..." style="width: 100%; padding: 0.5rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.7); color: white;" />
                    </div>

                    <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 0.75rem;">
                        <asp:Repeater ID="rptChoices" runat="server">
                            <ItemTemplate>
                                <div style="margin-bottom: 0.75rem;">
                                    <asp:HiddenField ID="hfChoiceId" runat="server" Value='<%# Eval("Id") %>' />
            
                                    <label style="display: block; font-weight: 600; margin-bottom: 0.3rem; font-size: 0.85rem;">
                                        Option <%# GetChoicePlaceholder(Container.ItemIndex) %>
                                    </label>
            
                                    <asp:TextBox ID="txtChoiceText" runat="server" Text='<%# Eval("Text") %>' CssClass="form-control" placeholder='<%# "Choice " + GetChoicePlaceholder(Container.ItemIndex) %>' style="width: 100%; padding: 0.5rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.7); color: white;" />
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        
                    </div>

                    <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 0.75rem;">
                        <div>
                            <label style="display: block; font-weight: 600; margin-bottom: 0.3rem; font-size: 0.85rem;">Correct Answer Key</label>
                            <asp:DropDownList ID="ddlCorrectIndex" runat="server" CssClass="form-control" style="width: 100%; padding: 0.55rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.7); color: white;">
                                <asp:ListItem Value="0" Text="Option A" />
                                <asp:ListItem Value="1" Text="Option B" />
                                <asp:ListItem Value="2" Text="Option C" />
                                <asp:ListItem Value="3" Text="Option D" />
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div>
                        <label style="display: block; font-weight: 600; margin-bottom: 0.3rem; font-size: 0.85rem;">Rule Explanation / Citation</label>
                        <asp:TextBox ID="txtQuestionExplanation" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control" placeholder="Explanation for candidate after answering..." style="width: 100%; padding: 0.5rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.7); color: white;" />
                    </div>

                    <div style="display: flex; gap: 0.5rem;">
                        <asp:Button ID="btnAddQuestion" runat="server" Text="➕ Save Question" OnClick="btnAddQuestion_Click" CssClass="btn btn-primary" style="flex: 1; padding: 0.75rem; font-weight: 700;" />
                        <asp:Button ID="btnCancelQuestionEdit" runat="server" Text="❌ Cancel" OnClick="btnCancelQuestionEdit_Click" Visible="false" CssClass="btn btn-secondary" style="padding: 0.75rem;" />
                    </div>
                </div>
            </div>

            <!-- Existing Questions List Grid -->
            <div>
                <h3 style="font-family: var(--font-heading); margin-bottom: 1rem;">📋 Question Bank Grid</h3>
                <asp:GridView ID="gvQuestions" runat="server" AutoGenerateColumns="false" CssClass="data-table" DataKeyNames="Id" OnRowCommand="gvQuestions_RowCommand" EmptyDataText="No questions added to this quiz yet.">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="Q ID" ItemStyle-Width="60px" />
                        <asp:BoundField DataField="Text" HeaderText="Prompt" />
                        <asp:TemplateField HeaderText="Actions" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Button ID="btnEditQuestion" runat="server" Text="✏️" CommandName="EditQuestion" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-secondary btn-sm" Title="Edit Question" />
                                <asp:Button ID="btnDeleteQuestion" runat="server" Text="🗑️" CommandName="DeleteQuestion" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-secondary btn-sm" OnClientClick="return confirm('Delete this question?');" Title="Delete Question" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
