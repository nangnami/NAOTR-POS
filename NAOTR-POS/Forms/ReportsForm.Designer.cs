using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace NAOTR_POS.Forms
{
    partial class ReportsForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Panel pnlContent;

        private FlowLayoutPanel pnlFilters;

        private TableLayoutPanel tblMetrics;

        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblRevenue;
        private Label lblTransactions;
        private Label lblAverageOrder;
        private Label lblLowStock;

        private ComboBox cmbPeriod;
        private DateTimePicker dtpFrom;
        private DateTimePicker dtpTo;

        private Button btnGenerate;
        private Button btnExport;
        private Button btnPrint;

        private TabControl tabReports;

        private Chart chartDailySales;
        private Chart chartTopProducts;
        private Chart chartCategories;
        private Chart chartPayments;

        private DataGridView dgvDailySales;
        private DataGridView dgvTopProducts;
        private DataGridView dgvCategories;
        private DataGridView dgvPayments;
        private DataGridView dgvLowStock;

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
            pnlFilters = new FlowLayoutPanel();

            tblMetrics = new TableLayoutPanel();

            lblTitle = new Label();
            lblSubtitle = new Label();

            lblRevenue = new Label();
            lblTransactions = new Label();
            lblAverageOrder = new Label();
            lblLowStock = new Label();

            cmbPeriod = new ComboBox();

            dtpFrom = new DateTimePicker();
            dtpTo = new DateTimePicker();

            btnGenerate = new Button();
            btnExport = new Button();
            btnPrint = new Button();

            tabReports = new TabControl();

            chartDailySales = CreateChart(
                "Daily Sales Revenue",
                SeriesChartType.Column);

            chartTopProducts = CreateChart(
                "Top Selling Products",
                SeriesChartType.Bar);

            chartCategories = CreateChart(
                "Sales by Category",
                SeriesChartType.Doughnut);

            chartPayments = CreateChart(
                "Payment Methods",
                SeriesChartType.Doughnut);

            dgvDailySales = CreateGrid();
            dgvTopProducts = CreateGrid();
            dgvCategories = CreateGrid();
            dgvPayments = CreateGrid();
            dgvLowStock = CreateGrid();

            SuspendLayout();

            // ================= HEADER =================

            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 85;

            pnlHeader.BackColor =
                Color.FromArgb(16, 36, 62);

            lblTitle.Text = "Sales Reports & Analytics";

            lblTitle.Font = new Font(
                "Segoe UI", 22F, FontStyle.Bold);

            lblTitle.ForeColor = Color.White;

            lblTitle.Location = new Point(20, 10);
            lblTitle.AutoSize = true;

            lblSubtitle.Text =
                "Monitor sales performance and inventory.";

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

            pnlContent.Padding = new Padding(15);

            pnlContent.BackColor =
                Color.FromArgb(241, 245, 249);

            // ================= FILTERS =================

            pnlFilters.Dock = DockStyle.Top;
            pnlFilters.Height = 100;
            pnlFilters.Padding = new Padding(8);

            pnlFilters.BackColor = Color.White;

            pnlFilters.FlowDirection =
                FlowDirection.LeftToRight;

            pnlFilters.WrapContents = true;

            cmbPeriod.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbPeriod.Font =
                new Font("Segoe UI", 10F);

            cmbPeriod.Width = 135;

            cmbPeriod.Items.AddRange(new object[]
            {
                "Today",
                "This Week",
                "This Month",
                "Custom Range"
            });

            cmbPeriod.SelectedIndexChanged +=
                cmbPeriod_SelectedIndexChanged;

            ConfigureDatePicker(dtpFrom);
            ConfigureDatePicker(dtpTo);

            ConfigureButton(
                btnGenerate,
                "Generate",
                Color.FromArgb(37, 99, 235),
                105);

            ConfigureButton(
                btnExport,
                "Export CSV",
                Color.FromArgb(22, 163, 74),
                110);

            ConfigureButton(
                btnPrint,
                "Print Report",
                Color.FromArgb(16, 36, 62),
                110);

            btnGenerate.Click += btnGenerate_Click;
            btnExport.Click += btnExport_Click;
            btnPrint.Click += btnPrint_Click;

            pnlFilters.Controls.Add(CreateFilterLabel("Period"));
            pnlFilters.Controls.Add(cmbPeriod);

            pnlFilters.Controls.Add(CreateFilterLabel("From"));
            pnlFilters.Controls.Add(dtpFrom);

            pnlFilters.Controls.Add(CreateFilterLabel("To"));
            pnlFilters.Controls.Add(dtpTo);

            pnlFilters.Controls.Add(btnGenerate);
            pnlFilters.Controls.Add(btnExport);
            pnlFilters.Controls.Add(btnPrint);

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
                    "Total Revenue",
                    lblRevenue,
                    Color.FromArgb(22, 163, 74)),
                0, 0);

            tblMetrics.Controls.Add(
                CreateMetricCard(
                    "Transactions",
                    lblTransactions,
                    Color.FromArgb(37, 99, 235)),
                1, 0);

            tblMetrics.Controls.Add(
                CreateMetricCard(
                    "Average Order",
                    lblAverageOrder,
                    Color.FromArgb(8, 145, 178)),
                2, 0);

            tblMetrics.Controls.Add(
                CreateMetricCard(
                    "Low Stock Products",
                    lblLowStock,
                    Color.FromArgb(220, 38, 38)),
                3, 0);

            // ================= REPORT TABS =================

            tabReports.Dock = DockStyle.Fill;

            tabReports.Font =
                new Font("Segoe UI", 10F);

            tabReports.Padding = new Point(12, 8);

            tabReports.TabPages.Add(
                CreateReportTab(
                    "Daily Sales",
                    chartDailySales,
                    dgvDailySales));

            tabReports.TabPages.Add(
                CreateReportTab(
                    "Top Products",
                    chartTopProducts,
                    dgvTopProducts));

            tabReports.TabPages.Add(
                CreateReportTab(
                    "Categories",
                    chartCategories,
                    dgvCategories));

            tabReports.TabPages.Add(
                CreateReportTab(
                    "Payments",
                    chartPayments,
                    dgvPayments));

            TabPage lowStockTab =
                new TabPage("Low Stock");

            lowStockTab.BackColor = Color.White;
            lowStockTab.Padding = new Padding(10);

            lowStockTab.Controls.Add(dgvLowStock);

            tabReports.TabPages.Add(lowStockTab);

            // ================= ADD CONTROLS =================

            pnlContent.Controls.Add(tabReports);
            pnlContent.Controls.Add(tblMetrics);
            pnlContent.Controls.Add(pnlFilters);

            Controls.Add(pnlContent);
            Controls.Add(pnlHeader);

            AutoScaleMode = AutoScaleMode.None;

            ClientSize = new Size(1150, 750);

            FormBorderStyle = FormBorderStyle.None;

            Name = "ReportsForm";
            Text = "Sales Reports";

            ResumeLayout(false);
        }

        // ================= FILTER LABEL =================

        private Label CreateFilterLabel(string text)
        {
            return new Label
            {
                Text = text,
                AutoSize = true,
                Font = new Font(
                    "Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 65, 85),
                Margin = new Padding(5, 12, 3, 0)
            };
        }

        // ================= DATE PICKER =================

        private void ConfigureDatePicker(
            DateTimePicker picker)
        {
            picker.Format =
                DateTimePickerFormat.Custom;

            picker.CustomFormat = "MM/dd/yyyy";

            picker.Font =
                new Font("Segoe UI", 10F);

            picker.Width = 115;

            picker.Margin = new Padding(4, 7, 4, 4);
        }

        // ================= BUTTON =================

        private void ConfigureButton(
            Button button,
            string text,
            Color color,
            int width)
        {
            button.Text = text;

            button.Size = new Size(width, 36);

            button.BackColor = color;
            button.ForeColor = Color.White;

            button.Font = new Font(
                "Segoe UI", 10F, FontStyle.Bold);

            button.FlatStyle = FlatStyle.Flat;

            button.FlatAppearance.BorderSize = 0;

            button.Cursor = Cursors.Hand;

            button.Margin = new Padding(4);

            button.UseVisualStyleBackColor = false;
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
                Margin = new Padding(4, 10, 4, 10)
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
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(100, 116, 139),
                Dock = DockStyle.Top,
                Height = 35,
                Padding = new Padding(10, 8, 0, 0)
            };

            value.Text = "0";

            value.Font = new Font(
                "Segoe UI", 20F, FontStyle.Bold);

            value.ForeColor = accent;

            value.Dock = DockStyle.Fill;

            value.Padding = new Padding(10, 0, 0, 0);

            value.TextAlign =
                ContentAlignment.MiddleLeft;

            card.Controls.Add(value);
            card.Controls.Add(heading);
            card.Controls.Add(line);

            return card;
        }

        // ================= DATA GRID =================

        private DataGridView CreateGrid()
        {
            DataGridView grid = new DataGridView();

            grid.Dock = DockStyle.Fill;

            grid.BackgroundColor = Color.White;

            grid.BorderStyle = BorderStyle.None;

            grid.ReadOnly = true;

            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;

            grid.RowHeadersVisible = false;

            grid.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            grid.MultiSelect = false;

            grid.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            grid.EnableHeadersVisualStyles = false;

            grid.ColumnHeadersHeight = 42;

            grid.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(16, 36, 62);

            grid.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;

            grid.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10F, FontStyle.Bold);

            grid.DefaultCellStyle.Font =
                new Font("Segoe UI", 10F);

            grid.DefaultCellStyle.ForeColor =
                Color.FromArgb(51, 65, 85);

            grid.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(219, 234, 254);

            grid.DefaultCellStyle.SelectionForeColor =
                Color.FromArgb(16, 36, 62);

            grid.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(248, 250, 252);

            grid.RowTemplate.Height = 36;

            return grid;
        }

        // ================= CHART =================

        private Chart CreateChart(
            string title,
            SeriesChartType chartType)
        {
            Chart chart = new Chart();

            chart.Dock = DockStyle.Fill;
            chart.BackColor = Color.White;

            ChartArea area = new ChartArea("MainArea");

            area.BackColor = Color.White;

            area.AxisX.MajorGrid.Enabled = false;

            area.AxisY.MajorGrid.LineColor =
                Color.FromArgb(226, 232, 240);

            area.AxisX.LabelStyle.Font =
                new Font("Segoe UI", 9F);

            area.AxisY.LabelStyle.Font =
                new Font("Segoe UI", 9F);

            area.AxisX.Interval = 1;

            chart.ChartAreas.Add(area);

            Series series = new Series("Revenue");

            series.ChartType = chartType;

            series.ChartArea = "MainArea";

            series.Color =
                Color.FromArgb(37, 99, 235);

            series.IsValueShownAsLabel = true;

            series.LabelFormat = "#,##0.00";

            chart.Series.Add(series);

            chart.Titles.Add(
                new Title(
                    title,
                    Docking.Top,
                    new Font(
                        "Segoe UI", 13F, FontStyle.Bold),
                    Color.FromArgb(16, 36, 62)));

            if (chartType == SeriesChartType.Doughnut)
            {
                series.Color = Color.Empty;

                chart.Palette = ChartColorPalette.BrightPastel;

                chart.Legends.Add(new Legend("Legend"));

                series.Legend = "Legend";

                chart.Legends[0].Docking =
                    Docking.Right;

                series.Label = "#PERCENT{P0}";

                series.LegendText = "#AXISLABEL";
            }

            return chart;
        }

        // ================= REPORT TAB =================

        private TabPage CreateReportTab(
            string title,
            Chart chart,
            DataGridView grid)
        {
            TabPage tab = new TabPage(title);

            tab.BackColor = Color.White;

            tab.Padding = new Padding(8);

            TableLayoutPanel layout =
                new TableLayoutPanel();

            layout.Dock = DockStyle.Fill;

            layout.ColumnCount = 1;
            layout.RowCount = 2;

            layout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 100F));

            layout.RowStyles.Add(
                new RowStyle(SizeType.Percent, 60F));

            layout.RowStyles.Add(
                new RowStyle(SizeType.Percent, 40F));

            layout.Controls.Add(chart, 0, 0);
            layout.Controls.Add(grid, 0, 1);

            tab.Controls.Add(layout);

            return tab;
        }
    }
}
