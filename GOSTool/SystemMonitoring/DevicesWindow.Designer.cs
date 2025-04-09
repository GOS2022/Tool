namespace GOSTool.SystemMonitoring
{
    partial class DevicesWindow
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.deviceTree = new System.Windows.Forms.TreeView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.wirelessConfigUserControl1 = new GOSTool.SystemMonitoring.WirelessConfigUserControl();
            this.usbConfigUserControl1 = new GOSTool.UsbConfigUserControl();
            this.label1 = new System.Windows.Forms.Label();
            this.wirelessComRadioButton = new System.Windows.Forms.RadioButton();
            this.usbComRadioButton = new System.Windows.Forms.RadioButton();
            this.connectButton = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.deviceTree);
            this.groupBox3.Location = new System.Drawing.Point(332, 157);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Size = new System.Drawing.Size(459, 322);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Device list";
            // 
            // deviceTree
            // 
            this.deviceTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.deviceTree.Location = new System.Drawing.Point(3, 17);
            this.deviceTree.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.deviceTree.Name = "deviceTree";
            this.deviceTree.Size = new System.Drawing.Size(453, 303);
            this.deviceTree.TabIndex = 9;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.wirelessConfigUserControl1);
            this.groupBox2.Controls.Add(this.usbConfigUserControl1);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.wirelessComRadioButton);
            this.groupBox2.Controls.Add(this.usbComRadioButton);
            this.groupBox2.Location = new System.Drawing.Point(12, 11);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(779, 142);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Communication configuration";
            // 
            // wirelessConfigUserControl1
            // 
            this.wirelessConfigUserControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wirelessConfigUserControl1.Ip = "192.168.1.184";
            this.wirelessConfigUserControl1.Location = new System.Drawing.Point(5, 64);
            this.wirelessConfigUserControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.wirelessConfigUserControl1.Name = "wirelessConfigUserControl1";
            this.wirelessConfigUserControl1.Port = 3000;
            this.wirelessConfigUserControl1.Size = new System.Drawing.Size(765, 60);
            this.wirelessConfigUserControl1.TabIndex = 7;
            // 
            // usbConfigUserControl1
            // 
            this.usbConfigUserControl1.Baud = -2147483648;
            this.usbConfigUserControl1.Location = new System.Drawing.Point(5, 64);
            this.usbConfigUserControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.usbConfigUserControl1.Name = "usbConfigUserControl1";
            this.usbConfigUserControl1.Port = null;
            this.usbConfigUserControl1.Size = new System.Drawing.Size(469, 57);
            this.usbConfigUserControl1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Communication type";
            // 
            // wirelessComRadioButton
            // 
            this.wirelessComRadioButton.AutoSize = true;
            this.wirelessComRadioButton.Location = new System.Drawing.Point(252, 37);
            this.wirelessComRadioButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
            this.usbComRadioButton.Location = new System.Drawing.Point(179, 37);
            this.usbComRadioButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.usbComRadioButton.Name = "usbComRadioButton";
            this.usbComRadioButton.Size = new System.Drawing.Size(56, 20);
            this.usbComRadioButton.TabIndex = 0;
            this.usbComRadioButton.TabStop = true;
            this.usbComRadioButton.Text = "USB";
            this.usbComRadioButton.UseVisualStyleBackColor = true;
            this.usbComRadioButton.CheckedChanged += new System.EventHandler(this.usbComRadioButton_CheckedChanged);
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(12, 174);
            this.connectButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(144, 38);
            this.connectButton.TabIndex = 15;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // DevicesWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 490);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Name = "DevicesWindow";
            this.Text = "Devices";
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TreeView deviceTree;
        private System.Windows.Forms.GroupBox groupBox2;
        private WirelessConfigUserControl wirelessConfigUserControl1;
        private UsbConfigUserControl usbConfigUserControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton wirelessComRadioButton;
        private System.Windows.Forms.RadioButton usbComRadioButton;
        private System.Windows.Forms.Button connectButton;
    }
}