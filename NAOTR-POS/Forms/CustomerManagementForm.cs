
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using NAOTR_POS.Services;
using NAOTR_POS.Helpers;

namespace NAOTR_POS.Forms
{
    public partial class CustomerManagementForm : Form
    {
        private readonly CustomerService customerService =
            new CustomerService();

        private bool isLoading = true;

        public CustomerManagementForm()
        {
            InitializeComponent();
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

            isLoading = false;

            RefreshCustomers();
        }

        // REFRESH CUSTOMERS

        private void RefreshCustomers()
        {
            if (isLoading)
                return;

            try
            {
                LoadCustomers();
                LoadMetrics();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        // LOAD CUSTOMER RECORDS

        private void LoadCustomers()
        {
            DataTable customers =
                customerService.GetCustomers(
                    txtSearch.Text.Trim());

            dgvCustomers.DataSource = customers;

            ConfigureGridColumns();

            dgvCustomers.ClearSelection();

            lblRecordCount.Text =
                "Showing " +
                customers.Rows.Count.ToString("N0") +
                " customer record(s)";
        }

        // CONFIGURE GRID

        private void ConfigureGridColumns()
        {
            HideColumn("Address");
            HideColumn("CreatedAt");

            SetColumn(
                "CustomerId",
                "ID",
                45);

            SetColumn(
                "FullName",
                "Full Name",
                150);

            SetColumn(
                "Phone",
                "Phone Number",
                120);

            SetColumn(
                "Email",
                "Email Address",
                170);

            SetColumn(
                "OrderCount",
                "Orders",
                65);

            SetColumn(
                "TotalSpent",
                "Total Spent",
                100);

            if (dgvCustomers.Columns.Contains(
                "TotalSpent"))
            {
                dgvCustomers.Columns["TotalSpent"]
                    .DefaultCellStyle.Format =
                        "₱#,##0.00";

                dgvCustomers.Columns["TotalSpent"]
                    .DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment
                            .MiddleRight;
            }

            if (dgvCustomers.Columns.Contains(
                "OrderCount"))
            {
                dgvCustomers.Columns["OrderCount"]
                    .DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment
                            .MiddleCenter;
            }
        }

        private void HideColumn(string name)
        {
            if (dgvCustomers.Columns.Contains(name))
            {
                dgvCustomers.Columns[name]
                    .Visible = false;
            }
        }

        private void SetColumn(
            string name,
            string header,
            float weight)
        {
            if (dgvCustomers.Columns.Contains(name))
            {
                dgvCustomers.Columns[name]
                    .HeaderText = header;

                dgvCustomers.Columns[name]
                    .FillWeight = weight;
            }
        }

        // LOAD SUMMARY CARDS

        private void LoadMetrics()
        {
            DataTable customers =
                customerService.GetCustomers();

            int total = customers.Rows.Count;
            int purchasing = 0;

            foreach (DataRow row in customers.Rows)
            {
                int orderCount =
                    Convert.ToInt32(
                        row["OrderCount"]);

                if (orderCount > 0)
                    purchasing++;
            }

            lblTotalCustomers.Text =
                total.ToString("N0");

            lblPurchasingCustomers.Text =
                purchasing.ToString("N0");

            lblWithoutPurchases.Text =
                (total - purchasing).ToString("N0");
        }

        // SELECTED CUSTOMER

        private int GetSelectedCustomerId()
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Please select a customer first.",
                    "No Customer Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return 0;
            }

            return Convert.ToInt32(
                dgvCustomers.SelectedRows[0]
                    .Cells["CustomerId"].Value);
        }

        // ADD CUSTOMER

        private void btnAdd_Click(
            object sender, EventArgs e)
        {
            using (CustomerForm form =
                new CustomerForm())
            {
                if (form.ShowDialog(this) ==
                    DialogResult.OK)
                {
                    RefreshCustomers();
                }
            }
        }

        // EDIT CUSTOMER

        private void btnEdit_Click(
            object sender, EventArgs e)
        {
            int customerId =
                GetSelectedCustomerId();

            if (customerId == 0)
                return;

            using (CustomerForm form =
                new CustomerForm(customerId))
            {
                if (form.ShowDialog(this) ==
                    DialogResult.OK)
                {
                    RefreshCustomers();
                }
            }
        }

        // DELETE CUSTOMER

        private void btnDelete_Click(
            object sender, EventArgs e)
        {
            int customerId =
                GetSelectedCustomerId();

            if (customerId == 0)
                return;

            DataGridViewRow row =
                dgvCustomers.SelectedRows[0];

            string customerName =
                Convert.ToString(
                    row.Cells["FullName"].Value);

            int orderCount =
                Convert.ToInt32(
                    row.Cells["OrderCount"].Value);

            if (orderCount > 0)
            {
                MessageBox.Show(
                    "Cannot delete this customer." +
                    "\n\nCustomer: " + customerName +
                    "\nExisting orders: " + orderCount +
                    "\n\nSales history must be preserved.",
                    "Customer Has Orders",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DialogResult confirm =
                MessageBox.Show(
                    "Are you sure you want to delete " +
                    customerName + "?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                customerService.DeleteCustomer(
                    customerId);

                MessageBox.Show(
                    "Customer deleted successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                RefreshCustomers();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50202 ||
                    ex.Number == 547)
                {
                    MessageBox.Show(
                        "Cannot delete a customer " +
                        "with existing sales records.",
                        "Customer Has Orders",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    RefreshCustomers();
                }
                else
                {
                    ShowError(ex);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        // REFRESH BUTTON

        private void btnRefresh_Click(
            object sender, EventArgs e)
        {
            RefreshCustomers();
        }

        // SEARCH

        private void txtSearch_TextChanged(
            object sender, EventArgs e)
        {
            if (isLoading)
                return;

            try
            {
                LoadCustomers();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        // DOUBLE CLICK TO EDIT

        private void dgvCustomers_CellDoubleClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            dgvCustomers.ClearSelection();

            dgvCustomers.Rows[e.RowIndex]
                .Selected = true;

            btnEdit_Click(sender, EventArgs.Empty);
        }

        // ERROR HANDLING

        private void ShowError(Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Customer Management Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
