<%@ Page Title="DriveLingo | Store" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Shop.aspx.cs" Inherits="DriveLingo.Shop" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .shop-category-btn {
            background: rgba(15, 23, 42, 0.6);
            border: 1px solid rgba(255, 255, 255, 0.1);
            color: var(--text-secondary);
            padding: 0.6rem 1.25rem;
            border-radius: var(--radius-sm);
            cursor: pointer;
            font-weight: 600;
            font-size: 0.9rem;
            transition: all 0.2s ease;
        }
        .shop-category-btn.active {
            background: linear-gradient(135deg, var(--primary), var(--secondary));
            color: white;
            border-color: transparent;
            box-shadow: 0 4px 12px rgba(99, 102, 241, 0.3);
        }
        .category-pill {
            display: inline-block;
            padding: 0.2rem 0.6rem;
            border-radius: 12px;
            font-size: 0.72rem;
            font-weight: 700;
            text-transform: uppercase;
            margin-bottom: 0.5rem;
        }
        .cat-border { background: rgba(168, 85, 247, 0.2); color: #c084fc; border: 1px solid rgba(168, 85, 247, 0.4); }
        .cat-icon { background: rgba(59, 130, 246, 0.2); color: #60a5fa; border: 1px solid rgba(59, 130, 246, 0.4); }
        .cat-badge { background: rgba(245, 158, 11, 0.2); color: #fbbf24; border: 1px solid rgba(245, 158, 11, 0.4); }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Notification Banner -->
    <asp:Panel ID="pnlNotification" runat="server" Visible="false" CssClass="glass-card" style="margin-bottom: 1.5rem; border-left: 4px solid var(--success); padding: 1rem;">
        <asp:Literal ID="litNotificationText" runat="server" />
    </asp:Panel>

    <!-- Header Panel -->
    <div class="glass-card" style="margin-bottom: 2rem;">
        <h2 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">🛒 Candidate Points Marketplace</h2>
        <p style="color: var(--text-secondary); margin: 0;">Redeem quiz reward points for exclusive profile borders (icon frames), custom avatar icons, and name badges.</p>
    </div>

    <!-- Category Filter Tabs -->
    <div style="display: flex; gap: 0.75rem; margin-bottom: 2rem; flex-wrap: wrap;">
        <asp:LinkButton ID="btnTabAll" runat="server" OnClick="btnCategoryFilter_Click" CommandArgument="ALL" CssClass="shop-category-btn active">
            🌐 All Cosmetics
        </asp:LinkButton>
        <asp:LinkButton ID="btnTabBorder" runat="server" OnClick="btnCategoryFilter_Click" CommandArgument="Border" CssClass="shop-category-btn">
            ✨ Border (Icon Frame)
        </asp:LinkButton>
        <asp:LinkButton ID="btnTabIcon" runat="server" OnClick="btnCategoryFilter_Click" CommandArgument="Icon" CssClass="shop-category-btn">
            🏎️ Custom Avatar Icons
        </asp:LinkButton>
        <asp:LinkButton ID="btnTabBadge" runat="server" OnClick="btnCategoryFilter_Click" CommandArgument="Badge" CssClass="shop-category-btn">
            ⚡ Name Badges
        </asp:LinkButton>
    </div>

    <!-- Store Items Grid -->
    <div class="grid-3-col">
        <asp:Repeater ID="rptStore" runat="server" OnItemCommand="rptStore_ItemCommand">
            <ItemTemplate>
                <div class="glass-card" style="text-align: center; display: flex; flex-direction: column; justify-content: space-between;">
                    <div>
                        <span class='<%# GetCategoryBadgeClass(Eval("Category").ToString()) %>'>
                            <%# Eval("Category") %>
                        </span>
                        <span style="font-size: 3.5rem; display: block; margin: 0.5rem 0;"><%# Eval("Icon") %></span>
                        <h3 style="font-family: var(--font-heading); margin-bottom: 0.25rem;"><%# Eval("Title") %></h3>
                        <span class="badge" style="background: rgba(245, 158, 11, 0.2); color: var(--warning); margin-bottom: 0.75rem; display: inline-block;">
                            🪙 <%# Eval("Price") %> Points
                        </span>
                        <p style="color: var(--text-secondary); font-size: 0.88rem;"><%# Eval("Description") %></p>
                    </div>
                    
                    <div style="margin-top: 1.5rem;">
                        <asp:PlaceHolder ID="phOwned" runat="server" Visible='<%# Convert.ToBoolean(Eval("Owned")) %>'>
                            <span class="badge badge-success" style="font-size: 0.95rem; padding: 0.6rem 1.2rem; display: block; width: 100%;">Owned ✔</span>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="phUnowned" runat="server" Visible='<%# !Convert.ToBoolean(Eval("Owned")) %>'>
                            <asp:Button ID="btnBuyItem" runat="server" Text="Redeem Item" CommandName="BuyItem" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-primary" style="width: 100%;" />
                        </asp:PlaceHolder>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
