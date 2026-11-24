
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using NAOTR_POS.Helpers;
using NAOTR_POS.Models;
using NAOTR_POS.Services;

namespace NAOTR_POS.Forms
{
    public partial class SalesHistoryForm : Form
    {
        private readonly SalesHistoryService historyService =
            new SalesHistoryService();

        private readonly ReceiptService receiptService =
            new ReceiptService();

        private bool isLoading = true;

        public SalesHistoryForm()
        {
            InitializeComponent();
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

            InitializeFilters();

            isLoading = false;

            LoadHistory();
        }

        // ================= INITIALIZE FILTERS =================

        private void InitializeFilters()
        {
            dtpFrom.Value = new DateTime(
                DateTime.Today.Year,
                DateTime.Today.Month,
                1);

            dtpTo.Value = DateTime.Today;

            chkDateFilter.Checked = false;

            dtpFrom.Enabled = false;
            dtpTo.Enabled = false;

            cmbPayment.SelectedIndex = 0;
            cmbStatus.SelectedIndex = 0;

            btnDetails.Enabled = false;
            btnReceipt.Enabled = false;
            btnVoid.Enabled = false;
        }

        // ================= LOAD TRANSACTIONS =================

        private void LoadHistory()
        {
            if (isLoading)
                return;

            try
            {
                DateTime? fromDate =
                    chkDateFilter.Checked
                    ? (DateTime?)dtpFrom.Value.Date
                    : null;

                DateTime? toDate =
                    chkDateFilter.Checked
                    ? (DateTime?)dtpTo.Value.Date
                    : null;

                DataTable table =
                    historyService.GetHistory(
                        txtSearch.Text.Trim(),
                        fromDate,
                        toDate,
                        cmbPayment.Text,
                        cmbStatus.Text);

                dgvSales.DataSource = table;

                ConfigureGridColumns();

                LoadMetrics(table);

                dgvSales.ClearSelection();

                lblRecordCount.Text =
                    "Showing " +
                    table.Rows.Count.ToString("N0") +
                    " transaction(s)";

                UpdateActionButtons();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        // ================= GRID COLUMNS =================

        private void ConfigureGridColumns()
        {
            HideColumn("OrderId");
            HideColumn("Subtotal");
            HideColumn("DiscountAmount");
            HideColumn("TaxAmount");
            HideColumn("AmountPaid");
            HideColumn("ChangeDue");

            SetColumn(
                "OrderNumber", "Order Number", 150);

            SetColumn(
                "OrderDate", "Date & Time", 135);

            SetColumn(
                "CustomerName", "Customer", 125);

            SetColumn(
                "CashierName", "Cashier", 125);

            SetColumn(
                "PaymentMethod", "Payment", 75);

            SetColumn(
                "Total", "Total", 85);

            SetColumn(
                "Status", "Status", 80);

            if (dgvSales.Columns.Contains("OrderDate"))
            {
                dgvSales.Columns["OrderDate"]
                    .DefaultCellStyle.Format =
                        "MM/dd/yyyy hh:mm tt";
            }

            if (dgvSales.Columns.Contains("Total"))
            {
                dgvSales.Columns["Total"]
                    .DefaultCellStyle.Format =
                        "₱#,##0.00";

                dgvSales.Columns["Total"]
                    .DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment
                            .MiddleRight;
            }
        }

        private void HideColumn(string column)
        {
            if (dgvSales.Columns.Contains(column))
            {
                dgvSales.Columns[column].Visible =
                    false;
            }
        }

        private void SetColumn(
            string column,
            string header,
            float weight)
        {
            if (!dgvSales.Columns.Contains(column))
                return;

            dgvSales.Columns[column].HeaderText =
                header;

            dgvSales.Columns[column].FillWeight =
                weight;
        }

        // ================= METRICS =================

        private void LoadMetrics(DataTable table)
        {
            int completed = 0;
            int voided = 0;

            decimal totalSales = 0;

            foreach (DataRow row in table.Rows)
            {
                string status =
                    Convert.ToString(row["Status"]);

                if (status == "Completed")
                {
                    completed++;

                    totalSales +=
                        Convert.ToDecimal(row["Total"]);
                }
                else if (status == "Voided")
                {
                    voided++;
                }
            }

            lblTransactions.Text =
                table.Rows.Count.ToString("N0");

            lblCompleted.Text =
                completed.ToString("N0");

            lblVoided.Text =
                voided.ToString("N0");

            lblTotalSales.Text =
                FormatMoney(totalSales);
        }

        // ================= SELECTED TRANSACTION =================

        private int GetSelectedOrderId()
        {
            if (dgvSales.SelectedRows.Count == 0 ||
                !dgvSales.Columns.Contains("OrderId"))
            {
                return 0;
            }

            object value =
                dgvSales.SelectedRows[0]
                    .Cells["OrderId"].Value;

            if (value == null ||
                value == DBNull.Value)
            {
                return 0;
            }

            return Convert.ToInt32(value);
        }

        private string GetSelectedStatus()
        {
            if (dgvSales.SelectedRows.Count == 0 ||
                !dgvSales.Columns.Contains("Status"))
            {
                return "";
            }

            return Convert.ToString(
                dgvSales.SelectedRows[0]
                    .Cells["Status"].Value);
        }

        private void UpdateActionButtons()
        {
            bool selected =
                GetSelectedOrderId() > 0;

            btnDetails.Enabled = selected;

            btnReceipt.Enabled = selected;

            btnVoid.Enabled =
                selected &&
                GetSelectedStatus() == "Completed" &&
                (
                    Session.IsInRole("Admin") ||
                    Session.IsInRole("Manager")
                );
        }

        private void dgvSales_SelectionChanged(
            object sender,
            EventArgs e)
        {
            UpdateActionButtons();
        }

        // ================= SEARCH =================

        private void btnSearch_Click(
            object sender,
            EventArgs e)
        {
            LoadHistory();
        }

        private void txtSearch_KeyDown(
            object sender,
            KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                LoadHistory();
            }
        }

        // ================= RESET =================

        private void btnReset_Click(
            object sender,
            EventArgs e)
        {
            isLoading = true;

            txtSearch.Clear();

            InitializeFilters();

            isLoading = false;

            LoadHistory();
        }

        private void chkDateFilter_CheckedChanged(
            object sender,
            EventArgs e)
        {
            dtpFrom.Enabled =
                chkDateFilter.Checked;

            dtpTo.Enabled =
                chkDateFilter.Checked;
        }

        // ================= VIEW DETAILS =================

        private void btnDetails_Click(
            object sender,
            EventArgs e)
        {
            int orderId = GetSelectedOrderId();

            if (orderId == 0)
                return;

            try
            {
                ShowTransactionDetails(orderId);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void ShowTransactionDetails(
            int orderId)
        {
            CheckoutResult order =
                receiptService.GetOrder(orderId);

            var items =
                receiptService.GetReceiptItems(orderId);

            using (Form dialog = new Form())
            {
                dialog.Text = "Transaction Details";

                dialog.Size = new Size(800, 650);

                dialog.MinimumSize =
                    new Size(650, 500);

                dialog.StartPosition =
                    FormStartPosition.CenterParent;

                dialog.BackColor = Color.White;

                dialog.MinimizeBox = false;

                dialog.Font =
                    new Font("Segoe UI", 10F);

                Panel header = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 145,

                    BackColor =
                        Color.FromArgb(16, 36, 62),

                    Padding = new Padding(15)
                };

                Label information = new Label
                {
                    Dock = DockStyle.Fill,

                    ForeColor = Color.White,

                    Font = new Font("Segoe UI", 11F),

                    Text =
                        "Order: " + order.OrderNumber +
                        "\nDate: " +
                        order.OrderDate.ToString(
                            "MM/dd/yyyy hh:mm tt") +
                        "\nCustomer: " +
                        order.CustomerName +
                        "\nCashier: " +
                        order.CashierName +
                        "\nStatus: " + order.Status
                };

                header.Controls.Add(information);

                DataGridView grid =
                    new DataGridView
                    {
                        Dock = DockStyle.Fill,

                        ReadOnly = true,

                        AllowUserToAddRows = false,

                        AllowUserToDeleteRows = false,

                        RowHeadersVisible = false,

                        AutoGenerateColumns = false,

                        BackgroundColor = Color.White,

                        BorderStyle = BorderStyle.None,

                        AutoSizeColumnsMode =
                            DataGridViewAutoSizeColumnsMode
                                .Fill
                    };

                AddDetailColumn(
                    grid, "Sku", "SKU", 80);

                AddDetailColumn(
                    grid, "ProductName", "Product", 160);

                AddDetailColumn(
                    grid, "Quantity", "Qty", 45);

                AddDetailColumn(
                    grid, "UnitPrice", "Unit Price", 80);

                AddDetailColumn(
                    grid, "LineTotal", "Total", 80);

                grid.Columns["UnitPrice"]
                    .DefaultCellStyle.Format =
                        "₱#,##0.00";

                grid.Columns["LineTotal"]
                    .DefaultCellStyle.Format =
                        "₱#,##0.00";

                grid.DataSource = items;

                Panel footer = new Panel
                {
                    Dock = DockStyle.Bottom,
                    Height = 220,

                    Padding = new Padding(15),

                    BackColor =
                        Color.FromArgb(241, 245, 249)
                };

                Label totals = new Label
                {
                    Dock = DockStyle.Fill,

                    Font = new Font("Segoe UI", 11F),

                    Text =
                        "Subtotal: " +
                        FormatMoney(order.Subtotal) +
                        "\nDiscount: " +
                        FormatMoney(order.DiscountAmount) +
                        "\nTax: " +
                        FormatMoney(order.TaxAmount) +
                        "\nTOTAL: " +
                        FormatMoney(order.Total) +
                        "\nPayment: " +
                        order.PaymentMethod +
                        "\nAmount Paid: " +
                        FormatMoney(order.AmountPaid) +
                        "\nChange: " +
                        FormatMoney(order.ChangeDue)
                };

                Button close = new Button
                {
                    Text = "Close",

                    Dock = DockStyle.Bottom,

                    Height = 35,

                    BackColor =
                        Color.FromArgb(16, 36, 62),

                    ForeColor = Color.White,

                    FlatStyle = FlatStyle.Flat,

                    DialogResult = DialogResult.Cancel
                };

                footer.Controls.Add(totals);
                footer.Controls.Add(close);

                dialog.Controls.Add(grid);
                dialog.Controls.Add(footer);
                dialog.Controls.Add(header);

                dialog.CancelButton = close;

                dialog.ShowDialog(this);
            }
        }

        private void AddDetailColumn(
            DataGridView grid,
            string property,
            string title,
            float weight)
        {
            grid.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = property,

                    DataPropertyName = property,

                    HeaderText = title,

                    FillWeight = weight
                });
        }

