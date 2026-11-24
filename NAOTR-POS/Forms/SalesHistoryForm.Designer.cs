
using System.Drawing;
using System.Windows.Forms;

namespace NAOTR_POS.Forms
{
    partial class SalesHistoryForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Panel pnlContent;
        private Panel pnlFooter;

        private TableLayoutPanel tblMetrics;
        private TableLayoutPanel tblToolbar;

        private FlowLayoutPanel pnlFilters;
        private FlowLayoutPanel pnlActions;

        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblTransactions;
        private Label lblTotalSales;
        private Label lblCompleted;
        private Label lblVoided;
        private Label lblRecordCount;

        private TextBox txtSearch;

        private DateTimePicker dtpFrom;
        private DateTimePicker dtpTo;

        private CheckBox chkDateFilter;

        private ComboBox cmbPayment;
        private ComboBox cmbStatus;

        private Button btnSearch;
        private Button btnReset;
        private Button btnDetails;
        private Button btnReceipt;
        private Button btnVoid;

        private DataGridView dgvSales;

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

            tblMetrics = new TableLayoutPanel();
            tblToolbar = new TableLayoutPanel();

            pnlFilters = new FlowLayoutPanel();
            pnlActions = new FlowLayoutPanel();

            lblTitle = new Label();
            lblSubtitle = new Label();

            lblTransactions = new Label();
            lblTotalSales = new Label();
            lblCompleted = new Label();
            lblVoided = new Label();
            lblRecordCount = new Label();

            txtSearch = new TextBox();

            dtpFrom = new DateTimePicker();
            dtpTo = new DateTimePicker();

            chkDateFilter = new CheckBox();

            cmbPayment = new ComboBox();
            cmbStatus = new ComboBox();

            btnSearch = new Button();
            btnReset = new Button();
            btnDetails = new Button();
            btnReceipt = new Button();
            btnVoid = new Button();

            dgvSales = new DataGridView();

            SuspendLayout();

            // ================= HEADER =================

            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 85;

            pnlHeader.BackColor =
                Color.FromArgb(16, 36, 62);

            lblTitle.Text = "Sales Transaction History";

            lblTitle.Font = new Font(
                "Segoe UI", 22F, FontStyle.Bold);

            lblTitle.ForeColor = Color.White;

            lblTitle.Location = new Point(20, 8);
            lblTitle.AutoSize = true;

            lblSubtitle.Text =
                "Review, reprint and manage sales transactions.";

            lblSubtitle.Font =
                new Font("Segoe UI", 10F);

            lblSubtitle.ForeColor =
                Color.LightSteelBlue;

