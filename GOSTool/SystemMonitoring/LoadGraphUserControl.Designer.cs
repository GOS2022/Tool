﻿
namespace GOSTool
{
    partial class LoadGraphUserControl
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
            this.SuspendLayout();
            // 
            // LoadGraphUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.DoubleBuffered = true;
            this.Name = "LoadGraphUserControl";
            this.Size = new System.Drawing.Size(614, 148);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.LoadGraphUserControl_Paint);
            this.MouseLeave += new System.EventHandler(this.LoadGraphUserControl_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LoadGraphUserControl_MouseMove);
            this.Resize += new System.EventHandler(this.LoadGraphUserControl_Resize);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