        // ================= REPRINT RECEIPT =================

        private void btnReceipt_Click(
            object sender,
            EventArgs e)
        {
            int orderId = GetSelectedOrderId();

            if (orderId == 0)
                return;

            using (ReceiptForm receipt =
                new ReceiptForm(orderId))
            {
                receipt.ShowDialog(this);
            }
        }

        // ================= VOID TRANSACTION =================

        private void btnVoid_Click(
            object sender,
            EventArgs e)
        {
            int orderId = GetSelectedOrderId();

            if (orderId == 0)
                return;

            if (GetSelectedStatus() != "Completed")
            {
                MessageBox.Show(
                    "This transaction cannot be voided.");

                return;
            }

            string reason = RequestVoidReason();

            if (string.IsNullOrWhiteSpace(reason))
                return;

            DialogResult confirm =
                MessageBox.Show(
                    "Void this transaction?" +
                    "\n\nAll purchased quantities will " +
                    "be restored to inventory." +
                    "\n\nConfirm that any required refund " +
                    "has been handled separately and the " +
                    "items can be returned to stock." +
                    "\n\nThis action cannot be undone here.",
                    "Confirm Transaction Void",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

            if (confirm != DialogResult.Yes)
                return;

            btnVoid.Enabled = false;

            try
            {
                historyService.VoidOrder(
                    orderId,
                    reason);

                MessageBox.Show(
                    "Transaction voided successfully." +
                    "\nInventory has been restored.",
                    "Transaction Voided",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadHistory();
            }
            catch (Exception ex)
            {
                ShowError(ex);

                LoadHistory();
            }
        }

        // ================= VOID REASON DIALOG =================

        private string RequestVoidReason()
        {
            using (Form dialog = new Form())
            {
                dialog.Text = "Void Transaction";

                dialog.ClientSize =
                    new Size(450, 245);

                dialog.StartPosition =
                    FormStartPosition.CenterParent;

                dialog.FormBorderStyle =
                    FormBorderStyle.FixedDialog;

                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;

                dialog.BackColor = Color.White;

                Label label = new Label
                {
                    Text = "Enter the reason for voiding:",

                    Location = new Point(20, 20),

                    AutoSize = true,

                    Font = new Font(
                        "Segoe UI", 11F, FontStyle.Bold)
                };

                TextBox reason = new TextBox
                {
                    Location = new Point(20, 55),

                    Size = new Size(410, 110),

                    Multiline = true,

                    MaxLength = 150,

                    ScrollBars = ScrollBars.Vertical,

                    Font = new Font("Segoe UI", 11F)
                };

                Button confirm = new Button
                {
                    Text = "Continue",

                    Location = new Point(200, 185),

                    Size = new Size(110, 38),

                    BackColor =
                        Color.FromArgb(220, 38, 38),

                    ForeColor = Color.White,

                    FlatStyle = FlatStyle.Flat
                };

                Button cancel = new Button
                {
                    Text = "Cancel",

                    Location = new Point(320, 185),

                    Size = new Size(110, 38),

                    DialogResult = DialogResult.Cancel
                };

                confirm.Click += delegate
                {
                    if (string.IsNullOrWhiteSpace(
                        reason.Text))
                    {
                        MessageBox.Show(
                            "Void reason is required.");

                        reason.Focus();

                        return;
                    }

                    dialog.DialogResult =
                        DialogResult.OK;

                    dialog.Close();
                };

                dialog.Controls.Add(label);
                dialog.Controls.Add(reason);
                dialog.Controls.Add(confirm);
                dialog.Controls.Add(cancel);

                dialog.AcceptButton = confirm;
                dialog.CancelButton = cancel;

                if (dialog.ShowDialog(this) ==
                    DialogResult.OK)
                {
                    return reason.Text.Trim();
                }

                return null;
            }
        }

        // ================= STATUS COLORS =================

        private void dgvSales_CellFormatting(
            object sender,
            DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 ||
                e.ColumnIndex < 0 ||
                e.Value == null)
            {
                return;
            }

            if (dgvSales.Columns[e.ColumnIndex].Name
                != "Status")
            {
                return;
            }

            string status =
                Convert.ToString(e.Value);

            if (status == "Completed")
            {
                e.CellStyle.ForeColor =
                    Color.SeaGreen;
            }
            else if (status == "Voided")
            {
                e.CellStyle.ForeColor =
                    Color.Firebrick;
            }
        }

        private void dgvSales_CellDoubleClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            dgvSales.ClearSelection();

            dgvSales.Rows[e.RowIndex].Selected =
                true;

            btnDetails_Click(sender, EventArgs.Empty);
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
                "Sales History Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
