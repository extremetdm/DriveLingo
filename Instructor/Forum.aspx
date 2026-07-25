<%@ Page Title="DriveLingo | Forum Discussions" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Forum.aspx.cs" Inherits="DriveLingo.Instructor.Forum" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <asp:Panel ID="pnlNotification" runat="server" Visible="false" CssClass="glass-card" style="margin-bottom: 1.5rem; border-left: 4px solid var(--success); padding: 1rem;">
    <asp:Literal ID="litNotificationText" runat="server" />
  </asp:Panel>

  <!-- PANEL 3: FORUM MODERATION -->
  <asp:Panel ID="pnlForum" runat="server">
    <div class="glass-card" style="margin-bottom: 2rem;">
      <h1 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">💬 Forum Discussion Moderation</h1>
      <p style="color: var(--text-secondary); margin: 0;">Provide educator-verified answers to student questions and answer community inquiries.</p>
    </div>

    <div class="glass-card">
      <h2 style="font-family: var(--font-heading); margin-bottom: 1.5rem;">Answer Student Inquiries</h2>
      <asp:Repeater ID="rptForumModeration" runat="server" OnItemCommand="rptForumModeration_ItemCommand" OnItemDataBound="rptForumModeration_ItemDataBound">
        <ItemTemplate>
          <div style="margin-bottom: 1.25rem; padding: 1.25rem; background: rgba(15,23,42,0.4); border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.05);">
            <div style="display: flex; justify-content: space-between; margin-bottom: 0.5rem;">
                <!-- TODO CHANGE THIS -->
              <span class="badge" style="background: rgba(99, 102, 241, 0.2); color: var(--primary);"><%# "what category bro" %></span>
              <span style="font-size: 0.85rem; color: var(--text-secondary);"><%# Eval("CreatedAt", "{0:g}") %></span>
            </div>
            <h4 style="margin: 0 0 0.5rem 0; font-size: 1.1rem;"><%# Eval("Title") %></h4>
            <p style="color: var(--text-secondary); font-size: 0.9rem; margin-bottom: 1rem;"><%# Eval("Content") %></p>

            <!-- Thread Comments & Educator Answers -->
            <asp:Repeater ID="rptEducatorReplies" runat="server" OnItemDataBound="rptReplies_ItemDataBound">
              <HeaderTemplate>
                <div style="margin-top: 1rem; margin-bottom: 1rem; padding-left: 1.25rem; border-left: 2px solid rgba(255,255,255,0.1); display: flex; flex-direction: column; gap: 0.75rem;">
              </HeaderTemplate>
              <ItemTemplate>
                <%-- 1. Instructor Answer State --%>
                <asp:PlaceHolder ID="phEducatorReply" runat="server" Visible="false">
                  <div style="padding: 0.75rem; background: rgba(16, 185, 129, 0.12); border-radius: var(--radius-sm); border: 1px solid var(--success);">
                    <div style="display: flex; justify-content: space-between; margin-bottom: 0.35rem;">
                      <span style="font-weight: 700; font-size: 0.85rem; color: var(--success);">
                        <%# Eval("AuthorAvatar") %> <%# Eval("AuthorName") %> ✔ (Instructor Verified Answer)
                      </span>
                      <span style="font-size: 0.75rem; color: var(--text-secondary);"><%# Eval("CreatedAt", "{0:g}") %></span>
                    </div>
                    <p style="margin: 0; font-size: 0.9rem; line-height: 1.4; color: var(--text-primary);"><%# Eval("Content") %></p>
                  </div>
                </asp:PlaceHolder>

                <%-- 2. Standard / Non-Educator Answer State --%>
                <asp:PlaceHolder ID="phStandardReply" runat="server" Visible="false">
                  <div style="padding: 0.75rem; background: rgba(15, 23, 42, 0.4); border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.05);">
                    <div style="display: flex; justify-content: space-between; margin-bottom: 0.35rem;">
                      <span style="font-weight: 700; font-size: 0.85rem; color: inherit;">
                        <%# Eval("AuthorAvatar") %> <%# Eval("AuthorName") %> (<%# Eval("AuthorRole") %>)
                      </span>
                      <span style="font-size: 0.75rem; color: var(--text-secondary);"><%# Eval("CreatedAt", "{0:g}") %></span>
                    </div>
                    <p style="margin: 0; font-size: 0.9rem; line-height: 1.4; color: var(--text-primary);"><%# Eval("Content") %></p>
                  </div>
                </asp:PlaceHolder>


              </ItemTemplate>
              <FooterTemplate>
                </div>
              </FooterTemplate>
            </asp:Repeater>
            
            <div style="display: flex; gap: 0.5rem;">
              <asp:TextBox ID="txtEducatorReply" runat="server" CssClass="form-control" placeholder="Write verified instructor response..." style="flex: 1; padding: 0.6rem; font-size: 0.85rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
              <asp:Button ID="btnSubmitReply" runat="server" Text="Post Answer" CommandName="Reply" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-primary btn-sm" />
            </div>
          </div>
        </ItemTemplate>
      </asp:Repeater>
    </div>
  </asp:Panel>
</asp:Content>

