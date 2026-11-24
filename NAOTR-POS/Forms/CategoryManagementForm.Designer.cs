
using System.Drawing;
using System.Windows.Forms;

namespace NAOTR_POS.Forms
{
    partial class CategoryManagementForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Panel pnlContent;
        private Panel pnlTable;
        private Panel pnlFooter;

        private FlowLayoutPanel pnlToolbar;

        private TableLayoutPanel tblMetrics;

        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblTotalCategories;
        private Label lblAssignedCategories;
        private Label lblUnusedCategories;
        private Label lblRecordCount;

        private TextBox txtSearch;

        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;

        private DataGridView dgvCategories;

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
            pnlTable = new Panel();
            pnlFooter = new Panel();

            pnlToolbar = new FlowLayoutPanel();

            tblMetrics = new TableLayoutPanel();

            lblTitle = new Label();
            lblSubtitle = new Label();

            lblTotalCategories = new Label();
            lblAssignedCategories = new Label();
            lblUnusedCategories = new Label();

            lblRecordCount = new Label();

            txtSearch = new TextBox();

            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnRefresh = new Button();

            dgvCategories = new DataGridView();

            SuspendLayout();

            // HEADER

            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 85;
            pnlHeader.BackColor = Color.White;

            lblTitle.Text = "Category Management";

            lblTitle.Font = new Font(
                "Segoe UI", 24F, FontStyle.Bold);

            lblTitle.ForeColor =
                Color.FromArgb(16, 36, 62);

            lblTitle.Location = new Point(20, 10);
            lblTitle.AutoSize = true;

            lblSubtitle.Text =
                "Organize and manage your product categories.";

            lblSubtitle.Font =
                new Font("Segoe UI", 10F);

            lblSubtitle.ForeColor = Color.SlateGray;

            lblSubtitle.Location =
                new Point(23, 53);

