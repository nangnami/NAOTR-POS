
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace NAOTR_POS.Forms
{
    partial class ProductForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Panel pnlFooter;

        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblStock;

        private TextBox txtSku;
        private TextBox txtBarcode;
        private TextBox txtName;

        private ComboBox cmbCategory;

        private NumericUpDown nudPrice;
        private NumericUpDown nudCost;
        private NumericUpDown nudStock;
        private NumericUpDown nudReorderLevel;

        private PictureBox picProduct;
        private CheckBox chkActive;

        private Button btnUploadImage;
        private Button btnRemoveImage;
        private Button btnSave;
        private Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (picProduct != null &&
                    picProduct.Image != null)
                {
                    picProduct.Image.Dispose();
                    picProduct.Image = null;
                }

                if (components != null)
                    components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            pnlFooter = new Panel();

            lblTitle = new Label();
            lblSubtitle = new Label();

            txtSku = new TextBox();
            txtBarcode = new TextBox();
            txtName = new TextBox();

            cmbCategory = new ComboBox();

            nudPrice = new NumericUpDown();
            nudCost = new NumericUpDown();
            nudStock = new NumericUpDown();
            nudReorderLevel = new NumericUpDown();

            picProduct = new PictureBox();
            chkActive = new CheckBox();

            btnUploadImage = new Button();
            btnRemoveImage = new Button();
            btnSave = new Button();
            btnCancel = new Button();

            SuspendLayout();

            // HEADER
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 90;
            pnlHeader.BackColor =
                Color.FromArgb(16, 36, 62);

            lblTitle.Text = "Add Product";
            lblTitle.Font = new Font(
                "Segoe UI", 22F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(25, 10);
            lblTitle.AutoSize = true;

            lblSubtitle.Text =
                "Enter the product information below.";

            lblSubtitle.Font = new Font("Segoe UI", 11F);
            lblSubtitle.ForeColor = Color.LightSteelBlue;
            lblSubtitle.Location = new Point(28, 55);
            lblSubtitle.AutoSize = true;

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubtitle);

            // AUTOMATIC SKU
            Controls.Add(CreateLabel("SKU (Auto-generated)", 30, 110));

            ConfigureTextBox(txtSku, 30, 135, 240);

            txtSku.MaxLength = 30;
            txtSku.ReadOnly = true;
            txtSku.TabStop = false;
            txtSku.BackColor = Color.FromArgb(241, 245, 249);
            txtSku.ForeColor = Color.FromArgb(100, 116, 139);

            // BARCODE
            // AUTOMATIC BARCODE
            Controls.Add(CreateLabel(
                "Barcode (Auto-generated)", 290, 110));

            ConfigureTextBox(txtBarcode, 290, 135, 240);

            txtBarcode.MaxLength = 50;
            txtBarcode.ReadOnly = true;
            txtBarcode.TabStop = false;

            txtBarcode.BackColor =
                Color.FromArgb(241, 245, 249);

            txtBarcode.ForeColor =
                Color.FromArgb(100, 116, 139);

            // PRODUCT NAME
            Controls.Add(CreateLabel(
                "Product Name *", 30, 185));

            ConfigureTextBox(txtName, 30, 210, 500);
            txtName.MaxLength = 150;

            // CATEGORY
            Controls.Add(CreateLabel(
                "Category *", 30, 260));

            cmbCategory.Location = new Point(30, 285);
            cmbCategory.Size = new Size(500, 35);
            cmbCategory.Font = new Font("Segoe UI", 11F);
            cmbCategory.DropDownStyle =
                ComboBoxStyle.DropDownList;

            // SELLING PRICE
            Controls.Add(CreateLabel(
                "Selling Price (PHP) *", 30, 335));

            ConfigureNumeric(
                nudPrice, 30, 360, 240, 2, 100000000);

            // COST PRICE
            Controls.Add(CreateLabel(
                "Cost Price (PHP)", 290, 335));

            ConfigureNumeric(
                nudCost, 290, 360, 240, 2, 100000000);

            // INITIAL STOCK
            lblStock = CreateLabel(
                "Initial Stock", 30, 410);

            Controls.Add(lblStock);

            ConfigureNumeric(
                nudStock, 30, 435, 240, 0, 1000000);

            // REORDER LEVEL
            Controls.Add(CreateLabel(
                "Reorder Level", 290, 410));

            ConfigureNumeric(
                nudReorderLevel, 290, 435,
                240, 0, 1000000);

            nudReorderLevel.Value = 10;

            // ACTIVE STATUS
            chkActive.Text = "Active product";
            chkActive.Font = new Font("Segoe UI", 11F);
            chkActive.Location = new Point(30, 490);
            chkActive.AutoSize = true;
            chkActive.Checked = true;

            // PRODUCT IMAGE
            Controls.Add(CreateLabel(
                "Product Image", 560, 110));

            picProduct.Location = new Point(560, 135);
            picProduct.Size = new Size(260, 260);
            picProduct.BackColor =
                Color.FromArgb(241, 245, 249);

            picProduct.BorderStyle =
                BorderStyle.FixedSingle;

            picProduct.SizeMode =
                PictureBoxSizeMode.Zoom;

            // UPLOAD IMAGE
            ConfigureButton(
                btnUploadImage,
                "Upload Image",
                Color.FromArgb(37, 99, 235));

            btnUploadImage.Location = new Point(560, 410);
            btnUploadImage.Size = new Size(260, 38);

            btnUploadImage.Click +=
                btnUploadImage_Click;

            // REMOVE IMAGE
            ConfigureButton(
                btnRemoveImage,
                "Remove Image",
                Color.FromArgb(71, 85, 105));

            btnRemoveImage.Location = new Point(560, 460);
            btnRemoveImage.Size = new Size(260, 38);

            btnRemoveImage.Click +=
                btnRemoveImage_Click;

            // FOOTER
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Height = 75;
            pnlFooter.BackColor =
                Color.FromArgb(241, 245, 249);

            ConfigureButton(
                btnSave,
                "Save Product",
                Color.FromArgb(37, 99, 235));

            btnSave.Location = new Point(660, 17);
            btnSave.Size = new Size(160, 42);
            btnSave.Click += btnSave_Click;

            ConfigureButton(
                btnCancel,
                "Cancel",
                Color.FromArgb(100, 116, 139));

            btnCancel.Location = new Point(530, 17);
            btnCancel.Size = new Size(115, 42);

            btnCancel.DialogResult = DialogResult.Cancel;

            pnlFooter.Controls.Add(btnSave);
            pnlFooter.Controls.Add(btnCancel);

            // ADD CONTROLS
            Controls.Add(txtSku);
            Controls.Add(txtBarcode);
            Controls.Add(txtName);
            Controls.Add(cmbCategory);

            Controls.Add(nudPrice);
            Controls.Add(nudCost);
            Controls.Add(nudStock);
            Controls.Add(nudReorderLevel);

            Controls.Add(chkActive);
            Controls.Add(picProduct);
            Controls.Add(btnUploadImage);
            Controls.Add(btnRemoveImage);

            Controls.Add(pnlHeader);
            Controls.Add(pnlFooter);

            // FORM SETTINGS
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(850, 610);

            BackColor = Color.White;
            Font = new Font("Segoe UI", 10F);

            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;

            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;

            AcceptButton = btnSave;
            CancelButton = btnCancel;

            Name = "ProductForm";
            Text = "Product Management";

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
                ForeColor = Color.FromArgb(51, 65, 85)
            };
        }

        private void ConfigureTextBox(
            TextBox textbox, int x, int y, int width)
        {
            textbox.Location = new Point(x, y);
            textbox.Size = new Size(width, 35);
            textbox.Font = new Font("Segoe UI", 12F);
        }

        private void ConfigureNumeric(
            NumericUpDown numeric,
            int x,
            int y,
            int width,
            int decimals,
            decimal maximum)
        {
            numeric.Location = new Point(x, y);
            numeric.Size = new Size(width, 35);
            numeric.Font = new Font("Segoe UI", 12F);

            numeric.DecimalPlaces = decimals;
            numeric.Minimum = 0;
            numeric.Maximum = maximum;
            numeric.ThousandsSeparator = true;
            numeric.TextAlign = HorizontalAlignment.Right;
        }

        private void ConfigureButton(
            Button button, string text, Color color)
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
