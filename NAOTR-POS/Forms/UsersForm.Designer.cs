
using System.Drawing;
using System.Windows.Forms;

namespace NAOTR_POS.Forms
{
    partial class UsersForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Panel pnlContent;
        private Panel pnlFooter;

        private FlowLayoutPanel pnlActions;
        private FlowLayoutPanel pnlFilters;

        private TableLayoutPanel tblMetrics;

        private Label lblTitle;
        private Label lblSubtitle;

        private Label lblTotalUsers;
        private Label lblActiveUsers;
        private Label lblInactiveUsers;
        private Label lblAdministrators;
        private Label lblRecordCount;

        private TextBox txtSearch;

        private ComboBox cmbRole;
        private ComboBox cmbStatus;

        private Button btnAdd;
        private Button btnEdit;
        private Button btnResetPassword;
        private Button btnToggleStatus;
        private Button btnRefresh;

        private DataGridView dgvUsers;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            pnlContent = new Panel();
            pnlFooter = new Panel();

            pnlActions = new FlowLayoutPanel();
            pnlFilters = new FlowLayoutPanel();

            tblMetrics = new TableLayoutPanel();

            lblTitle = new Label();
            lblSubtitle = new Label();

            lblTotalUsers = new Label();
            lblActiveUsers = new Label();
            lblInactiveUsers = new Label();
            lblAdministrators = new Label();
            lblRecordCount = new Label();

            txtSearch = new TextBox();

            cmbRole = new ComboBox();
            cmbStatus = new ComboBox();

            btnAdd = new Button();
            btnEdit = new Button();
            btnResetPassword = new Button();
            btnToggleStatus = new Button();
            btnRefresh = new Button();

            dgvUsers = new DataGridView();

            SuspendLayout();

            // ================= HEADER =================

            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 85;

            pnlHeader.BackColor =
                Color.FromArgb(16, 36, 62);

            lblTitle.Text = "User Management";

            lblTitle.Font = new Font(
                "Segoe UI", 22F, FontStyle.Bold);

            lblTitle.ForeColor = Color.White;

            lblTitle.Location = new Point(20, 10);
            lblTitle.AutoSize = true;

            lblSubtitle.Text =
                "Manage accounts, roles and system access.";

            lblSubtitle.Font =
                new Font("Segoe UI", 10F);

            lblSubtitle.ForeColor =
                Color.LightSteelBlue;

