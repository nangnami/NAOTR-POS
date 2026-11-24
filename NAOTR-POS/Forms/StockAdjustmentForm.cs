
using System;
using System.Drawing;
using System.Windows.Forms;
using NAOTR_POS.Models;
using NAOTR_POS.Services;
using NAOTR_POS.Helpers;

namespace NAOTR_POS.Forms
{
    public partial class StockAdjustmentForm : Form
    {
        private readonly ProductService productService =
            new ProductService();

        private readonly int productId;

        private Product selectedProduct;

        private int currentStock = 0;

        private bool isLoading = true;

        // DEFAULT CONSTRUCTOR FOR DESIGNER
        public StockAdjustmentForm() : this(0)
        {
        }

        // SELECTED PRODUCT CONSTRUCTOR
        public StockAdjustmentForm(int productId)
        {
            InitializeComponent();

            this.productId = productId;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Session.IsInRole("Admin") &&
                !Session.IsInRole("Manager"))
            {
                MessageBox.Show("Access denied.");
                Close();
                return;
            }

            try
            {
                isLoading = true;

                LoadProduct();

                cmbAdjustmentType.SelectedIndex = 0;

                LoadReasons();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Stock Adjustment Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                Close();
                return;
            }
            finally
            {
                isLoading = false;
            }

            CalculateNewStock();
        }

        // ================= LOAD PRODUCT =================

        private void LoadProduct()
        {
            selectedProduct =
                productService.GetProductById(productId);

            if (selectedProduct == null)
            {
                throw new Exception("Product not found.");
            }

            if (!selectedProduct.IsActive)
            {
                throw new Exception(
                    "This product is inactive.");
            }

            currentStock = selectedProduct.StockQty;

            lblProductName.Text =
                selectedProduct.Name;

            lblSku.Text =
                "SKU: " + selectedProduct.Sku;

            lblCurrentStock.Text =
                currentStock.ToString("N0");

            lblNewStock.Text =
                currentStock.ToString("N0");
        }

        // ================= LOAD REASONS =================

        private void LoadReasons()
        {
            cmbReason.Items.Clear();

            if (cmbAdjustmentType.Text == "Stock In")
            {
                cmbReason.Items.AddRange(
                    new object[]
                    {
                        "New Delivery",
                        "Customer Return",
                        "Stock Correction",
                        "Opening Stock",
                        "Other"
                    });
            }
            else
            {
                cmbReason.Items.AddRange(
                    new object[]
                    {
                        "Damaged Items",
                        "Expired Items",
                        "Supplier Return",
                        "Missing Items",
                        "Stock Correction",
                        "Other"
                    });
            }

            cmbReason.SelectedIndex = -1;
        }

        // ================= GET STOCK MOVEMENT =================

        private int GetChangeQuantity()
        {
            int quantity =
                Convert.ToInt32(nudQuantity.Value);

            if (cmbAdjustmentType.Text == "Stock Out")
            {
                return -quantity;
            }

            return quantity;
        }

        // ================= CALCULATE PROJECTED STOCK =================

        private void CalculateNewStock()
        {
            if (isLoading || selectedProduct == null)
                return;

            int changeQty = GetChangeQuantity();

            long newStock =
                (long)currentStock + changeQty;

            lblNewStock.Text = newStock.ToString("N0");

            if (newStock < 0)
            {
                lblNewStock.ForeColor = Color.Firebrick;

                lblWarning.Text =
                    "Insufficient stock for this adjustment.";

                btnSave.Enabled = false;
            }
            else if (newStock > int.MaxValue)
            {
                lblNewStock.ForeColor = Color.Firebrick;

                lblWarning.Text =
                    "Stock quantity exceeds the allowed limit.";

                btnSave.Enabled = false;
            }
            else
            {
                lblWarning.Text = "";
                btnSave.Enabled = true;

                if (newStock == 0)
                {
                    lblNewStock.ForeColor =
                        Color.Firebrick;
                }
                else if (
                    newStock <= selectedProduct.ReorderLevel)
                {
                    lblNewStock.ForeColor =
                        Color.DarkOrange;
                }
                else
                {
                    lblNewStock.ForeColor =
                        Color.SeaGreen;
                }
            }
        }

        // ================= ADJUSTMENT TYPE CHANGED =================

        private void cmbAdjustmentType_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            if (isLoading)
                return;

            LoadReasons();

            CalculateNewStock();
        }

        // ================= QUANTITY CHANGED =================

        private void nudQuantity_ValueChanged(
            object sender,
            EventArgs e)
        {
            CalculateNewStock();
        }

        // ================= VALIDATION =================

        private bool ValidateInput()
        {
            if (cmbAdjustmentType.SelectedIndex < 0)
            {
                MessageBox.Show(
                    "Please select an adjustment type.");

                cmbAdjustmentType.Focus();

                return false;
            }

            if (nudQuantity.Value <= 0)
            {
                MessageBox.Show(
                    "Quantity must be greater than zero.");

                nudQuantity.Focus();

                return false;
            }

            if (cmbReason.SelectedIndex < 0)
            {
                MessageBox.Show(
                    "Please select a reason.");

                cmbReason.Focus();

                return false;
            }

            if (cmbReason.Text == "Other" &&
                string.IsNullOrWhiteSpace(txtRemarks.Text))
            {
                MessageBox.Show(
                    "Please specify the adjustment reason.");

                txtRemarks.Focus();

                return false;
            }

            long newStock =
                (long)currentStock + GetChangeQuantity();

            if (newStock < 0 ||
                newStock > int.MaxValue)
            {
                MessageBox.Show(
                    "Invalid resulting stock quantity.");

                return false;
            }

            return true;
        }

        // ================= BUILD ADJUSTMENT REASON =================

        private string GetAdjustmentReason()
        {
            string reason = cmbReason.Text;

            string remarks = txtRemarks.Text.Trim();

            if (!string.IsNullOrWhiteSpace(remarks))
            {
                reason += " - " + remarks;
            }

            return reason;
        }

        // ================= SAVE STOCK ADJUSTMENT =================

        private void btnSave_Click(
            object sender,
            EventArgs e)
        {
            if (!ValidateInput())
                return;

            int changeQty = GetChangeQuantity();

            string reason = GetAdjustmentReason();

            if (reason.Length > 200)
            {
                MessageBox.Show(
                    "Adjustment reason is too long.");

                return;
            }

            string movement =
                changeQty > 0
                    ? "+" + changeQty
                    : changeQty.ToString();

            DialogResult confirm = MessageBox.Show(
                "Product: " + selectedProduct.Name +
                "\nAdjustment: " + cmbAdjustmentType.Text +
                "\nQuantity: " + movement +
                "\nReason: " + reason +
                "\n\nConfirm this stock adjustment?",
                "Confirm Stock Adjustment",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                btnSave.Enabled = false;
                btnSave.Text = "Processing...";

                int newStock =
                    productService.AdjustStock(
                        productId,
                        changeQty,
                        reason);

                MessageBox.Show(
                    "Stock adjusted successfully!" +
                    "\n\nNew stock quantity: " + newStock,
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Stock Adjustment Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                // Reload current stock in case another
                // user changed it during the adjustment.
                try
                {
                    LoadProduct();
                }
                catch
                {
                    Close();
                }
            }
            finally
            {
                if (!IsDisposed)
                {
                    btnSave.Text = "Confirm Adjustment";

                    CalculateNewStock();
                }
            }
        }
    }
}
