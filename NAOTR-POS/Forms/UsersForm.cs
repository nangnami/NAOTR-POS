
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using NAOTR_POS.Helpers;
using NAOTR_POS.Services;

namespace NAOTR_POS.Forms
{
    public partial class UsersForm : Form
    {
        private readonly UserService userService =
            new UserService();

        private bool isLoading = true;

        private DataTable allUsers;

        public UsersForm()
        {
            InitializeComponent();
        }

        // ================= FORM LOAD =================

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Session.IsInRole("Admin"))
            {
                MessageBox.Show(
                    "Only administrators can manage users.",
                    "Access Denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                Close();
                return;
            }

            try
            {
                LoadRoles();

                cmbStatus.SelectedIndex = 0;

                isLoading = false;

                RefreshUsers();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        // ================= LOAD ROLES =================

        private void LoadRoles()
        {
            DataTable roles =
                userService.GetRoles();

            DataRow allRoles = roles.NewRow();

            allRoles["RoleId"] = 0;
            allRoles["RoleName"] = "All Roles";

            roles.Rows.InsertAt(allRoles, 0);

            cmbRole.DisplayMember = "RoleName";
            cmbRole.ValueMember = "RoleId";

            cmbRole.DataSource = roles;

            cmbRole.SelectedIndex = 0;
        }

        // ================= REFRESH USERS =================

        private void RefreshUsers()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                allUsers = userService.GetUsers();

                LoadMetrics();

                ApplyFilters();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        // ================= LOAD METRICS =================

        private void LoadMetrics()
        {
            if (allUsers == null)
                return;

            int total = allUsers.Rows.Count;

            int active = 0;
            int inactive = 0;
            int administrators = 0;

            foreach (DataRow row in allUsers.Rows)
            {
                bool isActive =
                    Convert.ToBoolean(row["IsActive"]);

                string role =
                    Convert.ToString(row["RoleName"]);

                if (isActive)
                    active++;
                else
                    inactive++;

                if (role.Equals(
                    "Admin",
                    StringComparison.OrdinalIgnoreCase))
                {
                    administrators++;
                }
            }

            lblTotalUsers.Text = total.ToString("N0");

            lblActiveUsers.Text = active.ToString("N0");

            lblInactiveUsers.Text =
                inactive.ToString("N0");

            lblAdministrators.Text =
                administrators.ToString("N0");
        }

        // ================= SEARCH AND FILTER =================

        private void ApplyFilters()
        {
            if (isLoading || allUsers == null)
                return;

            try
            {
                string search =
                    txtSearch.Text.Trim();

                int roleId = 0;

                if (cmbRole.SelectedValue is int)
                {
                    roleId =
                        Convert.ToInt32(
                            cmbRole.SelectedValue);
                }

                string status = cmbStatus.Text;

                DataTable filtered =
                    allUsers.Clone();

                foreach (DataRow row in allUsers.Rows)
                {
                    string username =
                        Convert.ToString(row["Username"]);

                    string fullName =
                        Convert.ToString(row["FullName"]);

                    int userRoleId =
                        Convert.ToInt32(row["RoleId"]);

                    bool active =
                        Convert.ToBoolean(row["IsActive"]);

                    bool matchesSearch =
                        string.IsNullOrWhiteSpace(search)

                        || username.IndexOf(
                            search,
                            StringComparison.OrdinalIgnoreCase) >= 0

                        || fullName.IndexOf(
                            search,
                            StringComparison.OrdinalIgnoreCase) >= 0;

                    bool matchesRole =
                        roleId == 0 ||
                        userRoleId == roleId;

                    bool matchesStatus =
                        status == "All"

                        || (status == "Active" && active)

                        || (status == "Inactive" && !active);

                    if (matchesSearch &&
                        matchesRole &&
                        matchesStatus)
                    {
                        filtered.ImportRow(row);
                    }
                }

                dgvUsers.DataSource = filtered;

                ConfigureGridColumns();

                dgvUsers.ClearSelection();

                lblRecordCount.Text =
                    "Showing " +
                    filtered.Rows.Count +
                    " of " +
                    allUsers.Rows.Count +
                    " users";

                UpdateActionButtons();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        // ================= GRID COLUMNS =================

        private void ConfigureGridColumns()
        {
            HideColumn("RoleId");
            HideColumn("IsActive");

            ConfigureColumn(
                "UserId",
                "User ID",
                55);

            ConfigureColumn(
                "Username",
                "Username",
                95);

            ConfigureColumn(
                "FullName",
                "Full Name",
                155);

            ConfigureColumn(
                "RoleName",
                "Role",
                85);

            ConfigureColumn(
                "AccountStatus",
                "Status",
                80);

            ConfigureColumn(
                "CreatedAt",
                "Date Created",
                110,
                "MMM dd, yyyy");

            ConfigureColumn(
                "LastLoginAt",
                "Last Login",
                140,
                "MMM dd, yyyy hh:mm tt");

            if (dgvUsers.Columns.Contains("LastLoginAt"))
            {
                dgvUsers.Columns["LastLoginAt"]
                    .DefaultCellStyle.NullValue = "Never";
            }
        }

        private void HideColumn(string name)
        {
            if (dgvUsers.Columns.Contains(name))
            {
                dgvUsers.Columns[name].Visible = false;
            }
        }

        private void ConfigureColumn(
            string name,
            string header,
            float weight,
            string format = "")
        {
            if (!dgvUsers.Columns.Contains(name))
                return;

            DataGridViewColumn column =
                dgvUsers.Columns[name];

            column.HeaderText = header;

            column.FillWeight = weight;

            if (!string.IsNullOrEmpty(format))
            {
                column.DefaultCellStyle.Format =
                    format;
            }
        }

        // ================= SELECTED USER =================

        private int GetSelectedUserId()
        {
            if (dgvUsers.SelectedRows.Count == 0)
                return 0;

            DataGridViewRow row =
                dgvUsers.SelectedRows[0];

            return Convert.ToInt32(
                row.Cells["UserId"].Value);
        }

        private string GetSelectedUsername()
        {
            if (dgvUsers.SelectedRows.Count == 0)
                return "";

            return Convert.ToString(
                dgvUsers.SelectedRows[0]
                    .Cells["Username"].Value);
        }

        private bool GetSelectedStatus()
        {
            if (dgvUsers.SelectedRows.Count == 0)
                return false;

            return Convert.ToBoolean(
                dgvUsers.SelectedRows[0]
                    .Cells["IsActive"].Value);
        }

        // ================= ACTION BUTTON STATES =================

        private void UpdateActionButtons()
        {
            int userId = GetSelectedUserId();

            bool selected = userId > 0;

            bool isAdmin =
                Session.IsInRole("Admin");

            bool isOwnAccount =
                selected &&
                Session.CurrentUser != null &&
                userId == Session.CurrentUser.UserId;

            btnAdd.Enabled = isAdmin;

            btnEdit.Enabled =
                selected && isAdmin;

            btnResetPassword.Enabled =
                selected && isAdmin;

            btnToggleStatus.Enabled =
                selected &&
                isAdmin &&
                !isOwnAccount;

            if (selected && !GetSelectedStatus())
            {
                btnToggleStatus.Text = "Activate";

                btnToggleStatus.BackColor =
                    Color.FromArgb(22, 163, 74);
            }
            else
            {
                btnToggleStatus.Text = "Deactivate";

                btnToggleStatus.BackColor =
                    Color.FromArgb(220, 38, 38);
            }
        }

        // ================= ADD USER =================

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var form = new NAOTR_POS.Forms.UserForm())
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    RefreshUsers();
                }
            }
        }

