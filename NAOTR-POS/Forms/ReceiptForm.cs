
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using NAOTR_POS.Models;
using NAOTR_POS.Services;

namespace NAOTR_POS.Forms
{
    public partial class ReceiptForm : Form
    {
        private readonly ReceiptService receiptService =
            new ReceiptService();

        private readonly int orderId;

        private CheckoutResult order;

        private List<ReceiptItem> items;

        private string receiptText = "";

        private int printLineIndex = 0;

        private const int ReceiptWidth = 42;

        // Default constructor for Designer.

        public ReceiptForm()
            : this(0)
        {
        }

        public ReceiptForm(int orderId)
        {
            InitializeComponent();

            this.orderId = orderId;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            try
            {
                if (orderId <= 0)
                {
                    throw new Exception(
                        "Invalid transaction ID.");
                }

                LoadReceipt();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Receipt Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                Close();
            }
        }

        // ================= LOAD RECEIPT =================

        private void LoadReceipt()
        {
            order =
                receiptService.GetOrder(orderId);

            items =
                receiptService.GetReceiptItems(orderId);

            receiptText = GenerateReceipt();

            txtReceipt.Text = receiptText;

            txtReceipt.SelectionStart = 0;

            txtReceipt.ScrollToCaret();

            printDocument.DocumentName =
                order.OrderNumber;

            lblSubtitle.Text =
                "Transaction: " + order.OrderNumber;
        }

        // ================= GENERATE RECEIPT =================

        private string GenerateReceipt()
        {
            StringBuilder receipt =
                new StringBuilder();

            string separator =
                new string('-', ReceiptWidth);

            string doubleSeparator =
                new string('=', ReceiptWidth);

            receipt.AppendLine(
                CenterText("NAOTR-POS"));

            receipt.AppendLine(
                CenterText("Retail & Inventory System"));

            receipt.AppendLine(
                CenterText("SALES RECEIPT"));

            receipt.AppendLine(
                CenterText("School Prototype"));

            receipt.AppendLine(doubleSeparator);

            AppendWrapped(
                receipt,
                "Order: " + order.OrderNumber);

            AppendWrapped(
                receipt,
                "Date: " +
                order.OrderDate.ToString(
                    "MM/dd/yyyy hh:mm tt"));

            AppendWrapped(
                receipt,
                "Cashier: " + order.CashierName);

            AppendWrapped(
                receipt,
                "Customer: " + order.CustomerName);

            receipt.AppendLine(separator);

            if (order.Status == "Voided")
            {
                receipt.AppendLine(
                    CenterText("*** VOIDED ***"));

                receipt.AppendLine(separator);
            }

            receipt.AppendLine(
                AlignColumns("ITEM / QTY", "AMOUNT"));

            receipt.AppendLine(separator);

            int totalQuantity = 0;

            foreach (ReceiptItem item in items)
            {
                AppendWrapped(
                    receipt,
                    item.ProductName);

                string quantityPrice =
                    item.Quantity +
                    " x " +
                    item.UnitPrice.ToString("N2");

                receipt.AppendLine(
                    AlignColumns(
                        quantityPrice,
                        item.LineTotal.ToString("N2")));

                totalQuantity += item.Quantity;
            }

            receipt.AppendLine(separator);

            receipt.AppendLine(
                AlignColumns(
                    "Total Items",
                    totalQuantity.ToString()));

            receipt.AppendLine(
                AlignColumns(
                    "Subtotal",
                    order.Subtotal.ToString("N2")));

            receipt.AppendLine(
                AlignColumns(
                    "Discount",
                    order.DiscountAmount.ToString("N2")));

            receipt.AppendLine(
                AlignColumns(
                    "Tax",
                    order.TaxAmount.ToString("N2")));

            receipt.AppendLine(doubleSeparator);

            receipt.AppendLine(
                AlignColumns(
                    "TOTAL (PHP)",
                    order.Total.ToString("N2")));

            receipt.AppendLine(doubleSeparator);

            receipt.AppendLine(
                AlignColumns(
                    "Payment Method",
                    order.PaymentMethod));

            receipt.AppendLine(
                AlignColumns(
                    "Amount Paid",
                    order.AmountPaid.ToString("N2")));

            receipt.AppendLine(
                AlignColumns(
                    "Change",
                    order.ChangeDue.ToString("N2")));

            receipt.AppendLine(
                AlignColumns(
                    "Status",
                    order.Status));

            receipt.AppendLine(separator);

            receipt.AppendLine();

            receipt.AppendLine(
                CenterText("Thank you for shopping!"));

            receipt.AppendLine(
                CenterText("Please keep this receipt."));

            receipt.AppendLine();

            receipt.AppendLine(
                CenterText("NOT AN OFFICIAL TAX INVOICE"));

            receipt.AppendLine(doubleSeparator);

            return receipt.ToString();
        }

