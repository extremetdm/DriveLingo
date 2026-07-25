<%@ Page Title="DriveLingo | Malaysian JPJ Driving Theory Exam Simulator" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DriveLingo.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .hero-banner {
            position: relative;
            padding: 4rem 2rem;
            border-radius: var(--radius-lg);
            background: linear-gradient(135deg, rgba(30, 41, 59, 0.8), rgba(15, 23, 42, 0.9)), url('uploads/expressway_sign.svg') center/cover no-repeat;
            border: 1px solid rgba(255, 255, 255, 0.12);
            box-shadow: var(--shadow-glass);
            text-align: center;
            overflow: hidden;
            margin-bottom: 2.5rem;
        }
        .hero-title {
            font-size: 3rem;
            font-family: var(--font-heading);
            font-weight: 800;
            background: linear-gradient(135deg, #ffffff, #a5b4fc);
            -webkit-background-clip: text;
            -webkit-text-fill-color: transparent;
            margin-bottom: 1.25rem;
            line-height: 1.2;
        }
        .hero-subtitle {
            color: var(--text-secondary);
            max-width: 760px;
            margin: 0 auto 2.5rem auto;
            font-size: 1.15rem;
            line-height: 1.7;
        }
        .feature-card {
            background: rgba(15, 23, 42, 0.6);
            border: 1px solid rgba(255, 255, 255, 0.08);
            border-radius: var(--radius-md);
            padding: 1.75rem;
            transition: all 0.3s ease;
            display: flex;
            flex-direction: column;
            align-items: flex-start;
        }
        .feature-card:hover {
            transform: translateY(-5px);
            border-color: var(--primary);
            box-shadow: 0 12px 30px rgba(99, 102, 241, 0.2);
        }
        .feature-icon-wrapper {
            width: 60px;
            height: 60px;
            border-radius: 16px;
            background: rgba(99, 102, 241, 0.15);
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 2rem;
            margin-bottom: 1.25rem;
            border: 1px solid rgba(99, 102, 241, 0.3);
        }
        .cta-btn-group {
            display: flex;
            justify-content: center;
            gap: 1rem;
            flex-wrap: wrap;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- HERO SECTION: INTRODUCTION TO DRIVELINGO -->
    <div class="hero-banner">
        <div style="font-size: 4rem; margin-bottom: 1rem;">🚗⚡</div>
        <h1 class="hero-title">Master the Malaysian JPJ Driving Theory Exam</h1>
        <p class="hero-subtitle">
            Welcome to <strong>DriveLingo</strong> — Malaysia's premier AI-assisted KPP01 Driving Theory Exam simulator. Practice official road sign regulations, test your vision with Ishihara plates, challenge timed 78-question exam simulations, and earn rewards as you progress.
        </p>

        <!-- Call To Action Button Bar -->
        <div class="cta-btn-group">
            <asp:HyperLink ID="lnkSignIn" runat="server" NavigateUrl="~/Login.aspx" CssClass="btn btn-primary" style="font-size: 1.1rem; padding: 0.9rem 2.2rem; font-weight: 700;">
                🔑 Sign In / Log In
            </asp:HyperLink>

            <asp:HyperLink ID="lnkRegister" runat="server" NavigateUrl="~/Register.aspx" CssClass="btn btn-secondary" style="font-size: 1.1rem; padding: 0.9rem 2.2rem; font-weight: 700;">
                📝 Create Account
            </asp:HyperLink>

            <asp:Button ID="btnContinueGuest" runat="server" Text="🔍 Continue as Guest (Limited Access)" OnClick="btnContinueGuest_Click" CssClass="btn btn-secondary" style="font-size: 1.1rem; padding: 0.9rem 2rem; border-color: rgba(255,255,255,0.2);" />
        </div>
    </div>

    <!-- SECTION: JPJ CURRICULUM SECTIONS OVERVIEW -->
    <div style="margin-bottom: 3rem;">
        <h2 style="font-family: var(--font-heading); text-align: center; font-size: 2rem; margin-bottom: 0.5rem;">
            📚 Complete JPJ KPP01 Test Syllabus
        </h2>
        <p style="color: var(--text-secondary); text-align: center; max-width: 600px; margin: 0 auto 2rem auto;">
            Our practice modules cover all four core pillars of the official Malaysian Road Transport Department examination.
        </p>

        <div class="grid-2-col" style="gap: 1.5rem;">
            <div class="feature-card">
                <div class="feature-icon-wrapper" style="background: rgba(239, 68, 68, 0.15); border-color: rgba(239, 68, 68, 0.3);">🛑</div>
                <h3 style="font-family: var(--font-heading); font-size: 1.3rem; margin-bottom: 0.5rem;">Section A - Road Signs</h3>
                <p style="color: var(--text-secondary); line-height: 1.6; font-size: 0.95rem; margin-bottom: 1rem;">
                    Master prohibitory circular signs, yellow diamond danger warnings, green expressway indicators, and municipal blue route badges with high-resolution visual sign recognition.
                </p>
                <span class="badge" style="background: rgba(239, 68, 68, 0.2); color: #f87171;">Prohibitory & Mandatory Signs</span>
            </div>

            <div class="feature-card">
                <div class="feature-icon-wrapper" style="background: rgba(59, 130, 246, 0.15); border-color: rgba(59, 130, 246, 0.3);">🚗</div>
                <h3 style="font-family: var(--font-heading); font-size: 1.3rem; margin-bottom: 0.5rem;">Section B - Rules of the Road</h3>
                <p style="color: var(--text-secondary); line-height: 1.6; font-size: 0.95rem; margin-bottom: 1rem;">
                    Learn legal speed limits (Expressway 110 km/h, Federal 90 km/h, Town 60 km/h, School 30 km/h), right-of-way priorities, lane discipline, and overtaking regulations.
                </p>
                <span class="badge" style="background: rgba(59, 130, 246, 0.2); color: #60a5fa;">Traffic Laws & Speed Limits</span>
            </div>

            <div class="feature-card">
                <div class="feature-icon-wrapper" style="background: rgba(16, 185, 129, 0.15); border-color: rgba(16, 185, 129, 0.3);">🚦</div>
                <h3 style="font-family: var(--font-heading); font-size: 1.3rem; margin-bottom: 0.5rem;">Section C - KEJARA & Safety</h3>
                <p style="color: var(--text-secondary); line-height: 1.6; font-size: 0.95rem; margin-bottom: 1rem;">
                    Understand Malaysian KEJARA demerit point penalties, blood alcohol content thresholds, seatbelt requirements, vehicle safety checks, and emergency response.
                </p>
                <span class="badge" style="background: rgba(16, 185, 129, 0.2); color: #34d399;">Demerit System & Road Safety</span>
            </div>

            <div class="feature-card">
                <div class="feature-icon-wrapper" style="background: rgba(245, 158, 11, 0.15); border-color: rgba(245, 158, 11, 0.3);">👁️</div>
                <h3 style="font-family: var(--font-heading); font-size: 1.3rem; margin-bottom: 0.5rem;">Color Blind Screening</h3>
                <p style="color: var(--text-secondary); line-height: 1.6; font-size: 0.95rem; margin-bottom: 1rem;">
                    Prepare for the obligatory JPJ candidate eye test using authentic Ishihara color vision plates before sitting for your official computer test.
                </p>
                <span class="badge" style="background: rgba(245, 158, 11, 0.2); color: #fbbf24;">Ishihara Vision Screening</span>
            </div>
        </div>
    </div>

    <!-- SECTION: ROLE PORTALS QUICK LAUNCHERS -->
    <div style="margin-bottom: 2rem;">
        <h2 style="font-family: var(--font-heading); text-align: center; font-size: 1.8rem; margin-bottom: 1.5rem;">
            🚀 Portal Access Shortcuts
        </h2>

        <div class="grid-3-col">
            <div class="glass-card" style="text-align: center;">
                <span style="font-size: 3rem; display: block; margin-bottom: 0.75rem;">🚘</span>
                <h3 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">Candidate Practice</h3>
                <p style="color: var(--text-secondary); font-size: 0.9rem; margin-bottom: 1.5rem;">
                    Take practice tests, unlock achievements, earn XP & points, and redeem custom avatar cosmetics.
                </p>
                <asp:Button ID="btnQuickLearner" runat="server" Text="Log in as Candidate" OnClick="btnQuickLearner_Click" CssClass="btn btn-primary" style="width: 100%; font-weight: 700;" />
            </div>

            <div class="glass-card" style="text-align: center;">
                <span style="font-size: 3rem; display: block; margin-bottom: 0.75rem;">👨‍✈️</span>
                <h3 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">Educator Hub</h3>
                <p style="color: var(--text-secondary); font-size: 0.9rem; margin-bottom: 1.5rem;">
                    Create practice quizzes under curriculum modules, author question banks, and answer candidate questions.
                </p>
                <asp:Button ID="btnQuickEducator" runat="server" Text="Log in as Educator" OnClick="btnQuickEducator_Click" CssClass="btn btn-secondary" style="width: 100%; font-weight: 700;" />
            </div>

            <div class="glass-card" style="text-align: center;">
                <span style="font-size: 3rem; display: block; margin-bottom: 0.75rem;">👑</span>
                <h3 style="font-family: var(--font-heading); margin-bottom: 0.5rem;">Admin Operations</h3>
                <p style="color: var(--text-secondary); font-size: 0.9rem; margin-bottom: 1.5rem;">
                    Manage curriculum modules dynamically, configure points per question rates, and oversee platform users.
                </p>
                <asp:Button ID="btnQuickAdmin" runat="server" Text="Log in as Admin" OnClick="btnQuickAdmin_Click" CssClass="btn btn-secondary" style="width: 100%; font-weight: 700;" />
            </div>
        </div>
    </div>
</asp:Content>