        // ================= EDIT USER =================

        private void btnEdit_Click(
            object sender,
            EventArgs e)
        {
            if (!Session.IsInRole("Admin"))
                return;

            int userId = GetSelectedUserId();

            if (userId == 0)
            {
                MessageBox.Show(
                    "Please select a user.");

                return;
            }

            using (UserForm form =
                new UserForm(userId))
            {
                if (form.ShowDialog(this) ==
                    DialogResult.OK)
                {
                    RefreshUsers();
                }
            }
        }

        // ================= RESET PASSWORD =================

        private void btnResetPassword_Click(
            object sender,
            EventArgs e)
        {
            if (!Session.IsInRole("Admin"))
                return;

            int userId = GetSelectedUserId();

            if (userId == 0)
            {
                MessageBox.Show(
                    "Please select a user.");

                return;
            }

            using (ResetPasswordForm form =
                new ResetPasswordForm(userId))
            {
                if (form.ShowDialog(this) ==
                    DialogResult.OK)
                {
                    RefreshUsers();
                }
            }
        }

        // ================= ACTIVATE / DEACTIVATE =================

        private void btnToggleStatus_Click(
            object sender,
            EventArgs e)
        {
            if (!Session.IsInRole("Admin"))
                return;

            int userId = GetSelectedUserId();

            if (userId == 0)
            {
                MessageBox.Show(
                    "Please select a user.");

                return;
            }

            if (userId == Session.CurrentUser.UserId)
            {
                MessageBox.Show(
                    "You cannot deactivate your own account.",
                    "Action Not Allowed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            bool currentStatus =
                GetSelectedStatus();

            bool newStatus = !currentStatus;

            string username =
                GetSelectedUsername();

            string action =
                newStatus ? "activate" : "deactivate";

            DialogResult confirmation =
                MessageBox.Show(
                    "Are you sure you want to " +
                    action +
                    " the account '" +
                    username +
                    "'?",

                    "Confirm Account Status",

                    MessageBoxButtons.YesNo,

                    MessageBoxIcon.Question,

                    MessageBoxDefaultButton.Button2);

            if (confirmation != DialogResult.Yes)
                return;

            try
            {
                userService.SetStatus(
                    userId,
                    newStatus);

                MessageBox.Show(
                    newStatus
                        ? "User activated successfully."
                        : "User deactivated successfully.",

                    "Success",

                    MessageBoxButtons.OK,

                    MessageBoxIcon.Information);

                RefreshUsers();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        // ================= REFRESH =================

        private void btnRefresh_Click(
            object sender,
            EventArgs e)
        {
            RefreshUsers();
        }

        // ================= FILTER EVENTS =================

        private void txtSearch_TextChanged(
            object sender,
            EventArgs e)
        {
            ApplyFilters();
        }

        private void cmbRole_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            ApplyFilters();
        }

        private void cmbStatus_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            ApplyFilters();
        }

        // ================= GRID EVENTS =================

        private void dgvUsers_SelectionChanged(
            object sender,
            EventArgs e)
        {
            if (isLoading)
                return;

            if (!dgvUsers.Columns.Contains("UserId") ||
                !dgvUsers.Columns.Contains("IsActive"))
            {
                return;
            }

            UpdateActionButtons();
        }

        private void dgvUsers_CellDoubleClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnEdit_Click(sender, EventArgs.Empty);
            }
        }

        private void dgvUsers_CellFormatting(
            object sender,
            DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 ||
                e.ColumnIndex < 0 ||
                e.Value == null)
            {
                return;
            }

            string columnName =
                dgvUsers.Columns[e.ColumnIndex].Name;

            if (columnName == "AccountStatus")
            {
                string status =
                    e.Value.ToString();

                if (status == "Active")
                {
                    e.CellStyle.ForeColor =
                        Color.FromArgb(22, 163, 74);
                }
                else
                {
                    e.CellStyle.ForeColor =
                        Color.FromArgb(220, 38, 38);
                }
            }

            if (columnName == "RoleName")
            {
                string role =
                    e.Value.ToString();

                switch (role)
                {
                    case "Admin":

                        e.CellStyle.ForeColor =
                            Color.FromArgb(124, 58, 237);

                        break;

                    case "Manager":

                        e.CellStyle.ForeColor =
                            Color.FromArgb(37, 99, 235);

                        break;

                    case "Cashier":

                        e.CellStyle.ForeColor =
                            Color.FromArgb(8, 145, 178);

                        break;
                }
            }
        }

        // ================= ERROR MESSAGE =================

        private void ShowError(string message)
        {
            MessageBox.Show(
                message,
                "User Management Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
