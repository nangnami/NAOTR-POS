
using System.Drawing;
using System.Windows.Forms;

namespace NAOTR_POS.Forms
{
    partial class InventoryForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Panel pnlContent;
        private Panel pnlTable;
        private Panel pnlToolbar;
        private Panel pnlFooter;

        private FlowLayoutPanel pnlActions;
        private FlowLayoutPanel pnlFilters;

        private TableLayoutPanel tblMetrics;

        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblTotalProducts;
        private Label lblInStock;
        private Label lblLowStock;
        private Label lblOutOfStock;
        private Label lblRecordCount;

        private TextBox txtSearch;
        private ComboBox cmbCategory;

        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnAdjustStock;
        private Button btnHistory;
        private Button btnRefresh;

        private DataGridView dgvProducts;

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
            pnlToolbar = new Panel();
            pnlFooter = new Panel();

            pnlActions = new FlowLayoutPanel();
            pnlFilters = new FlowLayoutPanel();

            tblMetrics = new TableLayoutPanel();

            lblTitle = new Label();
            lblSubtitle = new Label();

            lblTotalProducts = new Label();
            lblInStock = new Label();
            lblLowStock = new Label();
            lblOutOfStock = new Label();
            lblRecordCount = new Label();

            txtSearch = new TextBox();
            cmbCategory = new ComboBox();

            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnAdjustStock = new Button();
            btnHistory = new Button();
            btnRefresh = new Button();

            dgvProducts = new DataGridView();

            SuspendLayout();

            // ==========================================
            // HEADER
            // ==========================================

            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 85;
            pnlHeader.BackColor = Color.White;

            lblTitle.Text = "Inventory Management";

            lblTitle.Font = new Font(
                "Segoe UI", 24F, FontStyle.Bold);

            lblTitle.ForeColor =
                Color.FromArgb(16, 36, 62);

            lblTitle.Location = new Point(20, 10);
            lblTitle.AutoSize = true;

            lblSubtitle.Text =
                "Manage products, stock levels and inventory movements.";

            lblSubtitle.Font =
                new Font("Segoe UI", 10F);

            lblSubtitle.ForeColor =
                Color.FromArgb(100, 116, 139);