            lblSubtitle.Location = new Point(23, 52);
            lblSubtitle.AutoSize = true;

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubtitle);

            // ================= CONTENT =================

            pnlContent.Dock = DockStyle.Fill;

            pnlContent.Padding = new Padding(15);

            pnlContent.BackColor =
                Color.FromArgb(241, 245, 249);

            // ================= METRICS =================

            tblMetrics.Dock = DockStyle.Top;
            tblMetrics.Height = 105;

            tblMetrics.ColumnCount = 4;
            tblMetrics.RowCount = 1;

            for (int i = 0; i < 4; i++)
            {
                tblMetrics.ColumnStyles.Add(
                    new ColumnStyle(
                        SizeType.Percent, 25F));
            }

            tblMetrics.RowStyles.Add(
                new RowStyle(
                    SizeType.Percent, 100F));

            tblMetrics.Controls.Add(
                CreateMetricCard(
                    "Transactions",
                    lblTransactions,
                    Color.FromArgb(37, 99, 235)),
                0, 0);

            tblMetrics.Controls.Add(
                CreateMetricCard(
                    "Total Sales",
                    lblTotalSales,
                    Color.FromArgb(22, 163, 74)),
                1, 0);

            tblMetrics.Controls.Add(
                CreateMetricCard(
                    "Completed",
                    lblCompleted,
                    Color.FromArgb(8, 145, 178)),
                2, 0);

            tblMetrics.Controls.Add(
                CreateMetricCard(
                    "Voided",
                    lblVoided,
                    Color.FromArgb(220, 38, 38)),
                3, 0);

            // ================= TOOLBAR =================

            tblToolbar.Dock = DockStyle.Top;

            tblToolbar.Height = 185;

            tblToolbar.ColumnCount = 1;
            tblToolbar.RowCount = 2;

            tblToolbar.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent, 100F));

            tblToolbar.RowStyles.Add(
                new RowStyle(
                    SizeType.Percent, 65F));

            tblToolbar.RowStyles.Add(
                new RowStyle(
                    SizeType.Percent, 35F));

            tblToolbar.BackColor = Color.White;

            // ================= FILTERS =================

            pnlFilters.Dock = DockStyle.Fill;

            pnlFilters.FlowDirection =
                FlowDirection.LeftToRight;

            pnlFilters.WrapContents = true;

            pnlFilters.AutoScroll = true;

            pnlFilters.Padding = new Padding(5);

            txtSearch.Width = 220;
            txtSearch.MaxLength = 150;

            txtSearch.Font =
                new Font("Segoe UI", 10F);

            txtSearch.KeyDown +=
                txtSearch_KeyDown;

            chkDateFilter.Text = "Filter by date";

            chkDateFilter.AutoSize = true;

            chkDateFilter.Margin =
                new Padding(8, 12, 8, 8);

            chkDateFilter.CheckedChanged +=
                chkDateFilter_CheckedChanged;

            ConfigureDatePicker(dtpFrom);
            ConfigureDatePicker(dtpTo);

            ConfigureComboBox(cmbPayment, 125);
            ConfigureComboBox(cmbStatus, 125);

            cmbPayment.Items.AddRange(
                new object[]
                {
                    "All",
                    "Cash",
                    "Card",
                    "Mobile"
                });

            cmbStatus.Items.AddRange(
                new object[]
                {
                    "All",
                    "Completed",
                    "Voided"
                });

            pnlFilters.Controls.Add(
                CreateFilter("Search", txtSearch, 235));

            pnlFilters.Controls.Add(chkDateFilter);

            pnlFilters.Controls.Add(
                CreateFilter("From", dtpFrom, 135));

            pnlFilters.Controls.Add(
                CreateFilter("To", dtpTo, 135));

            pnlFilters.Controls.Add(
                CreateFilter("Payment", cmbPayment, 135));

            pnlFilters.Controls.Add(
                CreateFilter("Status", cmbStatus, 135));

            // ================= ACTION BUTTONS =================

            pnlActions.Dock = DockStyle.Fill;

            pnlActions.WrapContents = true;
            pnlActions.AutoScroll = true;

            pnlActions.Padding = new Padding(5);

            ConfigureButton(
                btnSearch, "Search",
                Color.FromArgb(37, 99, 235), 90);

            ConfigureButton(
                btnReset, "Reset",
                Color.FromArgb(100, 116, 139), 90);

            ConfigureButton(
                btnDetails, "View Details",
                Color.FromArgb(16, 36, 62), 120);

            ConfigureButton(
                btnReceipt, "Reprint Receipt",
                Color.FromArgb(22, 163, 74), 135);

            ConfigureButton(
                btnVoid, "Void Transaction",
                Color.FromArgb(220, 38, 38), 140);

            btnSearch.Click += btnSearch_Click;
            btnReset.Click += btnReset_Click;
            btnDetails.Click += btnDetails_Click;
            btnReceipt.Click += btnReceipt_Click;
            btnVoid.Click += btnVoid_Click;

            pnlActions.Controls.Add(btnSearch);
            pnlActions.Controls.Add(btnReset);
            pnlActions.Controls.Add(btnDetails);
            pnlActions.Controls.Add(btnReceipt);
            pnlActions.Controls.Add(btnVoid);

            tblToolbar.Controls.Add(
                pnlFilters, 0, 0);

            tblToolbar.Controls.Add(
                pnlActions, 0, 1);

            // ================= SALES GRID =================

            dgvSales.Dock = DockStyle.Fill;

            dgvSales.BackgroundColor = Color.White;

            dgvSales.BorderStyle = BorderStyle.None;

            dgvSales.ReadOnly = true;

            dgvSales.AllowUserToAddRows = false;
            dgvSales.AllowUserToDeleteRows = false;
            dgvSales.AllowUserToResizeRows = false;

            dgvSales.RowHeadersVisible = false;
            dgvSales.MultiSelect = false;

            dgvSales.AutoGenerateColumns = true;

            dgvSales.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvSales.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvSales.EnableHeadersVisualStyles = false;

            dgvSales.ColumnHeadersHeight = 42;
            dgvSales.RowTemplate.Height = 42;

            dgvSales.ColumnHeadersDefaultCellStyle
                .BackColor =
                    Color.FromArgb(16, 36, 62);

            dgvSales.ColumnHeadersDefaultCellStyle
                .ForeColor = Color.White;

            dgvSales.ColumnHeadersDefaultCellStyle
                .Font = new Font(
                    "Segoe UI", 9F, FontStyle.Bold);

            dgvSales.DefaultCellStyle.Font =
                new Font("Segoe UI", 9F);

            dgvSales.DefaultCellStyle
                .SelectionBackColor =
                    Color.FromArgb(219, 234, 254);

            dgvSales.DefaultCellStyle
                .SelectionForeColor =
                    Color.FromArgb(16, 36, 62);

            dgvSales.AlternatingRowsDefaultCellStyle
                .BackColor =
                    Color.FromArgb(248, 250, 252);

            dgvSales.CellBorderStyle =
                DataGridViewCellBorderStyle
                    .SingleHorizontal;

            dgvSales.GridColor =
                Color.FromArgb(226, 232, 240);

            dgvSales.CellFormatting +=
                dgvSales_CellFormatting;

            dgvSales.CellDoubleClick +=
                dgvSales_CellDoubleClick;

            dgvSales.SelectionChanged +=
                dgvSales_SelectionChanged;

            // ================= FOOTER =================

            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Height = 40;

            pnlFooter.BackColor = Color.White;

            lblRecordCount.Dock = DockStyle.Fill;

            lblRecordCount.Padding =
                new Padding(10, 0, 0, 0);

            lblRecordCount.TextAlign =
                ContentAlignment.MiddleLeft;

            lblRecordCount.ForeColor = Color.SlateGray;

            lblRecordCount.Font =
                new Font("Segoe UI", 10F);

            lblRecordCount.Text =
                "Showing 0 transactions";

            pnlFooter.Controls.Add(lblRecordCount);

            // ================= ADD CONTROLS =================

            pnlContent.Controls.Add(dgvSales);
            pnlContent.Controls.Add(pnlFooter);
            pnlContent.Controls.Add(tblToolbar);
            pnlContent.Controls.Add(tblMetrics);

            Controls.Add(pnlContent);
            Controls.Add(pnlHeader);

            AutoScaleMode = AutoScaleMode.None;

            ClientSize = new Size(1100, 675);

            BackColor =
                Color.FromArgb(241, 245, 249);

            FormBorderStyle =
                FormBorderStyle.None;

            Font = new Font("Segoe UI", 10F);

            Name = "SalesHistoryForm";

            Text = "Sales Transaction History";

            ResumeLayout(false);
        }

        // ================= METRIC CARD =================

        private Panel CreateMetricCard(
            string title,
            Label value,
            Color color)
        {
            Panel card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = new Padding(4)
            };

            Panel accent = new Panel
            {
                Dock = DockStyle.Left,
                Width = 5,
                BackColor = color
            };

            Label heading = new Label
            {
                Text = title,

                Font = new Font(
                    "Segoe UI", 10F),

                ForeColor = Color.SlateGray,

                Dock = DockStyle.Top,
                Height = 35,

                Padding = new Padding(12, 8, 0, 0)
            };

            value.Text = "0";

            value.Dock = DockStyle.Fill;

            value.Font = new Font(
                "Segoe UI", 20F, FontStyle.Bold);

            value.ForeColor = color;

            value.TextAlign =
                ContentAlignment.MiddleLeft;

            value.Padding =
                new Padding(12, 0, 0, 0);

            card.Controls.Add(value);
            card.Controls.Add(heading);
            card.Controls.Add(accent);

            return card;
        }

        // ================= FILTER PANEL =================

        private Panel CreateFilter(
            string title,
            Control control,
            int width)
        {
            Panel panel = new Panel
            {
                Width = width,
                Height = 53,
                Margin = new Padding(4)
            };

            Label label = new Label
            {
                Text = title,
                AutoSize = true,

                Font = new Font(
                    "Segoe UI", 9F, FontStyle.Bold),

                Location = new Point(0, 0)
            };

            control.Location = new Point(0, 20);

            control.Width = width - 5;

            panel.Controls.Add(label);
            panel.Controls.Add(control);

            return panel;
        }

        private void ConfigureDatePicker(
            DateTimePicker picker)
        {
            picker.Format =
                DateTimePickerFormat.Custom;

            picker.CustomFormat = "MM/dd/yyyy";

            picker.Font =
                new Font("Segoe UI", 10F);
        }

        private void ConfigureComboBox(
            ComboBox combo,
            int width)
        {
            combo.Width = width;

            combo.DropDownStyle =
                ComboBoxStyle.DropDownList;

            combo.Font =
                new Font("Segoe UI", 10F);
        }

        private void ConfigureButton(
            Button button,
            string text,
            Color color,
            int width)
        {
            button.Text = text;

            button.Size = new Size(width, 38);

            button.Margin = new Padding(4);

            button.BackColor = color;

            button.ForeColor = Color.White;

            button.Font = new Font(
                "Segoe UI", 9F, FontStyle.Bold);

            button.FlatStyle = FlatStyle.Flat;

            button.FlatAppearance.BorderSize = 0;

            button.Cursor = Cursors.Hand;

            button.UseVisualStyleBackColor = false;
        }
    }
}
