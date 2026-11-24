
using System.Drawing;
using System.Windows.Forms;

namespace NAOTR_POS.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlSidebar;
        private Panel pnlHeader;
        private Panel pnlContent;
        private Panel pnlWorkspace;

        private Panel pnlBrand;
        private Panel pnlNavigation;
        private Panel pnlUserInfo;

        private Label lblBrand;
        private Label lblSubtitle;
        private Label lblPageTitle;
        private Label lblUser;
        private Label lblRole;

        private Button btnDashboard;
        private Button btnPOS;
        private Button btnInventory;
        private Button btnCategories;
        private Button btnCustomers;
        private Button btnSalesHistory;
        private Button btnReports;
        private Button btnUsers;
        private Button btnLogout;
        private Button btnToggle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlSidebar = new Panel();
            pnlHeader = new Panel();
            pnlContent = new Panel();
            pnlWorkspace = new Panel();

            pnlBrand = new Panel();
            pnlNavigation = new Panel();
            pnlUserInfo = new Panel();

            lblBrand = new Label();
            lblSubtitle = new Label();
            lblPageTitle = new Label();
            lblUser = new Label();
            lblRole = new Label();

            btnDashboard = new Button();
            btnPOS = new Button();
            btnInventory = new Button();
            btnCategories = new Button();
            btnCustomers = new Button();
            btnSalesHistory = new Button();
            btnReports = new Button();
            btnUsers = new Button();
            btnLogout = new Button();
            btnToggle = new Button();

            SuspendLayout();

            // ================= SIDEBAR =================

            pnlSidebar.Dock = DockStyle.Left;

            pnlSidebar.Width = 240;

            pnlSidebar.BackColor =
                Color.FromArgb(16, 36, 62);

            // ================= BRAND PANEL =================

            pnlBrand.Dock = DockStyle.Top;

            pnlBrand.Height = 110;

            pnlBrand.BackColor =
                Color.FromArgb(16, 36, 62);

            lblBrand.Text = "NAOTR-POS";

            lblBrand.Font = new Font(
                "Segoe UI", 21F, FontStyle.Bold);

            lblBrand.ForeColor = Color.White;

            lblBrand.Location = new Point(20, 25);

            lblBrand.AutoSize = true;

            lblSubtitle.Text =
                "Enterprise Management";

            lblSubtitle.Font =
                new Font("Segoe UI", 10F);

            lblSubtitle.ForeColor =
                Color.LightSteelBlue;

            lblSubtitle.Location =
                new Point(23, 70);

            lblSubtitle.AutoSize = true;

            pnlBrand.Controls.Add(lblBrand);
            pnlBrand.Controls.Add(lblSubtitle);

            // ================= NAVIGATION PANEL =================

            pnlNavigation.Dock = DockStyle.Fill;

            pnlNavigation.AutoScroll = true;

            pnlNavigation.Padding =
                new Padding(10, 5, 10, 5);

            pnlNavigation.BackColor =
                Color.FromArgb(16, 36, 62);

            // ================= NAVIGATION BUTTONS =================

            ConfigureMenuButton(
                btnDashboard, "Dashboard", 5);

            ConfigureMenuButton(
                btnPOS, "Point of Sale", 55);

            ConfigureMenuButton(
                btnInventory, "Inventory", 105);

            ConfigureMenuButton(
                btnCategories, "Categories", 155);

            ConfigureMenuButton(
                btnCustomers, "Customers", 205);

            ConfigureMenuButton(
                btnSalesHistory, "Sales History", 255);

            ConfigureMenuButton(
                btnReports, "Sales Reports", 305);

            ConfigureMenuButton(
                btnUsers, "User Management", 355);

            // ================= BUTTON EVENTS =================

            btnDashboard.Click +=
                btnDashboard_Click;

            btnPOS.Click +=
                btnPOS_Click;

            btnInventory.Click +=
                btnInventory_Click;

            btnCategories.Click +=
                btnCategories_Click;

            btnCustomers.Click +=
                btnCustomers_Click;

            btnSalesHistory.Click +=
                btnSalesHistory_Click;

            btnReports.Click +=
                btnReports_Click;

            btnUsers.Click +=
                btnUsers_Click;

            // ================= ADD NAVIGATION BUTTONS =================

            pnlNavigation.Controls.Add(btnDashboard);
            pnlNavigation.Controls.Add(btnPOS);
            pnlNavigation.Controls.Add(btnInventory);
            pnlNavigation.Controls.Add(btnCategories);
            pnlNavigation.Controls.Add(btnCustomers);
            pnlNavigation.Controls.Add(btnSalesHistory);
            pnlNavigation.Controls.Add(btnReports);
            pnlNavigation.Controls.Add(btnUsers);

            // ================= USER INFORMATION PANEL =================

            pnlUserInfo.Dock = DockStyle.Bottom;

            pnlUserInfo.Height = 135;

            pnlUserInfo.BackColor =
                Color.FromArgb(12, 29, 51);

            lblUser.Text = "Current User";

            lblUser.Font = new Font(
                "Segoe UI", 10F, FontStyle.Bold);

            lblUser.ForeColor = Color.White;

            lblUser.Location =
                new Point(20, 12);

            lblUser.Size =
                new Size(200, 38);

            lblUser.TextAlign =
                ContentAlignment.MiddleLeft;

            lblRole.Text = "Role";

            lblRole.Font =
                new Font("Segoe UI", 10F);

            lblRole.ForeColor =
                Color.LightSteelBlue;

            lblRole.Location =
                new Point(20, 50);

            lblRole.AutoSize = true;

            // ================= LOGOUT =================

            ConfigureMenuButton(
                btnLogout, "Logout", 80);

            btnLogout.BackColor =
                Color.FromArgb(185, 28, 28);

            btnLogout.ForeColor = Color.White;

            btnLogout.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(220, 38, 38);

            btnLogout.Click +=
                btnLogout_Click;

            pnlUserInfo.Controls.Add(lblUser);
            pnlUserInfo.Controls.Add(lblRole);
            pnlUserInfo.Controls.Add(btnLogout);

            // ================= ADD SIDEBAR PANELS =================

            pnlSidebar.Controls.Add(pnlNavigation);
            pnlSidebar.Controls.Add(pnlUserInfo);
            pnlSidebar.Controls.Add(pnlBrand);

            // ================= WORKSPACE =================

            pnlWorkspace.Dock = DockStyle.Fill;

            pnlWorkspace.BackColor =
                Color.FromArgb(241, 245, 249);

            // ================= HEADER =================

            pnlHeader.Dock = DockStyle.Top;

            pnlHeader.Height = 70;

            pnlHeader.BackColor = Color.White;

            btnToggle.Text = "☰";

            btnToggle.Font =
                new Font("Segoe UI", 18F);

            btnToggle.FlatStyle = FlatStyle.Flat;

            btnToggle.FlatAppearance.BorderSize = 0;

            btnToggle.Location =
                new Point(15, 12);

            btnToggle.Size =
                new Size(45, 45);

            btnToggle.Cursor = Cursors.Hand;

            btnToggle.Click +=
                btnToggle_Click;

            lblPageTitle.Text = "Dashboard";

            lblPageTitle.Font = new Font(
                "Segoe UI", 17F, FontStyle.Bold);

            lblPageTitle.ForeColor =
                Color.FromArgb(16, 36, 62);

            lblPageTitle.Location =
                new Point(75, 18);

            lblPageTitle.AutoSize = true;

            pnlHeader.Controls.Add(btnToggle);
            pnlHeader.Controls.Add(lblPageTitle);

            // ================= CONTENT =================

            pnlContent.Dock = DockStyle.Fill;

            pnlContent.BackColor =
                Color.FromArgb(241, 245, 249);

            pnlWorkspace.Controls.Add(pnlContent);
            pnlWorkspace.Controls.Add(pnlHeader);

            // ================= MAIN FORM =================

            AutoScaleMode = AutoScaleMode.None;

            ClientSize = new Size(1250, 700);

            MinimumSize = new Size(1000, 700);

            BackColor =
                Color.FromArgb(241, 245, 249);

            Controls.Add(pnlWorkspace);
            Controls.Add(pnlSidebar);

            StartPosition =
                FormStartPosition.CenterScreen;

            WindowState =
                FormWindowState.Maximized;

            Name = "MainForm";

            Text =
                "NAOTR-POS | Enterprise Management";

            ResumeLayout(false);
        }

        // ================= MENU BUTTON DESIGN =================

        private void ConfigureMenuButton(
            Button button,
            string text,
            int top)
        {
            button.Text = text;

            button.Font =
                new Font("Segoe UI", 11F);

            button.ForeColor =
                Color.LightSteelBlue;

            button.BackColor =
                Color.FromArgb(16, 36, 62);

            button.FlatStyle =
                FlatStyle.Flat;

            button.FlatAppearance.BorderSize = 0;

            button.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(37, 65, 100);

            button.TextAlign =
                ContentAlignment.MiddleLeft;

            button.Padding =
                new Padding(15, 0, 0, 0);

            button.Location =
                new Point(10, top);

            button.Size =
                new Size(200, 44);

            button.Cursor = Cursors.Hand;

            button.UseVisualStyleBackColor = false;
        }
    }
}
