
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

using NAOTR_POS.Helpers;
using NAOTR_POS.Models;
using NAOTR_POS.Services;

namespace NAOTR_POS.Forms
{
    public partial class UserForm : Form
    {
        private readonly UserService userService =
            new UserService();

        private readonly int userId;

        private bool isSaving = false;

        public UserForm() : this(0)
        {
        }

        public UserForm(int userId)
        {
            InitializeComponent();
            this.userId = userId;
        }

        // ================= FORM LOAD =================

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Session.IsInRole("Admin"))
            {
                MessageBox.Show(
                    "Access denied.");

                Close();
                return;
            }

            try
            {
                LoadRoles();

                if (userId > 0)
                {
                    ConfigureEditMode();

                    LoadUser();
                }
                else
                {
                    ConfigureAddMode();
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);

                Close();
            }
        }

        // ================= LOAD ROLES =================

        private void LoadRoles()
        {
            DataTable roles =
                userService.GetRoles();

            cmbRole.DisplayMember = "RoleName";
            cmbRole.ValueMember = "RoleId";

            cmbRole.DataSource = roles;

            cmbRole.SelectedIndex = -1;
        }

        // ================= ADD MODE =================

        private void ConfigureAddMode()
        {
            lblTitle.Text = "Add User";

            lblSubtitle.Text =
                "Create a new system user account.";

            txtUserId.Text =
                "Automatically generated";

            btnSave.Text = "Save User";

            txtUsername.Focus();
        }

        // ================= EDIT MODE =================

        private void ConfigureEditMode()
        {
            lblTitle.Text = "Edit User";

            lblSubtitle.Text =
                "Update existing account information.";

            btnSave.Text = "Update User";

            lblPassword.Visible = false;
            txtPassword.Visible = false;

            lblConfirmPassword.Visible = false;
            txtConfirmPassword.Visible = false;

            chkShowPassword.Visible = false;

            ClientSize =
                new System.Drawing.Size(500, 480);
        }

        // ================= LOAD EXISTING USER =================

        private void LoadUser()
        {
            UserAccount user =
                userService.GetUserById(userId);

            if (user == null)
            {
                throw new InvalidOperationException(
                    "User account was not found.");
            }

            txtUserId.Text =
                user.UserId.ToString();

            txtUsername.Text = user.Username;

            txtFullName.Text = user.FullName;

            cmbRole.SelectedValue = user.RoleId;

            // Prevent changing own administrator role.

            if (userId == Session.CurrentUser.UserId)
            {
                cmbRole.Enabled = false;
            }
        }

        // ================= SHOW PASSWORD =================

        private void chkShowPassword_CheckedChanged(
            object sender,
            EventArgs e)
        {
            txtPassword.UseSystemPasswordChar =
                !chkShowPassword.Checked;

            txtConfirmPassword.UseSystemPasswordChar =
                !chkShowPassword.Checked;
        }

        // ================= INPUT VALIDATION =================

        private bool ValidateInput()
        {
            string username =
                txtUsername.Text.Trim();

            if (string.IsNullOrWhiteSpace(username))
            {
                ShowWarning("Username is required.");

                txtUsername.Focus();

                return false;
            }

            if (username.Length < 3)
            {
                ShowWarning(
                    "Username must contain at least 3 characters.");

                txtUsername.Focus();

                return false;
            }

            foreach (char character in username)
            {
                if (char.IsWhiteSpace(character))
                {
                    ShowWarning(
                        "Username cannot contain spaces.");

                    txtUsername.Focus();

                    return false;
                }
            }

            if (string.IsNullOrWhiteSpace(
                txtFullName.Text))
            {
                ShowWarning("Full name is required.");

                txtFullName.Focus();

                return false;
            }

            if (cmbRole.SelectedIndex < 0 ||
                !(cmbRole.SelectedValue is int))
            {
                ShowWarning(
                    "Please select an account role.");

                cmbRole.Focus();

                return false;
            }

            // Password required only for new users.

            if (userId == 0)
            {
                if (string.IsNullOrWhiteSpace(
                    txtPassword.Text) ||
                    txtPassword.Text.Length < 12)
                {
                    ShowWarning(
                        "Password must contain at least 12 characters.");

                    txtPassword.Focus();

                    return false;
                }

                if (txtPassword.Text !=
                    txtConfirmPassword.Text)
                {
                    ShowWarning(
                        "Passwords do not match.");

                    txtConfirmPassword.Focus();

                    return false;
                }
            }

            return true;
        }

        // ================= SAVE USER =================

        private void btnSave_Click(
            object sender,
            EventArgs e)
        {
            if (isSaving)
                return;

            if (!Session.IsInRole("Admin"))
                return;

            if (!ValidateInput())
                return;

            try
            {
                isSaving = true;

                btnSave.Enabled = false;

                Cursor = Cursors.WaitCursor;

                UserAccount user = new UserAccount
                {
                    UserId = userId,

                    Username =
                        txtUsername.Text.Trim(),

                    FullName =
                        txtFullName.Text.Trim(),

                    RoleId =
                        Convert.ToInt32(
                            cmbRole.SelectedValue)
                };

                if (userId == 0)
                {
                    int newUserId =
                        userService.InsertUser(
                            user,
                            txtPassword.Text);

                    MessageBox.Show(
                        "User created successfully.\n\n" +
                        "User ID: " + newUserId,

                        "Success",

                        MessageBoxButtons.OK,

                        MessageBoxIcon.Information);
                }
                else
                {
                    userService.UpdateUser(user);

                    if (userId ==
                        Session.CurrentUser.UserId)
                    {
                        Session.CurrentUser.Username =
                            user.Username;

                        Session.CurrentUser.FullName =
                            user.FullName;
                    }

                    MessageBox.Show(
                        "User updated successfully.",

                        "Success",

                        MessageBoxButtons.OK,

                        MessageBoxIcon.Information);
                }

                txtPassword.Clear();
                txtConfirmPassword.Clear();

                DialogResult = DialogResult.OK;

                Close();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2601 ||
                    ex.Number == 2627 ||
                    ex.Number == 50103)
                {
                    ShowWarning(
                        "Username already exists.");

                    txtUsername.Focus();
                }
                else
                {
                    ShowError(ex.Message);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
            finally
            {
                isSaving = false;

                btnSave.Enabled = true;

                Cursor = Cursors.Default;
            }
        }

        // ================= CANCEL =================

        private void btnCancel_Click(
            object sender,
            EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            Close();
        }

        private void ShowWarning(string message)
        {
            MessageBox.Show(
                message,

                "Validation",

                MessageBoxButtons.OK,

                MessageBoxIcon.Warning);
        }

        private void ShowError(string message)
        {
            MessageBox.Show(
                message,

                "User Account Error",

                MessageBoxButtons.OK,

                MessageBoxIcon.Error);
        }
    }
}
