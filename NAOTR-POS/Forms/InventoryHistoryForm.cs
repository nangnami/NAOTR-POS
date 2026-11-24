
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using NAOTR_POS.Services;
using NAOTR_POS.Helpers;

namespace NAOTR_POS.Forms
{
    public partial class InventoryHistoryForm : Form
    {
        private readonly InventoryHistoryService historyService =
            new InventoryHistoryService();

        private bool isLoading = true;

        public InventoryHistoryForm()
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

            InitializeFilters();

            isLoading = false;

            LoadHistory();
        }

        // ================= INITIALIZE FILTERS =================

        private void InitializeFilters()
        {
            cmbMovementType.SelectedIndex = 0;

            dtpFrom.Value = new DateTime(
                DateTime.Today.Year,
                DateTime.Today.Month,
                1);

            dtpTo.Value = DateTime.Today;

            chkDateFilter.Checked = false;

            dtpFrom.Enabled = false;
            dtpTo.Enabled = false;
        }

        // ================= LOAD HISTORY =================

        private void LoadHistory()
        {
            if (isLoading)
                return;

            try
            {
                string search = txtSearch.Text.Trim();

                string movementType =
                    cmbMovementType.SelectedIndex <= 0
                        ? "All"
                        : cmbMovementType.Text;

                DateTime? fromDate = null;
                DateTime? toDate = null;

                if (chkDateFilter.Checked)
                {
                    fromDate = dtpFrom.Value.Date;
                    toDate = dtpTo.Value.Date;

                    if (fromDate.Value > toDate.Value)
                    {
                        MessageBox.Show(
                            "From Date cannot be later than To Date.",
                            "Invalid Date Range",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        return;
                    }
                }

                DataTable table =
                    historyService.GetHistory(
                        search,
                        movementType,
                        fromDate,
                        toDate);

                dgvHistory.DataSource = table;

                ConfigureGridColumns();

                LoadMetrics(table);

                dgvHistory.ClearSelection();

                lblRecordCount.Text =
                    "Showing " +
                    table.Rows.Count.ToString("N0") +
                    " movement record(s)";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Inventory History Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ================= CONFIGURE GRID =================

        private void ConfigureGridColumns()
        {
            HideColumn("LogId");
            HideColumn("ProductId");
            HideColumn("Barcode");

            SetColumn("Sku", "SKU", 85);
            SetColumn("ProductName", "Product Name", 180);
            SetColumn("MovementType", "Movement", 90);
            SetColumn("ChangeQty", "Quantity", 65);
            SetColumn("Reason", "Reason", 170);
            SetColumn("AdjustedBy", "Adjusted By", 150);
            SetColumn("MovementDate", "Date and Time", 145);

            if (dgvHistory.Columns.Contains("ChangeQty"))
            {
                dgvHistory.Columns["ChangeQty"]
                    .DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleRight;

                dgvHistory.Columns["ChangeQty"]
                    .DefaultCellStyle.Format =
                        "+#,##0;-#,##0;0";
            }

            if (dgvHistory.Columns.Contains("MovementDate"))
            {
                dgvHistory.Columns["MovementDate"]
                    .DefaultCellStyle.Format =
                        "MMM dd, yyyy hh:mm tt";
            }
        }

        private void HideColumn(string name)
        {
            if (dgvHistory.Columns.Contains(name))
            {
                dgvHistory.Columns[name].Visible = false;
            }
        }

        private void SetColumn(
            string name,
            string header,
            float weight)
        {
            if (dgvHistory.Columns.Contains(name))
            {
                dgvHistory.Columns[name].HeaderText =
                    header;

                dgvHistory.Columns[name].FillWeight =
                    weight;
            }
        }

        // ================= SUMMARY METRICS =================

        private void LoadMetrics(DataTable table)
        {
            int totalMovements = table.Rows.Count;

            long totalStockIn = 0;
            long totalStockOut = 0;

            foreach (DataRow row in table.Rows)
            {
                long changeQty =
                    Convert.ToInt64(row["ChangeQty"]);

                if (changeQty > 0)
                {
                    totalStockIn += changeQty;
                }
                else if (changeQty < 0)
                {
                    totalStockOut += Math.Abs(changeQty);
                }
            }

            lblTotalMovements.Text =
                totalMovements.ToString("N0");

            lblStockIn.Text =
                totalStockIn.ToString("N0");

            lblStockOut.Text =
                totalStockOut.ToString("N0");
        }

        // ================= COLOR MOVEMENTS =================

        private void dgvHistory_CellFormatting(
            object sender,
            DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 ||
                e.ColumnIndex < 0 ||
                e.Value == null ||
                e.Value == DBNull.Value)
            {
                return;
            }

            string columnName =
                dgvHistory.Columns[e.ColumnIndex].Name;

            if (columnName == "MovementType")
            {
                string movement = e.Value.ToString();

                if (movement == "Stock In")
                {
                    e.CellStyle.ForeColor = Color.SeaGreen;
                }
                else if (movement == "Stock Out")
                {
                    e.CellStyle.ForeColor = Color.Firebrick;
                }
            }

            if (columnName == "ChangeQty")
            {
                int quantity;

                if (int.TryParse(
                    e.Value.ToString(),
                    out quantity))
                {
                    e.CellStyle.ForeColor =
                        quantity > 0
                            ? Color.SeaGreen
                            : quantity < 0
                                ? Color.Firebrick
                                : Color.SlateGray;
                }
            }
        }

        // ================= SEARCH =================

        private void txtSearch_KeyDown(
            object sender,
            KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                LoadHistory();
            }
        }

        // ================= MOVEMENT FILTER =================

        private void cmbMovementType_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            if (!isLoading)
            {
                LoadHistory();
            }
        }

        // ================= DATE FILTER =================

        private void chkDateFilter_CheckedChanged(
            object sender,
            EventArgs e)
        {
            dtpFrom.Enabled = chkDateFilter.Checked;
            dtpTo.Enabled = chkDateFilter.Checked;

            if (!isLoading)
            {
                LoadHistory();
            }
        }

        // ================= REFRESH =================

        private void btnRefresh_Click(
            object sender,
            EventArgs e)
        {
            LoadHistory();
        }

        // ================= RESET FILTERS =================

        private void btnReset_Click(
            object sender,
            EventArgs e)
        {
            isLoading = true;

            try
            {
                txtSearch.Clear();

                InitializeFilters();
            }
            finally
            {
                isLoading = false;
            }

            LoadHistory();
        }
        private void btnHistory_Click(
    object sender, EventArgs e)
        {
            using (InventoryHistoryForm form =
                new InventoryHistoryForm())
            {
                form.ShowDialog(this);
            }
        }
        // ================= CLOSE =================

        private void btnClose_Click(
            object sender,
            EventArgs e)
        {
            Close();
        }
    }
}
