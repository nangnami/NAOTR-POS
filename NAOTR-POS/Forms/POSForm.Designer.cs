
using System.Drawing;
using System.Windows.Forms;

namespace NAOTR_POS.Forms
{
    partial class POSForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Label lblTitle;
        private Label lblSubtitle;

        private SplitContainer splitMain;

        private TableLayoutPanel tblCatalog;
        private TableLayoutPanel tblCart;

        private FlowLayoutPanel pnlFilters;
        private FlowLayoutPanel pnlProducts;
        private FlowLayoutPanel pnlCartActions;

        private Panel pnlCartHeader;
        private Panel pnlTotals;

        private TextBox txtSearch;
        private ComboBox cmbCategory;

        private Button btnRefresh;
        private Button btnIncrease;
        private Button btnDecrease;
        private Button btnRemove;
        private Button btnClearCart;
        private Button btnCheckout;

        private Label lblProductCount;
        private Label lblCartTitle;
        private Label lblCartCount;
        private Label lblSubtotal;
        private Label lblTotal;

        private DataGridView dgvCart;

        private Timer searchTimer;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ReleaseProductCards();

                if (components != null)
                    components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components =
                new System.ComponentModel.Container();

            pnlHeader = new Panel();
            lblTitle = new Label();
            lblSubtitle = new Label();

            splitMain = new SplitContainer();

            tblCatalog = new TableLayoutPanel();
            tblCart = new TableLayoutPanel();

            pnlFilters = new FlowLayoutPanel();
            pnlProducts = new FlowLayoutPanel();
            pnlCartActions = new FlowLayoutPanel();

            pnlCartHeader = new Panel();
            pnlTotals = new Panel();

            txtSearch = new TextBox();
            cmbCategory = new ComboBox();

            btnRefresh = new Button();
            btnIncrease = new Button();
            btnDecrease = new Button();
            btnRemove = new Button();
            btnClearCart = new Button();
            btnCheckout = new Button();

            lblProductCount = new Label();
            lblCartTitle = new Label();
            lblCartCount = new Label();
            lblSubtotal = new Label();
            lblTotal = new Label();

            dgvCart = new DataGridView();

            searchTimer = new Timer(components);

            SuspendLayout();

            // ================= HEADER =================

            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 75;

            pnlHeader.BackColor =
                Color.FromArgb(16, 36, 62);

            lblTitle.Text = "Point of Sale";

            lblTitle.Font = new Font(
                "Segoe UI", 22F, FontStyle.Bold);

            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(20, 5);
            lblTitle.AutoSize = true;

            lblSubtitle.Text =
                "Create a new sales transaction.";

            lblSubtitle.Font =
                new Font("Segoe UI", 10F);

            lblSubtitle.ForeColor =
                Color.LightSteelBlue;

            lblSubtitle.Location =
                new Point(23, 47);

