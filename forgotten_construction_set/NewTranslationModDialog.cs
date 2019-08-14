using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace forgotten_construction_set
{
	public class NewTranslationModDialog : Form
	{
		private IContainer components;

		private Button btnCancel;

		private Button btnAccept;

		private ComboBox cbLanguage;

		public NewTranslationModDialog()
		{
			this.InitializeComponent();
			this.btnAccept.Enabled = false;
		}

		private void btnAccept_Click(object sender, EventArgs e)
		{
			if (this.cbLanguage.SelectedItem == null)
			{
				base.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			}
			else
			{
				TranslationManager.CreateTranslationFolder(this.cbLanguage.SelectedItem as TranslationManager.TranslationCulture);
				base.DialogResult = System.Windows.Forms.DialogResult.OK;
			}
			base.Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			base.Close();
		}

		private void cbLanguage_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.btnAccept.Enabled = this.cbLanguage.SelectedIndex >= 0;
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
			this.btnCancel = new Button();
			this.btnAccept = new Button();
			this.cbLanguage = new ComboBox();
			Label label = new Label();
			base.SuspendLayout();
			label.AutoSize = true;
			label.Location = new Point(12, 15);
			label.Name = "lblLanguage";
			label.Size = new System.Drawing.Size(58, 13);
			label.TabIndex = 3;
			label.Text = "语言:";
			this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new Point(206, 52);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 0;
			this.btnCancel.Text = "取消";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
			this.btnAccept.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.btnAccept.Location = new Point(288, 52);
			this.btnAccept.Name = "btnAccept";
			this.btnAccept.Size = new System.Drawing.Size(75, 23);
			this.btnAccept.TabIndex = 1;
			this.btnAccept.Text = "创建";
			this.btnAccept.UseVisualStyleBackColor = true;
			this.btnAccept.Click += new EventHandler(this.btnAccept_Click);
			this.cbLanguage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.cbLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cbLanguage.FormattingEnabled = true;
			this.cbLanguage.Location = new Point(76, 12);
			this.cbLanguage.Name = "cbLanguage";
			this.cbLanguage.Size = new System.Drawing.Size(287, 21);
			this.cbLanguage.Sorted = true;
			this.cbLanguage.TabIndex = 2;
			this.cbLanguage.SelectedIndexChanged += new EventHandler(this.cbLanguage_SelectedIndexChanged);
			base.AcceptButton = this.btnAccept;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.btnCancel;
			base.ClientSize = new System.Drawing.Size(375, 87);
			base.ControlBox = false;
			base.Controls.Add(label);
			base.Controls.Add(this.cbLanguage);
			base.Controls.Add(this.btnAccept);
			base.Controls.Add(this.btnCancel);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "NewTranslationModDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			this.Text = "新翻译MOD";
			base.Load += new EventHandler(this.NewTranslationModDialog_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		private void NewTranslationModDialog_Load(object sender, EventArgs e)
		{
			CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
			int num = -1;
			for (int i = 0; i < (int)cultures.Length; i++)
			{
				this.cbLanguage.Items.Add(new TranslationManager.TranslationCulture(cultures[i], string.Empty, string.Empty));
				if (cultures[i].Equals(CultureInfo.CurrentCulture))
				{
					num = i;
				}
			}
			this.cbLanguage.SelectedIndex = num;
		}
	}
}