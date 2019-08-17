namespace forgotten_construction_set
{
    partial class Donate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Donate));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.donateMessage = new System.Windows.Forms.Label();
            this.sourceLink = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(318, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(265, 370);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // donateMessage
            // 
            this.donateMessage.Dock = System.Windows.Forms.DockStyle.Left;
            this.donateMessage.Location = new System.Drawing.Point(0, 0);
            this.donateMessage.Margin = new System.Windows.Forms.Padding(3);
            this.donateMessage.Name = "donateMessage";
            this.donateMessage.Padding = new System.Windows.Forms.Padding(3);
            this.donateMessage.Size = new System.Drawing.Size(312, 432);
            this.donateMessage.TabIndex = 1;
            this.donateMessage.Text = resources.GetString("donateMessage.Text");
            // 
            // sourceLink
            // 
            this.sourceLink.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sourceLink.AutoSize = true;
            this.sourceLink.Location = new System.Drawing.Point(384, 390);
            this.sourceLink.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.sourceLink.Name = "sourceLink";
            this.sourceLink.Size = new System.Drawing.Size(125, 12);
            this.sourceLink.TabIndex = 2;
            this.sourceLink.TabStop = true;
            this.sourceLink.Text = "点击下载本程序源代码";
            this.sourceLink.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.sourceLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.SourceLink_LinkClicked);
            // 
            // Donate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 432);
            this.Controls.Add(this.sourceLink);
            this.Controls.Add(this.donateMessage);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Donate";
            this.Text = "捐赠";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label donateMessage;
        private System.Windows.Forms.LinkLabel sourceLink;
    }
}