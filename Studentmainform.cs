using System;
using System.Windows.Forms;

namespace ONLINE_SYSTEM
{
    public partial class Studentmainform : Form
    {
        private ContextMenuStrip profileMenu;
        private readonly string studentEmail;

        public Studentmainform(string email)
        {
            InitializeComponent();

            studentEmail = email;
            SetupProfileMenu();
            SetupDashboardUI();

            SessionManager.StartTracking();
            SessionManager.OnSessionTimeout += HandleSessionTimeout;

            this.MouseMove += ResetActivity;
            this.KeyPress += ResetActivity;
        }

        private void ResetActivity(object sender, EventArgs e)
        {
            SessionManager.RefreshActivity();
        }

        private void HandleSessionTimeout()
        {
            MessageBox.Show("You have been logged out due to inactivity.", "Session Timeout", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Invoke((MethodInvoker)delegate
            {
                this.Hide();
                SessionManager.StopTracking();
                FirstPage firstPage = new FirstPage();
                firstPage.Show();
                this.Close();
            });
        }

        // ---------------- Profile Menu ----------------
        private void SetupProfileMenu()
        {
            profileMenu = new ContextMenuStrip();

            ToolStripMenuItem changePasswordItem = new ToolStripMenuItem("Change Password");
            changePasswordItem.Click += ChangePassword_Click;
            profileMenu.Items.Add(changePasswordItem);

            ToolStripMenuItem logoutItem = new ToolStripMenuItem("Logout");
            logoutItem.Click += Logout_Click;
            profileMenu.Items.Add(logoutItem);

            btn_profile.Click += (s, e) =>
            {
                profileMenu.Show(btn_profile, 0, btn_profile.Height);
            };
        }

        private void ChangePassword_Click(object sender, EventArgs e)
        {
            using (ChangePasswordForm changeForm = new ChangePasswordForm(studentEmail, "Students"))
            {
                this.Hide();
                changeForm.ShowDialog();
                this.Close();
            }
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?",
                                                  "Logout",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
                NavigateToLogin();
        }

        private void NavigateToLogin()
        {
            this.Hide();
            using (StudentLogin loginForm = new StudentLogin())
            {
                loginForm.ShowDialog();
            }
            this.Close();
        }

        private void btn_profile_Click(object sender, EventArgs e) { }

        // ---------------- Dashboard UI ----------------
        private Panel panelSidebar;
        private Panel panelContent;
        private Button btnHome;
        private Button btnProfessor;
        private Button btnReservation;
        private Button btnCalendar;
        private Label lblHeader;

        private void SetupDashboardUI()
        {
            // Sidebar
            panelSidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 180,
                BackColor = System.Drawing.Color.FromArgb(40, 40, 60)
            };
            this.Controls.Add(panelSidebar);

            // Header
            lblHeader = new Label
            {
                Text = "Student Dashboard",
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                BackColor = System.Drawing.Color.FromArgb(50, 50, 70)
            };
            this.Controls.Add(lblHeader);

            // Content Panel
            panelContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = System.Drawing.Color.White
            };
            this.Controls.Add(panelContent);

            // Buttons
            btnHome = new Button
            {
                Text = "Home",
                Dock = DockStyle.Top,
                Height = 50,
                FlatStyle = FlatStyle.Flat,
                ForeColor = System.Drawing.Color.White
            };
            btnHome.FlatAppearance.BorderSize = 0;
            btnHome.Click += (s, e) => LoadPage("Home");

            btnProfessor = new Button
            {
                Text = "Professor",
                Dock = DockStyle.Top,
                Height = 50,
                FlatStyle = FlatStyle.Flat,
                ForeColor = System.Drawing.Color.White
            };
            btnProfessor.FlatAppearance.BorderSize = 0;
            btnProfessor.Click += (s, e) => LoadPage("Professor");

            btnReservation = new Button
            {
                Text = "Reservation",
                Dock = DockStyle.Top,
                Height = 50,
                FlatStyle = FlatStyle.Flat,
                ForeColor = System.Drawing.Color.White
            };
            btnReservation.FlatAppearance.BorderSize = 0;
            btnReservation.Click += (s, e) => LoadPage("Reservation");

            btnCalendar = new Button
            {
                Text = "Calendar",
                Dock = DockStyle.Top,
                Height = 50,
                FlatStyle = FlatStyle.Flat,
                ForeColor = System.Drawing.Color.White
            };
            btnCalendar.FlatAppearance.BorderSize = 0;
            btnCalendar.Click += (s, e) => LoadPage("Calendar");

            // Add buttons to sidebar
            panelSidebar.Controls.Add(btnCalendar);
            panelSidebar.Controls.Add(btnReservation);
            panelSidebar.Controls.Add(btnProfessor);
            panelSidebar.Controls.Add(btnHome);
        }

        // ---------------- Load Page Content ----------------
        private void LoadPage(string pageName)
        {
            panelContent.Controls.Clear();

            Label lbl = new Label
            {
                Text = $"{pageName} Page",
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Font = new System.Drawing.Font("Segoe UI", 18, System.Drawing.FontStyle.Bold)
            };

            panelContent.Controls.Add(lbl);
        }
    }
}
