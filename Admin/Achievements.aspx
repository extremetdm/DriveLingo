<%@ Page Title="DriveLingo | Achievement Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Achievements.aspx.cs" Inherits="DriveLingo.Admin.Achievements" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Notification Banner -->
    <asp:Panel ID="pnlNotification" runat="server" Visible="false" CssClass="glass-card" style="margin-bottom: 1.5rem; border-left: 4px solid var(--success); padding: 1rem;">
        <asp:Literal ID="litNotificationText" runat="server" />
    </asp:Panel>

    <!-- PANEL: CRUD ACHIEVEMENTS -->
    <asp:Panel ID="pnlAchievements" runat="server">
        <div class="glass-card" style="margin-bottom: 2rem;">
            <h1 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">🏆 CRUD System Achievements</h1>
            <p style="color: var(--text-secondary); margin: 0;">Define achievement milestones, target count thresholds, metric types, and reward XP bonuses for candidate practice milestones.</p>
        </div>

        <div class="grid-2-col">
            <!-- Create / Edit Form -->
            <div class="glass-card">
                <asp:HiddenField ID="hfEditingAchId" runat="server" Value="" />
                <h2 style="font-family: var(--font-heading); margin-bottom: 1rem;">
                    <asp:Literal ID="litAchFormTitle" runat="server" Text="➕ Create Achievement" />
                </h2>

                <div style="display: flex; flex-direction: column; gap: 1rem;">
                    <div>
                        <label style="display: block; font-weight: 600; margin-bottom: 0.4rem; font-size: 0.85rem;">Achievement Title</label>
                        <asp:TextBox ID="txtAchTitle" runat="server" CssClass="form-control" placeholder="Theory Master" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
                    </div>

                    <div style="display: grid; grid-template-columns: 1fr 1fr 1fr; gap: 0.75rem;">
                        <div>
                            <label style="display: block; font-weight: 600; margin-bottom: 0.4rem; font-size: 0.85rem;">Icon Emoji</label>
                            <asp:TextBox ID="txtAchIcon" runat="server" Text="🏆" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white; text-align: center;" />
                        </div>
                        <div>
                            <label style="display: block; font-weight: 600; margin-bottom: 0.4rem; font-size: 0.85rem;">XP Bonus</label>
                            <asp:TextBox ID="txtAchXp" runat="server" Text="100" TextMode="Number" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
                        </div>
                        <div>
                            <label style="display: block; font-weight: 600; margin-bottom: 0.4rem; font-size: 0.85rem;">Target Count</label>
                            <asp:TextBox ID="txtTargetCount" runat="server" Text="5" TextMode="Number" CssClass="form-control" placeholder="5" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
                        </div>
                    </div>

                    <div>
                        <label style="display: block; font-weight: 600; margin-bottom: 0.4rem; font-size: 0.85rem;">Milestone Metric Type</label>
                        <asp:DropDownList ID="ddlMetricType" runat="server" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;">
                            <asp:ListItem Value="quiz_count" Text="📝 Quizzes Answered" />
                            <asp:ListItem Value="perfect_score" Text="💯 Flawless 100% Scores" />
                            <asp:ListItem Value="materials_read" Text="📖 Study Guides Read" />
                        </asp:DropDownList>
                    </div>

                    <div>
                        <label style="display: block; font-weight: 600; margin-bottom: 0.4rem; font-size: 0.85rem;">Achievement Description</label>
                        <asp:TextBox ID="txtAchDesc" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control" placeholder="Answer 5 practice quizzes to unlock" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
                    </div>

                    <div style="display: flex; gap: 0.5rem; margin-top: 0.5rem;">
                        <asp:Button ID="btnAddAch" runat="server" Text="➕ Create Achievement" OnClick="btnAddAch_Click" CssClass="btn btn-primary" style="flex: 1; padding: 0.85rem; font-weight: 700;" />
                        <asp:Button ID="btnCancelAchEdit" runat="server" Text="❌ Cancel Edit" OnClick="btnCancelAchEdit_Click" Visible="false" CssClass="btn btn-secondary" style="padding: 0.85rem;" />
                    </div>
                </div>
            </div>

            <!-- Data Table Grid -->
            <div class="glass-card">
                <h2 style="font-family: var(--font-heading); margin-bottom: 1rem;">🗃️ Active System Achievements</h2>
                <asp:GridView ID="gvAchievements" runat="server" AutoGenerateColumns="false" CssClass="data-table" DataKeyNames="Id" OnRowCommand="gvAchievements_RowCommand" EmptyDataText="No achievements found.">
                    <Columns>
                        <asp:BoundField DataField="Icon" HeaderText="Icon" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="Title" HeaderText="Achievement Title" />
                        <asp:BoundField DataField="MetricType" HeaderText="Metric Type" ItemStyle-Width="110px" />
                        <asp:BoundField DataField="TargetCount" HeaderText="Target" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="XpBonus" HeaderText="XP Bonus" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="Actions" ItemStyle-Width="140px" ItemStyle-HorizontalAlign="Right">
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
