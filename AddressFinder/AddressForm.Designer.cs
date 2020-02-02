using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;


namespace FormValidator
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.textBoxStreetName1 = new System.Windows.Forms.TextBox();
            this.textBoxHouseNumber = new System.Windows.Forms.TextBox();
            this.textBoxCity = new System.Windows.Forms.TextBox();
            this.textBoxPostalCode = new System.Windows.Forms.TextBox();
            this.labelStreetName = new System.Windows.Forms.Label();
            this.labelHouseNumber = new System.Windows.Forms.Label();
            this.labelCity = new System.Windows.Forms.Label();
            this.labelPostalCode = new System.Windows.Forms.Label();
            this.labelCountry = new System.Windows.Forms.Label();
            this.progressBarSearch = new System.Windows.Forms.ProgressBar();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.comboBoxSearch = new System.Windows.Forms.ComboBox();
            this.textBoxCountry1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize) (this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxStreetName1
            // 
            resources.ApplyResources(this.textBoxStreetName1, "textBoxStreetName1");
            this.textBoxStreetName1.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (0)))),
                ((int) (((byte) (0)))), ((int) (((byte) (64)))));
            this.textBoxStreetName1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxStreetName1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.errorProvider1.SetError(this.textBoxStreetName1, resources.GetString("textBoxStreetName1.Error"));
            this.textBoxStreetName1.ForeColor = System.Drawing.Color.White;
            this.errorProvider1.SetIconAlignment(this.textBoxStreetName1,
                ((System.Windows.Forms.ErrorIconAlignment) (resources.GetObject("textBoxStreetName1.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.textBoxStreetName1,
                ((int) (resources.GetObject("textBoxStreetName1.IconPadding"))));
            this.textBoxStreetName1.Name = "textBoxStreetName1";
            this.textBoxStreetName1.ReadOnly = true;
            // 
            // textBoxHouseNumber
            // 
            resources.ApplyResources(this.textBoxHouseNumber, "textBoxHouseNumber");
            this.textBoxHouseNumber.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (0)))),
                ((int) (((byte) (0)))), ((int) (((byte) (64)))));
            this.textBoxHouseNumber.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxHouseNumber.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.errorProvider1.SetError(this.textBoxHouseNumber, resources.GetString("textBoxHouseNumber.Error"));
            this.textBoxHouseNumber.ForeColor = System.Drawing.Color.White;
            this.errorProvider1.SetIconAlignment(this.textBoxHouseNumber,
                ((System.Windows.Forms.ErrorIconAlignment) (resources.GetObject("textBoxHouseNumber.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.textBoxHouseNumber,
                ((int) (resources.GetObject("textBoxHouseNumber.IconPadding"))));
            this.textBoxHouseNumber.Name = "textBoxHouseNumber";
            this.textBoxHouseNumber.ReadOnly = true;
            // 
            // textBoxCity
            // 
            resources.ApplyResources(this.textBoxCity, "textBoxCity");
            this.textBoxCity.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (0)))), ((int) (((byte) (0)))),
                ((int) (((byte) (64)))));
            this.textBoxCity.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxCity.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.errorProvider1.SetError(this.textBoxCity, resources.GetString("textBoxCity.Error"));
            this.textBoxCity.ForeColor = System.Drawing.Color.White;
            this.errorProvider1.SetIconAlignment(this.textBoxCity,
                ((System.Windows.Forms.ErrorIconAlignment) (resources.GetObject("textBoxCity.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.textBoxCity,
                ((int) (resources.GetObject("textBoxCity.IconPadding"))));
            this.textBoxCity.Name = "textBoxCity";
            this.textBoxCity.ReadOnly = true;
            // 
            // textBoxPostalCode
            // 
            resources.ApplyResources(this.textBoxPostalCode, "textBoxPostalCode");
            this.textBoxPostalCode.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (0)))),
                ((int) (((byte) (0)))), ((int) (((byte) (64)))));
            this.textBoxPostalCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxPostalCode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.errorProvider1.SetError(this.textBoxPostalCode, resources.GetString("textBoxPostalCode.Error"));
            this.textBoxPostalCode.ForeColor = System.Drawing.Color.White;
            this.errorProvider1.SetIconAlignment(this.textBoxPostalCode,
                ((System.Windows.Forms.ErrorIconAlignment) (resources.GetObject("textBoxPostalCode.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.textBoxPostalCode,
                ((int) (resources.GetObject("textBoxPostalCode.IconPadding"))));
            this.textBoxPostalCode.Name = "textBoxPostalCode";
            this.textBoxPostalCode.ReadOnly = true;
            // 
            // labelStreetName
            // 
            resources.ApplyResources(this.labelStreetName, "labelStreetName");
            this.labelStreetName.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.errorProvider1.SetError(this.labelStreetName, resources.GetString("labelStreetName.Error"));
            this.labelStreetName.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.errorProvider1.SetIconAlignment(this.labelStreetName,
                ((System.Windows.Forms.ErrorIconAlignment) (resources.GetObject("labelStreetName.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.labelStreetName,
                ((int) (resources.GetObject("labelStreetName.IconPadding"))));
            this.labelStreetName.Name = "labelStreetName";
            // 
            // labelHouseNumber
            // 
            resources.ApplyResources(this.labelHouseNumber, "labelHouseNumber");
            this.labelHouseNumber.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.errorProvider1.SetError(this.labelHouseNumber, resources.GetString("labelHouseNumber.Error"));
            this.labelHouseNumber.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.errorProvider1.SetIconAlignment(this.labelHouseNumber,
                ((System.Windows.Forms.ErrorIconAlignment) (resources.GetObject("labelHouseNumber.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.labelHouseNumber,
                ((int) (resources.GetObject("labelHouseNumber.IconPadding"))));
            this.labelHouseNumber.Name = "labelHouseNumber";
            // 
            // labelCity
            // 
            resources.ApplyResources(this.labelCity, "labelCity");
            this.labelCity.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.errorProvider1.SetError(this.labelCity, resources.GetString("labelCity.Error"));
            this.labelCity.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.errorProvider1.SetIconAlignment(this.labelCity,
                ((System.Windows.Forms.ErrorIconAlignment) (resources.GetObject("labelCity.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.labelCity, ((int) (resources.GetObject("labelCity.IconPadding"))));
            this.labelCity.Name = "labelCity";
            // 
            // labelPostalCode
            // 
            resources.ApplyResources(this.labelPostalCode, "labelPostalCode");
            this.labelPostalCode.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.errorProvider1.SetError(this.labelPostalCode, resources.GetString("labelPostalCode.Error"));
            this.labelPostalCode.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.errorProvider1.SetIconAlignment(this.labelPostalCode,
                ((System.Windows.Forms.ErrorIconAlignment) (resources.GetObject("labelPostalCode.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.labelPostalCode,
                ((int) (resources.GetObject("labelPostalCode.IconPadding"))));
            this.labelPostalCode.Name = "labelPostalCode";
            // 
            // labelCountry
            // 
            resources.ApplyResources(this.labelCountry, "labelCountry");
            this.labelCountry.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.errorProvider1.SetError(this.labelCountry, resources.GetString("labelCountry.Error"));
            this.labelCountry.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.errorProvider1.SetIconAlignment(this.labelCountry,
                ((System.Windows.Forms.ErrorIconAlignment) (resources.GetObject("labelCountry.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.labelCountry,
                ((int) (resources.GetObject("labelCountry.IconPadding"))));
            this.labelCountry.Name = "labelCountry";
            // 
            // progressBarSearch
            // 
            resources.ApplyResources(this.progressBarSearch, "progressBarSearch");
            this.progressBarSearch.BackColor = System.Drawing.Color.Gray;
            this.progressBarSearch.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.progressBarSearch.ForeColor = System.Drawing.Color.Navy;
            this.progressBarSearch.Name = "progressBarSearch";
            this.progressBarSearch.Style = ProgressBarStyle.Marquee;
            this.progressBarSearch.MarqueeAnimationSpeed = 5;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            resources.ApplyResources(this.errorProvider1, "errorProvider1");
            // 
            // comboBoxSearch
            // 
            resources.ApplyResources(this.comboBoxSearch, "comboBoxSearch");
            this.comboBoxSearch.AllowDrop = true;
            this.comboBoxSearch.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.comboBoxSearch.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (0)))),
                ((int) (((byte) (0)))), ((int) (((byte) (64)))));
            this.comboBoxSearch.ForeColor = System.Drawing.Color.Lavender;
            this.comboBoxSearch.FormattingEnabled = true;
            this.comboBoxSearch.Name = "comboBoxSearch";
            this.comboBoxSearch.SelectedIndexChanged += new System.EventHandler(this.SetAddress);
            this.comboBoxSearch.TextChanged += new System.EventHandler(this.RunSearchRequest);
            this.comboBoxSearch.Location = new Point(15, 7);
            this.comboBoxSearch.Size = new Size(355, 23);
            this.comboBoxSearch.Items.Insert(0, "Search...");
            this.comboBoxSearch.SelectedIndex = 0;
            // 
            // textBoxCountry1
            // 
            resources.ApplyResources(this.textBoxCountry1, "textBoxCountry1");
            this.textBoxCountry1.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (0)))),
                ((int) (((byte) (0)))), ((int) (((byte) (64)))));
            this.textBoxCountry1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxCountry1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.errorProvider1.SetError(this.textBoxCountry1, resources.GetString("textBoxCountry1.Error"));
            this.textBoxCountry1.ForeColor = System.Drawing.Color.White;
            this.errorProvider1.SetIconAlignment(this.textBoxCountry1,
                ((System.Windows.Forms.ErrorIconAlignment) (resources.GetObject("textBoxCountry1.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.textBoxCountry1,
                ((int) (resources.GetObject("textBoxCountry1.IconPadding"))));
            this.textBoxCountry1.Name = "textBoxCountry1";
            this.textBoxCountry1.ReadOnly = true;
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (0)))), ((int) (((byte) (0)))),
                ((int) (((byte) (64)))));
            this.Controls.Add(this.textBoxCountry1);
            this.Controls.Add(this.comboBoxSearch);
            this.Controls.Add(this.labelCountry);
            this.Controls.Add(this.progressBarSearch);
            this.Controls.Add(this.textBoxPostalCode);
            this.Controls.Add(this.labelPostalCode);
            this.Controls.Add(this.labelCity);
            this.Controls.Add(this.labelHouseNumber);
            this.Controls.Add(this.textBoxCity);
            this.Controls.Add(this.textBoxHouseNumber);
            this.Controls.Add(this.textBoxStreetName1);
            this.Controls.Add(this.labelStreetName);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Size = new Size(400, 240);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            ((System.ComponentModel.ISupportInitialize) (this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label labelStreetName;
        private System.Windows.Forms.TextBox textBoxStreetName1;
        private System.Windows.Forms.TextBox textBoxHouseNumber;
        private System.Windows.Forms.TextBox textBoxCity;
        private System.Windows.Forms.Label labelHouseNumber;
        private System.Windows.Forms.Label labelCity;
        private System.Windows.Forms.Label labelPostalCode;
        private System.Windows.Forms.TextBox textBoxPostalCode;
        private System.Windows.Forms.ProgressBar progressBarSearch;
        private System.Windows.Forms.Label labelCountry;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.TextBox textBoxCountry1;
        private System.Windows.Forms.ComboBox comboBoxSearch;
    }
}

