
using System.Drawing;
using System.Windows.Forms;

namespace NAOTR_POS.Forms
{
    partial class CustomerForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Panel pnlFooter;

        private Label lblTitle;
        private Label lblSubtitle;

        private TextBox txtCustomerId;
        private TextBox txtFullName;
        private TextBox txtPhone;
        private TextBox txtEmail;
        private TextBox txtAddress;

        private Button btnSave;
        private Button btnCancel;

        private ErrorProvider errorProvider;

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
            pnlFooter = new Panel();

            lblTitle = new Label();
            lblSubtitle = new Label();

            txtCustomerId = new TextBox();
            txtFullName = new TextBox();
            txtPhone = new TextBox();
            txtEmail = new TextBox();
            txtAddress = new TextBox();

            btnSave = new Button();
            btnCancel = new Button();

            errorProvider =
                new ErrorProvider(components);

            SuspendLayout();

            // HEADER

            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 85;

            pnlHeader.BackColor =
                Color.FromArgb(16, 36, 62);

            lblTitle.Text = "Add Customer";

            lblTitle.Font = new Font(
                "Segoe UI", 22F, FontStyle.Bold);

            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(25, 10);
            lblTitle.AutoSize = true;

            lblSubtitle.Text =
                "Register a new customer.";

            lblSubtitle.Font =
                new Font("Segoe UI", 10F);

            lblSubtitle.ForeColor =
                Color.LightSteelBlue;

            lblSubtitle.Location =
                new Point(28, 53);

            lblSubtitle.AutoSize = true;

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubtitle);

            // CUSTOMER ID

            Controls.Add(CreateLabel(
                "Customer ID", 25, 105));

            ConfigureTextBox(
                txtCustomerId, 25, 130, 480, 30);

            txtCustomerId.ReadOnly = true;
            txtCustomerId.TabStop = false;

            txtCustomerId.BackColor =
                Color.FromArgb(241, 245, 249);

            txtCustomerId.Text =
                "Automatically generated";

            // FULL NAME

            Controls.Add(CreateLabel(
                "Full Name *", 25, 180));

            ConfigureTextBox(
                txtFullName, 25, 205, 480, 100);

            // PHONE

            Controls.Add(CreateLabel(
                "Phone Number", 25, 255));

            ConfigureTextBox(
                txtPhone, 25, 280, 480, 30);

            // EMAIL

            Controls.Add(CreateLabel(
                "Email Address", 25, 330));

            ConfigureTextBox(
                txtEmail, 25, 355, 480, 120);

            // ADDRESS

            Controls.Add(CreateLabel(
                "Address", 25, 405));

            ConfigureTextBox(
                txtAddress, 25, 430, 480, 250);

            txtAddress.Multiline = true;
            txtAddress.Height = 90;

            txtAddress.ScrollBars =
                ScrollBars.Vertical;

            // FOOTER

            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Height = 75;

            pnlFooter.BackColor =
                Color.FromArgb(241, 245, 249);

            ConfigureButton(
                btnCancel,
                "Cancel",
                Color.FromArgb(100, 116, 139));

            btnCancel.Location =
                new Point(220, 17);

            btnCancel.Size = new Size(120, 40);

            btnCancel.DialogResult =
                DialogResult.Cancel;

            ConfigureButton(
                btnSave,
                "Save Customer",
                Color.FromArgb(37, 99, 235));

            btnSave.Location =
                new Point(350, 17);

            btnSave.Size = new Size(155, 40);

            btnSave.Click += btnSave_Click;

            pnlFooter.Controls.Add(btnCancel);
            pnlFooter.Controls.Add(btnSave);

            // ERROR PROVIDER

            errorProvider.ContainerControl = this;

            errorProvider.BlinkStyle =
                ErrorBlinkStyle.NeverBlink;

            // ADD CONTROLS

            Controls.Add(txtCustomerId);
            Controls.Add(txtFullName);
            Controls.Add(txtPhone);
            Controls.Add(txtEmail);
            Controls.Add(txtAddress);

            Controls.Add(pnlHeader);
            Controls.Add(pnlFooter);

            // FORM SETTINGS

            AutoScaleMode = AutoScaleMode.None;

            ClientSize = new Size(550, 625);

            BackColor = Color.White;

            Font = new Font("Segoe UI", 10F);

            FormBorderStyle =
                FormBorderStyle.FixedDialog;

            StartPosition =
                FormStartPosition.CenterParent;

            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;

            AcceptButton = btnSave;
            CancelButton = btnCancel;

            Name = "CustomerForm";
            Text = "Customer";

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

                ForeColor =
                    Color.FromArgb(51, 65, 85)
            };
        }

        private void ConfigureTextBox(
            TextBox textBox,
            int x,
            int y,
            int width,
            int maxLength)
        {
            textBox.Location = new Point(x, y);

            textBox.Size = new Size(width, 30);

            textBox.Font =
                new Font("Segoe UI", 11F);

            textBox.MaxLength = maxLength;
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
