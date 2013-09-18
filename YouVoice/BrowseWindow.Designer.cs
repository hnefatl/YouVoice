namespace YouVoice
{
    partial class BrowseWindow
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
            this.SearchBox = new System.Windows.Forms.TextBox();
            this.SearchButton = new System.Windows.Forms.Button();
            this.Status = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Videos = new System.Windows.Forms.ListView();
            this.VideoNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VideoName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VideoType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VideoUploader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SearchResults = new System.Windows.Forms.NumericUpDown();
            this.VideoThumbnail = new System.Windows.Forms.PictureBox();
            this.VideoDescription = new System.Windows.Forms.RichTextBox();
            this.Status.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SearchResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VideoThumbnail)).BeginInit();
            this.SuspendLayout();
            // 
            // SearchBox
            // 
            this.SearchBox.Location = new System.Drawing.Point(12, 13);
            this.SearchBox.Name = "SearchBox";
            this.SearchBox.Size = new System.Drawing.Size(356, 20);
            this.SearchBox.TabIndex = 0;
            // 
            // SearchButton
            // 
            this.SearchButton.AutoSize = true;
            this.SearchButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.SearchButton.Location = new System.Drawing.Point(421, 11);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(51, 23);
            this.SearchButton.TabIndex = 1;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // Status
            // 
            this.Status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.Status.Location = new System.Drawing.Point(0, 340);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(774, 22);
            this.Status.TabIndex = 3;
            this.Status.Text = "Status";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // Videos
            // 
            this.Videos.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.VideoNumber,
            this.VideoName,
            this.VideoType,
            this.VideoUploader});
            this.Videos.FullRowSelect = true;
            this.Videos.Location = new System.Drawing.Point(12, 39);
            this.Videos.MultiSelect = false;
            this.Videos.Name = "Videos";
            this.Videos.Size = new System.Drawing.Size(460, 298);
            this.Videos.TabIndex = 4;
            this.Videos.UseCompatibleStateImageBehavior = false;
            this.Videos.View = System.Windows.Forms.View.Details;
            // 
            // VideoNumber
            // 
            this.VideoNumber.Text = "No.";
            this.VideoNumber.Width = 32;
            // 
            // VideoName
            // 
            this.VideoName.Text = "Name";
            this.VideoName.Width = 260;
            // 
            // VideoType
            // 
            this.VideoType.Text = "Type";
            // 
            // VideoUploader
            // 
            this.VideoUploader.Text = "Uploader";
            this.VideoUploader.Width = 101;
            // 
            // SearchResults
            // 
            this.SearchResults.Location = new System.Drawing.Point(374, 14);
            this.SearchResults.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.SearchResults.Name = "SearchResults";
            this.SearchResults.Size = new System.Drawing.Size(41, 20);
            this.SearchResults.TabIndex = 5;
            this.SearchResults.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // VideoThumbnail
            // 
            this.VideoThumbnail.Location = new System.Drawing.Point(479, 11);
            this.VideoThumbnail.Name = "VideoThumbnail";
            this.VideoThumbnail.Size = new System.Drawing.Size(111, 73);
            this.VideoThumbnail.TabIndex = 7;
            this.VideoThumbnail.TabStop = false;
            // 
            // VideoDescription
            // 
            this.VideoDescription.Location = new System.Drawing.Point(479, 91);
            this.VideoDescription.Name = "VideoDescription";
            this.VideoDescription.ReadOnly = true;
            this.VideoDescription.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.VideoDescription.Size = new System.Drawing.Size(283, 246);
            this.VideoDescription.TabIndex = 8;
            this.VideoDescription.Text = "";
            // 
            // BrowseWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 362);
            this.Controls.Add(this.VideoDescription);
            this.Controls.Add(this.VideoThumbnail);
            this.Controls.Add(this.SearchResults);
            this.Controls.Add(this.Videos);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.SearchBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BrowseWindow";
            this.Text = "BrowseWindow";
            this.Status.ResumeLayout(false);
            this.Status.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SearchResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VideoThumbnail)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox SearchBox;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.StatusStrip Status;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.ListView Videos;
        private System.Windows.Forms.ColumnHeader VideoNumber;
        private System.Windows.Forms.ColumnHeader VideoName;
        private System.Windows.Forms.ColumnHeader VideoUploader;
        private System.Windows.Forms.NumericUpDown SearchResults;
        private System.Windows.Forms.ColumnHeader VideoType;
        private System.Windows.Forms.PictureBox VideoThumbnail;
        private System.Windows.Forms.RichTextBox VideoDescription;
    }
}