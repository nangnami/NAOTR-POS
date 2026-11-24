
namespace NAOTR_POS.Forms
{
    partial class ResetPasswordForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlFooter;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblUserId;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblNewPassword;
        private System.Windows.Forms.Label lblConfirmPassword;
        private System.Windows.Forms.Label lblPasswordHint;

        private System.Windows.Forms.TextBox txtUserId;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtNewPassword;
        private System.Windows.Forms.TextBox txtConfirmPassword;

        private System.Windows.Forms.CheckBox chkShowPassword;

        private System.Windows.Forms.Button btnResetPassword;
        private System.Windows.Forms.Button btnCancel;

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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pnlFooter = new System.Windows.Forms.Panel();

            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblUserId = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblNewPassword = new System.Windows.Forms.Label();
            this.lblConfirmPassword = new System.Windows.Forms.Label();
            this.lblPasswordHint = new System.Windows.Forms.Label();

            this.txtUserId = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtNewPassword = new System.Windows.Forms.TextBox();
            this.txtConfirmPassword = new System.Windows.Forms.TextBox();

            this.chkShowPassword = new System.Windows.Forms.CheckBox();

            this.btnResetPassword = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            this.pnlHeader.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();

            // pnlHeader
            this.pnlHeader.BackColor =
                System.Drawing.Color.FromArgb(16, 36, 62);

            this.pnlHeader.Dock =
                System.Windows.Forms.DockStyle.Top;

