
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

using NAOTR_POS.Helpers;
using NAOTR_POS.Services;

namespace NAOTR_POS.Forms
{
    public partial class ReportsForm : Form
    {
        private readonly ReportService reportService =
            new ReportService();

        private bool isLoading = true;
        private bool hasGeneratedReport = false;

        private DateTime generatedFrom;
        private DateTime generatedTo;

        private DataTable printTable;
        private string printTitle;
        private int printRowIndex;

        public ReportsForm()
        {
            InitializeComponent();
        }

        // ================= FORM LOAD =================

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Session.IsInRole("Admin") &&
                !Session.IsInRole("Manager"))
            {
                MessageBox.Show(
                    "Access denied.",
                    "Permission Denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                Close();
                return;
            }

            dtpFrom.MaxDate = DateTime.Today;
            dtpTo.MaxDate = DateTime.Today;

            cmbPeriod.SelectedIndex = 2;

            isLoading = false;

            ApplyPeriod();

            LoadReports();
        }

        // ================= DATE FILTER =================

        private void cmbPeriod_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            if (isLoading)
                return;

            ApplyPeriod();
        }

        private void ApplyPeriod()
        {
            DateTime today = DateTime.Today;

            bool customRange =
                cmbPeriod.Text == "Custom Range";

            dtpFrom.Enabled = customRange;
            dtpTo.Enabled = customRange;

            switch (cmbPeriod.Text)
            {
                case "Today":

                    dtpFrom.Value = today;
                    dtpTo.Value = today;

                    break;

                case "This Week":

                    int difference =
                        ((int)today.DayOfWeek + 6) % 7;

                    dtpFrom.Value =
                        today.AddDays(-difference);

                    dtpTo.Value = today;

                    break;

                case "This Month":

                    dtpFrom.Value = new DateTime(
                        today.Year,
                        today.Month,
                        1);

                    dtpTo.Value = today;

                    break;
            }
        }

        // ================= GENERATE REPORTS =================

        private void btnGenerate_Click(
            object sender,
            EventArgs e)
        {
            LoadReports();
        }

        private void LoadReports()
        {
            DateTime from = dtpFrom.Value.Date;
            DateTime to = dtpTo.Value.Date;

            if (from > to)
            {
                MessageBox.Show(
                    "The start date cannot be later " +
                    "than the end date.",
                    "Invalid Date Range",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;

                btnGenerate.Enabled = false;
                btnExport.Enabled = false;
                btnPrint.Enabled = false;

                hasGeneratedReport = false;

                DataTable daily =
                    reportService.GetDailySales(from, to);

                DataTable products =
                    reportService.GetTopProducts(from, to);

                DataTable categories =
                    reportService.GetSalesByCategory(from, to);

                DataTable payments =
                    reportService.GetPaymentMethods(from, to);

                DataTable lowStock =
                    reportService.GetLowStock();

                // Load summary cards

                LoadSummary(daily, lowStock);

                // Load grids

                dgvDailySales.DataSource = daily;
                dgvTopProducts.DataSource = products;
                dgvCategories.DataSource = categories;
                dgvPayments.DataSource = payments;
                dgvLowStock.DataSource = lowStock;

                FormatGrids();

                // Load charts

                LoadChart(
                    chartDailySales,
                    daily,
                    "SaleDate",
                    "Revenue",
                    true);

                LoadChart(
                    chartTopProducts,
                    products,
                    "ProductName",
                    "Revenue");

                LoadChart(
                    chartCategories,
                    categories,
                    "CategoryName",
                    "Revenue");

                LoadChart(
                    chartPayments,
                    payments,
                    "PaymentMethod",
                    "Revenue");

                generatedFrom = from;
                generatedTo = to;

                hasGeneratedReport = true;

                lblSubtitle.Text =
                    "Sales period: " +
                    from.ToString("MMM dd, yyyy") +
                    " - " +
                    to.ToString("MMM dd, yyyy");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to generate reports.\n\n" +
                    ex.Message,
                    "Report Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;

                btnGenerate.Enabled = true;
                btnExport.Enabled = hasGeneratedReport;
                btnPrint.Enabled = hasGeneratedReport;
            }
        }

        // ================= SUMMARY CARDS =================

        private void LoadSummary(
            DataTable daily,
            DataTable lowStock)
        {
            decimal revenue = 0;
            int transactions = 0;

            foreach (DataRow row in daily.Rows)
            {
                revenue +=
                    Convert.ToDecimal(row["Revenue"]);

                transactions +=
                    Convert.ToInt32(row["OrderCount"]);
            }

            decimal average = transactions == 0
                ? 0
                : revenue / transactions;

            lblRevenue.Text =
                "₱" + revenue.ToString("N2");

            lblTransactions.Text =
                transactions.ToString("N0");

            lblAverageOrder.Text =
                "₱" + average.ToString("N2");

            lblLowStock.Text =
                lowStock.Rows.Count.ToString("N0");
        }

        // ================= CHART DATA =================

        private void LoadChart(
            Chart chart,
            DataTable table,
            string labelColumn,
            string valueColumn,
            bool isDate = false)
        {
            Series series = chart.Series[0];

            series.Points.Clear();

            foreach (DataRow row in table.Rows)
            {
                string label;

                if (isDate)
                {
                    label = Convert.ToDateTime(
                        row[labelColumn])
                        .ToString("MMM dd");
                }
                else
                {
                    label = Convert.ToString(
                        row[labelColumn]);
                }

                decimal value =
                    Convert.ToDecimal(row[valueColumn]);

                series.Points.AddXY(
                    label,
                    Convert.ToDouble(value));
            }

            chart.ChartAreas[0].RecalculateAxesScale();

            chart.Invalidate();
        }

        // ================= GRID FORMATTING =================

        private void FormatGrids()
        {
            FormatColumn(
                dgvDailySales,
                "SaleDate",
                "Sale Date",
                "MMM dd, yyyy");

            FormatColumn(
                dgvDailySales,
                "OrderCount",
                "Transactions",
                "N0");

            FormatColumn(
                dgvDailySales,
                "Revenue",
                "Total Revenue",
                "₱#,##0.00");

            FormatColumn(
                dgvTopProducts,
                "ProductName",
                "Product");

            FormatColumn(
                dgvTopProducts,
                "QtySold",
                "Quantity Sold",
                "N0");

            FormatColumn(
                dgvTopProducts,
                "Revenue",
                "Product Sales",
                "₱#,##0.00");

            FormatColumn(
                dgvCategories,
                "CategoryName",
                "Category");

            FormatColumn(
                dgvCategories,
                "Revenue",
                "Category Sales",
                "₱#,##0.00");

            FormatColumn(
                dgvPayments,
                "PaymentMethod",
                "Payment Method");

            FormatColumn(
                dgvPayments,
                "OrderCount",
                "Transactions",
                "N0");

            FormatColumn(
                dgvPayments,
                "Revenue",
                "Total Revenue",
                "₱#,##0.00");

            if (dgvLowStock.Columns.Contains("ProductId"))
            {
                dgvLowStock.Columns["ProductId"]
                    .Visible = false;
            }

            FormatColumn(
                dgvLowStock,
                "Sku",
                "SKU");

            FormatColumn(
                dgvLowStock,
                "Name",
                "Product Name");

            FormatColumn(
                dgvLowStock,
                "StockQty",
                "Current Stock",
                "N0");

            FormatColumn(
                dgvLowStock,
                "ReorderLevel",
                "Reorder Level",
                "N0");

            dgvLowStock.DefaultCellStyle.ForeColor =
                Color.FromArgb(185, 28, 28);

            dgvDailySales.ClearSelection();
            dgvTopProducts.ClearSelection();
            dgvCategories.ClearSelection();
            dgvPayments.ClearSelection();
            dgvLowStock.ClearSelection();
        }

        private void FormatColumn(
            DataGridView grid,
            string column,
            string header,
            string format = "")
        {
            if (!grid.Columns.Contains(column))
                return;

            grid.Columns[column].HeaderText = header;

            if (!string.IsNullOrWhiteSpace(format))
            {
                grid.Columns[column]
                    .DefaultCellStyle.Format = format;
            }
        }

        // ================= SELECTED REPORT =================

        private DataGridView GetCurrentGrid()
        {
            switch (tabReports.SelectedIndex)
            {
                case 0:
                    return dgvDailySales;

                case 1:
                    return dgvTopProducts;

                case 2:
                    return dgvCategories;

                case 3:
                    return dgvPayments;

                case 4:
                    return dgvLowStock;

                default:
                    return null;
            }
        }

        private string GetReportTitle()
        {
            return tabReports.SelectedTab == null
                ? "Sales Report"
                : tabReports.SelectedTab.Text + " Report";
        }

        // ================= CSV EXPORT =================

        private void btnExport_Click(
            object sender,
            EventArgs e)
        {
            if (!hasGeneratedReport)
                return;

            DataGridView grid = GetCurrentGrid();

            if (grid == null || grid.Rows.Count == 0)
            {
                MessageBox.Show(
                    "There are no records to export.");

                return;
            }

            using (SaveFileDialog dialog =
                new SaveFileDialog())
            {
                dialog.Filter =
                    "CSV File (*.csv)|*.csv";

                dialog.DefaultExt = "csv";
                dialog.AddExtension = true;

                dialog.FileName =
                    GetReportTitle().Replace(" ", "_") +
                    "_" +
                    DateTime.Now.ToString("yyyyMMdd") +
                    ".csv";

                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    StringBuilder csv =
                        new StringBuilder();

                    bool first = true;

                    // Column headers

                    foreach (DataGridViewColumn column
                        in grid.Columns)
                    {
                        if (!column.Visible)
                            continue;

                        if (!first)
                            csv.Append(",");

                        csv.Append(
                            EscapeCsv(column.HeaderText));

                        first = false;
                    }

                    csv.AppendLine();

                    // Data rows

                    foreach (DataGridViewRow row
                        in grid.Rows)
                    {
                        if (row.IsNewRow)
                            continue;

                        first = true;

                        foreach (DataGridViewColumn column
                            in grid.Columns)
                        {
                            if (!column.Visible)
                                continue;

                            if (!first)
                                csv.Append(",");

                            object value =
                                row.Cells[column.Index].Value;

                            string text;

                            if (value is DateTime)
                            {
                                text = ((DateTime)value)
                                    .ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                text = Convert.ToString(
                                    value,
                                    CultureInfo.InvariantCulture);
                            }

                            // Protect spreadsheet users
                            // from formula-like text.

                            if (value is string)
                            {
                                string trimmed =
                                    text.TrimStart();

                                if (trimmed.StartsWith("=") ||
                                    trimmed.StartsWith("+") ||
                                    trimmed.StartsWith("-") ||
                                    trimmed.StartsWith("@"))
                                {
                                    text = "'" + text;
                                }
                            }

                            csv.Append(EscapeCsv(text));

                            first = false;
                        }

                        csv.AppendLine();
                    }

                    File.WriteAllText(
                        dialog.FileName,
                        csv.ToString(),
                        new UTF8Encoding(true));

                    MessageBox.Show(
                        "Report exported successfully.",
                        "Export Complete",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "Export Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private string EscapeCsv(string value)
        {
            value = value ?? "";

            return "\"" +
                value.Replace("\"", "\"\"") +
                "\"";
        }

        // ================= PRINT PREVIEW =================

        private void btnPrint_Click(
            object sender,
            EventArgs e)
        {
            if (!hasGeneratedReport)
                return;

            DataGridView grid = GetCurrentGrid();

            if (grid == null || grid.Rows.Count == 0)
            {
                MessageBox.Show(
                    "There are no records to print.");

                return;
            }

            printTable = new DataTable();

            foreach (DataGridViewColumn column
                in grid.Columns)
            {
                if (column.Visible)
                {
                    printTable.Columns.Add(
                        column.HeaderText);
                }
            }

            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.IsNewRow)
                    continue;

                DataRow newRow = printTable.NewRow();

                int index = 0;

                foreach (DataGridViewColumn column
                    in grid.Columns)
                {
                    if (!column.Visible)
                        continue;

                    newRow[index++] = Convert.ToString(
                        row.Cells[column.Index]
                            .FormattedValue);
                }

                printTable.Rows.Add(newRow);
            }

            printTitle = GetReportTitle();

            try
            {
                using (PrintDocument document =
                    new PrintDocument())
                {
                    document.DocumentName =
                        "NAOTR-POS - " + printTitle;

                    document.DefaultPageSettings.Landscape =
                        true;

                    document.DefaultPageSettings.Margins =
                        new System.Drawing.Printing.Margins(45, 45, 45, 45);

                    document.BeginPrint +=
                        (s, args) => printRowIndex = 0;

                    document.PrintPage +=
                        Document_PrintPage;

                    using (PrintPreviewDialog preview =
                        new PrintPreviewDialog())
                    {
                        preview.Document = document;

                        preview.WindowState =
                            FormWindowState.Maximized;

                        preview.ShowDialog(this);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Printing Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ================= PRINT PAGE CONTENT =================

        private void Document_PrintPage(
            object sender,
            PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;

            float x = e.MarginBounds.Left;
            float y = e.MarginBounds.Top;

            float width = e.MarginBounds.Width;

            using (Font brandFont = new Font(
                "Segoe UI", 22F, FontStyle.Bold))

            using (Font titleFont = new Font(
                "Segoe UI", 15F, FontStyle.Bold))

            using (Font normalFont = new Font(
                "Segoe UI", 10F))

            using (Font headerFont = new Font(
                "Segoe UI", 10F, FontStyle.Bold))

            using (SolidBrush navy = new SolidBrush(
                Color.FromArgb(16, 36, 62)))

            using (StringFormat format =
                new StringFormat())
            {
                format.Trimming =
                    StringTrimming.EllipsisCharacter;

                format.FormatFlags =
                    StringFormatFlags.NoWrap;

                g.DrawString(
                    "NAOTR-POS",
                    brandFont,
                    navy,
                    x,
                    y);

                y += 40;

                g.DrawString(
                    printTitle,
                    titleFont,
                    Brushes.Black,
                    x,
                    y);

                y += 30;

                string period =
                    printTitle == "Low Stock Report"
                    ? "Current inventory at report generation"
                    : "Period: " +
                        generatedFrom.ToString("MMM dd, yyyy") +
                        " - " +
                        generatedTo.ToString("MMM dd, yyyy");

                g.DrawString(
                    period,
                    normalFont,
                    Brushes.Black,
                    x,
                    y);

                y += 22;

                g.DrawString(
                    "Prepared by: " +
                    Session.CurrentUser.FullName,
                    normalFont,
                    Brushes.Black,
                    x,
                    y);

                y += 35;

                float columnWidth =
                    width / printTable.Columns.Count;

                float rowHeight = 30;

                // Table header

                g.FillRectangle(
                    navy,
                    x,
                    y,
                    width,
                    rowHeight);

                for (int i = 0;
                    i < printTable.Columns.Count;
                    i++)
                {
                    RectangleF cell = new RectangleF(
                        x + (i * columnWidth) + 5,
                        y + 5,
                        columnWidth - 10,
                        rowHeight - 8);

                    g.DrawString(
                        printTable.Columns[i].ColumnName,
                        headerFont,
                        Brushes.White,
                        cell,
                        format);
                }

                y += rowHeight;

                // Table rows with pagination

                while (printRowIndex < printTable.Rows.Count)
                {
                    if (y + rowHeight >
                        e.MarginBounds.Bottom - 25)
                    {
                        e.HasMorePages = true;
                        return;
                    }

                    DataRow row =
                        printTable.Rows[printRowIndex];

                    if (printRowIndex % 2 == 0)
                    {
                        g.FillRectangle(
                            Brushes.WhiteSmoke,
                            x,
                            y,
                            width,
                            rowHeight);
                    }

                    for (int i = 0;
                        i < printTable.Columns.Count;
                        i++)
                    {
                        RectangleF cell =
                            new RectangleF(
                                x + (i * columnWidth) + 5,
                                y + 5,
                                columnWidth - 10,
                                rowHeight - 8);

                        g.DrawString(
                            Convert.ToString(row[i]),
                            normalFont,
                            Brushes.Black,
                            cell,
                            format);
                    }

                    g.DrawLine(
                        Pens.LightGray,
                        x,
                        y + rowHeight,
                        x + width,
                        y + rowHeight);

                    y += rowHeight;

                    printRowIndex++;
                }

                y += 10;

                g.DrawString(
                    "Total records: " +
                    printTable.Rows.Count,
                    normalFont,
                    Brushes.Black,
                    x,
                    y);

                e.HasMorePages = false;
            }
        }
    }
}
