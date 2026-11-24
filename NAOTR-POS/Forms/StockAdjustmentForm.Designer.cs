
using System.Drawing;
using System.Windows.Forms;

namespace NAOTR_POS.Forms
{
    partial class StockAdjustmentForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Panel pnlProduct;
        private Panel pnlFooter;

        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblProductName;
        private Label lblSku;
        private Label lblCurrentStock;
        private Label lblNewStock;
        private Label lblWarning;

        private ComboBox cmbAdjustmentType;
        private ComboBox cmbReason;

        private NumericUpDown nudQuantity;
        private TextBox txtRemarks;

        private Button btnSave;
        private Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            pnlProduct = new Panel();
            pnlFooter = new Panel();

            lblTitle = new Label();
            lblSubtitle = new Label();
            lblProductName = new Label();
            lblSku = new Label();
            lblCurrentStock = new Label();
            lblNewStock = new Label();
            lblWarning = new Label();

            cmbAdjustmentType = new ComboBox();
            cmbReason = new ComboBox();

            nudQuantity = new NumericUpDown();
            txtRemarks = new TextBox();

            btnSave = new Button();
            btnCancel = new Button();

            SuspendLayout();

            // ================= HEADER =================

            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 85;
            pnlHeader.BackColor =
                Color.FromArgb(16, 36, 62);

            lblTitle.Text = "Stock Adjustment";
            lblTitle.Font = new Font(
                "Segoe UI", 22F, FontStyle.Bold);

            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(25, 10);
            lblTitle.AutoSize = true;