            this.pnlHeader.Height = 95;

            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblSubtitle);

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font(
                "Segoe UI", 19F,
                System.Drawing.FontStyle.Bold);

            this.lblTitle.ForeColor =
                System.Drawing.Color.White;

            this.lblTitle.Location =
                new System.Drawing.Point(25, 17);

            this.lblTitle.Text = "Reset Password";

            // lblSubtitle
            this.lblSubtitle.AutoSize = true;

            this.lblSubtitle.Font =
                new System.Drawing.Font("Segoe UI", 10F);

            this.lblSubtitle.ForeColor =
                System.Drawing.Color.LightSteelBlue;

            this.lblSubtitle.Location =
                new System.Drawing.Point(28, 55);

            this.lblSubtitle.Text =
                "Set a new password for the selected account.";

            // lblUserId
            this.lblUserId.AutoSize = true;
            this.lblUserId.Location =
                new System.Drawing.Point(25, 115);

            this.lblUserId.Font = new System.Drawing.Font(
                "Segoe UI", 10F,
                System.Drawing.FontStyle.Bold);

            this.lblUserId.Text = "User ID";

            // txtUserId
            this.txtUserId.Location =
                new System.Drawing.Point(28, 143);

            this.txtUserId.Size =
                new System.Drawing.Size(444, 30);

            this.txtUserId.ReadOnly = true;
            this.txtUserId.TabStop = false;

            this.txtUserId.BackColor =
                System.Drawing.Color.FromArgb(226, 232, 240);

            // lblUsername
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location =
                new System.Drawing.Point(25, 190);

            this.lblUsername.Font = new System.Drawing.Font(
                "Segoe UI", 10F,
                System.Drawing.FontStyle.Bold);

            this.lblUsername.Text = "Username";

            // txtUsername
            this.txtUsername.Location =
                new System.Drawing.Point(28, 218);

            this.txtUsername.Size =
                new System.Drawing.Size(444, 30);

            this.txtUsername.ReadOnly = true;
            this.txtUsername.TabStop = false;

            this.txtUsername.BackColor =
                System.Drawing.Color.FromArgb(226, 232, 240);

            // lblNewPassword
            this.lblNewPassword.AutoSize = true;

            this.lblNewPassword.Location =
                new System.Drawing.Point(25, 265);

            this.lblNewPassword.Font =
                new System.Drawing.Font(
                    "Segoe UI", 10F,
                    System.Drawing.FontStyle.Bold);

            this.lblNewPassword.Text = "New Password *";

            // txtNewPassword
            this.txtNewPassword.Location =
                new System.Drawing.Point(28, 293);

            this.txtNewPassword.Size =
                new System.Drawing.Size(444, 30);

            this.txtNewPassword.UseSystemPasswordChar = true;
            this.txtNewPassword.MaxLength = 128;
            this.txtNewPassword.TabIndex = 0;

            // lblPasswordHint
            this.lblPasswordHint.AutoSize = true;

            this.lblPasswordHint.Location =
                new System.Drawing.Point(25, 328);

            this.lblPasswordHint.Font =
                new System.Drawing.Font("Segoe UI", 9F);

            this.lblPasswordHint.ForeColor =
                System.Drawing.Color.DimGray;

            this.lblPasswordHint.Text =
                "Minimum of 12 characters.";

            // lblConfirmPassword
            this.lblConfirmPassword.AutoSize = true;

            this.lblConfirmPassword.Location =
                new System.Drawing.Point(25, 360);

            this.lblConfirmPassword.Font =
                new System.Drawing.Font(
                    "Segoe UI", 10F,
                    System.Drawing.FontStyle.Bold);

            this.lblConfirmPassword.Text =
                "Confirm Password *";

            // txtConfirmPassword
            this.txtConfirmPassword.Location =
                new System.Drawing.Point(28, 388);

            this.txtConfirmPassword.Size =
                new System.Drawing.Size(444, 30);

            this.txtConfirmPassword.UseSystemPasswordChar = true;
            this.txtConfirmPassword.MaxLength = 128;
            this.txtConfirmPassword.TabIndex = 1;

            // chkShowPassword
            this.chkShowPassword.AutoSize = true;

            this.chkShowPassword.Location =
                new System.Drawing.Point(28, 435);

            this.chkShowPassword.Text = "Show passwords";
            this.chkShowPassword.TabIndex = 2;

            this.chkShowPassword.CheckedChanged +=
                new System.EventHandler(
                    this.chkShowPassword_CheckedChanged);

            // pnlFooter
            this.pnlFooter.BackColor =
                System.Drawing.Color.FromArgb(241, 245, 249);

            this.pnlFooter.Dock =
                System.Windows.Forms.DockStyle.Bottom;

            this.pnlFooter.Height = 80;

            this.pnlFooter.Controls.Add(this.btnCancel);
            this.pnlFooter.Controls.Add(this.btnResetPassword);

            // btnCancel
            this.btnCancel.Location =
                new System.Drawing.Point(190, 20);

            this.btnCancel.Size =
                new System.Drawing.Size(100, 40);

            this.btnCancel.BackColor =
                System.Drawing.Color.FromArgb(226, 232, 240);

            this.btnCancel.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TabIndex = 4;

            this.btnCancel.DialogResult =
                System.Windows.Forms.DialogResult.Cancel;

            this.btnCancel.Click +=
                new System.EventHandler(this.btnCancel_Click);

            // btnResetPassword
            this.btnResetPassword.Location =
                new System.Drawing.Point(302, 20);

            this.btnResetPassword.Size =
                new System.Drawing.Size(170, 40);

            this.btnResetPassword.BackColor =
                System.Drawing.Color.FromArgb(37, 99, 235);

            this.btnResetPassword.ForeColor =
                System.Drawing.Color.White;

            this.btnResetPassword.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.btnResetPassword.FlatAppearance.BorderSize = 0;
            this.btnResetPassword.Text = "Reset Password";
            this.btnResetPassword.TabIndex = 3;

            this.btnResetPassword.Click +=
                new System.EventHandler(
                    this.btnResetPassword_Click);

            // ResetPasswordForm
            this.AutoScaleDimensions =
                new System.Drawing.SizeF(7F, 15F);

            this.AutoScaleMode =
                System.Windows.Forms.AutoScaleMode.Font;

            this.BackColor = System.Drawing.Color.White;

            this.ClientSize =
                new System.Drawing.Size(500, 560);

            this.Font =
                new System.Drawing.Font("Segoe UI", 10F);

            this.FormBorderStyle =
                System.Windows.Forms.FormBorderStyle.FixedDialog;

            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;

            this.StartPosition =
                System.Windows.Forms.FormStartPosition.CenterParent;

            this.AcceptButton = this.btnResetPassword;
            this.CancelButton = this.btnCancel;

            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.lblUserId);
            this.Controls.Add(this.txtUserId);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblNewPassword);
            this.Controls.Add(this.txtNewPassword);
            this.Controls.Add(this.lblPasswordHint);
            this.Controls.Add(this.lblConfirmPassword);
            this.Controls.Add(this.txtConfirmPassword);
            this.Controls.Add(this.chkShowPassword);
            this.Controls.Add(this.pnlFooter);

            this.Name = "ResetPasswordForm";
            this.Text = "Reset Password";

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();

            this.pnlFooter.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
