# DriveLingo (JPJ Hero) - Driving Theory Exam Portal

DriveLingo is an interactive web-based portal designed for Malaysian driving license candidates preparing for the **JPJ KPP01 Theory Exam**. It features a modern gaming-inspired dark theme, dynamic single-page routing (SPA), persistent browser state management, and customized workflows for **4 roles**: Learner, Educator, Admin, and Guest.

---

## 🚀 Quick Start Guide (For Teammates)

### Option 1: Open Directly in Browser
Since DriveLingo is built using pure **HTML5, ES6 JavaScript Modules, and Vanilla CSS**, you can run it without installing any heavy build tools or bundlers.

1. Extract the `.zip` file to any folder on your computer.
2. Serve the folder using any simple HTTP server (required for ES6 module loading):
   - **Using Python**: Open terminal in the unzipped directory and run:
     ```bash
     python -m http.server 8080
     ```
   - **Using VS Code**: Right-click `index.html` and select **Open with Live Server**.
   - **Using Node.js**:
     ```bash
     npx http-server ./ -p 8080
     ```
   - **Using IIS / IIS Express (Windows)**: Host the folder directly (includes `Web.config`).
3. Open your browser and go to: `http://localhost:8080` (or `http://127.0.0.1:5500` for Live Server).

---

## 🔑 Pre-Seeded Quick Login Accounts

On the Sign-In screen, click the **Developer Simulation Quick Login** buttons at the bottom to test any role instantly:

| Role | Email | Password | Primary Capabilities |
| :--- | :--- | :--- | :--- |
| **Learner** | `learner@drivelingo.com` | `learner123` | Read lessons, attempt JPJ Mock Exam with countdown timer & full answer explanations, collect points, redeem store rewards, and unlock achievements. |
| **Educator** | `educator@drivelingo.com` | `educator123` | Create & edit quizzes with custom question pools, view student performance SVG charts, and answer student forum questions with an Instructor badge. |
| **Admin** | `admin@drivelingo.com` | `admin123` | Promote/demote accounts in the User Matrix, curate driving syllabus chapters, tune global simulation timer/passing grade settings, and edit shop items. |

---

## 📁 Project File Structure

```text
WAPP.net/
├── index.html          # Main application wrapper layout & script mounts
├── Web.config          # IIS / IIS Express configuration file
├── README.md           # Instructions for team members
├── css/
│   └── styles.css      # Design system, CSS variables, dark theme & animations
└── js/
    ├── app.js          # Core application coordinator & modal handlers
    ├── router.js       # Client-side hash router with role-based guards
    ├── state.js        # Central state manager (localStorage + default mock data)
    └── views/
        ├── admin.js    # Director panel, user matrix, simulation config
        ├── auth.js     # Login & Registration views + Developer quick-switch
        ├── educator.js # Educator quiz builder, SVG performance charts, forum answers
        ├── learner.js  # Learner dashboard, lessons hub, exam engine, store, achievements
        └── profile.js  # User settings, avatar customizer, and system reset
```

---

## 🛠️ Data Management & Reset

- **Live Database**: Stored in the browser's `localStorage` (`drivelingo_app_state`).
- **Inspect DB in Console**: Open browser Developer Tools (`F12`), go to **Console**, and type `StateManager.state`.
- **Reset to Default Mockup Data**: Go to **Profile Settings** -> click **Reset App to Defaults** (or call `StateManager.resetAllData()` in console).
