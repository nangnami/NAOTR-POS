
using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using NAOTR_POS.Models;
using NAOTR_POS.Services;
using NAOTR_POS.Helpers;

namespace NAOTR_POS.Forms
{
    public partial class CustomerForm : Form
    {
        private readonly CustomerService customerService =
            new CustomerService();

        private readonly int customerId;

        public CustomerForm() : this(0)
        {
        }

        public CustomerForm(int customerId)
        {
            InitializeComponent();

            this.customerId = customerId;
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

            try
            {
                if (customerId > 0)
                {
                    lblTitle.Text = "Edit Customer";

                    lblSubtitle.Text =
                        "Update customer information.";

                    LoadCustomer();
                }
                else
                {
                    txtCustomerId.Text =
                        "Automatically generated";
                }

                txtFullName.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Customer Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                Close();
            }
        }

        // LOAD EXISTING CUSTOMER

        private void LoadCustomer()
        {
            Customer customer =
                customerService.GetCustomerById(
                    customerId);

            if (customer == null)
                throw new Exception(
                    "Customer not found.");

            txtCustomerId.Text =
                customer.CustomerId.ToString();

            txtFullName.Text = customer.FullName;

            txtPhone.Text = customer.Phone ?? "";

            txtEmail.Text = customer.Email ?? "";

            txtAddress.Text = customer.Address ?? "";
        }

        // EMAIL VALIDATION

        private bool IsValidEmail(string email)
        {
            try
            {
                MailAddress address =
                    new MailAddress(email);

                return string.Equals(
                    address.Address,
                    email,
                    StringComparison.OrdinalIgnoreCase);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        // FORM VALIDATION

        private bool ValidateInput()
        {
            errorProvider.Clear();

            bool valid = true;

            if (string.IsNullOrWhiteSpace(
                txtFullName.Text))
            {
                errorProvider.SetError(
                    txtFullName,
                    "Customer name is required.");

                valid = false;
            }

            string phone = txtPhone.Text.Trim();

            if (!string.IsNullOrWhiteSpace(phone))
            {
                string digits =
                    Regex.Replace(phone, @"\D", "");

                if (!Regex.IsMatch(
                    phone, @"^\+?[0-9\s().-]+$") ||
                    digits.Length < 7 ||
                    digits.Length > 15)
                {
                    errorProvider.SetError(
                        txtPhone,
                        "Enter a valid phone number.");

                    valid = false;
                }
            }

            string email = txtEmail.Text.Trim();

            if (!string.IsNullOrWhiteSpace(email) &&
                !IsValidEmail(email))
            {
                errorProvider.SetError(
                    txtEmail,
                    "Enter a valid email address.");

                valid = false;
            }

            return valid;
        }

        // SAVE CUSTOMER

        private void btnSave_Click(
            object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                btnSave.Enabled = false;
                btnSave.Text = "Saving...";

                Customer customer = new Customer
                {
                    CustomerId = customerId,

                    FullName =
                        txtFullName.Text.Trim(),

                    Phone =
                        txtPhone.Text.Trim(),

                    Email =
                        txtEmail.Text.Trim(),

                    Address =
                        txtAddress.Text.Trim()
                };

                if (customerId == 0)
                {
                    int newId =
                        customerService.InsertCustomer(
                            customer);

                    MessageBox.Show(
                        "Customer added successfully!" +
                        "\nCustomer ID: " + newId,
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    customerService.UpdateCustomer(
                        customer);

                    MessageBox.Show(
                        "Customer updated successfully!",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Customer Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                if (!IsDisposed)
                {
                    btnSave.Enabled = true;
                    btnSave.Text = "Save Customer";
                }
            }
        }
    }
}
