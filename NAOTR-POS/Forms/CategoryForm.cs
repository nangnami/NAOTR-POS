
using NAOTR_POS.Helpers;
using NAOTR_POS.Models;
using NAOTR_POS.Services;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml.Linq;

namespace NAOTR_POS.Forms
{
    public partial class CategoryForm : Form
    {
        private readonly CategoryService categoryService =
            new CategoryService();

        private readonly int categoryId;

        public CategoryForm() : this(0)
        {
        }

        public CategoryForm(int categoryId)
        {
            InitializeComponent();

            this.categoryId = categoryId;
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
                if (categoryId > 0)
                {
                    lblTitle.Text = "Edit Category";

                    lblSubtitle.Text =
                        "Update existing category information.";

                    LoadCategory();
                }
                else
                {
                    txtCategoryId.Text =
                        "Automatically generated";
                }

                txtName.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Category Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                Close();
            }
        }

        // LOAD CATEGORY

        private void LoadCategory()
        {
            Category category =
                categoryService.GetCategoryById(
                    categoryId);

            if (category == null)
                throw new Exception(
                    "Category not found.");

            txtCategoryId.Text =
                category.CategoryId.ToString();

            txtName.Text = category.Name;

            txtDescription.Text =
                category.Description ?? "";
        }

        // VALIDATE INPUT

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show(
                    "Please enter a category name.",
                    "Required Field",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtName.Focus();

                return false;
            }

            return true;
        }

        // SAVE CATEGORY

        private void btnSave_Click(
            object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                btnSave.Enabled = false;
                btnSave.Text = "Saving...";

                Category category = new Category
                {
                    CategoryId = categoryId,

                    Name = txtName.Text.Trim(),

                    Description =
                        txtDescription.Text.Trim()
                };

                if (categoryId == 0)
                {
                    int newId =
                        categoryService.InsertCategory(
                            category);

                    MessageBox.Show(
                        "Category added successfully!" +
                        "\nCategory ID: " + newId,
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    categoryService.UpdateCategory(
                        category);

                    MessageBox.Show(
                        "Category updated successfully!",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50101 ||
                    ex.Number == 2601 ||
                    ex.Number == 2627)
                {
                    MessageBox.Show(
                        "Category name already exists.",
                        "Duplicate Category",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtName.Focus();
                }
                else
                {
                    MessageBox.Show(
                        ex.Message,
                        "Database Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Category Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                if (!IsDisposed)
                {
                    btnSave.Enabled = true;
                    btnSave.Text = "Save Category";
                }
            }
        }
    }
}
