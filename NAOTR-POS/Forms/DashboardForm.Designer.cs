
using System.Drawing;
using System.Windows.Forms;

namespace NAOTR_POS.Forms
{
    partial class DashboardForm
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblWelcome;
        private Label lblSubtitle;
        private Label lblTodaySales;
        private Label lblTodayOrders;
        private Label lblActiveProducts;
        private Label lblLowStock;

        private Button btnRefresh;
        private DataGridView dgvLowStock;

        private Panel pnlTop;
        private Panel pnlLowStock;
        private TableLayoutPanel tblMetrics;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblWelcome = new Label();
            lblSubtitle = new Label();

            lblTodaySales = new Label();
            lblTodayOrders = new Label();
            lblActiveProducts = new Label();
            lblLowStock = new Label();

            btnRefresh = new Button();
            dgvLowStock = new DataGridView();

            pnlTop = new Panel();
            pnlLowStock = new Panel();
            tblMetrics = new TableLayoutPanel();

            SuspendLayout();

            // WELCOME SECTION
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Height = 105;

            lblWelcome.Text = "Welcome!";
            lblWelcome.Font = new Font(
                "Segoe UI", 23F, FontStyle.Bold);

            lblWelcome.ForeColor =
                Color.FromArgb(16, 36, 62);

            lblWelcome.Location = new Point(0, 5);
            lblWelcome.AutoSize = true;

            lblSubtitle.Text =
                "Here's what's happening in your store today.";

            lblSubtitle.Font = new Font("Segoe UI", 11F);
            lblSubtitle.ForeColor = Color.SlateGray;
            lblSubtitle.Location = new Point(3, 55);
            lblSubtitle.AutoSize = true;

            btnRefresh.Text = "Refresh";
            btnRefresh.Font = new Font(
                "Segoe UI", 10F, FontStyle.Bold);

            btnRefresh.BackColor =
                Color.FromArgb(37, 99, 235);

            btnRefresh.ForeColor = Color.White;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Size = new Size(110, 40);
            btnRefresh.Dock = DockStyle.Right;
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.Click += btnRefresh_Click;

            pnlTop.Controls.Add(lblWelcome);
            pnlTop.Controls.Add(lblSubtitle);
            pnlTop.Controls.Add(btnRefresh);

            // METRIC CARDS
            tblMetrics.Dock = DockStyle.Top;
            tblMetrics.Height = 145;
            tblMetrics.ColumnCount = 4;
            tblMetrics.RowCount = 1;

            for (int i = 0; i < 4; i++)
            {
                tblMetrics.ColumnStyles.Add(
                    new ColumnStyle(SizeType.Percent, 25F));
            }

            tblMetrics.Controls.Add(
                CreateMetricCard(
                    "Today's Sales",
                    lblTodaySales,
                    Color.RoyalBlue), 0, 0);

            tblMetrics.Controls.Add(
                CreateMetricCard(
                    "Today's Orders",
                    lblTodayOrders,
                    Color.DarkCyan), 1, 0);

            tblMetrics.Controls.Add(
                CreateMetricCard(
                    "Active Products",
                    lblActiveProducts,
                    Color.SeaGreen), 2, 0);

            tblMetrics.Controls.Add(
                CreateMetricCard(
                    "Low Stock",
                    lblLowStock,
                    Color.DarkOrange), 3, 0);

            // LOW STOCK SECTION
            pnlLowStock.Dock = DockStyle.Fill;
            pnlLowStock.BackColor = Color.White;
            pnlLowStock.Padding = new Padding(20);

            Label lblStockTitle = new Label();

            lblStockTitle.Text = "Low Stock Alert";
            lblStockTitle.Font = new Font(
                "Segoe UI", 16F, FontStyle.Bold);

            lblStockTitle.ForeColor =
                Color.FromArgb(16, 36, 62);

            lblStockTitle.Dock = DockStyle.Top;
            lblStockTitle.Height = 50;

            // DATAGRIDVIEW
            dgvLowStock.Dock = DockStyle.Fill;
            dgvLowStock.BackgroundColor = Color.White;
            dgvLowStock.BorderStyle = BorderStyle.None;

            dgvLowStock.ReadOnly = true;
            dgvLowStock.AllowUserToAddRows = false;
            dgvLowStock.AllowUserToDeleteRows = false;
            dgvLowStock.AllowUserToResizeRows = false;

            dgvLowStock.RowHeadersVisible = false;
            dgvLowStock.MultiSelect = false;

            dgvLowStock.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvLowStock.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvLowStock.EnableHeadersVisualStyles = false;

            dgvLowStock.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(16, 36, 62);

            dgvLowStock.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;

            dgvLowStock.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10F, FontStyle.Bold);

            dgvLowStock.ColumnHeadersHeight = 40;
            dgvLowStock.RowTemplate.Height = 38;

            dgvLowStock.DefaultCellStyle.Font =
                new Font("Segoe UI", 10F);

            dgvLowStock.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(219, 234, 254);

            dgvLowStock.DefaultCellStyle.SelectionForeColor =
                Color.FromArgb(16, 36, 62);

            dgvLowStock.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(248, 250, 252);

            dgvLowStock.CellBorderStyle =
                DataGridViewCellBorderStyle.SingleHorizontal;

            dgvLowStock.GridColor =
                Color.FromArgb(226, 232, 240);

            pnlLowStock.Controls.Add(dgvLowStock);
            pnlLowStock.Controls.Add(lblStockTitle);

            // DASHBOARD FORM
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1000, 600);
            BackColor = Color.FromArgb(241, 245, 249);
            Padding = new Padding(25);

            Controls.Add(pnlLowStock);
            Controls.Add(tblMetrics);
            Controls.Add(pnlTop);

            Name = "DashboardForm";
            Text = "Dashboard";

            ResumeLayout(false);
        }

        private Panel CreateMetricCard(
            string title, Label valueLabel, Color accentColor)
        {
            Panel card = new Panel();

            card.Dock = DockStyle.Fill;
            card.Margin = new Padding(0, 0, 12, 20);
            card.BackColor = Color.White;

            Panel accent = new Panel();

            accent.Dock = DockStyle.Left;
            accent.Width = 5;
            accent.BackColor = accentColor;

            Label titleLabel = new Label();

            titleLabel.Text = title;
            titleLabel.Font = new Font("Segoe UI", 11F);
            titleLabel.ForeColor = Color.SlateGray;
            titleLabel.Location = new Point(18, 20);
            titleLabel.AutoSize = true;

            valueLabel.Text = "0";
            valueLabel.Font = new Font(
                "Segoe UI", 25F, FontStyle.Bold);

            valueLabel.ForeColor =
                Color.FromArgb(16, 36, 62);

            valueLabel.Location = new Point(15, 52);
            valueLabel.AutoSize = true;

            card.Controls.Add(titleLabel);
            card.Controls.Add(valueLabel);
            card.Controls.Add(accent);

            return card;
        }
    }
}