            lblSubtitle.Text =
                "Manage inventory stock movements.";

            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.LightSteelBlue;
            lblSubtitle.Location = new Point(28, 53);
            lblSubtitle.AutoSize = true;

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubtitle);

            // ================= PRODUCT INFORMATION =================

            pnlProduct.Location = new Point(25, 105);
            pnlProduct.Size = new Size(550, 90);

            pnlProduct.BackColor =
                Color.FromArgb(241, 245, 249);

            lblProductName.Text = "Product Name";

            lblProductName.Font = new Font(
                "Segoe UI", 17F, FontStyle.Bold);

            lblProductName.ForeColor =
                Color.FromArgb(16, 36, 62);

            lblProductName.Location = new Point(15, 12);
            lblProductName.Size = new Size(520, 35);
            lblProductName.AutoEllipsis = true;

            lblSku.Text = "SKU: ---";
            lblSku.Font = new Font("Segoe UI", 11F);
            lblSku.ForeColor = Color.SlateGray;
            lblSku.Location = new Point(17, 50);
            lblSku.AutoSize = true;

            pnlProduct.Controls.Add(lblProductName);
            pnlProduct.Controls.Add(lblSku);

            // ================= STOCK CARDS =================

            Controls.Add(
                CreateStockCard(
                    "Current Stock",
                    lblCurrentStock,
                    25,
                    210,
                    Color.RoyalBlue));

            Controls.Add(
                CreateStockCard(
                    "Projected Stock",
                    lblNewStock,
                    310,
                    210,
                    Color.SeaGreen));

            // ================= ADJUSTMENT TYPE =================

            Controls.Add(CreateLabel(
                "Adjustment Type *", 25, 325));

            cmbAdjustmentType.Location =
                new Point(25, 350);

            cmbAdjustmentType.Size =
                new Size(550, 35);

            cmbAdjustmentType.Font =
                new Font("Segoe UI", 11F);

            cmbAdjustmentType.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbAdjustmentType.Items.AddRange(
                new object[]
                {
                    "Stock In",
                    "Stock Out"
                });

            cmbAdjustmentType.SelectedIndexChanged +=
                cmbAdjustmentType_SelectedIndexChanged;

            // ================= QUANTITY =================

            Controls.Add(CreateLabel(
                "Quantity *", 25, 395));

            nudQuantity.Location = new Point(25, 420);
            nudQuantity.Size = new Size(550, 35);
            nudQuantity.Font = new Font("Segoe UI", 12F);

            nudQuantity.Minimum = 1;
            nudQuantity.Maximum = 1000000;
            nudQuantity.Value = 1;
            nudQuantity.ThousandsSeparator = true;

            nudQuantity.ValueChanged +=
                nudQuantity_ValueChanged;

            // ================= REASON =================

            Controls.Add(CreateLabel(
                "Reason *", 25, 465));

            cmbReason.Location = new Point(25, 490);
            cmbReason.Size = new Size(550, 35);
            cmbReason.Font = new Font("Segoe UI", 11F);

            cmbReason.DropDownStyle =
                ComboBoxStyle.DropDownList;

            // ================= REMARKS =================

            Controls.Add(CreateLabel(
                "Additional Remarks", 25, 535));

            txtRemarks.Location = new Point(25, 560);
            txtRemarks.Size = new Size(550, 65);
            txtRemarks.Multiline = true;
            txtRemarks.ScrollBars = ScrollBars.Vertical;
            txtRemarks.Font = new Font("Segoe UI", 11F);
            txtRemarks.MaxLength = 150;

            // ================= WARNING =================

            lblWarning.Location = new Point(25, 635);
            lblWarning.Size = new Size(550, 30);

            lblWarning.Font = new Font(
                "Segoe UI", 10F, FontStyle.Bold);

            lblWarning.ForeColor = Color.Firebrick;
            lblWarning.Text = "";

            // ================= FOOTER =================

            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Height = 75;

            pnlFooter.BackColor =
                Color.FromArgb(241, 245, 249);

            ConfigureButton(
                btnCancel,
                "Cancel",
                Color.FromArgb(100, 116, 139));

            btnCancel.Location = new Point(280, 17);
            btnCancel.Size = new Size(110, 42);
            btnCancel.DialogResult = DialogResult.Cancel;

            ConfigureButton(
                btnSave,
                "Confirm Adjustment",
                Color.FromArgb(37, 99, 235));

            btnSave.Location = new Point(400, 17);
            btnSave.Size = new Size(175, 42);
            btnSave.Click += btnSave_Click;

            pnlFooter.Controls.Add(btnCancel);
            pnlFooter.Controls.Add(btnSave);

            // ================= FINAL LAYOUT =================

            Controls.Add(pnlProduct);
            Controls.Add(cmbAdjustmentType);
            Controls.Add(nudQuantity);
            Controls.Add(cmbReason);
            Controls.Add(txtRemarks);
            Controls.Add(lblWarning);

            Controls.Add(pnlHeader);
            Controls.Add(pnlFooter);

            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(600, 750);

            BackColor = Color.White;
            Font = new Font("Segoe UI", 10F);

            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;

            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;

            AcceptButton = btnSave;
            CancelButton = btnCancel;

            Name = "StockAdjustmentForm";
            Text = "Stock Adjustment";

            ResumeLayout(false);
            PerformLayout();
        }

        private Label CreateLabel(
            string text,
            int x,
            int y)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,

                Font = new Font(
                    "Segoe UI", 10F, FontStyle.Bold),

                ForeColor =
                    Color.FromArgb(51, 65, 85)
            };
        }

        private Panel CreateStockCard(
            string title,
            Label valueLabel,
            int x,
            int y,
            Color accentColor)
        {
            Panel card = new Panel();

            card.Location = new Point(x, y);
            card.Size = new Size(265, 100);

            card.BackColor =
                Color.FromArgb(241, 245, 249);

            Panel accent = new Panel();

            accent.Dock = DockStyle.Left;
            accent.Width = 5;
            accent.BackColor = accentColor;

            Label titleLabel = new Label();

            titleLabel.Text = title;
            titleLabel.Location = new Point(15, 12);
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI", 11F);
            titleLabel.ForeColor = Color.SlateGray;

            valueLabel.Text = "0";

            valueLabel.Font = new Font(
                "Segoe UI", 25F, FontStyle.Bold);

            valueLabel.ForeColor = accentColor;
            valueLabel.Location = new Point(12, 40);
            valueLabel.AutoSize = true;

            card.Controls.Add(titleLabel);
            card.Controls.Add(valueLabel);
            card.Controls.Add(accent);

            return card;
        }

        private void ConfigureButton(
            Button button,
            string text,
            Color color)
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
