
namespace GOSTool.Test
{
    partial class EditTest
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
            this.moveDownButton = new System.Windows.Forms.Button();
            this.moveUpButton = new System.Windows.Forms.Button();
            this.runCaseButton = new System.Windows.Forms.Button();
            this.logTB = new System.Windows.Forms.RichTextBox();
            this.runTestButton = new System.Windows.Forms.Button();
            this.deleteTestButton = new System.Windows.Forms.Button();
            this.saveTestButton = new System.Windows.Forms.Button();
            this.deleteStepButton = new System.Windows.Forms.Button();
            this.addCaseButton = new System.Windows.Forms.Button();
            this.testCaseListView = new System.Windows.Forms.ListView();
            this.testCasePropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.testCaseCB = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.editTestCasesButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // moveDownButton
            // 
            this.moveDownButton.Location = new System.Drawing.Point(411, 191);
            this.moveDownButton.Name = "moveDownButton";
            this.moveDownButton.Size = new System.Drawing.Size(98, 30);
            this.moveDownButton.TabIndex = 30;
            this.moveDownButton.Text = "Move down";
            this.moveDownButton.UseVisualStyleBackColor = true;
            // 
            // moveUpButton
            // 
            this.moveUpButton.Location = new System.Drawing.Point(411, 155);
            this.moveUpButton.Name = "moveUpButton";
            this.moveUpButton.Size = new System.Drawing.Size(98, 30);
            this.moveUpButton.TabIndex = 29;
            this.moveUpButton.Text = "Move up";
            this.moveUpButton.UseVisualStyleBackColor = true;
            // 
            // runCaseButton
            // 
            this.runCaseButton.Location = new System.Drawing.Point(411, 89);
            this.runCaseButton.Name = "runCaseButton";
            this.runCaseButton.Size = new System.Drawing.Size(98, 30);
            this.runCaseButton.TabIndex = 28;
            this.runCaseButton.Text = "Run case";
            this.runCaseButton.UseVisualStyleBackColor = true;
            this.runCaseButton.Click += new System.EventHandler(this.runCaseButton_Click);
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
            this.logTB.Location = new System.Drawing.Point(549, 365);
            this.logTB.Name = "logTB";
            this.logTB.Size = new System.Drawing.Size(287, 138);
            this.logTB.TabIndex = 27;
            this.logTB.Text = "";
            // 
            // runTestButton
            // 
            this.runTestButton.Location = new System.Drawing.Point(411, 332);
            this.runTestButton.Name = "runTestButton";
            this.runTestButton.Size = new System.Drawing.Size(98, 30);
            this.runTestButton.TabIndex = 26;
            this.runTestButton.Text = "Run test";
            this.runTestButton.UseVisualStyleBackColor = true;
            this.runTestButton.Click += new System.EventHandler(this.runTestButton_Click);
            // 
            // deleteTestButton
            // 
            this.deleteTestButton.Location = new System.Drawing.Point(411, 296);
            this.deleteTestButton.Name = "deleteTestButton";
            this.deleteTestButton.Size = new System.Drawing.Size(98, 30);
            this.deleteTestButton.TabIndex = 25;
            this.deleteTestButton.Text = "Delete test";
            this.deleteTestButton.UseVisualStyleBackColor = true;
            // 
            // saveTestButton
            // 
            this.saveTestButton.Location = new System.Drawing.Point(411, 260);
            this.saveTestButton.Name = "saveTestButton";
            this.saveTestButton.Size = new System.Drawing.Size(98, 30);
            this.saveTestButton.TabIndex = 24;
            this.saveTestButton.Text = "Save test";
            this.saveTestButton.UseVisualStyleBackColor = true;
            this.saveTestButton.Click += new System.EventHandler(this.saveTestButton_Click);
            // 
            // deleteStepButton
            // 
            this.deleteStepButton.Location = new System.Drawing.Point(411, 53);
            this.deleteStepButton.Name = "deleteStepButton";
            this.deleteStepButton.Size = new System.Drawing.Size(98, 30);
            this.deleteStepButton.TabIndex = 23;
            this.deleteStepButton.Text = "Delete case";
            this.deleteStepButton.UseVisualStyleBackColor = true;
            // 
            // addCaseButton
            // 
            this.addCaseButton.Location = new System.Drawing.Point(411, 15);
            this.addCaseButton.Name = "addCaseButton";
            this.addCaseButton.Size = new System.Drawing.Size(98, 30);
            this.addCaseButton.TabIndex = 22;
            this.addCaseButton.Text = "Add case";
            this.addCaseButton.UseVisualStyleBackColor = true;
            this.addCaseButton.Click += new System.EventHandler(this.addCaseButton_Click);
            // 
            // testCaseListView
            // 
            this.testCaseListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.testCaseListView.FullRowSelect = true;
            this.testCaseListView.GridLines = true;
            this.testCaseListView.HideSelection = false;
            this.testCaseListView.Location = new System.Drawing.Point(549, 15);
            this.testCaseListView.MultiSelect = false;
            this.testCaseListView.Name = "testCaseListView";
            this.testCaseListView.ShowGroups = false;
            this.testCaseListView.Size = new System.Drawing.Size(287, 344);
            this.testCaseListView.TabIndex = 21;
            this.testCaseListView.UseCompatibleStateImageBehavior = false;
            this.testCaseListView.View = System.Windows.Forms.View.List;
            this.testCaseListView.SelectedIndexChanged += new System.EventHandler(this.testCaseListView_SelectedIndexChanged);
            // 
            // testCasePropertyGrid
            // 
            this.testCasePropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.testCasePropertyGrid.HelpVisible = false;
            this.testCasePropertyGrid.Location = new System.Drawing.Point(13, 53);
            this.testCasePropertyGrid.Name = "testCasePropertyGrid";
            this.testCasePropertyGrid.Size = new System.Drawing.Size(361, 450);
            this.testCasePropertyGrid.TabIndex = 18;
            // 
            // testCaseCB
            // 
            this.testCaseCB.FormattingEnabled = true;
            this.testCaseCB.Location = new System.Drawing.Point(188, 21);
            this.testCaseCB.Name = "testCaseCB";
            this.testCaseCB.Size = new System.Drawing.Size(186, 24);
            this.testCaseCB.TabIndex = 17;
            this.testCaseCB.SelectedIndexChanged += new System.EventHandler(this.testCaseCB_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 17);
            this.label1.TabIndex = 16;
            this.label1.Text = "Test case:";
            // 
            // editTestCasesButton
            // 
            this.editTestCasesButton.Location = new System.Drawing.Point(411, 404);
            this.editTestCasesButton.Name = "editTestCasesButton";
            this.editTestCasesButton.Size = new System.Drawing.Size(98, 30);
            this.editTestCasesButton.TabIndex = 31;
            this.editTestCasesButton.Text = "Edit cases";
            this.editTestCasesButton.UseVisualStyleBackColor = true;
            this.editTestCasesButton.Click += new System.EventHandler(this.editTestCasesButton_Click);
            // 
            // EditTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(848, 515);
            this.Controls.Add(this.editTestCasesButton);
            this.Controls.Add(this.moveDownButton);
            this.Controls.Add(this.moveUpButton);
            this.Controls.Add(this.runCaseButton);
            this.Controls.Add(this.logTB);
            this.Controls.Add(this.runTestButton);
            this.Controls.Add(this.deleteTestButton);
            this.Controls.Add(this.saveTestButton);
            this.Controls.Add(this.deleteStepButton);
            this.Controls.Add(this.addCaseButton);
            this.Controls.Add(this.testCaseListView);
            this.Controls.Add(this.testCasePropertyGrid);
            this.Controls.Add(this.testCaseCB);
            this.Controls.Add(this.label1);
            this.Name = "EditTest";
            this.Text = "Edit Test";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button moveDownButton;
        private System.Windows.Forms.Button moveUpButton;
        private System.Windows.Forms.Button runCaseButton;
        private System.Windows.Forms.RichTextBox logTB;
        private System.Windows.Forms.Button runTestButton;
        private System.Windows.Forms.Button deleteTestButton;
        private System.Windows.Forms.Button saveTestButton;
        private System.Windows.Forms.Button deleteStepButton;
        private System.Windows.Forms.Button addCaseButton;
        private System.Windows.Forms.ListView testCaseListView;
        private System.Windows.Forms.PropertyGrid testCasePropertyGrid;
        private System.Windows.Forms.ComboBox testCaseCB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button editTestCasesButton;
    }
}