            lblSubtitle.Location = new Point(23, 53);
            lblSubtitle.AutoSize = true;

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubtitle);

            // ================= CONTENT =================

            pnlContent.Dock = DockStyle.Fill;

            pnlContent.Padding = new Padding(20);

            pnlContent.BackColor =
                Color.FromArgb(241, 245, 249);

            // ================= METRIC CARDS =================

            tblMetrics.Dock = DockStyle.Top;
            tblMetrics.Height = 115;

            tblMetrics.ColumnCount = 4;
            tblMetrics.RowCount = 1;

            tblMetrics.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100F));

            for (int i = 0; i < 4; i++)
            {
                tblMetrics.ColumnStyles.Add(
                    new ColumnStyle(
                        SizeType.Percent, 25F));
            }

            tblMetrics.Controls.Add(
                CreateMetricCard(
                    "Total Users",
                    lblTotalUsers,
                    Color.FromArgb(37, 99, 235)),
                0, 0);

            tblMetrics.Controls.Add(
                CreateMetricCard(
                    "Active Users",
                    lblActiveUsers,
                    Color.FromArgb(22, 163, 74)),
                1, 0);

            tblMetrics.Controls.Add(
                CreateMetricCard(
                    "Inactive Users",
                    lblInactiveUsers,
                    Color.FromArgb(220, 38, 38)),
                2, 0);

            tblMetrics.Controls.Add(
                CreateMetricCard(
                    "Administrators",
                    lblAdministrators,
                    Color.FromArgb(16, 36, 62)),
                3, 0);

            // ================= ACTION BUTTONS =================

            pnlActions.Dock = DockStyle.Top;

            pnlActions.AutoSize = true;

            pnlActions.AutoSizeMode =
                AutoSizeMode.GrowAndShrink;

            pnlActions.WrapContents = true;

            pnlActions.FlowDirection =
                FlowDirection.LeftToRight;

            pnlActions.Padding = new Padding(5);

            pnlActions.BackColor = Color.White;

            ConfigureButton(
                btnAdd,
                "+ Add User",
                Color.FromArgb(37, 99, 235),
                110);

            ConfigureButton(
                btnEdit,
                "Edit User",
                Color.FromArgb(16, 36, 62),
                105);

            ConfigureButton(
                btnResetPassword,
                "Reset Password",
                Color.FromArgb(217, 119, 6),
                145);

            ConfigureButton(
                btnToggleStatus,
                "Deactivate",
                Color.FromArgb(220, 38, 38),
                125);

            ConfigureButton(
                btnRefresh,
                "Refresh",
                Color.FromArgb(100, 116, 139),
                100);

            btnAdd.Click += btnAdd_Click;
            btnEdit.Click += btnEdit_Click;

            btnResetPassword.Click +=
                btnResetPassword_Click;

            btnToggleStatus.Click +=
                btnToggleStatus_Click;

            btnRefresh.Click += btnRefresh_Click;

            pnlActions.Controls.Add(btnAdd);
            pnlActions.Controls.Add(btnEdit);
            pnlActions.Controls.Add(btnResetPassword);
            pnlActions.Controls.Add(btnToggleStatus);
            pnlActions.Controls.Add(btnRefresh);

            // ================= FILTERS =================

            pnlFilters.Dock = DockStyle.Top;

            pnlFilters.AutoSize = true;

            pnlFilters.AutoSizeMode =
                AutoSizeMode.GrowAndShrink;

            pnlFilters.WrapContents = true;

            pnlFilters.Padding = new Padding(5, 10, 5, 10);

            pnlFilters.BackColor = Color.White;

            txtSearch.Font =
                new Font("Segoe UI", 11F);

            txtSearch.Width = 250;
            txtSearch.MaxLength = 100;

            txtSearch.Margin =
                new Padding(5, 5, 15, 5);

            txtSearch.TextChanged +=
                txtSearch_TextChanged;

            ConfigureComboBox(cmbRole, 145);
            ConfigureComboBox(cmbStatus, 130);

            cmbStatus.Items.AddRange(new object[]
            {
                "All",
                "Active",
                "Inactive"
            });

            cmbRole.SelectedIndexChanged +=
                cmbRole_SelectedIndexChanged;

            cmbStatus.SelectedIndexChanged +=
                cmbStatus_SelectedIndexChanged;

            pnlFilters.Controls.Add(
                CreateFilterLabel("Search:"));

            pnlFilters.Controls.Add(txtSearch);

            pnlFilters.Controls.Add(
                CreateFilterLabel("Role:"));

            pnlFilters.Controls.Add(cmbRole);

            pnlFilters.Controls.Add(
                CreateFilterLabel("Status:"));

            pnlFilters.Controls.Add(cmbStatus);

            // ================= USER GRID =================

            dgvUsers.Dock = DockStyle.Fill;

            dgvUsers.BackgroundColor = Color.White;

            dgvUsers.BorderStyle = BorderStyle.None;

            dgvUsers.ReadOnly = true;

            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.AllowUserToResizeRows = false;

            dgvUsers.RowHeadersVisible = false;

            dgvUsers.MultiSelect = false;

            dgvUsers.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvUsers.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvUsers.EnableHeadersVisualStyles = false;

            dgvUsers.ColumnHeadersHeight = 45;

            dgvUsers.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode
                    .DisableResizing;

            dgvUsers.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(16, 36, 62);

            dgvUsers.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;

            dgvUsers.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10F, FontStyle.Bold);

            dgvUsers.DefaultCellStyle.Font =
                new Font("Segoe UI", 10F);

            dgvUsers.DefaultCellStyle.ForeColor =
                Color.FromArgb(51, 65, 85);

            dgvUsers.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(219, 234, 254);

            dgvUsers.DefaultCellStyle.SelectionForeColor =
                Color.FromArgb(16, 36, 62);

            dgvUsers.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(248, 250, 252);

            dgvUsers.RowTemplate.Height = 42;

            dgvUsers.SelectionChanged +=
                dgvUsers_SelectionChanged;

            dgvUsers.CellFormatting +=
                dgvUsers_CellFormatting;

            dgvUsers.CellDoubleClick +=
                dgvUsers_CellDoubleClick;

            // ================= FOOTER =================

            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Height = 40;

            pnlFooter.BackColor = Color.White;

            lblRecordCount.Text = "Showing 0 users";

            lblRecordCount.Font =
                new Font("Segoe UI", 10F);

            lblRecordCount.ForeColor =
                Color.FromArgb(100, 116, 139);

            lblRecordCount.Dock = DockStyle.Fill;

            lblRecordCount.Padding =
                new Padding(10, 0, 0, 0);

            lblRecordCount.TextAlign =
                ContentAlignment.MiddleLeft;

            pnlFooter.Controls.Add(lblRecordCount);

            // ================= ADD CONTROLS =================

            pnlContent.Controls.Add(dgvUsers);
            pnlContent.Controls.Add(pnlFooter);
            pnlContent.Controls.Add(pnlFilters);
            pnlContent.Controls.Add(pnlActions);
            pnlContent.Controls.Add(tblMetrics);

            Controls.Add(pnlContent);
            Controls.Add(pnlHeader);

            AutoScaleMode = AutoScaleMode.None;

            ClientSize = new Size(1150, 750);

            FormBorderStyle = FormBorderStyle.None;

            Name = "UsersForm";

            Text = "User Management";

            ResumeLayout(false);
        }

        // ================= METRIC CARD =================

        private Panel CreateMetricCard(
            string title,
            Label value,
            Color accent)
        {
            Panel card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = new Padding(5, 8, 5, 12)
            };

            Panel line = new Panel
            {
                Dock = DockStyle.Left,
                Width = 5,
                BackColor = accent
            };

            Label heading = new Label
            {
                Text = title,

                Font = new Font(
                    "Segoe UI", 10F),

                ForeColor =
                    Color.FromArgb(100, 116, 139),

                Dock = DockStyle.Top,

                Height = 35,

                Padding =
                    new Padding(12, 10, 0, 0)
            };

            value.Text = "0";

            value.Font = new Font(
                "Segoe UI", 24F, FontStyle.Bold);

            value.ForeColor = accent;

            value.Dock = DockStyle.Fill;

            value.Padding =
                new Padding(12, 0, 0, 0);

            value.TextAlign =
                ContentAlignment.MiddleLeft;

            card.Controls.Add(value);
            card.Controls.Add(heading);
            card.Controls.Add(line);

            return card;
        }

        // ================= BUTTON DESIGN =================

        private void ConfigureButton(
            Button button,
            string text,
            Color color,
            int width)
        {
            button.Text = text;

            button.Size = new Size(width, 40);

            button.BackColor = color;
            button.ForeColor = Color.White;

            button.Font = new Font(
                "Segoe UI", 10F, FontStyle.Bold);

            button.FlatStyle = FlatStyle.Flat;

            button.FlatAppearance.BorderSize = 0;

            button.Cursor = Cursors.Hand;

            button.Margin = new Padding(5);

            button.UseVisualStyleBackColor = false;
        }

        // ================= COMBO BOX DESIGN =================

        private void ConfigureComboBox(
            ComboBox combo,
            int width)
        {
            combo.DropDownStyle =
                ComboBoxStyle.DropDownList;

            combo.Font =
                new Font("Segoe UI", 10F);

            combo.Width = width;

            combo.Margin =
                new Padding(5, 5, 15, 5);
        }

        // ================= FILTER LABEL =================

        private Label CreateFilterLabel(string text)
        {
            return new Label
            {
                Text = text,

                Font = new Font(
                    "Segoe UI", 10F, FontStyle.Bold),

                ForeColor =
                    Color.FromArgb(51, 65, 85),

                AutoSize = true,

                Margin =
                    new Padding(5, 9, 0, 0)
            };
        }
    }
}
