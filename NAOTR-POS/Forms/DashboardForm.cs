
using System;
using System.Data;
using System.Windows.Forms;
using NAOTR_POS.Helpers;
using NAOTR_POS.Models;
using NAOTR_POS.Services;

namespace NAOTR_POS.Forms
{
    public partial class DashboardForm : Form
    {
        private readonly DashboardService dashboardService =
            new DashboardService();

        public DashboardForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (Session.CurrentUser != null)
            {
                lblWelcome.Text =
                    "Welcome, " +
                    Session.CurrentUser.FullName + "!";
            }

            LoadDashboard();
        }

        private void LoadDashboard()
        {
            try
            {
                DashboardMetrics metrics =
                    dashboardService.GetMetrics();

                lblTodaySales.Text =
                    "₱" + metrics.TodaySales.ToString("N2");

                lblTodayOrders.Text =
                    metrics.TodayOrders.ToString();

                lblActiveProducts.Text =
                    metrics.ActiveProducts.ToString();

                lblLowStock.Text =
                    metrics.LowStockCount.ToString();

                LoadLowStockProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load dashboard.\n\n" + ex.Message,
                    "Dashboard Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LoadLowStockProducts()
        {
            DataTable table =
                dashboardService.GetLowStockProducts();

            dgvLowStock.DataSource = table;

            if (dgvLowStock.Columns.Contains("ProductId"))
            {
                dgvLowStock.Columns["ProductId"].Visible = false;
            }

            if (dgvLowStock.Columns.Contains("Sku"))
            {
                dgvLowStock.Columns["Sku"].HeaderText = "SKU";
            }

            if (dgvLowStock.Columns.Contains("Name"))
            {
                dgvLowStock.Columns["Name"].HeaderText =
                    "Product Name";

                dgvLowStock.Columns["Name"].FillWeight = 180;
            }

            if (dgvLowStock.Columns.Contains("StockQty"))
            {
                dgvLowStock.Columns["StockQty"].HeaderText =
                    "Stock Quantity";
            }

            if (dgvLowStock.Columns.Contains("ReorderLevel"))
            {
                dgvLowStock.Columns["ReorderLevel"].HeaderText =
                    "Reorder Level";
            }

            dgvLowStock.ClearSelection();
        }

        private void btnRefresh_Click(
            object sender, EventArgs e)
        {
            LoadDashboard();
        }
    }
}
