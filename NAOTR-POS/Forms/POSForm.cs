
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NAOTR_POS.Models;
using NAOTR_POS.Services;
using NAOTR_POS.Helpers;

namespace NAOTR_POS.Forms
{
    public partial class POSForm : Form
    {
        private readonly POSService posService =
            new POSService();

        private readonly CategoryService categoryService =
            new CategoryService();

        private readonly BindingList<CartItem> cart =
            new BindingList<CartItem>();

        private readonly BindingSource cartSource =
            new BindingSource();

        private bool isLoading = true;

        public POSForm()
        {
            InitializeComponent();

            cartSource.DataSource = cart;

            dgvCart.DataSource = cartSource;

            Disposed += delegate
            {
                cartSource.Dispose();
            };
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Session.IsInRole("Admin") &&
                !Session.IsInRole("Manager") &&
                !Session.IsInRole("Cashier"))
            {
                MessageBox.Show("Access denied.");

                Close();
                return;
            }

            try
            {
                LoadCategories();

                isLoading = false;

                LoadProducts();

                UpdateCart();

                txtSearch.Focus();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        // ================= LOAD CATEGORIES =================

        private void LoadCategories()
        {
            DataTable categories =
                categoryService.GetCategories();

            DataRow all = categories.NewRow();

            all["CategoryId"] = 0;
            all["Name"] = "All Categories";

            categories.Rows.InsertAt(all, 0);

            cmbCategory.DisplayMember = "Name";
            cmbCategory.ValueMember = "CategoryId";

            cmbCategory.DataSource = categories;

            cmbCategory.SelectedIndex = 0;
        }

        private int GetSelectedCategoryId()
        {
            if (cmbCategory.SelectedValue is int)
            {
                return (int)cmbCategory.SelectedValue;
            }

            return 0;
        }

        // ================= LOAD PRODUCTS =================

        private void LoadProducts()
        {
            if (isLoading)
                return;

            try
            {
                List<Product> products =
                    posService.GetProducts(
                        txtSearch.Text.Trim(),
                        GetSelectedCategoryId());

                pnlProducts.SuspendLayout();

                ReleaseProductCards();

                foreach (Product product in products)
                {
                    pnlProducts.Controls.Add(
                        CreateProductCard(product));
                }

                if (products.Count == 0)
                {
                    Label empty = new Label
                    {
                        Text = "No products found.",
                        AutoSize = true,

                        Font = new Font(
                            "Segoe UI", 12F),

                        ForeColor = Color.SlateGray,

                        Margin = new Padding(15)
                    };

                    pnlProducts.Controls.Add(empty);
                }

                lblProductCount.Text =
                    products.Count.ToString("N0") +
                    " product(s) found";
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
            finally
            {
                pnlProducts.ResumeLayout();
            }
        }

        // ================= CREATE PRODUCT CARD =================

        private Panel CreateProductCard(Product product)
        {
            Panel card = new Panel();

            card.Width = 175;
            card.Height = 235;

            card.Margin = new Padding(6);

            card.BackColor = Color.White;

            card.BorderStyle =
                BorderStyle.FixedSingle;

            // PRODUCT IMAGE

            PictureBox picture = new PictureBox();

            picture.Location = new Point(8, 8);

            picture.Size = new Size(157, 90);

            picture.SizeMode =
                PictureBoxSizeMode.Zoom;

            picture.BackColor =
                Color.FromArgb(248, 250, 252);

            picture.Image =
                CreateProductImage(product.Image);

            // PRODUCT NAME

            Label name = new Label();

            name.Text = product.Name;

            name.Location = new Point(8, 105);

            name.Size = new Size(157, 36);

            name.Font = new Font(
                "Segoe UI", 10F, FontStyle.Bold);

            name.ForeColor =
                Color.FromArgb(16, 36, 62);

            // SKU

            Label sku = new Label();

            sku.Text = product.Sku;

            sku.Location = new Point(8, 143);

            sku.Size = new Size(157, 17);

            sku.Font = new Font("Segoe UI", 8F);

            sku.ForeColor = Color.SlateGray;

            // PRICE

            Label price = new Label();

            price.Text =
                FormatMoney(product.Price);

            price.Location = new Point(8, 160);

            price.Size = new Size(157, 26);

            price.Font = new Font(
                "Segoe UI", 15F, FontStyle.Bold);

            price.ForeColor =
                Color.FromArgb(37, 99, 235);

            // STOCK

            Label stock = new Label();

            stock.Text =
                "Stock: " + product.StockQty;

            stock.Location = new Point(8, 187);

            stock.Size = new Size(157, 18);

            stock.Font = new Font("Segoe UI", 9F);

            if (product.StockQty == 0)
            {
                stock.ForeColor = Color.Firebrick;
            }
            else if (product.StockQty <=
                     product.ReorderLevel)
            {
                stock.ForeColor = Color.DarkOrange;
            }
            else
            {
                stock.ForeColor = Color.SeaGreen;
            }

            // ADD TO CART BUTTON

            Button add = new Button();

            add.Text = product.StockQty > 0
                ? "+ Add to Cart"
                : "Out of Stock";

            add.Location = new Point(8, 207);

            add.Size = new Size(157, 23);

            add.Font = new Font(
                "Segoe UI", 9F, FontStyle.Bold);

            add.FlatStyle = FlatStyle.Flat;

            add.FlatAppearance.BorderSize = 0;

            add.BackColor = product.StockQty > 0
                ? Color.FromArgb(37, 99, 235)
                : Color.FromArgb(148, 163, 184);

            add.ForeColor = Color.White;

            add.UseVisualStyleBackColor = false;

            add.Cursor = Cursors.Hand;

            add.Enabled = product.StockQty > 0;

            add.Click += delegate
            {
                AddToCart(product);
            };

            card.Controls.Add(picture);
            card.Controls.Add(name);
            card.Controls.Add(sku);
            card.Controls.Add(price);
            card.Controls.Add(stock);
            card.Controls.Add(add);

            return card;
        }

        // ================= PRODUCT IMAGE =================

        private Image CreateProductImage(byte[] bytes)
        {
            if (bytes != null && bytes.Length > 0)
            {
                try
                {
                    using (MemoryStream stream =
                        new MemoryStream(bytes))
                    using (Image original =
                        Image.FromStream(stream))
                    {
                        return new Bitmap(original);
                    }
                }
                catch
                {
                    // Use placeholder for invalid image.
                }
            }

            Bitmap placeholder =
                new Bitmap(150, 90);

            using (Graphics graphics =
                Graphics.FromImage(placeholder))
            using (Font font =
                new Font("Segoe UI", 10F))
            using (Brush brush =
                new SolidBrush(Color.SlateGray))
            using (StringFormat format =
                new StringFormat())
            {
                graphics.Clear(
                    Color.FromArgb(248, 250, 252));

                format.Alignment =
                    StringAlignment.Center;

                format.LineAlignment =
                    StringAlignment.Center;

                graphics.DrawString(
                    "No Image",
                    font,
                    brush,
                    new RectangleF(0, 0, 150, 90),
                    format);
            }

            return placeholder;
        }

        // ================= RELEASE CARD RESOURCES =================

        private void ReleaseProductCards()
        {
            if (pnlProducts == null ||
                pnlProducts.IsDisposed)
            {
                return;
            }

            while (pnlProducts.Controls.Count > 0)
            {
                Control control =
                    pnlProducts.Controls[0];

                foreach (Control child in control.Controls)
                {
                    PictureBox picture =
                        child as PictureBox;

                    if (picture != null &&
                        picture.Image != null)
                    {
                        Image image = picture.Image;

                        picture.Image = null;

                        image.Dispose();
                    }
                }

                pnlProducts.Controls.Remove(control);

                control.Dispose();
            }
        }

        // ================= ADD TO CART =================

        private void AddToCart(Product product)
        {
            if (product == null)
                return;

            if (product.StockQty <= 0)
            {
                MessageBox.Show(
                    "This product is out of stock.",
                    "Unavailable Product",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            CartItem existing =
                cart.FirstOrDefault(
                    x => x.ProductId ==
                         product.ProductId);

            if (existing != null)
            {
                existing.AvailableStock =
                    product.StockQty;

                if (existing.Quantity >=
                    existing.AvailableStock)
                {
                    ShowStockWarning(
                        existing.ProductName,
                        existing.AvailableStock);

                    return;
                }

                existing.Quantity++;
            }
            else
            {
                cart.Add(
                    new CartItem
                    {
                        ProductId =
                            product.ProductId,

                        Sku = product.Sku,

                        Barcode = product.Barcode,

                        ProductName = product.Name,

                        UnitPrice = product.Price,

                        Quantity = 1,

                        AvailableStock =
                            product.StockQty
                    });
            }

            UpdateCart();

            SelectCartItem(product.ProductId);

            txtSearch.Focus();
        }

        // ================= SELECT CART ITEM =================

        private CartItem GetSelectedCartItem()
        {
            if (dgvCart.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Please select an item from the cart.",
                    "No Item Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return null;
            }

            return dgvCart.SelectedRows[0]
                .DataBoundItem as CartItem;
        }

        private void SelectCartItem(int productId)
        {
            dgvCart.ClearSelection();

            foreach (DataGridViewRow row in dgvCart.Rows)
            {
                CartItem item =
                    row.DataBoundItem as CartItem;

                if (item != null &&
                    item.ProductId == productId)
                {
                    row.Selected = true;

                    dgvCart.CurrentCell =
                        row.Cells[0];

                    break;
                }
            }
        }

        // ================= INCREASE QUANTITY =================

        private void btnIncrease_Click(
            object sender, EventArgs e)
        {
            CartItem item = GetSelectedCartItem();

            if (item == null)
                return;

            if (item.Quantity >= item.AvailableStock)
            {
                ShowStockWarning(
                    item.ProductName,
                    item.AvailableStock);

                return;
            }

            item.Quantity++;

            UpdateCart();

            SelectCartItem(item.ProductId);
        }

        // ================= DECREASE QUANTITY =================

        private void btnDecrease_Click(
            object sender, EventArgs e)
        {
            CartItem item = GetSelectedCartItem();

            if (item == null)
                return;

            if (item.Quantity > 1)
            {
                item.Quantity--;

                UpdateCart();

                SelectCartItem(item.ProductId);
            }
            else
            {
                cart.Remove(item);

                UpdateCart();
            }
        }

        // ================= REMOVE ITEM =================

        private void btnRemove_Click(
            object sender, EventArgs e)
        {
            CartItem item = GetSelectedCartItem();

            if (item == null)
                return;

            cart.Remove(item);

            UpdateCart();
        }

        // ================= CLEAR CART =================

        private void btnClearCart_Click(
            object sender, EventArgs e)
        {
            if (cart.Count == 0)
                return;

            DialogResult confirm =
                MessageBox.Show(
                    "Remove all items from the cart?",
                    "Clear Cart",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);

            if (confirm != DialogResult.Yes)
                return;

            cart.Clear();

            UpdateCart();

            txtSearch.Focus();
        }

        // ================= UPDATE CART TOTALS =================

        private void UpdateCart()
        {
            cartSource.ResetBindings(false);

            decimal subtotal =
                cart.Sum(x => x.LineTotal);

            long totalItems =
                cart.Sum(x => (long)x.Quantity);

            lblCartCount.Text =
                totalItems.ToString("N0") +
                " item(s) | " +
                cart.Count.ToString("N0") +
                " product(s)";

            lblSubtotal.Text =
                FormatMoney(subtotal);

            lblTotal.Text =
                FormatMoney(subtotal);

            btnCheckout.Enabled =
                cart.Count > 0;

            btnClearCart.Enabled =
                cart.Count > 0;

            btnIncrease.Enabled =
                cart.Count > 0;

            btnDecrease.Enabled =
                cart.Count > 0;

            btnRemove.Enabled =
                cart.Count > 0;
        }

        // ================= SEARCH PRODUCTS =================

        private void txtSearch_TextChanged(
            object sender, EventArgs e)
        {
            if (isLoading)
                return;

            searchTimer.Stop();

            searchTimer.Start();
        }

        private void searchTimer_Tick(
            object sender, EventArgs e)
        {
            searchTimer.Stop();

            LoadProducts();
        }

        // ================= BARCODE SCANNING =================

        private void txtSearch_KeyDown(
            object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            e.SuppressKeyPress = true;

            searchTimer.Stop();

            string input =
                txtSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(input))
                return;

            try
            {
                Product product = null;

                if (input.Length <= 50)
                {
                    product =
                        posService.GetProductByBarcode(
                            input);
                }

                if (product != null)
                {
                    AddToCart(product);

                    txtSearch.Clear();

                    searchTimer.Stop();

                    LoadProducts();
                }
                else
                {
                    LoadProducts();
                }

                txtSearch.Focus();

                txtSearch.SelectAll();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        // ================= CATEGORY FILTER =================

        private void cmbCategory_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            if (isLoading)
                return;

            searchTimer.Stop();

            LoadProducts();
        }

        // ================= REFRESH =================

        private void btnRefresh_Click(
            object sender, EventArgs e)
        {
            try
            {
                searchTimer.Stop();

                List<Product> latestProducts =
                    posService.GetProducts();

                foreach (CartItem item in cart)
                {
                    Product latest =
                        latestProducts.FirstOrDefault(
                            p => p.ProductId ==
                                 item.ProductId);

                    item.AvailableStock =
                        latest == null
                        ? 0
                        : latest.StockQty;
                }

                UpdateCart();

                LoadProducts();

                bool invalidCart =
                    cart.Any(
                        x => x.Quantity >
                             x.AvailableStock);

                if (invalidCart)
                {
                    MessageBox.Show(
                        "Some cart quantities exceed " +
                        "the current available stock. " +
                        "Please adjust or remove them.",
                        "Stock Changed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        // ================= CHECKOUT PLACEHOLDER =================



        private void btnCheckout_Click(
            object sender, EventArgs e)
        {
            if (cart.Count == 0)
            {
                MessageBox.Show("The cart is empty.");
                return;
            }

            if (cart.Any(
                x => x.Quantity > x.AvailableStock))
            {
                MessageBox.Show(
                    "Please correct the cart quantities.",
                    "Insufficient Stock",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            using (CheckoutForm form =
                new CheckoutForm(cart.ToList()))
            {
                if (form.ShowDialog(this) != DialogResult.OK)
                    return;

                int orderId = form.CompletedOrderId;

                // Transaction has already been saved.

                cart.Clear();

                UpdateCart();

                txtSearch.Clear();

                searchTimer.Stop();

                LoadProducts();

                // Automatically display receipt.

                if (orderId > 0)
                {
                    using (ReceiptForm receipt =
                        new ReceiptForm(orderId))
                    {
                        receipt.ShowDialog(this);
                    }
                }

                txtSearch.Focus();
            }
        }



        // ================= STOCK WARNING =================

        private void ShowStockWarning(
            string productName,
            int availableStock)
        {
            MessageBox.Show(
                "Insufficient stock." +
                "\n\nProduct: " + productName +
                "\nAvailable stock: " +
                availableStock,
                "Stock Limit",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        // ================= MONEY FORMAT =================

        private string FormatMoney(decimal amount)
        {
            return "₱" + amount.ToString("N2");
        }

        // ================= ERROR HANDLING =================

        private void ShowError(Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Point of Sale Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
