
using System.Drawing;
using System.Windows.Forms;

namespace NAOTR_POS.Forms
{
    partial class InventoryHistoryForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Panel pnlContent;
        private Panel pnlFilters;
        private Panel pnlFooter;

        private TableLayoutPanel tblMetrics;

        private Label lblTitle;
        private Label lblSubtitle;

        private Label lblTotalMovements;
        private Label lblStockIn;
        private Label lblStockOut;
        private Label lblRecordCount;

        private TextBox txtSearch;
        private ComboBox cmbMovementType;

        private DateTimePicker dtpFrom;
        private DateTimePicker dtpTo;

        private CheckBox chkDateFilter;

        private Button btnRefresh;
        private Button btnHistory;
        private Button btnReset;
        private Button btnClose;

        private DataGridView dgvHistory;

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
            pnlFilters = new Panel();
            pnlFooter = new Panel();
            btnHistory = new Button();
            tblMetrics = new TableLayoutPanel();

            lblTitle = new Label();
            lblSubtitle = new Label();

            lblTotalMovements = new Label();
            lblStockIn = new Label();
            lblStockOut = new Label();
            lblRecordCount = new Label();

            txtSearch = new TextBox();
            cmbMovementType = new ComboBox();

            dtpFrom = new DateTimePicker();
            dtpTo = new DateTimePicker();

            chkDateFilter = new CheckBox();

            btnRefresh = new Button();
            btnReset = new Button();
            btnClose = new Button();

            dgvHistory = new DataGridView();

            SuspendLayout();

            // ================= HEADER =================

            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 85;

            pnlHeader.BackColor =
                Color.FromArgb(16, 36, 62);

            lblTitle.Text = "Inventory Movement History";

            lblTitle.Font = new Font(
                "Segoe UI", 22F, FontStyle.Bold);

            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(25, 10);
            lblTitle.AutoSize = true;

            lblSubtitle.Text =
                "Monitor and track all product stock movements.";

            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.LightSteelBlue;
            lblSubtitle.Location = new Point(28, 53);
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
            tblMetrics.ColumnCount = 3;
            tblMetrics.RowCount = 1;

