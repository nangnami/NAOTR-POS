
using System;
using System.Windows.Forms;
using NAOTR_POS.Helpers;
using NAOTR_POS.Models;
using NAOTR_POS.Services;

namespace NAOTR_POS.Forms
{
    public partial class ResetPasswordForm : Form
    {
        private readonly UserService userService =
            new UserService();

        private readonly int userId;

        private UserAccount selectedUser;

        public ResetPasswordForm() : this(0)
        {
        }

        public ResetPasswordForm(int userId)
        {
            InitializeComponent();

            this.userId = userId;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Session.IsInRole("Admin"))
            {
                MessageBox.Show(
                    "Only administrators can reset passwords.",
                    "Access Denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                Close();
                return;
            }

            LoadUser();
        }

        private void LoadUser()
        {
            try
            {
                if (userId <= 0)
                {
                    throw new Exception(
                        "No user account was selected.");
                }

                selectedUser =
                    userService.GetUserById(userId);

                if (selectedUser == null)
                {
                    throw new Exception(
                        "User account was not found.");
                }

                txtUserId.Text =
                    selectedUser.UserId.ToString();

                txtUsername.Text =
                    selectedUser.Username;

                txtNewPassword.Clear();
                txtConfirmPassword.Clear();

                txtNewPassword.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "User Account Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                Close();
            }
        }

        private bool ValidatePassword()
        {
            string password = txtNewPassword.Text;
            string confirmation = txtConfirmPassword.Text;

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(
                    "Please enter a new password.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtNewPassword.Focus();
                return false;
            }

            if (password.Length < 12 ||
                password.Length > 128)
            {
                MessageBox.Show(
                    "Password must contain 12 to 128 characters.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtNewPassword.Focus();
                return false;
            }

            if (password != confirmation)
            {
                MessageBox.Show(
                    "Passwords do not match.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtConfirmPassword.Focus();
                return false;
            }

            return true;
        }

        private void btnResetPassword_Click(
            object sender, EventArgs e)
        {
            if (selectedUser == null)
                return;

            if (!ValidatePassword())
                return;

            DialogResult confirmation = MessageBox.Show(
                "Are you sure you want to reset the password for "
                + selectedUser.Username + "?\n\n"
                + "The existing password will no longer work "
                + "for future logins.",
                "Confirm Password Reset",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (confirmation != DialogResult.Yes)
                return;

            try
            {
                btnResetPassword.Enabled = false;
                Cursor = Cursors.WaitCursor;

                userService.ResetPassword(
                    userId,
                    txtNewPassword.Text);

                txtNewPassword.Clear();
                txtConfirmPassword.Clear();

                MessageBox.Show(
                    "Password reset successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Password Reset Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                btnResetPassword.Enabled = true;
            }
        }

        private void chkShowPassword_CheckedChanged(
            object sender, EventArgs e)
        {
            txtNewPassword.UseSystemPasswordChar =
                !chkShowPassword.Checked;

            txtConfirmPassword.UseSystemPasswordChar =
                !chkShowPassword.Checked;
        }

        private void btnCancel_Click(
            object sender, EventArgs e)
        {
            txtNewPassword.Clear();
            txtConfirmPassword.Clear();

            DialogResult = DialogResult.Cancel;
            Close();
        }

        protected override void OnFormClosed(
            FormClosedEventArgs e)
        {
            txtNewPassword.Clear();
            txtConfirmPassword.Clear();

            base.OnFormClosed(e);
        }
    }
}