            lblSubtitle.AutoSize = true;

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubtitle);

            // ================= SPLIT CONTAINER =================

            splitMain.Dock = DockStyle.Fill;

            splitMain.Size = new Size(1100, 600);

            splitMain.Orientation =
                Orientation.Vertical;

            splitMain.FixedPanel =
                FixedPanel.Panel2;

            splitMain.SplitterWidth = 8;

            splitMain.Panel1MinSize = 250;
            splitMain.Panel2MinSize = 320;

            splitMain.SplitterDistance = 690;

            splitMain.BackColor =
                Color.FromArgb(226, 232, 240);

            splitMain.Panel1.BackColor =
                Color.FromArgb(241, 245, 249);

            splitMain.Panel2.BackColor =
                Color.White;

            // ================= PRODUCT CATALOG =================

            tblCatalog.Dock = DockStyle.Fill;

            tblCatalog.ColumnCount = 1;
            tblCatalog.RowCount = 3;

            tblCatalog.Padding =
                new Padding(10);

            tblCatalog.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent, 100F));

            tblCatalog.RowStyles.Add(
                new RowStyle(
                    SizeType.AutoSize));

            tblCatalog.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute, 30F));

            tblCatalog.RowStyles.Add(
                new RowStyle(
                    SizeType.Percent, 100F));

            // ================= FILTERS =================

            pnlFilters.Dock = DockStyle.Fill;

            pnlFilters.AutoSize = true;

            pnlFilters.AutoSizeMode =
                AutoSizeMode.GrowAndShrink;

            pnlFilters.WrapContents = true;

            pnlFilters.FlowDirection =
                FlowDirection.LeftToRight;

            pnlFilters.BackColor = Color.White;
            pnlFilters.Padding = new Padding(8);

            Label lblSearch = new Label
            {
                Text = "Search / Barcode",
                AutoSize = true,

                Font = new Font(
                    "Segoe UI", 9F, FontStyle.Bold),

                Margin = new Padding(5, 12, 5, 5)
            };

            txtSearch.Width = 220;

            txtSearch.Font =
                new Font("Segoe UI", 11F);

            txtSearch.MaxLength = 150;

            txtSearch.Margin =
                new Padding(5, 7, 5, 5);

            txtSearch.TextChanged +=
                txtSearch_TextChanged;

            txtSearch.KeyDown +=
                txtSearch_KeyDown;

            cmbCategory.Width = 160;

            cmbCategory.Font =
                new Font("Segoe UI", 10F);

            cmbCategory.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbCategory.Margin =
                new Padding(5, 7, 5, 5);

            cmbCategory.SelectedIndexChanged +=
                cmbCategory_SelectedIndexChanged;

            ConfigureButton(
                btnRefresh,
                "Refresh",
                Color.FromArgb(71, 85, 105),
                90);

            btnRefresh.Click += btnRefresh_Click;

            pnlFilters.Controls.Add(lblSearch);
            pnlFilters.Controls.Add(txtSearch);
            pnlFilters.Controls.Add(cmbCategory);
            pnlFilters.Controls.Add(btnRefresh);

            // ================= PRODUCT COUNT =================

            lblProductCount.Text = "0 products";

            lblProductCount.Dock = DockStyle.Fill;

            lblProductCount.TextAlign =
                ContentAlignment.MiddleLeft;

            lblProductCount.ForeColor =
                Color.SlateGray;

            lblProductCount.Font =
                new Font("Segoe UI", 10F);

            // ================= PRODUCT CARDS =================

            pnlProducts.Dock = DockStyle.Fill;

            pnlProducts.AutoScroll = true;

            pnlProducts.WrapContents = true;

            pnlProducts.FlowDirection =
                FlowDirection.LeftToRight;

            pnlProducts.BackColor =
                Color.FromArgb(241, 245, 249);

            tblCatalog.Controls.Add(
                pnlFilters, 0, 0);

            tblCatalog.Controls.Add(
                lblProductCount, 0, 1);

            tblCatalog.Controls.Add(
                pnlProducts, 0, 2);

            splitMain.Panel1.Controls.Add(
                tblCatalog);

            // ================= CART LAYOUT =================

            tblCart.Dock = DockStyle.Fill;

            tblCart.Padding = new Padding(10);

            tblCart.ColumnCount = 1;
            tblCart.RowCount = 4;

            tblCart.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent, 100F));

            tblCart.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute, 65F));

            tblCart.RowStyles.Add(
                new RowStyle(
                    SizeType.Percent, 100F));

            tblCart.RowStyles.Add(
                new RowStyle(
                    SizeType.AutoSize));

            tblCart.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute, 195F));

            // ================= CART HEADER =================

            pnlCartHeader.Dock = DockStyle.Fill;

            lblCartTitle.Text = "Shopping Cart";

            lblCartTitle.Font = new Font(
                "Segoe UI", 18F, FontStyle.Bold);

            lblCartTitle.ForeColor =
                Color.FromArgb(16, 36, 62);

            lblCartTitle.Location =
                new Point(0, 0);

            lblCartTitle.AutoSize = true;

            lblCartCount.Text = "0 items";

            lblCartCount.Font =
                new Font("Segoe UI", 10F);

            lblCartCount.ForeColor =
                Color.SlateGray;

            lblCartCount.Location =
                new Point(3, 37);

            lblCartCount.AutoSize = true;

            pnlCartHeader.Controls.Add(lblCartTitle);
            pnlCartHeader.Controls.Add(lblCartCount);

            // ================= CART GRID =================

            dgvCart.Dock = DockStyle.Fill;

            dgvCart.BackgroundColor = Color.White;

            dgvCart.BorderStyle = BorderStyle.None;

            dgvCart.ReadOnly = true;

            dgvCart.AllowUserToAddRows = false;
            dgvCart.AllowUserToDeleteRows = false;
            dgvCart.AllowUserToResizeRows = false;

            dgvCart.RowHeadersVisible = false;
            dgvCart.MultiSelect = false;

            dgvCart.AutoGenerateColumns = false;

            dgvCart.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvCart.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvCart.EnableHeadersVisualStyles = false;

            dgvCart.ColumnHeadersHeight = 40;
            dgvCart.RowTemplate.Height = 42;

            dgvCart.ColumnHeadersDefaultCellStyle
                .BackColor =
                    Color.FromArgb(16, 36, 62);

            dgvCart.ColumnHeadersDefaultCellStyle
                .ForeColor = Color.White;

            dgvCart.ColumnHeadersDefaultCellStyle
                .Font = new Font(
                    "Segoe UI", 9F, FontStyle.Bold);

            dgvCart.DefaultCellStyle.Font =
                new Font("Segoe UI", 9F);

            dgvCart.DefaultCellStyle
                .SelectionBackColor =
                    Color.FromArgb(219, 234, 254);

            dgvCart.DefaultCellStyle
                .SelectionForeColor =
                    Color.FromArgb(16, 36, 62);

            dgvCart.AlternatingRowsDefaultCellStyle
                .BackColor =
                    Color.FromArgb(248, 250, 252);

            dgvCart.CellBorderStyle =
                DataGridViewCellBorderStyle
                    .SingleHorizontal;

            dgvCart.GridColor =
                Color.FromArgb(226, 232, 240);

            dgvCart.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "ProductName",
                    HeaderText = "Product",
                    DataPropertyName = "ProductName",
                    FillWeight = 145
                });

            dgvCart.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "Quantity",
                    HeaderText = "Qty",
                    DataPropertyName = "Quantity",
                    FillWeight = 40
                });

            dgvCart.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "UnitPrice",
                    HeaderText = "Price",
                    DataPropertyName = "UnitPrice",
                    FillWeight = 65,

                    DefaultCellStyle =
                        new DataGridViewCellStyle
                        {
                            Format = "₱#,##0.00"
                        }
                });

            dgvCart.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "LineTotal",
                    HeaderText = "Total",
                    DataPropertyName = "LineTotal",
                    FillWeight = 75,

                    DefaultCellStyle =
                        new DataGridViewCellStyle
                        {
                            Format = "₱#,##0.00"
                        }
                });

            // ================= CART ACTIONS =================

            pnlCartActions.Dock = DockStyle.Fill;

            pnlCartActions.AutoSize = true;

            pnlCartActions.AutoSizeMode =
                AutoSizeMode.GrowAndShrink;

            pnlCartActions.WrapContents = true;

            pnlCartActions.Padding =
                new Padding(0, 5, 0, 5);

            ConfigureButton(
                btnIncrease,
                "+ Qty",
                Color.FromArgb(37, 99, 235),
                75);

            btnIncrease.Click +=
                btnIncrease_Click;

            ConfigureButton(
                btnDecrease,
                "- Qty",
                Color.FromArgb(71, 85, 105),
                75);

            btnDecrease.Click +=
                btnDecrease_Click;

            ConfigureButton(
                btnRemove,
                "Remove",
                Color.FromArgb(220, 38, 38),
                85);

            btnRemove.Click +=
                btnRemove_Click;

            pnlCartActions.Controls.Add(btnIncrease);
            pnlCartActions.Controls.Add(btnDecrease);
            pnlCartActions.Controls.Add(btnRemove);

            // ================= TOTALS =================

            pnlTotals.Dock = DockStyle.Fill;

            pnlTotals.BackColor =
                Color.FromArgb(248, 250, 252);

            TableLayoutPanel tblSummary =
                new TableLayoutPanel();

            tblSummary.Dock = DockStyle.Fill;

            tblSummary.Padding = new Padding(8);

            tblSummary.ColumnCount = 2;
            tblSummary.RowCount = 4;

            tblSummary.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent, 40F));

            tblSummary.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent, 60F));

            tblSummary.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute, 32F));

            tblSummary.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute, 48F));

            tblSummary.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute, 50F));

            tblSummary.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute, 42F));

            Label lblSubtotalTitle = new Label
            {
                Text = "Subtotal",
                Dock = DockStyle.Fill,

                TextAlign =
                    ContentAlignment.MiddleLeft,

                Font = new Font("Segoe UI", 11F),

                ForeColor = Color.SlateGray
            };

            lblSubtotal.Text = "₱0.00";

            lblSubtotal.Dock = DockStyle.Fill;

            lblSubtotal.TextAlign =
                ContentAlignment.MiddleRight;

            lblSubtotal.Font =
                new Font("Segoe UI", 12F);

            Label lblTotalTitle = new Label
            {
                Text = "TOTAL",

                Dock = DockStyle.Fill,

                TextAlign =
                    ContentAlignment.MiddleLeft,

                Font = new Font(
                    "Segoe UI", 14F, FontStyle.Bold)
            };

            lblTotal.Text = "₱0.00";

            lblTotal.Dock = DockStyle.Fill;

            lblTotal.TextAlign =
                ContentAlignment.MiddleRight;

            lblTotal.Font = new Font(
                "Segoe UI", 22F, FontStyle.Bold);

            lblTotal.ForeColor =
                Color.FromArgb(22, 163, 74);

            ConfigureButton(
                btnCheckout,
                "Proceed to Payment",
                Color.FromArgb(37, 99, 235),
                200);

            btnCheckout.Dock = DockStyle.Fill;

            btnCheckout.Enabled = false;

            btnCheckout.Click +=
                btnCheckout_Click;

            ConfigureButton(
                btnClearCart,
                "Clear Cart",
                Color.FromArgb(220, 38, 38),
                200);

            btnClearCart.Dock = DockStyle.Fill;

            btnClearCart.Click +=
                btnClearCart_Click;

            tblSummary.Controls.Add(
                lblSubtotalTitle, 0, 0);

            tblSummary.Controls.Add(
                lblSubtotal, 1, 0);

            tblSummary.Controls.Add(
                lblTotalTitle, 0, 1);

            tblSummary.Controls.Add(
                lblTotal, 1, 1);

            tblSummary.Controls.Add(
                btnCheckout, 0, 2);

            tblSummary.SetColumnSpan(
                btnCheckout, 2);

            tblSummary.Controls.Add(
                btnClearCart, 0, 3);

            tblSummary.SetColumnSpan(
                btnClearCart, 2);

            pnlTotals.Controls.Add(tblSummary);

            // ================= ADD CART CONTROLS =================

            tblCart.Controls.Add(
                pnlCartHeader, 0, 0);

            tblCart.Controls.Add(
                dgvCart, 0, 1);

            tblCart.Controls.Add(
                pnlCartActions, 0, 2);

            tblCart.Controls.Add(
                pnlTotals, 0, 3);

            splitMain.Panel2.Controls.Add(
                tblCart);

            // ================= SEARCH TIMER =================

            searchTimer.Interval = 300;

            searchTimer.Tick += searchTimer_Tick;

            // ================= FORM SETTINGS =================

            Controls.Add(splitMain);
            Controls.Add(pnlHeader);

            AutoScaleMode = AutoScaleMode.None;

            ClientSize = new Size(1100, 675);

            BackColor =
                Color.FromArgb(241, 245, 249);

            Font = new Font("Segoe UI", 10F);

            FormBorderStyle =
                FormBorderStyle.None;

            Name = "POSForm";

            Text = "Point of Sale";

            ResumeLayout(false);
            PerformLayout();
        }

        private void ConfigureButton(
            Button button,
            string text,
            Color color,
            int width)
        {
            button.Text = text;

            button.Size = new Size(width, 36);

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