            lblSubtitle.Location = new Point(23, 53);
            lblSubtitle.AutoSize = true;

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubtitle);

            // ==========================================
            // MAIN CONTENT
            // ==========================================

            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Padding = new Padding(20);

            pnlContent.BackColor =
                Color.FromArgb(241, 245, 249);

            // ==========================================
            // SUMMARY CARDS
            // ==========================================

            tblMetrics.Dock = DockStyle.Top;
            tblMetrics.Height = 115;
            tblMetrics.ColumnCount = 4;
            tblMetrics.RowCount = 1;

            tblMetrics.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 25F));

            tblMetrics.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 25F));

            tblMetrics.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 25F));

            tblMetrics.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 25F));

            tblMetrics.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100F));

            tblMetrics.Controls.Add(
                CreateMetricCard(
                    "Total Products",
                    lblTotalProducts,
                    Color.FromArgb(37, 99, 235)),
                0, 0);

            tblMetrics.Controls.Add(
                CreateMetricCard(
                    "In Stock",
                    lblInStock,
                    Color.FromArgb(22, 163, 74)),
                1, 0);

            tblMetrics.Controls.Add(
                CreateMetricCard(
                    "Low Stock",
                    lblLowStock,
                    Color.FromArgb(217, 119, 6)),
                2, 0);

            tblMetrics.Controls.Add(
                CreateMetricCard(
                    "Out of Stock",
                    lblOutOfStock,
                    Color.FromArgb(220, 38, 38)),
                3, 0);

            // ==========================================
            // TOOLBAR
            // ==========================================

            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.AutoSize = true;

            pnlToolbar.AutoSizeMode =
                AutoSizeMode.GrowAndShrink;

            pnlToolbar.BackColor = Color.White;
            pnlToolbar.Padding = new Padding(10);

            // ==========================================
            // SEARCH AND CATEGORY FILTER
            // ==========================================

            pnlFilters.Dock = DockStyle.Top;
            pnlFilters.AutoSize = true;

            pnlFilters.AutoSizeMode =
                AutoSizeMode.GrowAndShrink;

            pnlFilters.FlowDirection =
                FlowDirection.LeftToRight;

            pnlFilters.WrapContents = true;

            pnlFilters.Padding = new Padding(0, 0, 0, 8);

            Label lblSearch = new Label();

            lblSearch.Text = "Search:";
            lblSearch.AutoSize = true;

            lblSearch.Font = new Font(
                "Segoe UI", 10F, FontStyle.Bold);

            lblSearch.ForeColor =
                Color.FromArgb(51, 65, 85);

            lblSearch.Margin =
                new Padding(5, 9, 5, 5);

            txtSearch.Size = new Size(250, 30);

            txtSearch.Font =
                new Font("Segoe UI", 11F);

            txtSearch.MaxLength = 150;

            txtSearch.Margin =
                new Padding(5, 5, 15, 5);

            txtSearch.TextChanged +=
                txtSearch_TextChanged;

            Label lblCategory = new Label();

            lblCategory.Text = "Category:";
            lblCategory.AutoSize = true;

            lblCategory.Font = new Font(
                "Segoe UI", 10F, FontStyle.Bold);

            lblCategory.ForeColor =
                Color.FromArgb(51, 65, 85);

            lblCategory.Margin =
                new Padding(5, 9, 5, 5);

            cmbCategory.Size = new Size(200, 30);

            cmbCategory.Font =
                new Font("Segoe UI", 11F);

            cmbCategory.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbCategory.Margin =
                new Padding(5, 5, 5, 5);

            cmbCategory.SelectedIndexChanged +=
                cmbCategory_SelectedIndexChanged;

            pnlFilters.Controls.Add(lblSearch);
            pnlFilters.Controls.Add(txtSearch);
            pnlFilters.Controls.Add(lblCategory);
            pnlFilters.Controls.Add(cmbCategory);

            // ==========================================
            // RESPONSIVE ACTION BUTTONS
            // ==========================================

            pnlActions.Dock = DockStyle.Top;
            pnlActions.AutoSize = true;

            pnlActions.AutoSizeMode =
                AutoSizeMode.GrowAndShrink;

            pnlActions.FlowDirection =
                FlowDirection.LeftToRight;

            pnlActions.WrapContents = true;
            pnlActions.BackColor = Color.White;

            pnlActions.Padding =
                new Padding(0, 0, 0, 5);

            // ADD PRODUCT

            ConfigureButton(
                btnAdd,
                "+ Add Product",
                Color.FromArgb(37, 99, 235),
                135);

            btnAdd.Click += btnAdd_Click;

            // EDIT PRODUCT

            ConfigureButton(
                btnEdit,
                "Edit",
                Color.FromArgb(71, 85, 105),
                85);

            btnEdit.Click += btnEdit_Click;

            // DELETE PRODUCT

            ConfigureButton(
                btnDelete,
                "Delete",
                Color.FromArgb(220, 38, 38),
                85);

            btnDelete.Click += btnDelete_Click;

            // STOCK ADJUSTMENT

            ConfigureButton(
                btnAdjustStock,
                "Adjust Stock",
                Color.FromArgb(22, 163, 74),
                125);

            btnAdjustStock.Click +=
                btnAdjustStock_Click;

            // INVENTORY HISTORY

            ConfigureButton(
                btnHistory,
                "History",
                Color.FromArgb(16, 36, 62),
                100);

            btnHistory.Click += btnHistory_Click;

            // REFRESH

            ConfigureButton(
                btnRefresh,
                "Refresh",
                Color.FromArgb(100, 116, 139),
                95);

            btnRefresh.Click += btnRefresh_Click;

            // BUTTON ORDER

            pnlActions.Controls.Add(btnAdd);
            pnlActions.Controls.Add(btnEdit);
            pnlActions.Controls.Add(btnDelete);
            pnlActions.Controls.Add(btnAdjustStock);
            pnlActions.Controls.Add(btnHistory);
            pnlActions.Controls.Add(btnRefresh);

            pnlToolbar.Controls.Add(pnlActions);
            pnlToolbar.Controls.Add(pnlFilters);

            // ==========================================
            // TABLE CONTAINER
            // ==========================================

            pnlTable.Dock = DockStyle.Fill;
            pnlTable.BackColor = Color.White;

            pnlTable.Padding =
                new Padding(0, 12, 0, 0);

            // ==========================================
            // PRODUCT DATAGRIDVIEW
            // ==========================================

            dgvProducts.Dock = DockStyle.Fill;

            dgvProducts.BackgroundColor =
                Color.White;

            dgvProducts.BorderStyle =
                BorderStyle.None;

            dgvProducts.ReadOnly = true;

            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.AllowUserToDeleteRows = false;
            dgvProducts.AllowUserToResizeRows = false;

            dgvProducts.RowHeadersVisible = false;

            dgvProducts.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvProducts.MultiSelect = false;

            dgvProducts.AutoGenerateColumns = true;

            dgvProducts.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvProducts.EnableHeadersVisualStyles =
                false;

            dgvProducts.ColumnHeadersHeight = 45;

            dgvProducts.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode
                    .DisableResizing;

            dgvProducts.RowTemplate.Height = 42;

            dgvProducts.ColumnHeadersDefaultCellStyle
                .BackColor =
                    Color.FromArgb(16, 36, 62);

            dgvProducts.ColumnHeadersDefaultCellStyle
                .ForeColor = Color.White;

            dgvProducts.ColumnHeadersDefaultCellStyle
                .Font = new Font(
                    "Segoe UI", 10F, FontStyle.Bold);

            dgvProducts.ColumnHeadersDefaultCellStyle
                .SelectionBackColor =
                    Color.FromArgb(16, 36, 62);

            dgvProducts.ColumnHeadersDefaultCellStyle
                .SelectionForeColor = Color.White;

            dgvProducts.DefaultCellStyle.Font =
                new Font("Segoe UI", 10F);

            dgvProducts.DefaultCellStyle.ForeColor =
                Color.FromArgb(51, 65, 85);

            dgvProducts.DefaultCellStyle.BackColor =
                Color.White;

            dgvProducts.DefaultCellStyle
                .SelectionBackColor =
                    Color.FromArgb(219, 234, 254);

            dgvProducts.DefaultCellStyle
                .SelectionForeColor =
                    Color.FromArgb(16, 36, 62);

            dgvProducts.AlternatingRowsDefaultCellStyle
                .BackColor =
                    Color.FromArgb(248, 250, 252);

            dgvProducts.CellBorderStyle =
                DataGridViewCellBorderStyle
                    .SingleHorizontal;

            dgvProducts.GridColor =
                Color.FromArgb(226, 232, 240);

            dgvProducts.CellFormatting +=
                dgvProducts_CellFormatting;

            dgvProducts.CellDoubleClick +=
                dgvProducts_CellDoubleClick;

            // ==========================================
            // FOOTER
            // ==========================================

            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Height = 45;
            pnlFooter.BackColor = Color.White;

            lblRecordCount.Text =
                "Showing 0 products";

            lblRecordCount.AutoSize = true;

            lblRecordCount.Font =
                new Font("Segoe UI", 10F);

            lblRecordCount.ForeColor =
                Color.FromArgb(100, 116, 139);

            lblRecordCount.Location =
                new Point(15, 13);

            pnlFooter.Controls.Add(lblRecordCount);

            // ==========================================
            // ADD CONTROLS
            // ==========================================

            pnlTable.Controls.Add(dgvProducts);
            pnlTable.Controls.Add(pnlFooter);

            pnlContent.Controls.Add(pnlTable);
            pnlContent.Controls.Add(pnlToolbar);
            pnlContent.Controls.Add(tblMetrics);

            Controls.Add(pnlContent);
            Controls.Add(pnlHeader);

            // ==========================================
            // FORM SETTINGS
            // ==========================================

            AutoScaleMode = AutoScaleMode.None;

            ClientSize = new Size(1100, 650);

            BackColor =
                Color.FromArgb(241, 245, 249);

            Font = new Font("Segoe UI", 10F);

            FormBorderStyle = FormBorderStyle.None;

            Name = "InventoryForm";
            Text = "Inventory Management";

            ResumeLayout(false);
            PerformLayout();
        }

        // ==========================================
        // BUTTON STYLING
        // ==========================================

        private void ConfigureButton(
            Button button,
            string text,
            Color backgroundColor,
            int width)
        {
            button.Text = text;

            button.Size = new Size(width, 40);

            button.Margin =
                new Padding(5, 5, 5, 5);

            button.BackColor = backgroundColor;
            button.ForeColor = Color.White;

            button.Font = new Font(
                "Segoe UI", 10F, FontStyle.Bold);

            button.FlatStyle = FlatStyle.Flat;

            button.FlatAppearance.BorderSize = 0;

            button.Cursor = Cursors.Hand;

            button.UseVisualStyleBackColor = false;
        }

        // ==========================================
        // SUMMARY CARD
        // ==========================================

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

            titleLabel.ForeColor =
                Color.FromArgb(100, 116, 139);

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
