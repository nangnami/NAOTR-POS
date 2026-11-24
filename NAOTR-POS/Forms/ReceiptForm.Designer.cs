
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace NAOTR_POS.Forms
{
    partial class ReceiptForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Panel pnlContent;

        private FlowLayoutPanel pnlFooter;

        private Label lblTitle;
        private Label lblSubtitle;

        private RichTextBox txtReceipt;

        private Button btnPreview;
        private Button btnPrint;
        private Button btnCopy;
        private Button btnClose;

        private PrintDocument printDocument;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components =
                new System.ComponentModel.Container();

            pnlHeader = new Panel();
            pnlContent = new Panel();
            pnlFooter = new FlowLayoutPanel();

            lblTitle = new Label();
            lblSubtitle = new Label();

            txtReceipt = new RichTextBox();

            btnPreview = new Button();
            btnPrint = new Button();
            btnCopy = new Button();
            btnClose = new Button();

            printDocument = new PrintDocument();

            components.Add(printDocument);

            SuspendLayout();

            // ================= HEADER =================

            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 85;

            pnlHeader.BackColor =
                Color.FromArgb(16, 36, 62);

            lblTitle.Text = "Transaction Receipt";

            lblTitle.Font = new Font(
                "Segoe UI", 22F, FontStyle.Bold);

            lblTitle.ForeColor = Color.White;

            lblTitle.Location = new Point(20, 8);

            lblTitle.AutoSize = true;

            lblSubtitle.Text =
                "View and print the completed transaction.";

            lblSubtitle.Font =
                new Font("Segoe UI", 10F);

            lblSubtitle.ForeColor =
                Color.LightSteelBlue;

            lblSubtitle.Location =
                new Point(23, 52);

            lblSubtitle.AutoSize = true;

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubtitle);

            // ================= CONTENT =================

            pnlContent.Dock = DockStyle.Fill;

            pnlContent.Padding = new Padding(25);

            pnlContent.BackColor =
                Color.FromArgb(241, 245, 249);

            // ================= RECEIPT TEXT =================

            txtReceipt.Dock = DockStyle.Fill;

            txtReceipt.BackColor = Color.White;

            txtReceipt.ForeColor =
                Color.FromArgb(16, 36, 62);

            txtReceipt.Font =
                new Font("Consolas", 11F);

            txtReceipt.BorderStyle =
                BorderStyle.FixedSingle;

            txtReceipt.ReadOnly = true;

            txtReceipt.WordWrap = false;

            txtReceipt.DetectUrls = false;

            txtReceipt.ScrollBars =
                RichTextBoxScrollBars.Both;

            pnlContent.Controls.Add(txtReceipt);

            // ================= FOOTER =================

            pnlFooter.Dock = DockStyle.Bottom;

            pnlFooter.Height = 65;

            pnlFooter.Padding = new Padding(8);

            pnlFooter.BackColor = Color.White;

            pnlFooter.WrapContents = false;

            pnlFooter.FlowDirection =
                FlowDirection.LeftToRight;

            ConfigureButton(
                btnPreview,
                "Print Preview",
                Color.FromArgb(71, 85, 105));

            ConfigureButton(
                btnPrint,
                "Print Receipt",
                Color.FromArgb(37, 99, 235));

            ConfigureButton(
                btnCopy,
                "Copy Receipt",
                Color.FromArgb(71, 85, 105));

            ConfigureButton(
                btnClose,
                "Close",
                Color.FromArgb(100, 116, 139));

            btnPreview.Click += btnPreview_Click;
            btnPrint.Click += btnPrint_Click;
            btnCopy.Click += btnCopy_Click;
            btnClose.Click += btnClose_Click;

            pnlFooter.Controls.Add(btnPreview);
            pnlFooter.Controls.Add(btnPrint);
            pnlFooter.Controls.Add(btnCopy);
            pnlFooter.Controls.Add(btnClose);

            // ================= PRINT DOCUMENT =================

            printDocument.DocumentName =
                "NAOTR-POS Sales Receipt";

            printDocument.DefaultPageSettings.Margins =
                new Margins(30, 30, 30, 30);

            printDocument.BeginPrint +=
                printDocument_BeginPrint;

            printDocument.PrintPage +=
                printDocument_PrintPage;

            // ================= FORM SETTINGS =================

            Controls.Add(pnlContent);
            Controls.Add(pnlFooter);
            Controls.Add(pnlHeader);

            AutoScaleMode = AutoScaleMode.None;

            ClientSize = new Size(680, 720);

            MinimumSize = new Size(650, 500);

            BackColor = Color.White;

            StartPosition =
                FormStartPosition.CenterParent;

            FormBorderStyle =
                FormBorderStyle.Sizable;

            MinimizeBox = false;

            CancelButton = btnClose;

            Name = "ReceiptForm";

            Text = "Sales Receipt";

            ResumeLayout(false);
        }

        private void ConfigureButton(
            Button button,
            string text,
            Color color)
        {
            button.Text = text;

            button.Size = new Size(140, 42);

            button.Margin = new Padding(5);

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
