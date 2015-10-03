using System;
using System.Security.AccessControl;
using System.Windows.Forms;

namespace vuwall_motion {
    partial class TransparentForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.SuspendLayout();
            // 
            // TransparentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            var clientRes = Screen.PrimaryScreen.Bounds;
            this.ClientSize = clientRes.Size;
            this.Name = "TransparentForm";
            this.Text = "TransparentForm";
            this.Load += new EventHandler(this.TransparentForm_Load);
            this.Paint += new PaintEventHandler(this.TransparentForm_Paint);
            this.MouseClick += new MouseEventHandler(this.TransparentForm_MouseClick);
            this.MouseMove += new MouseEventHandler(this.TransparentForm_MouseMove);
            this.ResumeLayout(false);
            this.FormBorderStyle = FormBorderStyle.None;
        }

        #endregion
    }
}