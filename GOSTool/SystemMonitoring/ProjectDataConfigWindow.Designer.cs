namespace GOSTool.SystemMonitoring
{
    partial class ProjectDataConfigWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.softwareInfoPage = new System.Windows.Forms.TabPage();
            this.swInfoGridView = new System.Windows.Forms.DataGridView();
            this.paramCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hardwareInfoPage = new System.Windows.Forms.TabPage();
            this.hwInfoGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bldCfgPage = new System.Windows.Forms.TabPage();
            this.bldCfgGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wifiCfgPage = new System.Windows.Forms.TabPage();
            this.wifiCfgGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.writeButton = new System.Windows.Forms.Button();
            this.readButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.wirelessComRadioButton = new System.Windows.Forms.RadioButton();
            this.usbComRadioButton = new System.Windows.Forms.RadioButton();
            this.usbConfigUserControl1 = new GOSTool.UsbConfigUserControl();
            this.wirelessConfigUserControl1 = new GOSTool.SystemMonitoring.WirelessConfigUserControl();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.softwareInfoPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.swInfoGridView)).BeginInit();
            this.hardwareInfoPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hwInfoGridView)).BeginInit();
            this.bldCfgPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bldCfgGridView)).BeginInit();
            this.wifiCfgPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.wifiCfgGridView)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.tabControl1);
            this.groupBox1.Location = new System.Drawing.Point(4, 185);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(757, 326);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parameters";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.softwareInfoPage);
            this.tabControl1.Controls.Add(this.hardwareInfoPage);
            this.tabControl1.Controls.Add(this.bldCfgPage);
            this.tabControl1.Controls.Add(this.wifiCfgPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 18);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(751, 305);
            this.tabControl1.TabIndex = 6;
            // 
            // softwareInfoPage
            // 
            this.softwareInfoPage.Controls.Add(this.swInfoGridView);
            this.softwareInfoPage.Location = new System.Drawing.Point(4, 25);
            this.softwareInfoPage.Name = "softwareInfoPage";
            this.softwareInfoPage.Padding = new System.Windows.Forms.Padding(3);
            this.softwareInfoPage.Size = new System.Drawing.Size(743, 276);
            this.softwareInfoPage.TabIndex = 0;
            this.softwareInfoPage.Text = "Software Info";
            this.softwareInfoPage.UseVisualStyleBackColor = true;
            // 
            // swInfoGridView
            // 
            this.swInfoGridView.AllowUserToAddRows = false;
            this.swInfoGridView.AllowUserToDeleteRows = false;
            this.swInfoGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.swInfoGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.paramCol,
            this.valCol});
            this.swInfoGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.swInfoGridView.Location = new System.Drawing.Point(3, 3);
            this.swInfoGridView.Name = "swInfoGridView";
            this.swInfoGridView.RowHeadersWidth = 51;
            this.swInfoGridView.RowTemplate.Height = 24;
            this.swInfoGridView.Size = new System.Drawing.Size(737, 270);
            this.swInfoGridView.TabIndex = 0;
            // 
            // paramCol
            // 
            this.paramCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.paramCol.HeaderText = "Parameter";
            this.paramCol.MinimumWidth = 6;
            this.paramCol.Name = "paramCol";
            this.paramCol.ReadOnly = true;
            this.paramCol.Width = 99;
            // 
            // valCol
            // 
            this.valCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.valCol.HeaderText = "Value";
            this.valCol.MinimumWidth = 6;
            this.valCol.Name = "valCol";
            // 
            // hardwareInfoPage
            // 
            this.hardwareInfoPage.Controls.Add(this.hwInfoGridView);
            this.hardwareInfoPage.Location = new System.Drawing.Point(4, 25);
            this.hardwareInfoPage.Name = "hardwareInfoPage";
            this.hardwareInfoPage.Size = new System.Drawing.Size(743, 276);
            this.hardwareInfoPage.TabIndex = 1;
            this.hardwareInfoPage.Text = "Hardware Info";
            this.hardwareInfoPage.UseVisualStyleBackColor = true;
            // 
            // hwInfoGridView
            // 
            this.hwInfoGridView.AllowUserToAddRows = false;
            this.hwInfoGridView.AllowUserToDeleteRows = false;
            this.hwInfoGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.hwInfoGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.hwInfoGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hwInfoGridView.Location = new System.Drawing.Point(0, 0);
            this.hwInfoGridView.Name = "hwInfoGridView";
            this.hwInfoGridView.RowHeadersWidth = 51;
            this.hwInfoGridView.RowTemplate.Height = 24;
            this.hwInfoGridView.Size = new System.Drawing.Size(743, 276);
            this.hwInfoGridView.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewTextBoxColumn1.HeaderText = "Parameter";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 99;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "Value";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // bldCfgPage
            // 
            this.bldCfgPage.Controls.Add(this.bldCfgGridView);
            this.bldCfgPage.Location = new System.Drawing.Point(4, 25);
            this.bldCfgPage.Name = "bldCfgPage";
            this.bldCfgPage.Size = new System.Drawing.Size(743, 276);
            this.bldCfgPage.TabIndex = 2;
            this.bldCfgPage.Text = "Bootloader configuration";
            this.bldCfgPage.UseVisualStyleBackColor = true;
            // 
            // bldCfgGridView
            // 
            this.bldCfgGridView.AllowUserToAddRows = false;
            this.bldCfgGridView.AllowUserToDeleteRows = false;
            this.bldCfgGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.bldCfgGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.bldCfgGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bldCfgGridView.Location = new System.Drawing.Point(0, 0);
            this.bldCfgGridView.Name = "bldCfgGridView";
            this.bldCfgGridView.RowHeadersWidth = 51;
            this.bldCfgGridView.RowTemplate.Height = 24;
            this.bldCfgGridView.Size = new System.Drawing.Size(743, 276);
            this.bldCfgGridView.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewTextBoxColumn3.HeaderText = "Parameter";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 99;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.HeaderText = "Value";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // wifiCfgPage
            // 
            this.wifiCfgPage.Controls.Add(this.wifiCfgGridView);
            this.wifiCfgPage.Location = new System.Drawing.Point(4, 25);
            this.wifiCfgPage.Name = "wifiCfgPage";
            this.wifiCfgPage.Size = new System.Drawing.Size(743, 276);
            this.wifiCfgPage.TabIndex = 3;
            this.wifiCfgPage.Text = "WiFi configuration";
            this.wifiCfgPage.UseVisualStyleBackColor = true;
            // 
            // wifiCfgGridView
            // 
            this.wifiCfgGridView.AllowUserToAddRows = false;
            this.wifiCfgGridView.AllowUserToDeleteRows = false;
            this.wifiCfgGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.wifiCfgGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6});
            this.wifiCfgGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wifiCfgGridView.Location = new System.Drawing.Point(0, 0);
            this.wifiCfgGridView.Name = "wifiCfgGridView";
            this.wifiCfgGridView.RowHeadersWidth = 51;
            this.wifiCfgGridView.RowTemplate.Height = 24;
            this.wifiCfgGridView.Size = new System.Drawing.Size(743, 276);
            this.wifiCfgGridView.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewTextBoxColumn5.HeaderText = "Parameter";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 99;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn6.HeaderText = "Value";
            this.dataGridViewTextBoxColumn6.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.writeButton);
            this.groupBox2.Controls.Add(this.readButton);
            this.groupBox2.Controls.Add(this.usbConfigUserControl1);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.wirelessComRadioButton);
            this.groupBox2.Controls.Add(this.usbComRadioButton);
            this.groupBox2.Controls.Add(this.wirelessConfigUserControl1);
            this.groupBox2.Location = new System.Drawing.Point(4, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(757, 176);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Communication configuration";
            // 
            // writeButton
            // 
            this.writeButton.Location = new System.Drawing.Point(145, 129);
            this.writeButton.Name = "writeButton";
            this.writeButton.Size = new System.Drawing.Size(102, 32);
            this.writeButton.TabIndex = 9;
            this.writeButton.Text = "Write";
            this.writeButton.UseVisualStyleBackColor = true;
            this.writeButton.Click += new System.EventHandler(this.writeButton_Click);
            // 
            // readButton
            // 
            this.readButton.Location = new System.Drawing.Point(25, 129);
            this.readButton.Name = "readButton";
            this.readButton.Size = new System.Drawing.Size(102, 32);
            this.readButton.TabIndex = 4;
            this.readButton.Text = "Read";
            this.readButton.UseVisualStyleBackColor = true;
            this.readButton.Click += new System.EventHandler(this.readButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Communication type";
            // 
            // wirelessComRadioButton
            // 
            this.wirelessComRadioButton.AutoSize = true;
            this.wirelessComRadioButton.Location = new System.Drawing.Point(252, 37);
            this.wirelessComRadioButton.Name = "wirelessComRadioButton";
            this.wirelessComRadioButton.Size = new System.Drawing.Size(81, 20);
            this.wirelessComRadioButton.TabIndex = 1;
            this.wirelessComRadioButton.Text = "Wireless";
            this.wirelessComRadioButton.UseVisualStyleBackColor = true;
            this.wirelessComRadioButton.CheckedChanged += new System.EventHandler(this.wirelessComRadioButton_CheckedChanged);
            // 
            // usbComRadioButton
            // 
            this.usbComRadioButton.AutoSize = true;
            this.usbComRadioButton.Checked = true;
            this.usbComRadioButton.Location = new System.Drawing.Point(178, 37);
            this.usbComRadioButton.Name = "usbComRadioButton";
            this.usbComRadioButton.Size = new System.Drawing.Size(56, 20);
            this.usbComRadioButton.TabIndex = 0;
            this.usbComRadioButton.TabStop = true;
            this.usbComRadioButton.Text = "USB";
            this.usbComRadioButton.UseVisualStyleBackColor = true;
            this.usbComRadioButton.CheckedChanged += new System.EventHandler(this.usbComRadioButton_CheckedChanged);
            // 
            // usbConfigUserControl1
            // 
            this.usbConfigUserControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.usbConfigUserControl1.Baud = -2147483648;
            this.usbConfigUserControl1.Location = new System.Drawing.Point(6, 64);
            this.usbConfigUserControl1.Name = "usbConfigUserControl1";
            this.usbConfigUserControl1.Port = null;
            this.usbConfigUserControl1.Size = new System.Drawing.Size(706, 56);
            this.usbConfigUserControl1.TabIndex = 3;
            // 
            // wirelessConfigUserControl1
            // 
            this.wirelessConfigUserControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wirelessConfigUserControl1.Ip = "192.168.1.184";
            this.wirelessConfigUserControl1.Location = new System.Drawing.Point(6, 63);
            this.wirelessConfigUserControl1.Name = "wirelessConfigUserControl1";
            this.wirelessConfigUserControl1.Port = 3000;
            this.wirelessConfigUserControl1.Size = new System.Drawing.Size(706, 60);
            this.wirelessConfigUserControl1.TabIndex = 6;
            // 
            // ProjectDataConfigWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 517);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "ProjectDataConfigWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Project data and configuration setting";
            this.Load += new System.EventHandler(this.ProjectDataConfigWindow_Load);
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.softwareInfoPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.swInfoGridView)).EndInit();
            this.hardwareInfoPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.hwInfoGridView)).EndInit();
            this.bldCfgPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bldCfgGridView)).EndInit();
            this.wifiCfgPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.wifiCfgGridView)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage softwareInfoPage;
        private System.Windows.Forms.DataGridView swInfoGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn paramCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn valCol;
        private System.Windows.Forms.TabPage bldCfgPage;
        private System.Windows.Forms.TabPage wifiCfgPage;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button writeButton;
        private System.Windows.Forms.Button readButton;
        private UsbConfigUserControl usbConfigUserControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton wirelessComRadioButton;
        private System.Windows.Forms.RadioButton usbComRadioButton;
        private WirelessConfigUserControl wirelessConfigUserControl1;
        private System.Windows.Forms.TabPage hardwareInfoPage;
        private System.Windows.Forms.DataGridView hwInfoGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridView bldCfgGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridView wifiCfgGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
    }
}