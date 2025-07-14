
namespace GOSTool.Test
{
    partial class EditTestCases
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
            this.label1 = new System.Windows.Forms.Label();
            this.testStepsCB = new System.Windows.Forms.ComboBox();
            this.testStepPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.label2 = new System.Windows.Forms.Label();
            this.testStepListView = new System.Windows.Forms.ListView();
            this.newStepButton = new System.Windows.Forms.Button();
            this.deleteStepButton = new System.Windows.Forms.Button();
            this.saveCaseButton = new System.Windows.Forms.Button();
            this.deleteCaseButton = new System.Windows.Forms.Button();
            this.runCaseButton = new System.Windows.Forms.Button();
            this.logTB = new System.Windows.Forms.RichTextBox();
            this.runStepButton = new System.Windows.Forms.Button();
            this.moveUpButton = new System.Windows.Forms.Button();
            this.moveDownButton = new System.Windows.Forms.Button();
            this.testCaseNameCB = new System.Windows.Forms.ComboBox();
            this.createCaseButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Test step:";
            // 
            // testStepsCB
            // 
            this.testStepsCB.FormattingEnabled = true;
            this.testStepsCB.Location = new System.Drawing.Point(187, 40);
            this.testStepsCB.Name = "testStepsCB";
            this.testStepsCB.Size = new System.Drawing.Size(186, 24);
            this.testStepsCB.TabIndex = 1;
            this.testStepsCB.SelectedIndexChanged += new System.EventHandler(this.testStepsCB_SelectedIndexChanged);
            // 
            // testStepPropertyGrid
            // 
            this.testStepPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.testStepPropertyGrid.HelpVisible = false;
            this.testStepPropertyGrid.Location = new System.Drawing.Point(12, 83);
            this.testStepPropertyGrid.Name = "testStepPropertyGrid";
            this.testStepPropertyGrid.Size = new System.Drawing.Size(361, 437);
            this.testStepPropertyGrid.TabIndex = 2;
            this.testStepPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.testStepPropertyGrid_PropertyValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Test case name:";
            // 
            // testStepListView
            // 
            this.testStepListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.testStepListView.FullRowSelect = true;
            this.testStepListView.GridLines = true;
            this.testStepListView.HideSelection = false;
            this.testStepListView.Location = new System.Drawing.Point(548, 9);
            this.testStepListView.MultiSelect = false;
            this.testStepListView.Name = "testStepListView";
            this.testStepListView.ShowGroups = false;
            this.testStepListView.Size = new System.Drawing.Size(318, 343);
            this.testStepListView.TabIndex = 6;
            this.testStepListView.UseCompatibleStateImageBehavior = false;
            this.testStepListView.View = System.Windows.Forms.View.List;
            this.testStepListView.SelectedIndexChanged += new System.EventHandler(this.testStepListView_SelectedIndexChanged);
            // 
            // newStepButton
            // 
            this.newStepButton.Location = new System.Drawing.Point(410, 162);
            this.newStepButton.Name = "newStepButton";
            this.newStepButton.Size = new System.Drawing.Size(98, 30);
            this.newStepButton.TabIndex = 7;
            this.newStepButton.Text = "Add step";
            this.newStepButton.UseVisualStyleBackColor = true;
            this.newStepButton.Click += new System.EventHandler(this.addStepButton_Click);
            // 
            // deleteStepButton
            // 
            this.deleteStepButton.Location = new System.Drawing.Point(410, 200);
            this.deleteStepButton.Name = "deleteStepButton";
            this.deleteStepButton.Size = new System.Drawing.Size(98, 30);
            this.deleteStepButton.TabIndex = 8;
            this.deleteStepButton.Text = "Delete step";
            this.deleteStepButton.UseVisualStyleBackColor = true;
            this.deleteStepButton.Click += new System.EventHandler(this.deleteStepButton_Click);
            // 
            // saveCaseButton
            // 
            this.saveCaseButton.Location = new System.Drawing.Point(410, 45);
            this.saveCaseButton.Name = "saveCaseButton";
            this.saveCaseButton.Size = new System.Drawing.Size(98, 30);
            this.saveCaseButton.TabIndex = 9;
            this.saveCaseButton.Text = "Save case";
            this.saveCaseButton.UseVisualStyleBackColor = true;
            this.saveCaseButton.Click += new System.EventHandler(this.saveCaseButton_Click);
            // 
            // deleteCaseButton
            // 
            this.deleteCaseButton.Location = new System.Drawing.Point(410, 81);
            this.deleteCaseButton.Name = "deleteCaseButton";
            this.deleteCaseButton.Size = new System.Drawing.Size(98, 30);
            this.deleteCaseButton.TabIndex = 10;
            this.deleteCaseButton.Text = "Delete case";
            this.deleteCaseButton.UseVisualStyleBackColor = true;
            this.deleteCaseButton.Click += new System.EventHandler(this.deleteCaseButton_Click);
            // 
            // runCaseButton
            // 
            this.runCaseButton.Location = new System.Drawing.Point(410, 126);
            this.runCaseButton.Name = "runCaseButton";
            this.runCaseButton.Size = new System.Drawing.Size(98, 30);
            this.runCaseButton.TabIndex = 11;
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
            this.logTB.Location = new System.Drawing.Point(548, 359);
            this.logTB.Name = "logTB";
            this.logTB.Size = new System.Drawing.Size(318, 161);
            this.logTB.TabIndex = 12;
            this.logTB.Text = "";
            this.logTB.TextChanged += new System.EventHandler(this.logTB_TextChanged);
            // 
            // runStepButton
            // 
            this.runStepButton.Location = new System.Drawing.Point(410, 236);
            this.runStepButton.Name = "runStepButton";
            this.runStepButton.Size = new System.Drawing.Size(98, 30);
            this.runStepButton.TabIndex = 13;
            this.runStepButton.Text = "Run step";
            this.runStepButton.UseVisualStyleBackColor = true;
            this.runStepButton.Click += new System.EventHandler(this.runStepButton_Click);
            // 
            // moveUpButton
            // 
            this.moveUpButton.Location = new System.Drawing.Point(410, 272);
            this.moveUpButton.Name = "moveUpButton";
            this.moveUpButton.Size = new System.Drawing.Size(98, 30);
            this.moveUpButton.TabIndex = 14;
            this.moveUpButton.Text = "Move up";
            this.moveUpButton.UseVisualStyleBackColor = true;
            this.moveUpButton.Click += new System.EventHandler(this.moveUpButton_Click);
            // 
            // moveDownButton
            // 
            this.moveDownButton.Location = new System.Drawing.Point(410, 308);
            this.moveDownButton.Name = "moveDownButton";
            this.moveDownButton.Size = new System.Drawing.Size(98, 30);
            this.moveDownButton.TabIndex = 15;
            this.moveDownButton.Text = "Move down";
            this.moveDownButton.UseVisualStyleBackColor = true;
            this.moveDownButton.Click += new System.EventHandler(this.moveDownButton_Click);
            // 
            // testCaseNameCB
            // 
            this.testCaseNameCB.FormattingEnabled = true;
            this.testCaseNameCB.Location = new System.Drawing.Point(187, 9);
            this.testCaseNameCB.Name = "testCaseNameCB";
            this.testCaseNameCB.Size = new System.Drawing.Size(186, 24);
            this.testCaseNameCB.TabIndex = 16;
            this.testCaseNameCB.SelectedIndexChanged += new System.EventHandler(this.testCaseNameCB_SelectedIndexChanged);
            // 
            // createCaseButton
            // 
            this.createCaseButton.Location = new System.Drawing.Point(410, 9);
            this.createCaseButton.Name = "createCaseButton";
            this.createCaseButton.Size = new System.Drawing.Size(98, 30);
            this.createCaseButton.TabIndex = 17;
            this.createCaseButton.Text = "Create case";
            this.createCaseButton.UseVisualStyleBackColor = true;
            this.createCaseButton.Click += new System.EventHandler(this.createCaseButton_Click);
            // 
            // EditTestCases
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 532);
            this.Controls.Add(this.createCaseButton);
            this.Controls.Add(this.testCaseNameCB);
            this.Controls.Add(this.moveDownButton);
            this.Controls.Add(this.moveUpButton);
            this.Controls.Add(this.runStepButton);
            this.Controls.Add(this.logTB);
            this.Controls.Add(this.runCaseButton);
            this.Controls.Add(this.deleteCaseButton);
            this.Controls.Add(this.saveCaseButton);
            this.Controls.Add(this.deleteStepButton);
            this.Controls.Add(this.newStepButton);
            this.Controls.Add(this.testStepListView);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.testStepPropertyGrid);
            this.Controls.Add(this.testStepsCB);
            this.Controls.Add(this.label1);
            this.Name = "EditTestCases";
            this.Text = "Edit Test Case";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox testStepsCB;
        private System.Windows.Forms.PropertyGrid testStepPropertyGrid;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView testStepListView;
        private System.Windows.Forms.Button newStepButton;
        private System.Windows.Forms.Button deleteStepButton;
        private System.Windows.Forms.Button saveCaseButton;
        private System.Windows.Forms.Button deleteCaseButton;
        private System.Windows.Forms.Button runCaseButton;
        private System.Windows.Forms.RichTextBox logTB;
        private System.Windows.Forms.Button runStepButton;
        private System.Windows.Forms.Button moveUpButton;
        private System.Windows.Forms.Button moveDownButton;
        private System.Windows.Forms.ComboBox testCaseNameCB;
        private System.Windows.Forms.Button createCaseButton;
    }
}