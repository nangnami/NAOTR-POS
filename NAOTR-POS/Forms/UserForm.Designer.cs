
using System.Drawing;
using System.Windows.Forms;

namespace NAOTR_POS.Forms
{
    partial class UserForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Panel pnlFooter;

        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblUserId;
        private Label lblUsername;
        private Label lblFullName;
        private Label lblRole;
        private Label lblPassword;
        private Label lblConfirmPassword;

        private TextBox txtUserId;
        private TextBox txtUsername;
        private TextBox txtFullName;
        private TextBox txtPassword;
        private TextBox txtConfirmPassword;

        private ComboBox cmbRole;
        private CheckBox chkShowPassword;

        private Button btnSave;
        private Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new Panel();
            this.pnlFooter = new Panel();

            this.lblTitle = new Label();
            this.lblSubtitle = new Label();
            this.lblUserId = new Label();
            this.lblUsername = new Label();
            this.lblFullName = new Label();
            this.lblRole = new Label();
            this.lblPassword = new Label();
            this.lblConfirmPassword = new Label();

            this.txtUserId = new TextBox();
            this.txtUsername = new TextBox();
            this.txtFullName = new TextBox();
            this.txtPassword = new TextBox();
            this.txtConfirmPassword = new TextBox();

            this.cmbRole = new ComboBox();
            this.chkShowPassword = new CheckBox();

            this.btnSave = new Button();
            this.btnCancel = new Button();

            this.pnlHeader.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();

            // ================= HEADER =================

            this.pnlHeader.BackColor =
                Color.FromArgb(16, 36, 62);

