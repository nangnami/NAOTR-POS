
using System.Drawing;
using System.Windows.Forms;

namespace NAOTR_POS.Forms
{
    partial class CheckoutForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Label lblTitle;
        private Label lblSubtitle;

        private TableLayoutPanel tblMain;
        private TableLayoutPanel tblSummary;
        private TableLayoutPanel tblPayment;

        private DataGridView dgvOrder;

        private ComboBox cmbCustomer;
        private ComboBox cmbPaymentMethod;

        private NumericUpDown nudDiscount;
        private NumericUpDown nudTaxRate;
        private NumericUpDown nudAmountPaid;

        private Label lblSubtotal;
        private Label lblDiscount;
        private Label lblTax;
        private Label lblTotal;
        private Label lblChange;
        private Label lblWarning;

        private Button btnExactAmount;
        private Button btnComplete;
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
            lblTitle = new Label();
            lblSubtitle = new Label();

            tblMain = new TableLayoutPanel();
            tblSummary = new TableLayoutPanel();
            tblPayment = new TableLayoutPanel();

            dgvOrder = new DataGridView();

            cmbCustomer = new ComboBox();
            cmbPaymentMethod = new ComboBox();

            nudDiscount = new NumericUpDown();
            nudTaxRate = new NumericUpDown();
            nudAmountPaid = new NumericUpDown();

            lblSubtotal = new Label();
            lblDiscount = new Label();
            lblTax = new Label();
            lblTotal = new Label();
            lblChange = new Label();
            lblWarning = new Label();

            btnExactAmount = new Button();
            btnComplete = new Button();
            btnCancel = new Button();

            SuspendLayout();

            // ================= HEADER =================

            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 85;

            pnlHeader.BackColor =
                Color.FromArgb(16, 36, 62);

            lblTitle.Text = "Checkout & Payment";

            lblTitle.Font = new Font(
                "Segoe UI", 23F, FontStyle.Bold);

            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(22, 8);
            lblTitle.AutoSize = true;

            lblSubtitle.Text =
                "Review and complete the transaction.";

            lblSubtitle.Font =
                new Font("Segoe UI", 10F);

            lblSubtitle.ForeColor =
                Color.LightSteelBlue;

            lblSubtitle.Location =
                new Point(25, 54);