            lblSubtitle.AutoSize = true;

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubtitle);

            // MAIN CONTENT

            pnlContent.Dock = DockStyle.Fill;

            pnlContent.Padding = new Padding(20);

            pnlContent.BackColor =
                Color.FromArgb(241, 245, 249);

            // METRIC CARDS

            tblMetrics.Dock = DockStyle.Top;
            tblMetrics.Height = 115;

            tblMetrics.ColumnCount = 3;
            tblMetrics.RowCount = 1;

            tblMetrics.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent, 33.33F));

            tblMetrics.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent, 33.33F));

            tblMetrics.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent, 33.34F));

            tblMetrics.RowStyles.Add(
                new RowStyle(
                    SizeType.Percent, 100F));

            tblMetrics.Controls.Add(
                CreateMetricCard(
                    "Total Categories",
                    lblTotalCategories,
                    Color.RoyalBlue),
                0, 0);

            tblMetrics.Controls.Add(
                CreateMetricCard(
                    "Assigned Categories",
                    lblAssignedCategories,
                    Color.SeaGreen),
                1, 0);

            tblMetrics.Controls.Add(
                CreateMetricCard(
                    "Unused Categories",
                    lblUnusedCategories,
                    Color.DarkOrange),
                2, 0);

            // RESPONSIVE TOOLBAR

            pnlToolbar.Dock = DockStyle.Top;

            pnlToolbar.AutoSize = true;

            pnlToolbar.AutoSizeMode =
                AutoSizeMode.GrowAndShrink;

            pnlToolbar.WrapContents = true;

            pnlToolbar.FlowDirection =
                FlowDirection.LeftToRight;

            pnlToolbar.BackColor = Color.White;
            pnlToolbar.Padding = new Padding(10);

            Label lblSearch = new Label();

            lblSearch.Text = "Search:";
            lblSearch.AutoSize = true;

            lblSearch.Font = new Font(
                "Segoe UI", 10F, FontStyle.Bold);

            lblSearch.Margin =
                new Padding(5, 15, 5, 5);

            txtSearch.Size = new Size(220, 30);

            txtSearch.Font =
                new Font("Segoe UI", 11F);

            txtSearch.MaxLength = 100;

            txtSearch.Margin =
                new Padding(5, 10, 10, 5);

            txtSearch.TextChanged +=
                txtSearch_TextChanged;

            ConfigureButton(
                btnAdd,
                "+ Add Category",
                Color.FromArgb(37, 99, 235),
                135);

            btnAdd.Click += btnAdd_Click;

            ConfigureButton(
                btnEdit,
                "Edit",
                Color.FromArgb(71, 85, 105),
                80);

            btnEdit.Click += btnEdit_Click;

            ConfigureButton(
                btnDelete,
                "Delete",
                Color.FromArgb(220, 38, 38),
                85);

            btnDelete.Click += btnDelete_Click;

            ConfigureButton(
                btnRefresh,
                "Refresh",
                Color.FromArgb(100, 116, 139),
                95);

            btnRefresh.Click += btnRefresh_Click;

            pnlToolbar.Controls.Add(lblSearch);
            pnlToolbar.Controls.Add(txtSearch);

            pnlToolbar.Controls.Add(btnAdd);
            pnlToolbar.Controls.Add(btnEdit);
            pnlToolbar.Controls.Add(btnDelete);
            pnlToolbar.Controls.Add(btnRefresh);

            // TABLE CONTAINER

            pnlTable.Dock = DockStyle.Fill;

            pnlTable.BackColor = Color.White;

            pnlTable.Padding =
                new Padding(0, 12, 0, 0);

            // DATAGRIDVIEW

            dgvCategories.Dock = DockStyle.Fill;

            dgvCategories.BackgroundColor =
                Color.White;

            dgvCategories.BorderStyle =
                BorderStyle.None;

            dgvCategories.ReadOnly = true;

            dgvCategories.AllowUserToAddRows = false;
            dgvCategories.AllowUserToDeleteRows = false;
            dgvCategories.AllowUserToResizeRows = false;

            dgvCategories.RowHeadersVisible = false;

            dgvCategories.MultiSelect = false;

            dgvCategories.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvCategories.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvCategories.EnableHeadersVisualStyles =
                false;

            dgvCategories.ColumnHeadersHeight = 45;

            dgvCategories.RowTemplate.Height = 45;

            dgvCategories.ColumnHeadersDefaultCellStyle
                .BackColor =
                    Color.FromArgb(16, 36, 62);

            dgvCategories.ColumnHeadersDefaultCellStyle
                .ForeColor = Color.White;

            dgvCategories.ColumnHeadersDefaultCellStyle
                .Font = new Font(
                    "Segoe UI", 10F, FontStyle.Bold);

            dgvCategories.DefaultCellStyle.Font =
                new Font("Segoe UI", 10F);

            dgvCategories.DefaultCellStyle.ForeColor =
                Color.FromArgb(51, 65, 85);

            dgvCategories.DefaultCellStyle
                .SelectionBackColor =
                    Color.FromArgb(219, 234, 254);

            dgvCategories.DefaultCellStyle
                .SelectionForeColor =
                    Color.FromArgb(16, 36, 62);

            dgvCategories.AlternatingRowsDefaultCellStyle
                .BackColor =
                    Color.FromArgb(248, 250, 252);

            dgvCategories.CellBorderStyle =
                DataGridViewCellBorderStyle
                    .SingleHorizontal;

            dgvCategories.GridColor =
                Color.FromArgb(226, 232, 240);

            dgvCategories.CellDoubleClick +=
                dgvCategories_CellDoubleClick;

            // FOOTER

            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Height = 45;
            pnlFooter.BackColor = Color.White;

            lblRecordCount.Text =
                "Showing 0 categories";

            lblRecordCount.Location =
                new Point(15, 13);

            lblRecordCount.AutoSize = true;

            lblRecordCount.Font =
                new Font("Segoe UI", 10F);

            lblRecordCount.ForeColor = Color.SlateGray;

            pnlFooter.Controls.Add(lblRecordCount);

            // ADD CONTROLS

            pnlTable.Controls.Add(dgvCategories);
            pnlTable.Controls.Add(pnlFooter);

            pnlContent.Controls.Add(pnlTable);
            pnlContent.Controls.Add(pnlToolbar);
            pnlContent.Controls.Add(tblMetrics);

            Controls.Add(pnlContent);
            Controls.Add(pnlHeader);

            // FORM SETTINGS

            AutoScaleMode = AutoScaleMode.None;

            ClientSize = new Size(1100, 650);

            BackColor =
                Color.FromArgb(241, 245, 249);

            Font = new Font("Segoe UI", 10F);

            FormBorderStyle =
                FormBorderStyle.None;

            Name = "CategoryManagementForm";

            Text = "Category Management";

            ResumeLayout(false);
            PerformLayout();
        }

        // BUTTON STYLING

        private void ConfigureButton(
            Button button,
            string text,
            Color color,
            int width)
        {
            button.Text = text;

            button.Size = new Size(width, 40);

            button.Margin = new Padding(5);

            button.BackColor = color;
            button.ForeColor = Color.White;

            button.Font = new Font(
                "Segoe UI", 10F, FontStyle.Bold);

            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Cursor = Cursors.Hand;
            button.UseVisualStyleBackColor = false;
        }

        // METRIC CARD

        private Panel CreateMetricCard(
            string title,
            Label valueLabel,
            Color accentColor)
        {
            Panel card = new Panel();

            card.Dock = DockStyle.Fill;

            card.Margin =
                new Padding(0, 0, 10, 15);

            card.BackColor = Color.White;

            Panel accent = new Panel();

            accent.Dock = DockStyle.Left;
            accent.Width = 5;
            accent.BackColor = accentColor;

            Label titleLabel = new Label();

            titleLabel.Text = title;
            titleLabel.AutoSize = true;

            titleLabel.Location =
                new Point(15, 12);

            titleLabel.Font =
                new Font("Segoe UI", 10F);

            titleLabel.ForeColor = Color.SlateGray;

            valueLabel.Text = "0";
            valueLabel.AutoSize = true;

            valueLabel.Location =
                new Point(12, 35);

            valueLabel.Font = new Font(
                "Segoe UI", 26F, FontStyle.Bold);

            valueLabel.ForeColor = accentColor;

            card.Controls.Add(titleLabel);
            card.Controls.Add(valueLabel);
            card.Controls.Add(accent);

            return card;
        }
    }
}
