
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using NAOTR_POS.Models;
using NAOTR_POS.Services;
using NAOTR_POS.Helpers;

namespace NAOTR_POS.Forms
{
    public partial class ProductForm : Form
    {
        private readonly ProductService productService =
            new ProductService();

        private readonly CategoryService categoryService =
            new CategoryService();

        private readonly int productId;

        private byte[] imageBytes;
        private bool imageChanged = false;

        // ADD PRODUCT CONSTRUCTOR
        public ProductForm() : this(0)
        {
        }

        // EDIT PRODUCT CONSTRUCTOR
        public ProductForm(int productId)
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
                LoadCategories();

                if (productId > 0)
                {
                    lblTitle.Text = "Edit Product";

                    lblSubtitle.Text =
                        "Update the selected product information.";

                    lblStock.Text = "Current Stock";

                    nudStock.Enabled = false;

                    LoadProduct();
                }
                else
                {
                    chkActive.Checked = true;
                    chkActive.Enabled = false;

                    txtSku.Text = "Generated upon saving";
                    txtBarcode.Text = "Generated upon saving";
                }
                txtName.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Product Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                Close();
            }
        }

        // LOAD CATEGORY DROPDOWN
        private void LoadCategories()
        {
            DataTable categories =
                categoryService.GetCategories();

            cmbCategory.DisplayMember = "Name";
            cmbCategory.ValueMember = "CategoryId";
            cmbCategory.DataSource = categories;

            cmbCategory.SelectedIndex = -1;
        }

        // LOAD EXISTING PRODUCT
        private void LoadProduct()
        {
            Product product =
                productService.GetProductById(productId);

            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            txtSku.Text = product.Sku;
            txtBarcode.Text = product.Barcode;
            txtName.Text = product.Name;

            cmbCategory.SelectedValue =
                product.CategoryId;

            nudPrice.Value = product.Price;
            nudCost.Value = product.Cost;
            nudStock.Value = product.StockQty;
            nudReorderLevel.Value = product.ReorderLevel;

            chkActive.Checked = product.IsActive;

            imageBytes = product.Image;

            DisplayImage(imageBytes);

            imageChanged = false;
        }

        // DISPLAY PRODUCT IMAGE
        private void DisplayImage(byte[] bytes)
        {
            Image newImage = null;

            if (bytes != null && bytes.Length > 0)
            {
                using (MemoryStream stream =
                    new MemoryStream(bytes))
                {
                    using (Image image =
                        Image.FromStream(stream))
                    {
                        newImage = new Bitmap(image);
                    }
                }
            }

            if (picProduct.Image != null)
            {
                picProduct.Image.Dispose();
            }

            picProduct.Image = newImage;
        }

        // UPLOAD IMAGE
        private void btnUploadImage_Click(
            object sender, EventArgs e)
        {
            using (OpenFileDialog dialog =
                new OpenFileDialog())
            {
                dialog.Title = "Select Product Image";

                dialog.Filter =
                    "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                dialog.Multiselect = false;

                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    FileInfo file =
                        new FileInfo(dialog.FileName);

                    if (file.Length > 5 * 1024 * 1024)
                    {
                        MessageBox.Show(
                            "Image must not exceed 5 MB.");

                        return;
                    }

                    byte[] selectedImage =
                        File.ReadAllBytes(dialog.FileName);

                    DisplayImage(selectedImage);

                    imageBytes = selectedImage;
                    imageChanged = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Unable to load image.\n" + ex.Message,
                        "Image Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        // REMOVE IMAGE
        private void btnRemoveImage_Click(
            object sender, EventArgs e)
        {
            DisplayImage(null);

            imageBytes = null;
            imageChanged = true;
        }

        // VALIDATE INPUT
        private bool ValidateInput()
        {

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                ShowValidation(
                    "Please enter the product name.",
                    txtName);

                return false;
            }

            if (cmbCategory.SelectedIndex < 0)
            {
                ShowValidation(
                    "Please select a category.",
                    cmbCategory);

                return false;
            }

            if (nudPrice.Value <= 0)
            {
                ShowValidation(
                    "Selling price must be greater than zero.",
                    nudPrice);

                return false;
            }

            if (nudPrice.Value < nudCost.Value)
            {
                DialogResult result = MessageBox.Show(
                    "Selling price is lower than cost price." +
                    "\nDo you want to continue?",
                    "Price Warning",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

                if (result != DialogResult.Yes)
                {
                    nudPrice.Focus();
                    return false;
                }
            }

            return true;
        }

        private void ShowValidation(
            string message, Control control)
        {
            MessageBox.Show(
                message,
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            control.Focus();
        }

        // SAVE PRODUCT
        private void btnSave_Click(
            object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                btnSave.Enabled = false;
                btnSave.Text = "Saving...";

                Product product = new Product
                {
                    ProductId = productId,

                    Sku = productId == 0
                    ? null
                    : txtSku.Text.Trim(),

                    Barcode = productId == 0
                    ? null
                    : txtBarcode.Text.Trim(),

                    Name = txtName.Text.Trim(),

                    CategoryId = Convert.ToInt32(
                        cmbCategory.SelectedValue),

                    Price = nudPrice.Value,

                    Cost = nudCost.Value,

                    StockQty = Convert.ToInt32(
                        nudStock.Value),

                    ReorderLevel = Convert.ToInt32(
                        nudReorderLevel.Value),

                    Image = imageBytes,

                    IsActive = chkActive.Checked,

                    ChangeImage = imageChanged
                };

                if (productId == 0)
                {
                    productService.InsertProduct(product);

                    MessageBox.Show(
                        "Product added successfully!",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    productService.UpdateProduct(product);

                    MessageBox.Show(
                        "Product updated successfully!",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2601 ||
                    ex.Number == 2627)
                {
                    MessageBox.Show(
                        "The SKU or barcode already exists.",
                        "Duplicate Product",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(
                        ex.Message,
                        "Database Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Product Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnSave.Enabled = true;
                btnSave.Text = "Save Product";
            }
        }
    }
}
