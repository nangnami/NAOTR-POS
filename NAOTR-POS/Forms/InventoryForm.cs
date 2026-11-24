
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using NAOTR_POS.Services;
using NAOTR_POS.Helpers;

namespace NAOTR_POS.Forms
{
    public partial class InventoryForm : Form
    {
        private readonly ProductService productService =
            new ProductService();

        private readonly CategoryService categoryService =
            new CategoryService();

        private bool isLoading = true;

        public InventoryForm()
        {
            InitializeComponent();
        }

        // ==========================================
        // FORM LOAD
        // ==========================================

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Session.IsInRole("Admin") &&
                !Session.IsInRole("Manager"))
            {
                MessageBox.Show(
                    "You do not have permission to access inventory.",
                    "Access Denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                Close();
                return;
            }

            InitializeInventory();
        }

        // ==========================================
        // INITIALIZE INVENTORY
        // ==========================================

        private void InitializeInventory()
        {
            isLoading = true;

            try
            {
                LoadCategories();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
            finally
            {
                isLoading = false;
            }

            RefreshInventory();
        }

        // ==========================================
        // LOAD CATEGORIES
        // ==========================================

        private void LoadCategories()
        {
            DataTable categories =
                categoryService.GetCategories();

            DataRow allCategories =
                categories.NewRow();

            allCategories["CategoryId"] = 0;
            allCategories["Name"] = "All Categories";

            categories.Rows.InsertAt(
                allCategories, 0);

            cmbCategory.DisplayMember = "Name";
            cmbCategory.ValueMember = "CategoryId";

            cmbCategory.DataSource = categories;
            cmbCategory.SelectedIndex = 0;
        }

        // ==========================================
        // GET SELECTED CATEGORY
        // ==========================================

        private int GetSelectedCategoryId()
        {
            int categoryId = 0;

            if (cmbCategory.SelectedValue != null)
            {
                int.TryParse(
                    cmbCategory.SelectedValue.ToString(),
                    out categoryId);
            }

            return categoryId;
        }

        // ==========================================
        // REFRESH INVENTORY
        // ==========================================

        private void RefreshInventory()
        {
            if (isLoading)
                return;

            try
            {
                LoadProducts();
                LoadMetrics();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        // ==========================================
        // LOAD PRODUCTS
        // ==========================================

        private void LoadProducts()
        {
            if (isLoading)
                return;

            string search =
                txtSearch.Text.Trim();

            int categoryId =
                GetSelectedCategoryId();

            DataTable products =
                productService.GetProducts(
                    search,
                    categoryId);

            dgvProducts.DataSource = products;

            ConfigureGridColumns();

            dgvProducts.ClearSelection();

            lblRecordCount.Text =
                "Showing " +
                products.Rows.Count.ToString("N0") +
                " product(s)";
        }

        // ==========================================
        // CONFIGURE GRID COLUMNS
        // ==========================================

        private void ConfigureGridColumns()
        {
            HideColumn("ProductId");
            HideColumn("CategoryId");
            HideColumn("Cost");
            HideColumn("ReorderLevel");
            HideColumn("Image");
            HideColumn("IsActive");
            HideColumn("CreatedAt");
            HideColumn("UpdatedAt");

            SetColumn("Sku", "SKU", 90);
            SetColumn("Barcode", "Barcode", 135);
            SetColumn("Name", "Product Name", 180);
            SetColumn("CategoryName", "Category", 110);
            SetColumn("Price", "Selling Price", 90);
            SetColumn("StockQty", "Stock", 65);
            SetColumn("StockStatus", "Status", 100);

            if (dgvProducts.Columns.Contains("Price"))
            {
                dgvProducts.Columns["Price"]
                    .DefaultCellStyle.Format =
                        "₱#,##0.00";

                dgvProducts.Columns["Price"]
                    .DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment
                            .MiddleRight;
            }

            if (dgvProducts.Columns.Contains("StockQty"))
            {
                dgvProducts.Columns["StockQty"]
                    .DefaultCellStyle.Format = "N0";

                dgvProducts.Columns["StockQty"]
                    .DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment
                            .MiddleCenter;
            }
        }

        private void HideColumn(string columnName)
        {
            if (dgvProducts.Columns.Contains(columnName))
            {
                dgvProducts.Columns[columnName]
                    .Visible = false;
            }
        }

        private void SetColumn(
            string columnName,
            string headerText,
            float fillWeight)
        {
            if (dgvProducts.Columns.Contains(columnName))
            {
                dgvProducts.Columns[columnName]
                    .HeaderText = headerText;

                dgvProducts.Columns[columnName]
                    .FillWeight = fillWeight;
            }
        }

        // ==========================================
        // LOAD SUMMARY CARDS
        // ==========================================

        private void LoadMetrics()
        {
            DataTable products =
                productService.GetProducts("", 0);

            int totalProducts = products.Rows.Count;
            int inStock = 0;
            int lowStock = 0;
            int outOfStock = 0;

            foreach (DataRow row in products.Rows)
            {
                int stockQty =
                    Convert.ToInt32(row["StockQty"]);

                int reorderLevel =
                    Convert.ToInt32(row["ReorderLevel"]);

                if (stockQty <= 0)
                {
                    outOfStock++;
                }
                else if (stockQty <= reorderLevel)
                {
                    lowStock++;
                }
                else
                {
                    inStock++;
                }
            }

            lblTotalProducts.Text =
                totalProducts.ToString("N0");

            lblInStock.Text =
                inStock.ToString("N0");

            lblLowStock.Text =
                lowStock.ToString("N0");

            lblOutOfStock.Text =
                outOfStock.ToString("N0");
        }

        // ==========================================
        // GET SELECTED PRODUCT
        // ==========================================

        private int GetSelectedProductId()
        {
            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Please select a product first.",
                    "No Product Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return 0;
            }

            object value =
                dgvProducts.SelectedRows[0]
                    .Cells["ProductId"].Value;

            if (value == null ||
                value == DBNull.Value)
            {
                return 0;
            }

            return Convert.ToInt32(value);
        }

        // ==========================================
        // ADD PRODUCT
        // ==========================================

        private void btnAdd_Click(
            object sender,
            EventArgs e)
        {
            using (ProductForm form =
                new ProductForm())
            {
                if (form.ShowDialog(this) ==
                    DialogResult.OK)
                {
                    RefreshInventory();
                }
            }
        }

        // ==========================================
        // EDIT PRODUCT
        // ==========================================

        private void btnEdit_Click(
            object sender,
            EventArgs e)
        {
            int productId =
                GetSelectedProductId();

            if (productId == 0)
                return;

            using (ProductForm form =
                new ProductForm(productId))
            {
                if (form.ShowDialog(this) ==
                    DialogResult.OK)
                {
                    RefreshInventory();
                }
            }
        }

        // ==========================================
        // DELETE PRODUCT
        // ==========================================

        private void btnDelete_Click(
            object sender,
            EventArgs e)
        {
            int productId =
                GetSelectedProductId();

            if (productId == 0)
                return;

            string productName =
                Convert.ToString(
                    dgvProducts.SelectedRows[0]
                        .Cells["Name"].Value);

            DialogResult confirm =
                MessageBox.Show(
                    "Are you sure you want to deactivate " +
                    productName + "?" +
                    "\n\nExisting sales and inventory history " +
                    "will be preserved.",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                productService.DeleteProduct(productId);

                MessageBox.Show(
                    "Product deactivated successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                RefreshInventory();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        // ==========================================
        // STOCK ADJUSTMENT
        // ==========================================

        private void btnAdjustStock_Click(
            object sender,
            EventArgs e)
        {
            int productId =
                GetSelectedProductId();

            if (productId == 0)
                return;

            using (StockAdjustmentForm form =
                new StockAdjustmentForm(productId))
            {
                if (form.ShowDialog(this) ==
                    DialogResult.OK)
                {
                    RefreshInventory();
                }
            }
        }

        // ==========================================
        // INVENTORY MOVEMENT HISTORY
        // ==========================================

        private void btnHistory_Click(
            object sender,
            EventArgs e)
        {
            if (!Session.IsInRole("Admin") &&
                !Session.IsInRole("Manager"))
            {
                MessageBox.Show("Access denied.");
                return;
            }

            using (InventoryHistoryForm form =
                new InventoryHistoryForm())
            {
                form.ShowDialog(this);
            }

            RefreshInventory();
        }

        // ==========================================
        // REFRESH BUTTON
        // ==========================================

        private void btnRefresh_Click(
            object sender,
            EventArgs e)
        {
            RefreshInventory();
        }

        // ==========================================
        // SEARCH PRODUCT
        // ==========================================

        private void txtSearch_TextChanged(
            object sender,
            EventArgs e)
        {
            if (isLoading)
                return;

            try
            {
                LoadProducts();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        // ==========================================
        // CATEGORY FILTER
        // ==========================================

        private void cmbCategory_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            if (isLoading)
                return;

            try
            {
                LoadProducts();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        // ==========================================
        // STOCK STATUS COLORS
        // ==========================================

        private void dgvProducts_CellFormatting(
            object sender,
            DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 ||
                e.ColumnIndex < 0 ||
                e.Value == null ||
                e.Value == DBNull.Value)
            {
                return;
            }

            string columnName =
                dgvProducts.Columns[e.ColumnIndex].Name;

            if (columnName == "StockStatus")
            {
                string status = e.Value.ToString();

                switch (status)
                {
                    case "In Stock":

                        e.CellStyle.ForeColor =
                            Color.FromArgb(22, 163, 74);

                        break;

                    case "Low Stock":

                        e.CellStyle.ForeColor =
                            Color.FromArgb(217, 119, 6);

                        break;

                    case "Out of Stock":

                        e.CellStyle.ForeColor =
                            Color.FromArgb(220, 38, 38);

                        break;

                    default:

                        e.CellStyle.ForeColor =
                            Color.FromArgb(51, 65, 85);

                        break;
                }
            }
        }

        // ==========================================
        // DOUBLE CLICK TO EDIT
        // ==========================================

        private void dgvProducts_CellDoubleClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            dgvProducts.ClearSelection();

            dgvProducts.Rows[e.RowIndex]
                .Selected = true;

            btnEdit_Click(sender, EventArgs.Empty);
        }

        // ==========================================
        // ERROR HANDLING
        // ==========================================

        private void ShowError(Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Inventory Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
