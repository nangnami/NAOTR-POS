
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using NAOTR_POS.Services;
using NAOTR_POS.Helpers;

namespace NAOTR_POS.Forms
{
    public partial class CategoryManagementForm : Form
    {
        private readonly CategoryService categoryService =
            new CategoryService();

        private bool isLoading = true;

        public CategoryManagementForm()
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

            RefreshCategories();
        }

        // REFRESH ALL DATA

        private void RefreshCategories()
        {
            if (isLoading)
                return;

            try
            {
                LoadCategories();
                LoadMetrics();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        // LOAD CATEGORY LIST

        private void LoadCategories()
        {
            DataTable categories =
                categoryService.GetAllCategories(
                    txtSearch.Text.Trim());

            dgvCategories.DataSource = categories;

            ConfigureGridColumns();

            dgvCategories.ClearSelection();

            lblRecordCount.Text =
                "Showing " +
                categories.Rows.Count.ToString("N0") +
                " category record(s)";
        }

        // GRID CONFIGURATION

        private void ConfigureGridColumns()
        {
            SetColumn(
                "CategoryId",
                "Category ID",
                75);

            SetColumn(
                "Name",
                "Category Name",
                150);

            SetColumn(
                "Description",
                "Description",
                250);

            SetColumn(
                "ProductCount",
                "Assigned Products",
                100);

            if (dgvCategories.Columns.Contains(
                "ProductCount"))
            {
                dgvCategories.Columns["ProductCount"]
                    .DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment
                            .MiddleCenter;
            }
        }

        private void SetColumn(
            string name,
            string header,
            float weight)
        {
            if (dgvCategories.Columns.Contains(name))
            {
                dgvCategories.Columns[name]
                    .HeaderText = header;

                dgvCategories.Columns[name]
                    .FillWeight = weight;
            }
        }

        // LOAD METRICS

        private void LoadMetrics()
        {
            DataTable categories =
                categoryService.GetAllCategories();

            int total = categories.Rows.Count;
            int assigned = 0;
            int unused = 0;

            foreach (DataRow row in categories.Rows)
            {
                int productCount =
                    Convert.ToInt32(
                        row["ProductCount"]);

                if (productCount > 0)
                    assigned++;
                else
                    unused++;
            }

            lblTotalCategories.Text =
                total.ToString("N0");

            lblAssignedCategories.Text =
                assigned.ToString("N0");

            lblUnusedCategories.Text =
                unused.ToString("N0");
        }

        // SELECTED CATEGORY

        private int GetSelectedCategoryId()
        {
            if (dgvCategories.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Please select a category first.",
                    "No Category Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return 0;
            }

            return Convert.ToInt32(
                dgvCategories.SelectedRows[0]
                    .Cells["CategoryId"].Value);
        }

        // ADD CATEGORY

        private void btnAdd_Click(
            object sender, EventArgs e)
        {
            using (CategoryForm form =
                new CategoryForm())
            {
                if (form.ShowDialog(this) ==
                    DialogResult.OK)
                {
                    RefreshCategories();
                }
            }
        }

        // EDIT CATEGORY

        private void btnEdit_Click(
            object sender, EventArgs e)
        {
            int categoryId =
                GetSelectedCategoryId();

            if (categoryId == 0)
                return;

            using (CategoryForm form =
                new CategoryForm(categoryId))
            {
                if (form.ShowDialog(this) ==
                    DialogResult.OK)
                {
                    RefreshCategories();
                }
            }
        }

        // DELETE CATEGORY

        private void btnDelete_Click(
            object sender, EventArgs e)
        {
            int categoryId =
                GetSelectedCategoryId();

            if (categoryId == 0)
                return;

            DataGridViewRow row =
                dgvCategories.SelectedRows[0];

            string categoryName =
                Convert.ToString(
                    row.Cells["Name"].Value);

            int productCount =
                Convert.ToInt32(
                    row.Cells["ProductCount"].Value);

            if (productCount > 0)
            {
                MessageBox.Show(
                    "Cannot delete this category." +
                    "\n\nCategory: " + categoryName +
                    "\nAssigned products: " +
                    productCount +
                    "\n\nReassign the products first.",
                    "Category In Use",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DialogResult confirm =
                MessageBox.Show(
                    "Are you sure you want to delete " +
                    categoryName + "?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                categoryService.DeleteCategory(
                    categoryId);

                MessageBox.Show(
                    "Category deleted successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                RefreshCategories();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50103 ||
                    ex.Number == 547)
                {
                    MessageBox.Show(
                        "Cannot delete a category " +
                        "assigned to products.",
                        "Category In Use",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    RefreshCategories();
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

        // REFRESH

        private void btnRefresh_Click(
            object sender, EventArgs e)
        {
            RefreshCategories();
        }

        // SEARCH

        private void txtSearch_TextChanged(
            object sender, EventArgs e)
        {
            if (isLoading)
                return;

            try
            {
                LoadCategories();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        // DOUBLE CLICK TO EDIT

        private void dgvCategories_CellDoubleClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            dgvCategories.ClearSelection();

            dgvCategories.Rows[e.RowIndex]
                .Selected = true;

            btnEdit_Click(sender, EventArgs.Empty);
        }

        // ERROR HANDLING

        private void ShowError(Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Category Management Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
