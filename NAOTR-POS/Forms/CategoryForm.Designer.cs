
using System.Drawing;
using System.Windows.Forms;

namespace NAOTR_POS.Forms
{
    partial class CategoryForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Panel pnlFooter;

        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblCategoryId;

        private TextBox txtCategoryId;
        private TextBox txtName;
        private TextBox txtDescription;

        private Button btnSave;
        private Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            pnlFooter = new Panel();

            lblTitle = new Label();
            lblSubtitle = new Label();
            lblCategoryId = new Label();

            txtCategoryId = new TextBox();
            txtName = new TextBox();
            txtDescription = new TextBox();

            btnSave = new Button();
            btnCancel = new Button();

            SuspendLayout();

            // HEADER

            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 85;

            pnlHeader.BackColor =
                Color.FromArgb(16, 36, 62);

            lblTitle.Text = "Add Category";

            lblTitle.Font = new Font(
                "Segoe UI", 22F, FontStyle.Bold);

            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(25, 10);
            lblTitle.AutoSize = true;

            lblSubtitle.Text =
                "Create a new product category.";

            lblSubtitle.Font =
                new Font("Segoe UI", 10F);

            lblSubtitle.ForeColor = Color.LightSteelBlue;
            lblSubtitle.Location = new Point(28, 53);
            lblSubtitle.AutoSize = true;

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubtitle);

            // CATEGORY ID

            lblCategoryId = CreateLabel(
                "Category ID", 25, 105);

            txtCategoryId.Location =
                new Point(25, 130);

            txtCategoryId.Size =
                new Size(450, 30);

            txtCategoryId.Font =
                new Font("Segoe UI", 11F);

            txtCategoryId.ReadOnly = true;
            txtCategoryId.TabStop = false;

            txtCategoryId.BackColor =
                Color.FromArgb(241, 245, 249);

            txtCategoryId.Text =
                "Automatically generated";

            // CATEGORY NAME

            Controls.Add(CreateLabel(
                "Category Name *", 25, 180));

            txtName.Location =
                new Point(25, 205);

            txtName.Size =
                new Size(450, 30);

            txtName.Font =
                new Font("Segoe UI", 11F);

            txtName.MaxLength = 100;

            // DESCRIPTION

            Controls.Add(CreateLabel(
                "Description", 25, 255));

            txtDescription.Location =
                new Point(25, 280);

            txtDescription.Size =
                new Size(450, 105);

            txtDescription.Font =
                new Font("Segoe UI", 11F);

            txtDescription.Multiline = true;
            txtDescription.MaxLength = 200;

            txtDescription.ScrollBars =
                ScrollBars.Vertical;

            // FOOTER

            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Height = 75;

            pnlFooter.BackColor =
                Color.FromArgb(241, 245, 249);

            ConfigureButton(
                btnCancel,
                "Cancel",
                Color.FromArgb(100, 116, 139));

            btnCancel.Location = new Point(210, 17);
            btnCancel.Size = new Size(110, 40);

            btnCancel.DialogResult =
                DialogResult.Cancel;

            ConfigureButton(
                btnSave,
                "Save Category",
                Color.FromArgb(37, 99, 235));

            btnSave.Location = new Point(330, 17);
            btnSave.Size = new Size(145, 40);

            btnSave.Click += btnSave_Click;

            pnlFooter.Controls.Add(btnCancel);
            pnlFooter.Controls.Add(btnSave);

            // ADD CONTROLS

            Controls.Add(lblCategoryId);
            Controls.Add(txtCategoryId);
            Controls.Add(txtName);
            Controls.Add(txtDescription);

            Controls.Add(pnlHeader);
            Controls.Add(pnlFooter);

            // FORM SETTINGS

            AutoScaleMode = AutoScaleMode.None;

            ClientSize = new Size(500, 490);

            BackColor = Color.White;

            Font = new Font("Segoe UI", 10F);

            FormBorderStyle =
                FormBorderStyle.FixedDialog;

            StartPosition =
                FormStartPosition.CenterParent;

            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;

            AcceptButton = btnSave;
            CancelButton = btnCancel;

            Name = "CategoryForm";
            Text = "Category";

            ResumeLayout(false);
            PerformLayout();
        }

        private Label CreateLabel(
            string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,

                Font = new Font(
                    "Segoe UI", 10F, FontStyle.Bold),

                ForeColor =
                    Color.FromArgb(51, 65, 85)
            };
        }

        private void ConfigureButton(
            Button button,
            string text,
            Color color)
        {
            button.Text = text;
            button.BackColor = color;
            button.ForeColor = Color.White;

            button.Font = new Font(
                "Segoe UI", 10F, FontStyle.Bold);

            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Cursor = Cursors.Hand;
            button.UseVisualStyleBackColor = false;
        }
    }
}
