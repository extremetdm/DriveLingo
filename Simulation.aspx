<%@ Page Title="DriveLingo | Simulation" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Simulation.aspx.cs" Inherits="DriveLingo.Simulation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <!-- Notification Banner -->
  <asp:Panel ID="pnlNotification" runat="server" Visible="false" CssClass="glass-card" style="margin-bottom: 1.5rem; border-left: 4px solid var(--success); padding: 1rem;">
    <asp:Literal ID="litNotificationText" runat="server" />
  </asp:Panel>

  <!-- TAB 6: SIMULATION -->
  <asp:Panel ID="pnlSimulation" runat="server" Visible="false">
    <div class="glass-card" style="margin-bottom: 2rem; padding: 2.5rem 1.5rem;">
      <div style="text-align: center; max-width: 800px; margin: 0 auto;">
        <span style="font-size: 4rem; display: block; margin-bottom: 1rem;">🏎️</span>
        <h2 style="font-size: 2.2rem; font-family: var(--font-heading); margin-bottom: 0.5rem;">Official JPJ KPP01 Full Simulation Test</h2>
        <p style="color: var(--text-secondary); font-size: 1.05rem; margin-bottom: 2rem; line-height: 1.6;">
          Official 75-minute examination simulation. Questions are randomly selected from a dedicated 180+ question bank across all 4 mandatory JPJ test sections.
        </p>

        <div style="display: grid; grid-template-columns: repeat(auto-fit, minmax(180px, 1fr)); gap: 1rem; margin-bottom: 2rem; text-align: left;">
          <div style="background: rgba(15, 23, 42, 0.5); padding: 1rem; border-radius: var(--radius-md); border: 1px solid rgba(255,255,255,0.08);">
            <div style="font-weight: 700; color: var(--primary); font-size: 0.95rem; margin-bottom: 0.25rem;">👁️ Color Blindness</div>
            <div style="font-size: 1.1rem; font-weight: 700;">8 Questions</div>
            <span class="badge" style="background: rgba(239, 68, 68, 0.2); color: var(--danger); font-size: 0.75rem; margin-top: 0.25rem; display: inline-block;">Must get 8/8 (100%)</span>
          </div>

          <div style="background: rgba(15, 23, 42, 0.5); padding: 1rem; border-radius: var(--radius-md); border: 1px solid rgba(255,255,255,0.08);">
            <div style="font-weight: 700; color: var(--warning); font-size: 0.95rem; margin-bottom: 0.25rem;">🛑 Section A: Signs</div>
            <div style="font-size: 1.1rem; font-weight: 700;">21 Questions</div>
            <span class="badge" style="background: rgba(245, 158, 11, 0.2); color: var(--warning); font-size: 0.75rem; margin-top: 0.25rem; display: inline-block;">Pass: 17 / 21</span>
          </div>

          <div style="background: rgba(15, 23, 42, 0.5); padding: 1rem; border-radius: var(--radius-md); border: 1px solid rgba(255,255,255,0.08);">
            <div style="font-weight: 700; color: var(--success); font-size: 0.95rem; margin-bottom: 0.25rem;">🛣️ Section B: Rules</div>
            <div style="font-size: 1.1rem; font-weight: 700;">35 Questions</div>
            <span class="badge" style="background: rgba(16, 185, 129, 0.2); color: var(--success); font-size: 0.75rem; margin-top: 0.25rem; display: inline-block;">Pass: 28 / 35</span>
          </div>

          <div style="background: rgba(15, 23, 42, 0.5); padding: 1rem; border-radius: var(--radius-md); border: 1px solid rgba(255,255,255,0.08);">
            <div style="font-weight: 700; color: #a855f7; font-size: 0.95rem; margin-bottom: 0.25rem;">⚠️ Section C: KEJARA</div>
            <div style="font-size: 1.1rem; font-weight: 700;">14 Questions</div>
            <span class="badge" style="background: rgba(168, 85, 247, 0.2); color: #a855f7; font-size: 0.75rem; margin-top: 0.25rem; display: inline-block;">Pass: 11 / 14</span>
          </div>
        </div>

        <div style="background: rgba(99, 102, 241, 0.1); border: 1px solid rgba(99, 102, 241, 0.3); padding: 1rem 1.5rem; border-radius: var(--radius-md); margin-bottom: 2rem; font-size: 0.95rem; color: var(--text-secondary);">
          ⏱️ <strong>Total Format:</strong> 78 Questions | 75 Minutes Time Limit | <strong>80% Overall Passing Mark</strong> + All Sectional Criteria Required.
        </div>

        <asp:Button ID="btnStartFullSim" runat="server" Text="🚀 Launch 78-Question JPJ Simulation Test" OnClick="btnStartFullSim_Click" CssClass="btn btn-primary" style="font-size: 1.1rem; padding: 0.85rem 2.5rem; font-weight: 700;" />
      </div>
    </div>
  </asp:Panel>


      <!-- Active Exam Interface -->
  <asp:Panel ID="pnlActiveExam" runat="server" Visible="false" CssClass="glass-card">
    <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 2rem; padding-bottom: 1rem; border-bottom: 1px solid rgba(255,255,255,0.1);">
      <h2 style="font-family: var(--font-heading); margin: 0;">
        <asp:Literal ID="litExamTitle" runat="server" />
      </h2>
      <span class="badge" style="background: rgba(16, 185, 129, 0.2); color: var(--success); font-weight: 700; font-size: 1rem;">
        ⏱️ Timed Mode Active
      </span>
    </div>

    <asp:Repeater ID="rptQuestions" runat="server" OnItemDataBound="rptQuestions_ItemDataBound">
      <ItemTemplate>
        <div style="margin-bottom: 2rem; padding: 1.5rem; background: rgba(15, 23, 42, 0.4); border-radius: var(--radius-md); border: 1px solid rgba(255,255,255,0.05);">
          <h4 style="font-size: 1.1rem; margin-bottom: 1rem; line-height: 1.5;">
            Question <%# Container.ItemIndex + 1 %>: <%# Eval("Text") %>
          </h4>


          <asp:Panel ID="pnlImageWrapper" runat="server" 
            CssClass="sign-image-wrapper"
            Visible='<%# Eval("Image") != null && !string.IsNullOrEmpty(Eval("Image").ToString()) %>'>
    
            <asp:Image ID="imgRoadSign" runat="server" 
                ImageUrl='<%# Eval("Image") %>' 
                AlternateText="Road Sign Visual" 
                CssClass="sign-image" />
        
            <span class="sign-caption">🚦 Road Sign Reference Visual</span>
        </asp:Panel>

          <asp:HiddenField ID="hfQuestionId" runat="server" Value='<%# Eval("Id") %>' />
          
          <asp:RadioButtonList ID="rblOptions" runat="server" CssClass="radio-options" style="width: 100%;">
          </asp:RadioButtonList>
        </div>
      </ItemTemplate>
    </asp:Repeater>

    <div style="display: flex; justify-content: flex-end; gap: 1rem;">
      <asp:Button ID="btnCancelExam" runat="server" Text="Cancel" OnClick="btnCancelExam_Click" CssClass="btn btn-secondary" />
      <asp:Button ID="btnSubmitExam" runat="server" Text="Submit & Grade Exam" OnClick="btnSubmitExam_Click" CssClass="btn btn-primary" style="padding: 0.75rem 2rem; font-weight: 700;" />
    </div>
  </asp:Panel>

  <!-- Exam Score Results Panel -->
  <asp:Panel ID="pnlExamResult" runat="server" Visible="false" CssClass="glass-card" style="text-align: center; padding: 3rem 1.5rem;">
    <span style="font-size: 4rem; display: block; margin-bottom: 1rem;">
      <asp:Literal ID="litResultIcon" runat="server" Text="🎉" />
    </span>
    <h2 style="font-size: 2.2rem; font-family: var(--font-heading); margin-bottom: 0.5rem;">
      <asp:Literal ID="litResultHeader" runat="server" Text="Test Passed!" />
    </h2>
    <p style="color: var(--text-secondary); font-size: 1.1rem; margin-bottom: 1.5rem;">
      Score: <strong style="color: var(--primary);"><asp:Literal ID="litResultScore" runat="server" /></strong> | 
      Percentage: <strong><asp:Literal ID="litResultPercentage" runat="server" />%</strong>
    </p>

    <asp:Panel ID="pnlResultBonus" runat="server" CssClass="badge" style="background: rgba(245, 158, 11, 0.2); color: var(--warning); font-size: 1.1rem; padding: 0.75rem 1.5rem; margin-bottom: 2rem; display: inline-block;">
      🏆 Awarded +<asp:Literal ID="litAwardedPoints" runat="server" /> Points & +<asp:Literal ID="litAwardedXP" runat="server" /> XP!
    </asp:Panel>

    <!-- Simulation Sectional Score Breakdown Grid -->
    <asp:Panel ID="pnlSimBreakdown" runat="server" Visible="false" style="margin: 2rem 0; text-align: left;">
      <h3 style="font-family: var(--font-heading); margin-bottom: 1rem; text-align: center;">📊 Official JPJ Sectional Criteria Performance</h3>
      <div style="display: grid; grid-template-columns: repeat(auto-fit, minmax(220px, 1fr)); gap: 1rem;">
        <div style="background: rgba(15, 23, 42, 0.6); padding: 1.25rem; border-radius: var(--radius-md); border: 1px solid rgba(255,255,255,0.1);">
          <div style="font-size: 0.85rem; color: var(--text-secondary); margin-bottom: 0.25rem;">👁️ Color Blindness Test</div>
          <div style="font-size: 1.5rem; font-weight: 700; margin-bottom: 0.25rem;">
            <asp:Literal ID="litSimCbScore" runat="server" Text="8 / 8" />
          </div>
          <div style="font-size: 0.8rem; color: var(--text-secondary);">Required: 8/8 (100%)</div>
          <div style="margin-top: 0.5rem;">
            <asp:Label ID="lblSimCbStatus" runat="server" CssClass="badge badge-success" Text="PASS 🟢" />
          </div>
        </div>

        <div style="background: rgba(15, 23, 42, 0.6); padding: 1.25rem; border-radius: var(--radius-md); border: 1px solid rgba(255,255,255,0.1);">
          <div style="font-size: 0.85rem; color: var(--text-secondary); margin-bottom: 0.25rem;">🛑 Section A - Road Signs</div>
          <div style="font-size: 1.5rem; font-weight: 700; margin-bottom: 0.25rem;">
            <asp:Literal ID="litSimSecAScore" runat="server" Text="19 / 21" />
          </div>
          <div style="font-size: 0.8rem; color: var(--text-secondary);">Required: 17/21</div>
          <div style="margin-top: 0.5rem;">
            <asp:Label ID="lblSimSecAStatus" runat="server" CssClass="badge badge-success" Text="PASS 🟢" />
          </div>
        </div>

        <div style="background: rgba(15, 23, 42, 0.6); padding: 1.25rem; border-radius: var(--radius-md); border: 1px solid rgba(255,255,255,0.1);">
          <div style="font-size: 0.85rem; color: var(--text-secondary); margin-bottom: 0.25rem;">🛣️ Section B - Rules of the Road</div>
          <div style="font-size: 1.5rem; font-weight: 700; margin-bottom: 0.25rem;">
            <asp:Literal ID="litSimSecBScore" runat="server" Text="30 / 35" />
          </div>
          <div style="font-size: 0.8rem; color: var(--text-secondary);">Required: 28/35</div>
          <div style="margin-top: 0.5rem;">
            <asp:Label ID="lblSimSecBStatus" runat="server" CssClass="badge badge-success" Text="PASS 🟢" />
          </div>
        </div>

        <div style="background: rgba(15, 23, 42, 0.6); padding: 1.25rem; border-radius: var(--radius-md); border: 1px solid rgba(255,255,255,0.1);">
          <div style="font-size: 0.85rem; color: var(--text-secondary); margin-bottom: 0.25rem;">⚠️ Section C - KEJARA & Safety</div>
          <div style="font-size: 1.5rem; font-weight: 700; margin-bottom: 0.25rem;">
            <asp:Literal ID="litSimSecCScore" runat="server" Text="12 / 14" />
          </div>
          <div style="font-size: 0.8rem; color: var(--text-secondary);">Required: 11/14</div>
          <div style="margin-top: 0.5rem;">
            <asp:Label ID="lblSimSecCStatus" runat="server" CssClass="badge badge-success" Text="PASS 🟢" />
          </div>
        </div>
      </div>
    </asp:Panel>

    <div style="display: flex; justify-content: center; gap: 1rem; margin-top: 1rem;">
      <asp:Button ID="btnBackToQuizzes" runat="server" Text="Back to Quiz Directory" OnClick="btnBackToQuizzes_Click" CssClass="btn btn-primary" />
    </div>
  </asp:Panel>
</asp:Panel>
</asp:Content>
