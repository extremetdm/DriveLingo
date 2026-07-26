<%@ Page Title="DriveLingo | Forum Discussions" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Forum.aspx.cs" Inherits="DriveLingo.Forum" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function toggleUpvote(btn) {
            var statusSpan = btn.querySelector('.upvote-status');
            var countSpan = btn.querySelector('.upvote-count');
            var currentCount = parseInt(countSpan.innerText) || 0;
            var isUpvoted = btn.classList.contains('is-upvoted');

            if (isUpvoted) {
                btn.classList.remove('is-upvoted');
                statusSpan.innerText = 'Upvote';
                countSpan.innerText = Math.max(0, currentCount - 1);
                btn.style.background = 'rgba(234, 179, 8, 0.1)';
                btn.style.color = 'var(--warning)';
                btn.style.borderColor = 'var(--warning)';
            } else {
                btn.classList.add('is-upvoted');
                statusSpan.innerText = 'Upvoted';
                countSpan.innerText = currentCount + 1;
                btn.style.background = 'var(--warning)';
                btn.style.color = '#0f172a';
                btn.style.borderColor = 'var(--warning)';
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Notification Banner -->
    <asp:Panel ID="pnlNotification" runat="server" Visible="false" CssClass="glass-card" style="margin-bottom: 1.5rem; border-left: 4px solid var(--success); padding: 1rem;">
        <asp:Literal ID="litNotificationText" runat="server" />
    </asp:Panel>

    <!-- TAB 5: FORUM -->
    <asp:Panel ID="pnlForum" runat="server">
        <div class="glass-card" style="margin-bottom: 2rem; display: flex; justify-content: space-between; align-items: center; flex-wrap: wrap; gap: 1rem;">
            <div>
                <h2 style="font-family: var(--font-heading); margin-bottom: 0.25rem;">💬 Candidate Community Q&A</h2>
                <p style="color: var(--text-secondary); margin: 0;">Ask questions about JPJ rules and get answers from fellow candidates and JPJ Educators.</p>
            </div>
            <asp:Button ID="btnToggleNewQuestion" runat="server" Text="➕ Ask a Question" OnClick="btnToggleNewQuestion_Click" CssClass="btn btn-primary"/>
        </div>

        <!-- New Question Form -->
        <asp:Panel ID="pnlNewQuestionForm" runat="server" Visible="false" CssClass="glass-card" style="margin-bottom: 2rem;">
            <h3 style="font-family: var(--font-heading); margin-bottom: 1rem;">Post New Question to Community</h3>
            <div style="display: flex; flex-direction: column; gap: 1rem;">
                <asp:TextBox ID="txtForumTitle" runat="server" CssClass="form-control" placeholder="Question Title (e.g. Speed limit on highways)" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
                <%--
                <asp:DropDownList ID="ddlForumCategory" runat="server" CssClass="form-control" style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;">
                    <asp:ListItem Value="Rules & Safety">Rules & Safety</asp:ListItem>
                    <asp:ListItem Value="Road Signs">Road Signs</asp:ListItem>
                    <asp:ListItem Value="Vehicle Checks">Vehicle Checks</asp:ListItem>
                </asp:DropDownList>
                --%>
                <asp:TextBox ID="txtForumContent" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" placeholder="Detailed description of your question..." style="width: 100%; padding: 0.75rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
                <asp:Button ID="btnPostQuestion" runat="server" Text="Post Question" OnClick="btnPostQuestion_Click" CssClass="btn btn-primary" style="align-self: flex-end;" />
            </div>
        </asp:Panel>

        <!-- Forum Threads Repeater -->
        <div style="display: flex; flex-direction: column; gap: 1.5rem;">
            <asp:Repeater ID="rptForum" runat="server" OnItemCommand="rptForum_ItemCommand" OnItemDataBound="rptForum_ItemDataBound">
                <ItemTemplate>
                    <div class="glass-card">
                        <div style="display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 0.75rem;">
                            <div>
                                <%-- 
                                <span class="badge" style="background: rgba(99, 102, 241, 0.2); color: var(--primary); margin-bottom: 0.5rem; display: inline-block;">Rules & Safety</span>
                                --%>
                                <h3 style="font-family: var(--font-heading); margin: 0;"><%# Eval("Title") %></h3>
                            </div>
                            <span style="font-size: 0.85rem; color: var(--text-secondary);"><%# Eval("CreatedAt", "{0:g}") %></span>
                        </div>

                        <p style="color: var(--text-secondary); line-height: 1.6; margin-bottom: 1.5rem;"><%# Eval("Content") %></p>

                        <div style="display: flex; align-items: center; justify-content: space-between; background: rgba(15, 23, 42, 0.4); padding: 0.75rem 1rem; border-radius: var(--radius-sm);">
                            <div style="display: flex; align-items: center; gap: 0.5rem;">
                                <span><%# Eval("AuthorAvatar") %></span>
                                <span style="font-weight: 600; font-size: 0.9rem;"><%# Eval("AuthorName") %></span>
                                <span class="badge" style="font-size: 0.75rem;"><%# Eval("AuthorRole") %></span>
                            </div>

                            <div style="display: flex; align-items: center; gap: 0.75rem;">
                                <button type="button" class="btn-upvote-ui" onclick="toggleUpvote(this)" style="background: rgba(234, 179, 8, 0.1); border: 1px solid var(--warning); color: var(--warning); font-weight: 700; border-radius: 20px; padding: 0.4rem 0.9rem; cursor: pointer; display: inline-flex; align-items: center; gap: 0.4rem; font-size: 0.85rem; transition: all 0.25s ease;">
                                    <span>👍</span>
                                    <span class="upvote-status">Upvote</span>
                                    (<span class="upvote-count"><%# Eval("Likes") %></span>)
                                </button>
                                <asp:Button ID="btnDeleteThread" runat="server" Text="🗑️ Delete Thread" CommandName="DeleteThread" CommandArgument='<%# Eval("Id") %>' Visible='<%# IsAdmin %>' CssClass="btn btn-secondary btn-sm" OnClientClick="return confirm('Delete this question thread?');" style="border-color: var(--danger); color: var(--danger);" />
                            </div>
                        </div>

                        <!-- Thread Replies -->
                        <asp:Repeater ID="rptReplies" runat="server" OnItemCommand="rptReplies_ItemCommand" OnItemDataBound="rptReplies_ItemDataBound">
                            <HeaderTemplate>
                                <div style="margin-top: 1rem; padding-left: 1.5rem; border-left: 2px solid rgba(255,255,255,0.1);">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <!-- Instructor State Reply -->
                                <asp:PlaceHolder ID="phEducatorReply" runat="server" Visible="false">
                                    <div style="margin-bottom: 1rem; padding: 0.75rem; background: rgba(16, 185, 129, 0.1); border-radius: var(--radius-sm); border: 1px solid var(--success); display: flex; justify-content: space-between; align-items: flex-start;">
                                        <div style="flex: 1;">
                                            <div style="display: flex; justify-content: space-between; margin-bottom: 0.25rem;">
                                                <span style="font-weight: 700; font-size: 0.85rem; color: var(--success);">
                                                    <%# Eval("AuthorAvatar") %> <%# Eval("AuthorName") %> ✔ (Instructor Verified Answer)
                                                </span>
                                                <span style="font-size: 0.75rem; color: var(--text-secondary);"><%# Eval("CreatedAt", "{0:g}") %></span>
                                            </div>
                                            <p style="margin: 0; font-size: 0.9rem; line-height: 1.4;"><%# Eval("Content") %></p>
                                        </div>
                                        <asp:Button ID="btnDeleteReply1" runat="server" Text="🗑️ Delete" CommandName="DeleteReply" CommandArgument='<%# Eval("Id") %>' Visible='<%# IsAdmin %>' CssClass="btn btn-secondary btn-sm" OnClientClick="return confirm('Delete this comment?');" style="margin-left: 0.5rem; border-color: var(--danger); color: var(--danger); padding: 0.2rem 0.5rem; font-size: 0.75rem;" />
                                    </div>
                                </asp:PlaceHolder>

                                <!-- Standard State Reply -->
                                <asp:PlaceHolder ID="phStandardReply" runat="server" Visible="false">
                                    <div style="margin-bottom: 1rem; padding: 0.75rem; background: rgba(15, 23, 42, 0.3); border-radius: var(--radius-sm); border: none; display: flex; justify-content: space-between; align-items: flex-start;">
                                        <div style="flex: 1;">
                                            <div style="display: flex; justify-content: space-between; margin-bottom: 0.25rem;">
                                                <span style="font-weight: 700; font-size: 0.85rem;">
                                                    <%# Eval("AuthorAvatar") %> <%# Eval("AuthorName") %>
                                                </span>
                                                <span style="font-size: 0.75rem; color: var(--text-secondary);"><%# Eval("CreatedAt", "{0:g}") %></span>
                                            </div>
                                            <p style="margin: 0; font-size: 0.9rem; line-height: 1.4;"><%# Eval("Content") %></p>
                                        </div>
                                        <asp:Button ID="btnDeleteReply2" runat="server" Text="🗑️ Delete" CommandName="DeleteReply" CommandArgument='<%# Eval("Id") %>' Visible='<%# IsAdmin %>' CssClass="btn btn-secondary btn-sm" OnClientClick="return confirm('Delete this comment?');" style="margin-left: 0.5rem; border-color: var(--danger); color: var(--danger); padding: 0.2rem 0.5rem; font-size: 0.75rem;" />
                                    </div>
                                </asp:PlaceHolder>
                            </ItemTemplate>
                            <FooterTemplate>
                                </div>
                            </FooterTemplate>
                        </asp:Repeater>

                        <!-- Candidate Comment Reply Box -->
                        <div style="margin-top: 1rem; display: flex; gap: 0.5rem;">
                            <asp:TextBox ID="txtCandidateReply" runat="server" CssClass="form-control" placeholder="Write a comment under this thread..." style="flex: 1; padding: 0.5rem; font-size: 0.85rem; border-radius: var(--radius-sm); border: 1px solid rgba(255,255,255,0.1); background: rgba(15, 23, 42, 0.6); color: white;" />
                            <asp:Button ID="btnPostReply" runat="server" Text="Reply" CommandName="ReplyThread" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-secondary btn-sm" />
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </asp:Panel>
</asp:Content>
