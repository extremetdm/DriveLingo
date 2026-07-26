<%@ Page Title="DriveLingo | Profile Settings" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="DriveLingo.UserProfilePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .inventory-card {
            display: flex;
            align-items: center;
            justify-content: space-between;
            padding: 1rem;
            background: rgba(15, 23, 42, 0.4);
            border-radius: var(--radius-sm);
            border: 1px solid rgba(255, 255, 255, 0.08);
            margin-bottom: 0.75rem;
        }
        .inventory-card.equipped {
            border-color: var(--primary);
            background: rgba(99, 102, 241, 0.1);
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="max-width: 720px; margin: 0 auto;">
        <!-- Profile Header Card -->
        <div class="glass-card" style="margin-bottom: 2rem; display: flex; align-items: center; gap: 2rem; flex-wrap: wrap;">
            <div id="divAvatarBox" runat="server" style="font-size: 4.5rem; width: 100px; height: 100px; background: rgba(99, 102, 241, 0.2); border-radius: 50%; display: flex; align-items: center; justify-content: center; border: 2px solid var(--primary);">
                <asp:Literal ID="litAvatar" runat="server" Text="🚗" />
            </div>

            <div style="flex: 1;">
                <span class="badge" style="background: rgba(99, 102, 241, 0.2); color: var(--primary); margin-bottom: 0.5rem; display: inline-block;">
                    <asp:Literal ID="litRoleBadge" runat="server" Text="LEARNER" />
                </span>
                <h1 style="font-family: var(--font-heading); margin-bottom: 0.25rem;">
                    <asp:Literal ID="litUserName" runat="server" Text="Alex Hero" />
                    <asp:Literal ID="litUserBadge" runat="server" />
                </h1>
                <p style="color: var(--text-secondary); margin: 0;">
                    <asp:Literal ID="litUserEmail" runat="server" Text="learner@drivelingo.com" /> | Joined <asp:Literal ID="litJoinedDate" runat="server" Text="2026-07-01" />
                </p>
            </div>
        </div>

        <!-- Notification Banner -->
        <asp:Panel ID="pnlNotification" runat="server" Visible="false" CssClass="glass-card" style="margin-bottom: 1.5rem; border-left: 4px solid var(--success); padding: 1rem;">
            <asp:Literal ID="litNotificationText" runat="server" />
        </asp:Panel>

        <!-- Profile Edit Form (Avatar Emoji picker removed; role default icon retained) -->
        <div class="glass-card" style="margin-bottom: 2rem;">
            <h2 style="font-family: var(--font-heading); margin-bottom: 1.25rem;">⚙️ Edit Profile Settings</h2>

            <div style="display: flex; flex-direction: column; gap: 1.25rem;">
                <div>
                    <label style="display: block; font-weight: 600; margin-bottom: 0.4rem; font-size: 0.9rem;">Display Name</label>
                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
                </div>

                <div>
                    <label style="display: block; font-weight: 600; margin-bottom: 0.4rem; font-size: 0.9rem;">New Password (leave blank to keep current)</label>
                    <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
                </div>

                <asp:Button ID="btnSaveProfile" runat="server" Text="Save Profile Changes" OnClick="btnSaveProfile_Click" CssClass="btn btn-primary" style="padding: 0.85rem; font-weight: 700;" />
            </div>
        </div>

        <!-- Inventory Showcase & Equipment System -->
        <div class="glass-card">
            <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 1.25rem;">
                <div>
                    <h2 style="font-family: var(--font-heading); margin: 0 0 0.25rem 0;">🎒 Cosmetics & Equipment Inventory</h2>
                    <p style="color: var(--text-secondary); margin: 0; font-size: 0.88rem;">Equip avatar borders, custom icons, and name badges unlocked from the store.</p>
                </div>
            </div>

            <asp:Repeater ID="rptInventory" runat="server" OnItemCommand="rptInventory_ItemCommand">
                <HeaderTemplate>
                    <div style="display: flex; flex-direction: column;">
                </HeaderTemplate>
                <ItemTemplate>
                    <div class='<%# Convert.ToBoolean(Eval("IsEquipped")) ? "inventory-card equipped" : "inventory-card" %>'>
                        <div style="display: flex; align-items: center; gap: 0.75rem;">
                            <span style="font-size: 2rem;"><%# Eval("Icon") %></span>
                            <div>
                                <strong style="display: block; font-size: 0.95rem;"><%# Eval("Name") %></strong>
                                <span class="badge" style="background: rgba(255,255,255,0.1); font-size: 0.72rem;"><%# Eval("Type") %></span>
                            </div>
                        </div>

                        <div>
                            <asp:Button ID="btnEquip" runat="server" 
                                Text='<%# Convert.ToBoolean(Eval("IsEquipped")) ? "Equipped ⚡" : "Equip Item" %>' 
                                CommandName='<%# Convert.ToBoolean(Eval("IsEquipped")) ? "UnequipItem" : "EquipItem" %>' 
                                CommandArgument='<%# Eval("Id") %>' 
                                CssClass='<%# Convert.ToBoolean(Eval("IsEquipped")) ? "btn btn-secondary btn-sm" : "btn btn-primary btn-sm" %>' />
                        </div>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                    </div>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>