        // ================= CENTER TEXT =================

        private string CenterText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";

            if (text.Length >= ReceiptWidth)
                return text;

            int padding =
                (ReceiptWidth - text.Length) / 2;

            return new string(' ', padding) + text;
        }

        // ================= ALIGN COLUMNS =================

        private string AlignColumns(
            string left,
            string right)
        {
            left = left ?? "";
            right = right ?? "";

            int available =
                ReceiptWidth - right.Length - 1;

            if (available < 1)
                return left + Environment.NewLine + right;

            if (left.Length > available)
            {
                left = left.Substring(
                    0, available);
            }

            int spaces =
                ReceiptWidth -
                left.Length -
                right.Length;

            return left +
                new string(' ', Math.Max(1, spaces)) +
                right;
        }

        // ================= WRAP LONG TEXT =================

        private void AppendWrapped(
            StringBuilder builder,
            string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                builder.AppendLine();
                return;
            }

            text = text.Replace("\r", " ")
                       .Replace("\n", " ")
                       .Trim();

            while (text.Length > ReceiptWidth)
            {
                int split =
                    text.LastIndexOf(
                        ' ',
                        ReceiptWidth - 1);

                if (split <= 0)
                    split = ReceiptWidth;

                builder.AppendLine(
                    text.Substring(0, split));

                text = text.Substring(split).Trim();
            }

            builder.AppendLine(text);
        }

        // ================= BEGIN PRINTING =================

        private void printDocument_BeginPrint(
            object sender,
            PrintEventArgs e)
        {
            printLineIndex = 0;
        }

        // ================= PRINT PAGE =================

        private void printDocument_PrintPage(
            object sender,
            PrintPageEventArgs e)
        {
            string[] lines = receiptText
                .Replace("\r\n", "\n")
                .Split('\n');

            float x = e.MarginBounds.Left;

            float y = e.MarginBounds.Top;

            float bottom = e.MarginBounds.Bottom;

            float fontSize = 10F;

            // Adjust font to fit the printable width.

            using (Font testFont =
                new Font("Consolas", fontSize))
            {
                float textWidth =
                    e.Graphics.MeasureString(
                        new string('0', ReceiptWidth),
                        testFont).Width;

                if (textWidth > e.MarginBounds.Width)
                {
                    fontSize *=
                        e.MarginBounds.Width / textWidth;
                }
            }

            fontSize = Math.Max(1F, fontSize);

            using (Font font =
                new Font("Consolas", fontSize))
            {
                float lineHeight =
                    font.GetHeight(e.Graphics) + 2;

                if (lineHeight > e.MarginBounds.Height)
                {
                    e.Cancel = true;
                    e.HasMorePages = false;
                    return;
                }

                while (printLineIndex < lines.Length)
                {
                    if (y + lineHeight > bottom)
                    {
                        e.HasMorePages = true;
                        return;
                    }

                    e.Graphics.DrawString(
                        lines[printLineIndex],
                        font,
                        Brushes.Black,
                        x,
                        y);

                    y += lineHeight;

                    printLineIndex++;
                }
            }

            e.HasMorePages = false;
        }

        // ================= PRINT PREVIEW =================

        private void btnPreview_Click(
            object sender,
            EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(receiptText))
                return;

            try
            {
                if (!printDocument.PrinterSettings.IsValid)
                {
                    MessageBox.Show(
                        "No valid printer is configured.");

                    return;
                }

                using (PrintPreviewDialog preview =
                    new PrintPreviewDialog())
                {
                    preview.Document = printDocument;

                    preview.Width = 900;
                    preview.Height = 700;

                    preview.StartPosition =
                        FormStartPosition.CenterParent;

                    preview.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        // ================= PRINT RECEIPT =================

        private void btnPrint_Click(
            object sender,
            EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(receiptText))
                return;

            try
            {
                using (PrintDialog dialog =
                    new PrintDialog())
                {
                    dialog.Document = printDocument;

                    dialog.UseEXDialog = true;

                    dialog.AllowSomePages = false;
                    dialog.AllowSelection = false;

                    if (dialog.ShowDialog(this) ==
                        DialogResult.OK)
                    {
                        printDocument.Print();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        // ================= COPY RECEIPT =================

        private void btnCopy_Click(
            object sender,
            EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(receiptText))
                return;

            try
            {
                Clipboard.SetText(receiptText);

                MessageBox.Show(
                    "Receipt copied to clipboard.",
                    "Receipt",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        // ================= CLOSE =================

        private void btnClose_Click(
            object sender,
            EventArgs e)
        {
            Close();
        }

        // ================= ERROR HANDLING =================

        private void ShowError(Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Receipt Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
