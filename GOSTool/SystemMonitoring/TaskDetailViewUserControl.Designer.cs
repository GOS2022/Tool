
namespace GOSTool
{
    partial class TaskDetailViewUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel3 = new System.Windows.Forms.Panel();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cpuUtilLabel = new System.Windows.Forms.Label();
            this.avgCpuUtil = new System.Windows.Forms.Label();
            this.cpuLoadGraph = new GOSTool.LoadGraphUserControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.stackUtilLabel = new System.Windows.Forms.Label();
            this.stackLoadGraph = new GOSTool.LoadGraphUserControl();
            this.listView1 = new System.Windows.Forms.ListView();
            this.paramCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.valCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listView1);
            this.splitContainer1.Size = new System.Drawing.Size(599, 657);
            this.splitContainer1.SplitterDistance = 394;
            this.splitContainer1.TabIndex = 14;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.splitContainer2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(599, 394);
            this.panel3.TabIndex = 9;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.panel2);
            this.splitContainer2.Size = new System.Drawing.Size(599, 394);
            this.splitContainer2.SplitterDistance = 190;
            this.splitContainer2.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.cpuUtilLabel);
            this.panel1.Controls.Add(this.avgCpuUtil);
            this.panel1.Controls.Add(this.cpuLoadGraph);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(599, 190);
            this.panel1.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "CPU utilization";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(198, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(156, 17);
            this.label4.TabIndex = 12;
            this.label4.Text = "Average CPU utilization";
            // 
            // cpuUtilLabel
            // 
            this.cpuUtilLabel.AutoSize = true;
            this.cpuUtilLabel.Location = new System.Drawing.Point(108, 13);
            this.cpuUtilLabel.Name = "cpuUtilLabel";
            this.cpuUtilLabel.Size = new System.Drawing.Size(47, 17);
            this.cpuUtilLabel.TabIndex = 9;
            this.cpuUtilLabel.Text = "N/A %";
            // 
            // avgCpuUtil
            // 
            this.avgCpuUtil.AutoSize = true;
            this.avgCpuUtil.Location = new System.Drawing.Point(360, 13);
            this.avgCpuUtil.Name = "avgCpuUtil";
            this.avgCpuUtil.Size = new System.Drawing.Size(47, 17);
            this.avgCpuUtil.TabIndex = 13;
            this.avgCpuUtil.Text = "N/A %";
            // 
            // cpuLoadGraph
            // 
            this.cpuLoadGraph.BackColor = System.Drawing.Color.Black;
            this.cpuLoadGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cpuLoadGraph.Location = new System.Drawing.Point(0, 0);
            this.cpuLoadGraph.Name = "cpuLoadGraph";
            this.cpuLoadGraph.NumberOfSamples = 200;
            this.cpuLoadGraph.Size = new System.Drawing.Size(599, 190);
            this.cpuLoadGraph.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.stackUtilLabel);
            this.panel2.Controls.Add(this.stackLoadGraph);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(599, 200);
            this.panel2.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 17);
            this.label2.TabIndex = 10;
            this.label2.Text = "Stack utilization";
            // 
            // stackUtilLabel
            // 
            this.stackUtilLabel.AutoSize = true;
            this.stackUtilLabel.Location = new System.Drawing.Point(115, 9);
            this.stackUtilLabel.Name = "stackUtilLabel";
            this.stackUtilLabel.Size = new System.Drawing.Size(47, 17);
            this.stackUtilLabel.TabIndex = 11;
            this.stackUtilLabel.Text = "N/A %";
            // 
            // stackLoadGraph
            // 
            this.stackLoadGraph.BackColor = System.Drawing.Color.Black;
            this.stackLoadGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stackLoadGraph.Location = new System.Drawing.Point(0, 0);
            this.stackLoadGraph.Name = "stackLoadGraph";
            this.stackLoadGraph.NumberOfSamples = 200;
            this.stackLoadGraph.Size = new System.Drawing.Size(599, 200);
            this.stackLoadGraph.TabIndex = 1;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.paramCol,
            this.valCol});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(599, 259);
            this.listView1.TabIndex = 7;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // paramCol
            // 
            this.paramCol.Text = "Parameter";
            this.paramCol.Width = 100;
            // 
            // valCol
            // 
            this.valCol.Text = "Value";
            this.valCol.Width = 200;
            // 
            // TaskDetailViewUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "TaskDetailViewUserControl";
            this.Size = new System.Drawing.Size(599, 657);
            this.Load += new System.EventHandler(this.TaskDetailViewUserControl_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label stackUtilLabel;
        private LoadGraphUserControl stackLoadGraph;
        private System.Windows.Forms.Label avgCpuUtil;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label cpuUtilLabel;
        private System.Windows.Forms.Label label4;
        private LoadGraphUserControl cpuLoadGraph;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader paramCol;
        private System.Windows.Forms.ColumnHeader valCol;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.SplitContainer splitContainer2;
    }
}
