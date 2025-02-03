namespace GOSTool.SystemMonitoring
{
    partial class MonitoringDataWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MonitoringDataWindow));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pollPeriodNUD = new System.Windows.Forms.NumericUpDown();
            this.connectButton = new System.Windows.Forms.Button();
            this.usbConfigUserControl1 = new GOSTool.UsbConfigUserControl();
            this.label1 = new System.Windows.Forms.Label();
            this.wirelessComRadioButton = new System.Windows.Forms.RadioButton();
            this.usbComRadioButton = new System.Windows.Forms.RadioButton();
            this.wirelessConfigUserControl1 = new GOSTool.SystemMonitoring.WirelessConfigUserControl();
            this.mdiListView = new System.Windows.Forms.ListView();
            this.nameCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.typeCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.valCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pollPeriodNUD)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.pollPeriodNUD);
            this.groupBox2.Controls.Add(this.connectButton);
            this.groupBox2.Controls.Add(this.usbConfigUserControl1);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.wirelessComRadioButton);
            this.groupBox2.Controls.Add(this.usbComRadioButton);
            this.groupBox2.Controls.Add(this.wirelessConfigUserControl1);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(784, 176);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Communication configuration";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(302, 141);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 16);
            this.label2.TabIndex = 8;
            this.label2.Text = "Poll period [ms]";
            // 
            // pollPeriodNUD
            // 
            this.pollPeriodNUD.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.pollPeriodNUD.Location = new System.Drawing.Point(418, 135);
            this.pollPeriodNUD.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.pollPeriodNUD.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.pollPeriodNUD.Name = "pollPeriodNUD";
            this.pollPeriodNUD.Size = new System.Drawing.Size(120, 22);
            this.pollPeriodNUD.TabIndex = 7;
            this.pollPeriodNUD.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(25, 129);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(102, 32);
            this.connectButton.TabIndex = 4;
            this.connectButton.Text = "Monitor";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // usbConfigUserControl1
            // 
            this.usbConfigUserControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.usbConfigUserControl1.Baud = -2147483648;
            this.usbConfigUserControl1.Location = new System.Drawing.Point(6, 64);
            this.usbConfigUserControl1.Name = "usbConfigUserControl1";
            this.usbConfigUserControl1.Port = null;
            this.usbConfigUserControl1.Size = new System.Drawing.Size(772, 56);
            this.usbConfigUserControl1.TabIndex = 3;
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
            // wirelessConfigUserControl1
            // 
            this.wirelessConfigUserControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wirelessConfigUserControl1.Ip = "192.168.1.184";
            this.wirelessConfigUserControl1.Location = new System.Drawing.Point(6, 63);
            this.wirelessConfigUserControl1.Name = "wirelessConfigUserControl1";
            this.wirelessConfigUserControl1.Port = 3000;
            this.wirelessConfigUserControl1.Size = new System.Drawing.Size(772, 60);
            this.wirelessConfigUserControl1.TabIndex = 6;
            // 
            // mdiListView
            // 
            this.mdiListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mdiListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameCol,
            this.typeCol,
            this.valCol});
            this.mdiListView.HideSelection = false;
            this.mdiListView.Location = new System.Drawing.Point(12, 196);
            this.mdiListView.Name = "mdiListView";
            this.mdiListView.Size = new System.Drawing.Size(784, 196);
            this.mdiListView.TabIndex = 4;
            this.mdiListView.UseCompatibleStateImageBehavior = false;
            this.mdiListView.View = System.Windows.Forms.View.Details;
            // 
            // nameCol
            // 
            this.nameCol.Text = "Name";
            this.nameCol.Width = 150;
            // 
            // typeCol
            // 
            this.typeCol.Text = "Type";
            this.typeCol.Width = 80;
            // 
            // valCol
            // 
            this.valCol.Text = "Value";
            this.valCol.Width = 200;
            // 
            // MonitoringDataWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(808, 404);
            this.Controls.Add(this.mdiListView);
            this.Controls.Add(this.groupBox2);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MonitoringDataWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Monitoring Data Window";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MonitoringDataWindow_FormClosing);
            this.Load += new System.EventHandler(this.MonitoringDataWindow_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pollPeriodNUD)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private WirelessConfigUserControl wirelessConfigUserControl1;
        private System.Windows.Forms.Button connectButton;
        private UsbConfigUserControl usbConfigUserControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton wirelessComRadioButton;
        private System.Windows.Forms.RadioButton usbComRadioButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown pollPeriodNUD;
        private System.Windows.Forms.ListView mdiListView;
        private System.Windows.Forms.ColumnHeader nameCol;
        private System.Windows.Forms.ColumnHeader typeCol;
        private System.Windows.Forms.ColumnHeader valCol;
    }
}