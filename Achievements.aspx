<%@ Page Title="DriveLingo | Achievements Progress" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Achievements.aspx.cs" Inherits="DriveLingo.Achievements" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .achievement-card {
            background: rgba(15, 23, 42, 0.6);
            border: 1px solid rgba(255, 255, 255, 0.08);
            border-radius: var(--radius-md);
            padding: 1.5rem;
            display: flex;
            flex-direction: column;
            justify-content: space-between;
            position: relative;
            overflow: hidden;
            transition: all 0.3s ease;
        }
        .achievement-card.unlocked {
            border-color: var(--success);
            box-shadow: 0 0 15px rgba(16, 185, 129, 0.15);
        }
        .progress-bar-bg {
            height: 8px;
            background: rgba(255, 255, 255, 0.1);
            border-radius: 4px;
            overflow: hidden;
            margin: 0.75rem 0;
        }
        .progress-bar-fill {
            height: 100%;
            background: linear-gradient(90deg, var(--primary), var(--secondary));
            border-radius: 4px;
            transition: width 0.4s ease;
        }
        .progress-bar-fill.completed {
            background: linear-gradient(90deg, var(--success), #34d399);
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
        <h1 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">🏆 Candidate Milestones & Progress Tracker</h1>
        <p style="color: var(--text-secondary); margin: 0;">Complete practice exams, achieve perfect scores, and study JPJ guides to unlock achievements and earn XP bonuses!</p>
    </div>

    <!-- Achievements Grid with Visual Progress Bars -->
    <div class="grid-3-col">
        <asp:Repeater ID="rptAchievements" runat="server">
            <ItemTemplate>
                <div class='<%# Convert.ToBoolean(Eval("IsUnlocked")) ? "achievement-card unlocked" : "achievement-card" %>'>
                    <div style="text-align: center;">
                        <span style="font-size: 3.5rem; display: block; margin-bottom: 0.5rem;"><%# Eval("Icon") %></span>
                        <h3 style="font-family: var(--font-heading); margin-bottom: 0.25rem;"><%# Eval("Title") %></h3>
                        <p style="color: var(--text-secondary); font-size: 0.88rem; margin-bottom: 1rem; min-height: 40px;"><%# Eval("Description") %></p>
                    </div>

                    <div>
                        <!-- Progress text e.g. "3 / 5 Quizzes (60%)" -->
                        <div style="display: flex; justify-content: space-between; font-size: 0.8rem; font-weight: 700; color: var(--text-secondary);">
                            <span>Progress</span>
                            <span><%# Eval("CurrentProgress") %> / <%# Eval("TargetCount") %> (<%# Eval("ProgressPercentage") %>%)</span>
                        </div>

                        <!-- Animated Progress Bar -->
                        <div class="progress-bar-bg">
                            <div class='<%# Convert.ToBoolean(Eval("IsUnlocked")) ? "progress-bar-fill completed" : "progress-bar-fill" %>' style='<%# "width: " + Eval("ProgressPercentage") + "%;" %>'></div>
                        </div>

                        <!-- Status Badge -->
                        <div style="text-align: center; margin-top: 0.5rem;">
                            <%# Convert.ToBoolean(Eval("IsUnlocked")) 
                                ? "<span class='badge' style='background: rgba(16, 185, 129, 0.2); color: var(--success); font-weight: 700;'>🏆 Unlocked (+ " + Eval("XpBonus") + " XP)</span>" 
                                : "<span class='badge' style='background: rgba(255, 255, 255, 0.1); color: var(--text-secondary);'>🔒 Locked (" + Eval("XpBonus") + " XP Reward)</span>" %>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
