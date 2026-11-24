
using System;
using System.Drawing;
using System.Windows.Forms;
using NAOTR_POS.Data;
using NAOTR_POS.Helpers;
using NAOTR_POS.Models;
using NAOTR_POS.Services;

namespace NAOTR_POS.Forms
{
    public partial class LoginForm : Form
    {
        private readonly AuthService authService =
            new AuthService();

        public LoginForm()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            CheckDatabaseConnection();
            txtUsername.Focus();
        }

        private void CheckDatabaseConnection()
        {
            try
            {
                DatabaseHelper.TestConnection();

                lblConnectionStatus.Text =
                    "● Database connected";

                lblConnectionStatus.ForeColor =
                    Color.SeaGreen;

                btnLogin.Enabled = true;
            }
            catch (Exception)
            {
                lblConnectionStatus.Text =
                    "● Database disconnected";

                lblConnectionStatus.ForeColor =
                    Color.Firebrick;

                btnLogin.Enabled = false;
            }
        }

        private void chkShowPassword_CheckedChanged(
            object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar =
                !chkShowPassword.Checked;
        }

        private void btnLogin_Click(
            object sender, EventArgs e)
        {
            lblError.Text = "";

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username))
            {
                lblError.Text =
                    "Please enter your username.";

                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                lblError.Text =
                    "Please enter your password.";

                txtPassword.Focus();
                return;
            }

            try
            {
                btnLogin.Enabled = false;
                btnLogin.Text = "Signing in...";

                User user = authService.Login(
                    username, password);

                if (user == null)
                {
                    lblError.Text =
                        "Invalid credentials or inactive account.";

                    txtPassword.Clear();
                    txtPassword.Focus();
                    return;
                }

                Session.Login(user);

                txtPassword.Clear();

                this.Hide();

                try
                {
                    using (MainForm mainForm = new MainForm())
                    {
                        mainForm.ShowDialog();
                    }
                }
                finally
                {
                    Session.Logout();

                    this.Show();

                    txtUsername.Clear();
                    txtPassword.Clear();
                    chkShowPassword.Checked = false;
                    txtUsername.Focus();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);

                lblError.Text =
                    "Unable to sign in. Please try again.";
            }
            finally
            {
                btnLogin.Enabled = true;
                btnLogin.Text = "Sign in";
            }
        }
    }
}
