
namespace GOSTool.Test
{
    partial class ManageTests
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
            this.editTestButton = new System.Windows.Forms.Button();
            this.logTB = new System.Windows.Forms.RichTextBox();
            this.runTestButton = new System.Windows.Forms.Button();
            this.saveTestButton = new System.Windows.Forms.Button();
            this.deleteStepButton = new System.Windows.Forms.Button();
            this.newTestButton = new System.Windows.Forms.Button();
            this.testsListView = new System.Windows.Forms.ListView();
            this.testPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.label2 = new System.Windows.Forms.Label();
            this.testNameTB = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // editTestButton
            // 
            this.editTestButton.Location = new System.Drawing.Point(411, 87);
            this.editTestButton.Name = "editTestButton";
            this.editTestButton.Size = new System.Drawing.Size(98, 30);
            this.editTestButton.TabIndex = 58;
            this.editTestButton.Text = "Edit test";
            this.editTestButton.UseVisualStyleBackColor = true;
            this.editTestButton.Click += new System.EventHandler(this.editTestButton_Click);
            // 
            // logTB
            // 
            this.logTB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logTB.BackColor = System.Drawing.Color.Black;
            this.logTB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.logTB.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logTB.ForeColor = System.Drawing.Color.White;
            this.logTB.Location = new System.Drawing.Point(549, 363);
            this.logTB.Name = "logTB";
            this.logTB.Size = new System.Drawing.Size(318, 161);
            this.logTB.TabIndex = 57;
            this.logTB.Text = "";
            // 
            // runTestButton
            // 
            this.runTestButton.Location = new System.Drawing.Point(411, 159);
            this.runTestButton.Name = "runTestButton";
            this.runTestButton.Size = new System.Drawing.Size(98, 30);
            this.runTestButton.TabIndex = 56;
            this.runTestButton.Text = "Run test";
            this.runTestButton.UseVisualStyleBackColor = true;
            this.runTestButton.Click += new System.EventHandler(this.runTestButton_Click);
            // 
            // saveTestButton
            // 
            this.saveTestButton.Location = new System.Drawing.Point(411, 123);
            this.saveTestButton.Name = "saveTestButton";
            this.saveTestButton.Size = new System.Drawing.Size(98, 30);
            this.saveTestButton.TabIndex = 54;
            this.saveTestButton.Text = "Save test";
            this.saveTestButton.UseVisualStyleBackColor = true;
            // 
            // deleteStepButton
            // 
            this.deleteStepButton.Location = new System.Drawing.Point(411, 51);
            this.deleteStepButton.Name = "deleteStepButton";
            this.deleteStepButton.Size = new System.Drawing.Size(98, 30);
            this.deleteStepButton.TabIndex = 53;
            this.deleteStepButton.Text = "Delete test";
            this.deleteStepButton.UseVisualStyleBackColor = true;
            // 
            // newTestButton
            // 
            this.newTestButton.Location = new System.Drawing.Point(411, 13);
            this.newTestButton.Name = "newTestButton";
            this.newTestButton.Size = new System.Drawing.Size(98, 30);
            this.newTestButton.TabIndex = 52;
            this.newTestButton.Text = "New test";
            this.newTestButton.UseVisualStyleBackColor = true;
            this.newTestButton.Click += new System.EventHandler(this.newTestButton_Click);
            // 
            // testsListView
            // 
            this.testsListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.testsListView.FullRowSelect = true;
            this.testsListView.GridLines = true;
            this.testsListView.HideSelection = false;
            this.testsListView.Location = new System.Drawing.Point(549, 13);
            this.testsListView.MultiSelect = false;
            this.testsListView.Name = "testsListView";
            this.testsListView.ShowGroups = false;
            this.testsListView.Size = new System.Drawing.Size(318, 343);
            this.testsListView.TabIndex = 51;
            this.testsListView.UseCompatibleStateImageBehavior = false;
            this.testsListView.View = System.Windows.Forms.View.List;
            this.testsListView.SelectedIndexChanged += new System.EventHandler(this.testsListView_SelectedIndexChanged);
            // 
            // testPropertyGrid
            // 
            this.testPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.testPropertyGrid.HelpVisible = false;
            this.testPropertyGrid.Location = new System.Drawing.Point(13, 51);
            this.testPropertyGrid.Name = "testPropertyGrid";
            this.testPropertyGrid.Size = new System.Drawing.Size(361, 473);
            this.testPropertyGrid.TabIndex = 48;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 17);
            this.label2.TabIndex = 49;
            this.label2.Text = "Test name:";
            // 
            // testNameTB
            // 
            this.testNameTB.Location = new System.Drawing.Point(188, 13);
            this.testNameTB.Name = "testNameTB";
            this.testNameTB.Size = new System.Drawing.Size(186, 22);
            this.testNameTB.TabIndex = 50;
            // 
            // ManageTests
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(880, 536);
            this.Controls.Add(this.editTestButton);
            this.Controls.Add(this.logTB);
            this.Controls.Add(this.runTestButton);
            this.Controls.Add(this.saveTestButton);
            this.Controls.Add(this.deleteStepButton);
            this.Controls.Add(this.newTestButton);
            this.Controls.Add(this.testsListView);
            this.Controls.Add(this.testNameTB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.testPropertyGrid);
            this.Name = "ManageTests";
            this.Text = "Manage tests";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button editTestButton;
        private System.Windows.Forms.RichTextBox logTB;
        private System.Windows.Forms.Button runTestButton;
        private System.Windows.Forms.Button saveTestButton;
        private System.Windows.Forms.Button deleteStepButton;
        private System.Windows.Forms.Button newTestButton;
        private System.Windows.Forms.ListView testsListView;
        private System.Windows.Forms.PropertyGrid testPropertyGrid;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox testNameTB;
    }
}