            tblMetrics.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 33.33F));

            tblMetrics.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 33.33F));

            tblMetrics.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 33.34F));

            tblMetrics.Controls.Add(
                CreateMetricCard(
                    "Total Movements",
                    lblTotalMovements,
                    Color.RoyalBlue),
                0, 0);

            tblMetrics.Controls.Add(
                CreateMetricCard(
                    "Total Stock In",
                    lblStockIn,
                    Color.SeaGreen),
                1, 0);

            tblMetrics.Controls.Add(
                CreateMetricCard(
                    "Total Stock Out",
                    lblStockOut,
                    Color.Firebrick),
                2, 0);

            // ================= FILTERS =================

            pnlFilters.Dock = DockStyle.Top;
            pnlFilters.Height = 125;
            pnlFilters.BackColor = Color.White;

            pnlFilters.Controls.Add(
                CreateLabel("Search Product", 15, 10));

            txtSearch.Location = new Point(15, 35);
            txtSearch.Size = new Size(250, 30);
            txtSearch.Font = new Font("Segoe UI", 11F);
            txtSearch.MaxLength = 150;

            txtSearch.KeyDown += txtSearch_KeyDown;

            pnlFilters.Controls.Add(
                CreateLabel("Movement Type", 280, 10));

            cmbMovementType.Location =
                new Point(280, 35);

            cmbMovementType.Size =
                new Size(155, 30);

            cmbMovementType.Font =
                new Font("Segoe UI", 11F);

            cmbMovementType.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbMovementType.Items.AddRange(
                new object[]
                {
                    "All Movements",
                    "Stock In",
                    "Stock Out"
                });

            cmbMovementType.SelectedIndexChanged +=
                cmbMovementType_SelectedIndexChanged;

            // ================= DATE FILTER =================

            pnlFilters.Controls.Add(
                CreateLabel("From Date", 450, 10));

            dtpFrom.Location = new Point(450, 35);
            dtpFrom.Size = new Size(130, 30);

            dtpFrom.Format =
                DateTimePickerFormat.Custom;

            dtpFrom.CustomFormat = "MM/dd/yyyy";

            dtpFrom.Font =
                new Font("Segoe UI", 10F);

            pnlFilters.Controls.Add(
                CreateLabel("To Date", 595, 10));

            dtpTo.Location = new Point(595, 35);
            dtpTo.Size = new Size(130, 30);

            dtpTo.Format =
                DateTimePickerFormat.Custom;

            dtpTo.CustomFormat = "MM/dd/yyyy";

            dtpTo.Font =
                new Font("Segoe UI", 10F);

            chkDateFilter.Text = "Enable date filter";

            chkDateFilter.Location =
                new Point(450, 80);

            chkDateFilter.AutoSize = true;

            chkDateFilter.Font =
                new Font("Segoe UI", 10F);

            chkDateFilter.CheckedChanged +=
                chkDateFilter_CheckedChanged;

            // ================= FILTER BUTTONS =================

            ConfigureButton(
                btnRefresh,
                "Search / Refresh",
                Color.FromArgb(37, 99, 235));

            btnRefresh.Location = new Point(740, 30);
            btnRefresh.Size = new Size(155, 38);
            btnRefresh.Click += btnRefresh_Click;

            ConfigureButton(
                btnReset,
                "Reset",
                Color.FromArgb(100, 116, 139));

            btnReset.Location = new Point(905, 30);
            btnReset.Size = new Size(90, 38);
            btnReset.Click += btnReset_Click;

            pnlFilters.Controls.Add(txtSearch);
            pnlFilters.Controls.Add(cmbMovementType);
            pnlFilters.Controls.Add(dtpFrom);
            pnlFilters.Controls.Add(dtpTo);
            pnlFilters.Controls.Add(chkDateFilter);
            pnlFilters.Controls.Add(btnRefresh);
            pnlFilters.Controls.Add(btnReset);

            // ================= DATAGRIDVIEW =================

            dgvHistory.Dock = DockStyle.Fill;

            dgvHistory.BackgroundColor = Color.White;
            dgvHistory.BorderStyle = BorderStyle.None;

            dgvHistory.ReadOnly = true;
            dgvHistory.AllowUserToAddRows = false;
            dgvHistory.AllowUserToDeleteRows = false;
            dgvHistory.AllowUserToResizeRows = false;

            dgvHistory.RowHeadersVisible = false;

            dgvHistory.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvHistory.MultiSelect = false;

            dgvHistory.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvHistory.EnableHeadersVisualStyles = false;

            dgvHistory.ColumnHeadersHeight = 45;
            dgvHistory.RowTemplate.Height = 42;

            dgvHistory.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(16, 36, 62);

            dgvHistory.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;

            dgvHistory.ColumnHeadersDefaultCellStyle.Font =
                new Font(
                    "Segoe UI", 10F, FontStyle.Bold);

            dgvHistory.DefaultCellStyle.Font =
                new Font("Segoe UI", 10F);

            dgvHistory.DefaultCellStyle.ForeColor =
                Color.FromArgb(51, 65, 85);

            dgvHistory.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(219, 234, 254);

            dgvHistory.DefaultCellStyle.SelectionForeColor =
                Color.FromArgb(16, 36, 62);

            dgvHistory.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(248, 250, 252);

            dgvHistory.CellBorderStyle =
                DataGridViewCellBorderStyle.SingleHorizontal;

            dgvHistory.GridColor =
                Color.FromArgb(226, 232, 240);

            dgvHistory.CellFormatting +=
                dgvHistory_CellFormatting;

            // ================= FOOTER =================

            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Height = 60;
            pnlFooter.BackColor = Color.White;

            lblRecordCount.Text = "Showing 0 records";

            lblRecordCount.Font =
                new Font("Segoe UI", 10F);

            lblRecordCount.ForeColor = Color.SlateGray;

            lblRecordCount.Location = new Point(15, 20);
            lblRecordCount.AutoSize = true;

            ConfigureButton(
                btnClose,
                "Close",
                Color.FromArgb(100, 116, 139));

            btnClose.Dock = DockStyle.Right;
            btnClose.Width = 120;
            btnClose.Click += btnClose_Click;

            pnlFooter.Controls.Add(lblRecordCount);
            pnlFooter.Controls.Add(btnClose);

            // ================= ADD CONTROLS =================

            pnlContent.Controls.Add(dgvHistory);
            pnlContent.Controls.Add(pnlFooter);
            pnlContent.Controls.Add(pnlFilters);
            pnlContent.Controls.Add(tblMetrics);

            Controls.Add(pnlContent);
            Controls.Add(pnlHeader);

            AutoScaleMode = AutoScaleMode.None;

            ClientSize = new Size(1200, 720);
            MinimumSize = new Size(1080, 600);

            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterParent;

            BackColor = Color.White;
            Font = new Font("Segoe UI", 10F);

            Name = "InventoryHistoryForm";
            Text = "Inventory Movement History";

            // INVENTORY HISTORY BUTTON

            btnHistory.Text = "History";
            btnHistory.Size = new Size(110, 38);

            btnHistory.Font = new Font(
                "Segoe UI", 10F, FontStyle.Bold);

            btnHistory.BackColor = Color.FromArgb(16, 36, 62);
            btnHistory.ForeColor = Color.White;

            btnHistory.FlatStyle = FlatStyle.Flat;
            btnHistory.FlatAppearance.BorderSize = 0;
            btnHistory.Cursor = Cursors.Hand;
            btnHistory.UseVisualStyleBackColor = false;

            btnHistory.Location = new Point(
                btnRefresh.Right + 10,
                btnRefresh.Top);

            btnHistory.Click += btnHistory_Click;

            // Use the existing Refresh button container.
            btnRefresh.Parent.Controls.Add(btnHistory);

            btnHistory.BringToFront();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label CreateLabel(
            string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,

                Font = new Font(
                    "Segoe UI", 10F, FontStyle.Bold),

                ForeColor =
                    Color.FromArgb(51, 65, 85)
            };
        }

        private Panel CreateMetricCard(
            string title,
            Label valueLabel,
            Color accentColor)
        {
            Panel card = new Panel();

            card.Dock = DockStyle.Fill;
            card.Margin = new Padding(0, 0, 12, 15);
            card.BackColor = Color.White;

            Panel accent = new Panel();

            accent.Dock = DockStyle.Left;
            accent.Width = 5;
            accent.BackColor = accentColor;

            Label titleLabel = new Label();

            titleLabel.Text = title;
            titleLabel.Location = new Point(18, 12);
            titleLabel.AutoSize = true;

            titleLabel.Font =
                new Font("Segoe UI", 11F);

            titleLabel.ForeColor = Color.SlateGray;

            valueLabel.Text = "0";

            valueLabel.Font = new Font(
                "Segoe UI", 27F, FontStyle.Bold);

            valueLabel.ForeColor = accentColor;
            valueLabel.Location = new Point(15, 35);
            valueLabel.AutoSize = true;

            card.Controls.Add(titleLabel);
            card.Controls.Add(valueLabel);
            card.Controls.Add(accent);

            return card;
        }

        private void ConfigureButton(
            Button button,
            string text,
            Color color)
        {
            button.Text = text;
            button.BackColor = color;
            button.ForeColor = Color.White;

            button.Font = new Font(
                "Segoe UI", 10F, FontStyle.Bold);

            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Cursor = Cursors.Hand;
            button.UseVisualStyleBackColor = false;
        }
    }
}