            this.pnlHeader.Dock = DockStyle.Top;
            this.pnlHeader.Location = new Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new Size(500, 85);

            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblSubtitle);

            // lblTitle

            this.lblTitle.AutoSize = true;

            this.lblTitle.Font = new Font(
                "Segoe UI", 22F, FontStyle.Bold);

            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Location = new Point(20, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Text = "Add User";

            // lblSubtitle

            this.lblSubtitle.AutoSize = true;

            this.lblSubtitle.Font =
                new Font("Segoe UI", 10F);

            this.lblSubtitle.ForeColor =
                Color.LightSteelBlue;

            this.lblSubtitle.Location = new Point(23, 53);
            this.lblSubtitle.Name = "lblSubtitle";

            this.lblSubtitle.Text =
                "Create a new system user account.";

            // ================= USER ID =================

            this.lblUserId.AutoSize = true;

            this.lblUserId.Font = new Font(
                "Segoe UI", 10F, FontStyle.Bold);

            this.lblUserId.ForeColor =
                Color.FromArgb(51, 65, 85);

            this.lblUserId.Location = new Point(25, 110);
            this.lblUserId.Name = "lblUserId";
            this.lblUserId.Text = "User ID";

            // txtUserId

            this.txtUserId.BackColor =
                Color.FromArgb(226, 232, 240);

            this.txtUserId.BorderStyle =
                BorderStyle.FixedSingle;

            this.txtUserId.Font =
                new Font("Segoe UI", 11F);

            this.txtUserId.Location = new Point(25, 135);
            this.txtUserId.Name = "txtUserId";
            this.txtUserId.ReadOnly = true;
            this.txtUserId.Size = new Size(450, 32);
            this.txtUserId.TabStop = false;

            this.txtUserId.Text =
                "Automatically generated";

            // ================= USERNAME =================

            this.lblUsername.AutoSize = true;

            this.lblUsername.Font = new Font(
                "Segoe UI", 10F, FontStyle.Bold);

            this.lblUsername.ForeColor =
                Color.FromArgb(51, 65, 85);

            this.lblUsername.Location = new Point(25, 180);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Text = "Username *";

            // txtUsername

            this.txtUsername.BorderStyle =
                BorderStyle.FixedSingle;

            this.txtUsername.Font =
                new Font("Segoe UI", 11F);

            this.txtUsername.Location = new Point(25, 205);
            this.txtUsername.MaxLength = 50;
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new Size(450, 32);
            this.txtUsername.TabIndex = 0;

            // ================= FULL NAME =================

            this.lblFullName.AutoSize = true;

            this.lblFullName.Font = new Font(
                "Segoe UI", 10F, FontStyle.Bold);

            this.lblFullName.ForeColor =
                Color.FromArgb(51, 65, 85);

            this.lblFullName.Location = new Point(25, 250);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Text = "Full Name *";

            // txtFullName

            this.txtFullName.BorderStyle =
                BorderStyle.FixedSingle;

            this.txtFullName.Font =
                new Font("Segoe UI", 11F);

            this.txtFullName.Location = new Point(25, 275);
            this.txtFullName.MaxLength = 100;
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new Size(450, 32);
            this.txtFullName.TabIndex = 1;

            // ================= ROLE =================

            this.lblRole.AutoSize = true;

            this.lblRole.Font = new Font(
                "Segoe UI", 10F, FontStyle.Bold);

            this.lblRole.ForeColor =
                Color.FromArgb(51, 65, 85);

            this.lblRole.Location = new Point(25, 320);
            this.lblRole.Name = "lblRole";
            this.lblRole.Text = "Account Role *";

            // cmbRole

            this.cmbRole.DropDownStyle =
                ComboBoxStyle.DropDownList;

            this.cmbRole.Font =
                new Font("Segoe UI", 11F);

            this.cmbRole.FormattingEnabled = true;
            this.cmbRole.Location = new Point(25, 345);
            this.cmbRole.Name = "cmbRole";
            this.cmbRole.Size = new Size(450, 32);
            this.cmbRole.TabIndex = 2;

            // ================= PASSWORD =================

            this.lblPassword.AutoSize = true;

            this.lblPassword.Font = new Font(
                "Segoe UI", 10F, FontStyle.Bold);

            this.lblPassword.ForeColor =
                Color.FromArgb(51, 65, 85);

            this.lblPassword.Location = new Point(25, 390);
            this.lblPassword.Name = "lblPassword";

            this.lblPassword.Text =
                "Password * (Minimum 12 characters)";

            // txtPassword

            this.txtPassword.BorderStyle =
                BorderStyle.FixedSingle;

            this.txtPassword.Font =
                new Font("Segoe UI", 11F);

            this.txtPassword.Location = new Point(25, 415);
            this.txtPassword.MaxLength = 128;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new Size(450, 32);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.UseSystemPasswordChar = true;

            // ================= CONFIRM PASSWORD =================

            this.lblConfirmPassword.AutoSize = true;

            this.lblConfirmPassword.Font = new Font(
                "Segoe UI", 10F, FontStyle.Bold);

            this.lblConfirmPassword.ForeColor =
                Color.FromArgb(51, 65, 85);

            this.lblConfirmPassword.Location =
                new Point(25, 460);

            this.lblConfirmPassword.Name =
                "lblConfirmPassword";

            this.lblConfirmPassword.Text =
                "Confirm Password *";

            // txtConfirmPassword

            this.txtConfirmPassword.BorderStyle =
                BorderStyle.FixedSingle;

            this.txtConfirmPassword.Font =
                new Font("Segoe UI", 11F);

            this.txtConfirmPassword.Location =
                new Point(25, 485);

            this.txtConfirmPassword.MaxLength = 128;

            this.txtConfirmPassword.Name =
                "txtConfirmPassword";

            this.txtConfirmPassword.Size =
                new Size(450, 32);

            this.txtConfirmPassword.TabIndex = 4;

            this.txtConfirmPassword.UseSystemPasswordChar =
                true;

            // ================= SHOW PASSWORD =================

            this.chkShowPassword.AutoSize = true;

            this.chkShowPassword.Font =
                new Font("Segoe UI", 10F);

            this.chkShowPassword.Location =
                new Point(25, 530);

            this.chkShowPassword.Name = "chkShowPassword";
            this.chkShowPassword.TabIndex = 5;
            this.chkShowPassword.Text = "Show passwords";
            this.chkShowPassword.UseVisualStyleBackColor = true;

            this.chkShowPassword.CheckedChanged +=
                new System.EventHandler(
                    this.chkShowPassword_CheckedChanged);

            // ================= FOOTER =================

            this.pnlFooter.BackColor =
                Color.FromArgb(241, 245, 249);

            this.pnlFooter.Dock = DockStyle.Bottom;
            this.pnlFooter.Location = new Point(0, 575);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new Size(500, 75);

            this.pnlFooter.Controls.Add(this.btnSave);
            this.pnlFooter.Controls.Add(this.btnCancel);

            // ================= SAVE BUTTON =================

            this.btnSave.BackColor =
                Color.FromArgb(37, 99, 235);

            this.btnSave.Cursor = Cursors.Hand;

            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = FlatStyle.Flat;

            this.btnSave.Font = new Font(
                "Segoe UI", 10F, FontStyle.Bold);

            this.btnSave.ForeColor = Color.White;
            this.btnSave.Location = new Point(220, 17);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(125, 42);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save User";
            this.btnSave.UseVisualStyleBackColor = false;

            this.btnSave.Click +=
                new System.EventHandler(this.btnSave_Click);

            // ================= CANCEL BUTTON =================

            this.btnCancel.BackColor =
                Color.FromArgb(100, 116, 139);

            this.btnCancel.Cursor = Cursors.Hand;
            this.btnCancel.DialogResult = DialogResult.Cancel;

            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = FlatStyle.Flat;

            this.btnCancel.Font = new Font(
                "Segoe UI", 10F, FontStyle.Bold);

            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.Location = new Point(350, 17);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(125, 42);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;

            this.btnCancel.Click +=
                new System.EventHandler(this.btnCancel_Click);

            // ================= USER FORM =================

            this.AcceptButton = this.btnSave;
            this.CancelButton = this.btnCancel;

            this.AutoScaleMode = AutoScaleMode.None;

            this.BackColor = Color.White;
            this.ClientSize = new Size(500, 650);

            this.Controls.Add(this.lblUserId);
            this.Controls.Add(this.txtUserId);

            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtUsername);

            this.Controls.Add(this.lblFullName);
            this.Controls.Add(this.txtFullName);

            this.Controls.Add(this.lblRole);
            this.Controls.Add(this.cmbRole);

            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);

            this.Controls.Add(this.lblConfirmPassword);
            this.Controls.Add(this.txtConfirmPassword);

            this.Controls.Add(this.chkShowPassword);

            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);

            this.FormBorderStyle =
                FormBorderStyle.FixedDialog;

            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.Name = "UserForm";
            this.ShowInTaskbar = false;

            this.StartPosition =
                FormStartPosition.CenterParent;

            this.Text = "User Account";

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();

            this.pnlFooter.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