            lblSubtitle.AutoSize = true;

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubtitle);

            // ================= MAIN LAYOUT =================

            tblMain.Dock = DockStyle.Fill;
            tblMain.Padding = new Padding(15);

            tblMain.ColumnCount = 2;
            tblMain.RowCount = 1;

            tblMain.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent, 50F));

            tblMain.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent, 50F));

            tblMain.RowStyles.Add(
                new RowStyle(
                    SizeType.Percent, 100F));

            // ================= ORDER SUMMARY =================

            tblSummary.Dock = DockStyle.Fill;

            tblSummary.Padding =
                new Padding(12);

            tblSummary.BackColor =
                Color.FromArgb(248, 250, 252);

            tblSummary.ColumnCount = 2;
            tblSummary.RowCount = 6;

            tblSummary.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent, 50F));

            tblSummary.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent, 50F));

            tblSummary.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 42));

            tblSummary.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100));

            tblSummary.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 38));

            tblSummary.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 38));

            tblSummary.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 38));

            tblSummary.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 65));

            Label lblSummaryTitle = CreateLabel(
                "Order Summary", 17F, true);

            tblSummary.Controls.Add(
                lblSummaryTitle, 0, 0);

            tblSummary.SetColumnSpan(
                lblSummaryTitle, 2);

            // ================= ORDER GRID =================

            dgvOrder.Dock = DockStyle.Fill;
            dgvOrder.BackgroundColor = Color.White;
            dgvOrder.BorderStyle = BorderStyle.None;

            dgvOrder.ReadOnly = true;
            dgvOrder.AllowUserToAddRows = false;
            dgvOrder.AllowUserToDeleteRows = false;
            dgvOrder.AllowUserToResizeRows = false;

            dgvOrder.RowHeadersVisible = false;
            dgvOrder.AutoGenerateColumns = false;

            dgvOrder.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvOrder.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvOrder.EnableHeadersVisualStyles = false;

            dgvOrder.ColumnHeadersHeight = 38;
            dgvOrder.RowTemplate.Height = 38;

            dgvOrder.ColumnHeadersDefaultCellStyle
                .BackColor =
                    Color.FromArgb(16, 36, 62);

            dgvOrder.ColumnHeadersDefaultCellStyle
                .ForeColor = Color.White;

            dgvOrder.DefaultCellStyle.Font =
                new Font("Segoe UI", 10F);

            dgvOrder.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    HeaderText = "Product",
                    DataPropertyName = "ProductName",
                    FillWeight = 150
                });

            dgvOrder.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    HeaderText = "Qty",
                    DataPropertyName = "Quantity",
                    FillWeight = 40
                });

            dgvOrder.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    HeaderText = "Total",
                    DataPropertyName = "LineTotal",
                    FillWeight = 80,

                    DefaultCellStyle =
                        new DataGridViewCellStyle
                        {
                            Format = "₱#,##0.00"
                        }
                });

            tblSummary.Controls.Add(
                dgvOrder, 0, 1);

            tblSummary.SetColumnSpan(
                dgvOrder, 2);

            AddSummaryRow(
                "Subtotal", lblSubtotal, 2);

            AddSummaryRow(
                "Discount", lblDiscount, 3);

            AddSummaryRow(
                "Tax", lblTax, 4);

            AddSummaryRow(
                "TOTAL DUE", lblTotal, 5);

            lblTotal.Font = new Font(
                "Segoe UI", 24F, FontStyle.Bold);

            lblTotal.ForeColor =
                Color.FromArgb(22, 163, 74);

            // ================= PAYMENT LAYOUT =================

            tblPayment.Dock = DockStyle.Fill;

            tblPayment.Padding =
                new Padding(15, 5, 5, 5);

            tblPayment.ColumnCount = 2;
            tblPayment.RowCount = 13;

            tblPayment.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent, 50F));

            tblPayment.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent, 50F));

            int[] rowHeights =
            {
                27, 38,
                27, 38,
                27, 38,
                27, 42,
                40, 52,
                40, 50, 42
            };

            foreach (int height in rowHeights)
            {
                tblPayment.RowStyles.Add(
                    new RowStyle(
                        SizeType.Absolute, height));
            }

            // CUSTOMER

            AddPaymentLabel(
                "Customer", 0, 0, 2);

            ConfigureComboBox(cmbCustomer);

            tblPayment.Controls.Add(
                cmbCustomer, 0, 1);

            tblPayment.SetColumnSpan(
                cmbCustomer, 2);

            // PAYMENT METHOD

            AddPaymentLabel(
                "Payment Method", 0, 2, 2);

            ConfigureComboBox(cmbPaymentMethod);

            cmbPaymentMethod.Items.AddRange(
                new object[]
                {
                    "Cash",
                    "Card",
                    "Mobile"
                });

            cmbPaymentMethod.SelectedIndexChanged +=
                cmbPaymentMethod_SelectedIndexChanged;

            tblPayment.Controls.Add(
                cmbPaymentMethod, 0, 3);

            tblPayment.SetColumnSpan(
                cmbPaymentMethod, 2);

            // DISCOUNT AND TAX

            AddPaymentLabel(
                "Discount (₱)", 0, 4, 1);

            AddPaymentLabel(
                "Tax Rate (%)", 1, 4, 1);

            ConfigureNumeric(
                nudDiscount, 100000000M);

            ConfigureNumeric(
                nudTaxRate, 100M);

            nudDiscount.ValueChanged +=
                PaymentValueChanged;

            nudTaxRate.ValueChanged +=
                PaymentValueChanged;

            tblPayment.Controls.Add(
                nudDiscount, 0, 5);

            tblPayment.Controls.Add(
                nudTaxRate, 1, 5);

            // AMOUNT TENDERED

            AddPaymentLabel(
                "Amount Tendered (₱)", 0, 6, 2);

            ConfigureNumeric(
                nudAmountPaid, 100000000M);

            nudAmountPaid.Font =
                new Font("Segoe UI", 16F);

            nudAmountPaid.ValueChanged +=
                PaymentValueChanged;

            tblPayment.Controls.Add(
                nudAmountPaid, 0, 7);

            tblPayment.SetColumnSpan(
                nudAmountPaid, 2);

            // EXACT AMOUNT BUTTON

            ConfigureButton(
                btnExactAmount,
                "Use Exact Amount",
                Color.FromArgb(71, 85, 105));

            btnExactAmount.Click +=
                btnExactAmount_Click;

            tblPayment.Controls.Add(
                btnExactAmount, 0, 8);

            tblPayment.SetColumnSpan(
                btnExactAmount, 2);

            // CHANGE DUE

            Label changeTitle = CreateLabel(
                "Change Due", 13F, true);

            lblChange.Text = "₱0.00";

            lblChange.Dock = DockStyle.Fill;

            lblChange.TextAlign =
                ContentAlignment.MiddleRight;

            lblChange.Font = new Font(
                "Segoe UI", 22F, FontStyle.Bold);

            lblChange.ForeColor =
                Color.FromArgb(22, 163, 74);

            tblPayment.Controls.Add(
                changeTitle, 0, 9);

            tblPayment.Controls.Add(
                lblChange, 1, 9);

            // WARNING

            lblWarning.Dock = DockStyle.Fill;

            lblWarning.ForeColor = Color.Firebrick;

            lblWarning.Font =
                new Font("Segoe UI", 10F);

            lblWarning.TextAlign =
                ContentAlignment.MiddleLeft;

            tblPayment.Controls.Add(
                lblWarning, 0, 10);

            tblPayment.SetColumnSpan(
                lblWarning, 2);

            // COMPLETE TRANSACTION

            ConfigureButton(
                btnComplete,
                "Complete Transaction",
                Color.FromArgb(22, 163, 74));

            btnComplete.Click += btnComplete_Click;

            btnComplete.Enabled = false;

            tblPayment.Controls.Add(
                btnComplete, 0, 11);

            tblPayment.SetColumnSpan(
                btnComplete, 2);

            // CANCEL

            ConfigureButton(
                btnCancel,
                "Cancel",
                Color.FromArgb(100, 116, 139));

            btnCancel.DialogResult =
                DialogResult.Cancel;

            tblPayment.Controls.Add(
                btnCancel, 0, 12);

            tblPayment.SetColumnSpan(
                btnCancel, 2);

            // ================= FORM SETTINGS =================

            tblMain.Controls.Add(
                tblSummary, 0, 0);

            tblMain.Controls.Add(
                tblPayment, 1, 0);

            Controls.Add(tblMain);
            Controls.Add(pnlHeader);

            AutoScaleMode = AutoScaleMode.None;

            ClientSize = new Size(920, 675);

            MinimumSize = new Size(850, 700);

            BackColor = Color.White;

            Font = new Font("Segoe UI", 10F);

            StartPosition =
                FormStartPosition.CenterParent;

            FormBorderStyle =
                FormBorderStyle.FixedDialog;

            MaximizeBox = false;
            MinimizeBox = false;

            CancelButton = btnCancel;

            Name = "CheckoutForm";
            Text = "Checkout & Payment";

            ResumeLayout(false);
        }

        // ================= UI HELPERS =================

        private Label CreateLabel(
            string text,
            float size,
            bool bold)
        {
            return new Label
            {
                Text = text,
                Dock = DockStyle.Fill,

                Font = new Font(
                    "Segoe UI",
                    size,
                    bold
                        ? FontStyle.Bold
                        : FontStyle.Regular),

                ForeColor =
                    Color.FromArgb(51, 65, 85),

                TextAlign =
                    ContentAlignment.MiddleLeft
            };
        }

        private void AddSummaryRow(
            string title,
            Label value,
            int row)
        {
            Label label = CreateLabel(
                title, 12F, row == 5);

            value.Text = "₱0.00";
            value.Dock = DockStyle.Fill;

            value.Font = new Font(
                "Segoe UI", 12F);

            value.TextAlign =
                ContentAlignment.MiddleRight;

            tblSummary.Controls.Add(
                label, 0, row);

            tblSummary.Controls.Add(
                value, 1, row);
        }

        private void AddPaymentLabel(
            string text,
            int column,
            int row,
            int span)
        {
            Label label =
                CreateLabel(text, 10F, true);

            tblPayment.Controls.Add(
                label, column, row);

            tblPayment.SetColumnSpan(
                label, span);
        }

        private void ConfigureComboBox(
            ComboBox combo)
        {
            combo.Dock = DockStyle.Fill;

            combo.DropDownStyle =
                ComboBoxStyle.DropDownList;

            combo.Font =
                new Font("Segoe UI", 12F);
        }

        private void ConfigureNumeric(
            NumericUpDown numeric,
            decimal maximum)
        {
            numeric.Dock = DockStyle.Fill;

            numeric.DecimalPlaces = 2;

            numeric.Minimum = 0;

            numeric.Maximum = maximum;

            numeric.ThousandsSeparator = true;

            numeric.Font =
                new Font("Segoe UI", 12F);

            numeric.TextAlign =
                HorizontalAlignment.Right;
        }

        private void ConfigureButton(
            Button button,
            string text,
            Color color)
        {
            button.Text = text;

            button.Dock = DockStyle.Fill;

            button.BackColor = color;
            button.ForeColor = Color.White;

            button.Font = new Font(
                "Segoe UI", 11F, FontStyle.Bold);

            button.FlatStyle = FlatStyle.Flat;

            button.FlatAppearance.BorderSize = 0;

            button.Cursor = Cursors.Hand;

            button.UseVisualStyleBackColor = false;
        }
    }
}
