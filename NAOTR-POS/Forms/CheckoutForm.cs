
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NAOTR_POS.Models;
using NAOTR_POS.Services;
using NAOTR_POS.Helpers;

namespace NAOTR_POS.Forms
{
    public partial class CheckoutForm : Form
    {
        private readonly CheckoutService checkoutService =
            new CheckoutService();

        private readonly List<CartItem> items;

        private bool isLoading = true;
        private bool isCalculating = false;
        private bool isProcessing = false;

        private decimal subtotal;
        private decimal taxAmount;
        private decimal total;

        public int CompletedOrderId
        {
            get;
            private set;
        }

        public CheckoutResult Result
        {
            get;
            private set;
        }

        // Default constructor for WinForms Designer.

        public CheckoutForm()
            : this(new List<CartItem>())
        {
        }

        public CheckoutForm(List<CartItem> cartItems)
        {
            InitializeComponent();

            items = cartItems ?? new List<CartItem>();
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

            if (items.Count == 0)
            {
                MessageBox.Show("The cart is empty.");

                Close();
                return;
            }

            try
            {
                LoadCustomers();

                dgvOrder.DataSource = items;

                subtotal =
                    items.Sum(x => x.LineTotal);

                nudDiscount.Maximum = subtotal;

                nudAmountPaid.Maximum =
                    Math.Max(
                        100000000M,
                        subtotal * 2M);

                nudDiscount.Value = 0;

                nudTaxRate.Value = 0;

                cmbPaymentMethod.SelectedIndex = 0;

                isLoading = false;

                CalculateTotals();

                nudAmountPaid.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Checkout Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                Close();
            }
        }

        // ================= LOAD CUSTOMERS =================

        private void LoadCustomers()
        {
            DataTable customers =
                checkoutService.GetCustomers();

            DataRow walkIn = customers.NewRow();

            walkIn["CustomerId"] = 0;

            walkIn["FullName"] =
                "Walk-in Customer";

            customers.Rows.InsertAt(walkIn, 0);

            cmbCustomer.DisplayMember =
                "FullName";

            cmbCustomer.ValueMember =
                "CustomerId";

            cmbCustomer.DataSource = customers;

            cmbCustomer.SelectedIndex = 0;
        }

        private int? GetCustomerId()
        {
            if (cmbCustomer.SelectedValue is int)
            {
                int id =
                    (int)cmbCustomer.SelectedValue;

                if (id > 0)
                    return id;
            }

            return null;
        }

        // ================= CALCULATE TOTALS =================

        private void CalculateTotals()
        {
            if (isLoading || isCalculating)
                return;

            isCalculating = true;

            try
            {
                decimal discount =
                    nudDiscount.Value;

                decimal taxRate =
                    nudTaxRate.Value;

                decimal taxableAmount =
                    subtotal - discount;

                taxAmount = Math.Round(
                    taxableAmount * taxRate / 100M,
                    2,
                    MidpointRounding.AwayFromZero);

                total = taxableAmount + taxAmount;

                string method =
                    cmbPaymentMethod.Text;

                bool isCash = method == "Cash";

                nudAmountPaid.Enabled = isCash;

                btnExactAmount.Enabled = isCash;

                if (!isCash)
                {
                    nudAmountPaid.Value = total;
                }

                decimal amountPaid =
                    nudAmountPaid.Value;

                decimal change =
                    amountPaid - total;

                lblSubtotal.Text =
                    FormatMoney(subtotal);

                lblDiscount.Text =
                    "-" + FormatMoney(discount);

                lblTax.Text =
                    FormatMoney(taxAmount);

                lblTotal.Text =
                    FormatMoney(total);

                lblChange.Text =
                    FormatMoney(
                        Math.Max(0M, change));

                if (amountPaid < total)
                {
                    lblWarning.Text =
                        "Insufficient payment. Remaining: " +
                        FormatMoney(total - amountPaid);

                    lblWarning.ForeColor =
                        Color.Firebrick;

                    btnComplete.Enabled = false;
                }
                else
                {
                    lblWarning.Text =
                        "Payment amount is sufficient.";

                    lblWarning.ForeColor =
                        Color.SeaGreen;

                    btnComplete.Enabled =
                        !isProcessing;
                }
            }
            finally
            {
                isCalculating = false;
            }
        }

        // ================= PAYMENT EVENTS =================

        private void PaymentValueChanged(
            object sender, EventArgs e)
        {
            CalculateTotals();
        }

        private void cmbPaymentMethod_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            CalculateTotals();
        }

        private void btnExactAmount_Click(
            object sender, EventArgs e)
        {
            nudAmountPaid.Value = total;

            CalculateTotals();
        }

        // ================= COMPLETE TRANSACTION =================

        private void btnComplete_Click(
            object sender, EventArgs e)
        {
            if (isProcessing ||
                CompletedOrderId > 0)
            {
                return;
            }

            CalculateTotals();

            if (nudAmountPaid.Value < total)
            {
                MessageBox.Show(
                    "Insufficient payment.");

                return;
            }

            string method =
                cmbPaymentMethod.Text;

            string confirmation =
                "Complete this transaction?" +
                "\n\nTotal: " + FormatMoney(total) +
                "\nPayment Method: " + method +
                "\nAmount Paid: " +
                FormatMoney(nudAmountPaid.Value);

            if (method != "Cash")
            {
                confirmation +=
                    "\n\nConfirm that payment was " +
                    "received through the external " +
                    "payment terminal or service.";
            }

            DialogResult confirm =
                MessageBox.Show(
                    confirmation,
                    "Confirm Transaction",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);

            if (confirm != DialogResult.Yes)
                return;

            isProcessing = true;

            btnComplete.Enabled = false;
            btnCancel.Enabled = false;

            btnComplete.Text = "Processing...";

            try
            {
                CompletedOrderId =
                    checkoutService.CreateOrder(
                        items,
                        GetCustomerId(),
                        method,
                        nudDiscount.Value,
                        nudTaxRate.Value,
                        nudAmountPaid.Value);

                // The order has already been saved.
                // Do not retry if receipt lookup fails.

                try
                {
                    Result = checkoutService.GetOrder(
                        CompletedOrderId);
                }
                catch
                {
                    Result = null;
                }

                string message;

                if (Result != null)
                {
                    message =
                        "Transaction completed successfully!" +
                        "\n\nOrder Number: " +
                        Result.OrderNumber +
                        "\nCustomer: " +
                        Result.CustomerName +
                        "\nTotal: " +
                        FormatMoney(Result.Total) +
                        "\nAmount Paid: " +
                        FormatMoney(Result.AmountPaid) +
                        "\nChange: " +
                        FormatMoney(Result.ChangeDue);
                }
                else
                {
                    message =
                        "Transaction saved successfully." +
                        "\nOrder ID: " +
                        CompletedOrderId +
                        "\nOrder details could not be loaded.";
                }

                MessageBox.Show(
                    message,
                    "Transaction Successful",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message +
                    "\n\nIf the connection was interrupted, " +
                    "verify the Orders table before retrying.",
                    "Transaction Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                isProcessing = false;

                btnCancel.Enabled = true;

                btnComplete.Text =
                    "Complete Transaction";

                CalculateTotals();
            }
        }

        protected override void OnFormClosing(
            FormClosingEventArgs e)
        {
            if (isProcessing &&
                CompletedOrderId == 0)
            {
                e.Cancel = true;
                return;
            }

            base.OnFormClosing(e);
        }

        // ================= MONEY FORMAT =================

        private string FormatMoney(decimal amount)
        {
            return "₱" + amount.ToString("N2");
        }
    }
}
