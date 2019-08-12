using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace forgotten_construction_set
{
	public class TextDialog : Form
	{
		private IContainer components;

		private TextBox text;

		private Button bConfirm;

		private Button bCancel;

		private SplitContainer split;

		private TextBox topText;

		public string Value
		{
			get
			{
				return this.text.Text;
			}
		}

		public TextDialog(string title, string s, string top = null)
		{
			this.InitializeComponent();
			this.Text = title;
			s = s.Replace("\r", string.Empty);
			this.text.Lines = s.Split(new char[] { '\n' });
			this.text.MaxLength = 4000;
			if (top == null)
			{
				this.split.Panel1Collapsed = true;
				return;
			}
			this.topText.Lines = top.Split(new char[] { '\n' });
		}

		private void bConfirm_Click(object sender, EventArgs e)
		{
			this.text.Text = string.Join("\n", this.text.Lines);
			base.DialogResult = System.Windows.Forms.DialogResult.OK;
			base.Close();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.text = new TextBox();
			this.bConfirm = new Button();
			this.bCancel = new Button();
			this.split = new SplitContainer();
			this.topText = new TextBox();
			((ISupportInitialize)this.split).BeginInit();
			this.split.Panel1.SuspendLayout();
			this.split.Panel2.SuspendLayout();
			this.split.SuspendLayout();
			base.SuspendLayout();
			this.text.AcceptsReturn = true;
			this.text.AcceptsTab = true;
			this.text.Dock = DockStyle.Fill;
			this.text.Location = new Point(0, 0);
			this.text.MaxLength = 4000;
			this.text.Multiline = true;
			this.text.Name = "text";
			this.text.ScrollBars = ScrollBars.Both;
			this.text.Size = new System.Drawing.Size(467, 118);
			this.text.TabIndex = 0;
			this.bConfirm.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.bConfirm.Location = new Point(386, 253);
			this.bConfirm.Name = "bConfirm";
			this.bConfirm.Size = new System.Drawing.Size(75, 23);
			this.bConfirm.TabIndex = 1;
			this.bConfirm.TabStop = false;
			this.bConfirm.Text = "Ok";
			this.bConfirm.UseVisualStyleBackColor = true;
			this.bConfirm.Click += new EventHandler(this.bConfirm_Click);
			this.bCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bCancel.Location = new Point(109, 92);
			this.bCancel.Name = "bCancel";
			this.bCancel.Size = new System.Drawing.Size(75, 23);
			this.bCancel.TabIndex = 2;
			this.bCancel.Text = "Cancel";
			this.bCancel.UseVisualStyleBackColor = true;
			this.split.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.split.Location = new Point(2, 3);
			this.split.Name = "split";
			this.split.Orientation = Orientation.Horizontal;
			this.split.Panel1.Controls.Add(this.topText);
			this.split.Panel2.Controls.Add(this.text);
			this.split.Size = new System.Drawing.Size(467, 244);
			this.split.SplitterDistance = 122;
			this.split.TabIndex = 3;
			this.topText.Dock = DockStyle.Fill;
			this.topText.Location = new Point(0, 0);
			this.topText.Multiline = true;
			this.topText.Name = "topText";
			this.topText.ReadOnly = true;
			this.topText.ScrollBars = ScrollBars.Vertical;
			this.topText.Size = new System.Drawing.Size(467, 122);
			this.topText.TabIndex = 0;
			base.AcceptButton = this.bConfirm;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.bCancel;
			base.ClientSize = new System.Drawing.Size(473, 288);
			base.Controls.Add(this.split);
			base.Controls.Add(this.bConfirm);
			base.Controls.Add(this.bCancel);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "TextDialog";
			this.Text = "Text Editor";
			this.split.Panel1.ResumeLayout(false);
			this.split.Panel1.PerformLayout();
			this.split.Panel2.ResumeLayout(false);
			this.split.Panel2.PerformLayout();
			((ISupportInitialize)this.split).EndInit();
			this.split.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}