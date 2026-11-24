
using System;
using System.Drawing;
using System.Windows.Forms;
using NAOTR_POS.Helpers;

namespace NAOTR_POS.Forms
{
    public partial class MainForm : Form
    {
        private Form activeForm;
        private bool sidebarExpanded = true;

        public MainForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Session.IsLoggedIn)
            {
                Close();
                return;
            }

            lblUser.Text = Session.CurrentUser.FullName;
            lblRole.Text = Session.CurrentUser.RoleName;

            ApplyRolePermissions();
            OpenDashboard();
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            if (!Session.IsInRole("Admin"))
            {
                MessageBox.Show(
                    "Only administrators can manage users.",
                    "Access Denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            lblPageTitle.Text = "User Management";

            SetActiveButton(btnUsers);

            OpenChildForm(new UsersForm());
        }

        private void ApplyRolePermissions()
        {
            string role = Session.CurrentUser.RoleName;

            bool isAdmin = role == "Admin";
            bool isManager = role == "Manager";

            btnInventory.Visible = isAdmin || isManager;
            btnCategories.Visible = isAdmin || isManager;
            btnReports.Visible =
                Session.IsInRole("Admin") ||
                Session.IsInRole("Manager");
            btnUsers.Visible = Session.IsInRole("Admin");
            btnCustomers.Visible = isAdmin || isManager;
            btnSalesHistory.Visible =
    Session.IsInRole("Admin") ||
    Session.IsInRole("Manager");
        }

        private void btnReports_Click(
            object sender,
            EventArgs e)
        {
            if (!Session.IsInRole("Admin") &&
                !Session.IsInRole("Manager"))
            {
                MessageBox.Show(
                    "Access denied.",
                    "Permission Denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            lblPageTitle.Text = "Sales Reports";

            SetActiveButton(btnReports);

            OpenChildForm(new ReportsForm());
        }

        private void OpenChildForm(Form childForm)
        {
            if (activeForm != null)
            {
                activeForm.Close();
                activeForm.Dispose();
            }

            activeForm = childForm;

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            pnlContent.Controls.Clear();
            pnlContent.Controls.Add(childForm);

            childForm.Show();
        }


        private void SetActiveButton(Button selectedButton)
        {
            foreach (Control control in pnlNavigation.Controls)
            {
                if (control is Button button)
                {
                    button.BackColor =
                        Color.FromArgb(16, 36, 62);

                    button.ForeColor =
                        Color.LightSteelBlue;
                }
            }

            if (selectedButton != null)
            {
                selectedButton.BackColor =
                    Color.FromArgb(37, 99, 235);

                selectedButton.ForeColor =
                    Color.White;
            }
        }


        private void OpenDashboard()
        {
            lblPageTitle.Text = "Dashboard";
            SetActiveButton(btnDashboard);
            OpenChildForm(new DashboardForm());
        }

        private void btnDashboard_Click(
            object sender, EventArgs e)
        {
            OpenDashboard();
        }

        private void btnInventory_Click(
    object sender, EventArgs e)
        {
            if (!Session.IsInRole("Admin") &&
                !Session.IsInRole("Manager"))
            {
                MessageBox.Show(
                    "Access denied.",
                    "Unauthorized",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            lblPageTitle.Text = "Inventory";

            SetActiveButton(btnInventory);

            OpenChildForm(new InventoryForm());
        }

        private void btnSalesHistory_Click(
            object sender,
            EventArgs e)
        {
            if (!Session.IsInRole("Admin") &&
                !Session.IsInRole("Manager"))
            {
                MessageBox.Show(
                    "Access denied.",
                    "Permission Denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            lblPageTitle.Text = "Sales History";

            SetActiveButton(btnSalesHistory);

            OpenChildForm(new SalesHistoryForm());
        }

        private void btnPOS_Click(
    object sender, EventArgs e)
        {
            if (!Session.IsInRole("Admin") &&
                !Session.IsInRole("Manager") &&
                !Session.IsInRole("Cashier"))
            {
                MessageBox.Show(
                    "Access denied.",
                    "Permission Denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            lblPageTitle.Text = "Point of Sale";

            SetActiveButton(btnPOS);

            OpenChildForm(new POSForm());
        }
        private void btnCustomers_Click(
    object sender, EventArgs e)
        {
            if (!Session.IsInRole("Admin") &&
                !Session.IsInRole("Manager"))
            {
                MessageBox.Show(
                    "Access denied.",
                    "Permission Denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            lblPageTitle.Text = "Customers";

            SetActiveButton(btnCustomers);

            OpenChildForm(
                new CustomerManagementForm());
        }
        private void btnCategories_Click(
    object sender, EventArgs e)
        {
            if (!Session.IsInRole("Admin") &&
                !Session.IsInRole("Manager"))
            {
                MessageBox.Show(
                    "Access denied.",
                    "Permission Denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            lblPageTitle.Text = "Categories";

            SetActiveButton(btnCategories);

            OpenChildForm(
                new CategoryManagementForm());
        }
        private void btnComingSoon_Click(
            object sender, EventArgs e)
        {
            Button button = sender as Button;

            MessageBox.Show(
                button.Text + " is not yet implemented.",
                "Coming Soon",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnToggle_Click(
            object sender, EventArgs e)
        {
            sidebarExpanded = !sidebarExpanded;
            pnlSidebar.Visible = sidebarExpanded;
        }

        private void btnLogout_Click(
            object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to log out?",
                "Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Session.Logout();
                Close();
            }
        }
    }
}
