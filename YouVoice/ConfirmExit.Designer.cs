namespace YouVoice
{
    partial class ConfirmExit
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
            this.ButtonYes = new System.Windows.Forms.Button();
            this.LabelQuery = new System.Windows.Forms.Label();
            this.ButtonNo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ButtonYes
            // 
            this.ButtonYes.Location = new System.Drawing.Point(15, 29);
            this.ButtonYes.Name = "ButtonYes";
            this.ButtonYes.Size = new System.Drawing.Size(75, 23);
            this.ButtonYes.TabIndex = 0;
            this.ButtonYes.Text = "Yes";
            this.ButtonYes.UseVisualStyleBackColor = true;
            this.ButtonYes.Click += new System.EventHandler(this.ButtonYes_Click);
            // 
            // LabelQuery
            // 
            this.LabelQuery.AutoSize = true;
            this.LabelQuery.Location = new System.Drawing.Point(12, 9);
            this.LabelQuery.Name = "LabelQuery";
            this.LabelQuery.Size = new System.Drawing.Size(149, 13);
            this.LabelQuery.TabIndex = 1;
            this.LabelQuery.Text = "Are you sure you want to exit?";
            // 
            // ButtonNo
            // 
            this.ButtonNo.Location = new System.Drawing.Point(96, 29);
            this.ButtonNo.Name = "ButtonNo";
            this.ButtonNo.Size = new System.Drawing.Size(75, 23);
            this.ButtonNo.TabIndex = 2;
            this.ButtonNo.Text = "No";
            this.ButtonNo.UseVisualStyleBackColor = true;
            this.ButtonNo.Click += new System.EventHandler(this.ButtonNo_Click);
            // 
            // ConfirmExit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(183, 64);
            this.Controls.Add(this.ButtonNo);
            this.Controls.Add(this.LabelQuery);
            this.Controls.Add(this.ButtonYes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfirmExit";
            this.Text = "Exit?";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonYes;
        private System.Windows.Forms.Label LabelQuery;
        private System.Windows.Forms.Button ButtonNo;
    }
}