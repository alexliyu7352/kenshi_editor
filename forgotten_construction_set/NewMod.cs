using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace forgotten_construction_set
{
	public class NewMod : Form
	{
		public string directory;

		public string name;

		private IContainer components;

		private Button create;

		private Button cancel;

		private Label label1;

		private TextBox modName;

		public NewMod(string dir)
		{
			this.InitializeComponent();
			this.directory = dir;
			this.create.Enabled = !string.IsNullOrWhiteSpace(this.modName.Text);
		}

		private void cancel_Click(object sender, EventArgs e)
		{
			this.name = string.Empty;
			base.Hide();
		}

		private void create_Click(object sender, EventArgs e)
		{
			this.name = this.modName.Text;
			if (this.name.EndsWith(".mod"))
			{
				this.name.Remove(this.name.Length - 4);
			}
			string str = string.Concat(this.directory, "\\", this.name);
			string str1 = string.Concat(str, "\\", this.name, ".mod");
			this.name = string.Concat(this.name, ".mod");
			if ((new FileInfo(str1)).Exists)
			{
				MessageBox.Show(string.Concat(this.name, " 已经存在"), "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			Directory.CreateDirectory(str);
			(new GameData()).save(str1);
			base.Hide();
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
			this.create = new Button();
			this.cancel = new Button();
			this.label1 = new Label();
			this.modName = new TextBox();
			base.SuspendLayout();
			this.create.Location = new Point(196, 64);
			this.create.Name = "create";
			this.create.Size = new System.Drawing.Size(78, 26);
			this.create.TabIndex = 1;
			this.create.Text = "创建";
			this.create.UseVisualStyleBackColor = true;
			this.create.Click += new EventHandler(this.create_Click);
			this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancel.Location = new Point(101, 64);
			this.cancel.Name = "cancel";
			this.cancel.Size = new System.Drawing.Size(79, 25);
			this.cancel.TabIndex = 2;
			this.cancel.Text = "取消";
			this.cancel.UseVisualStyleBackColor = true;
			this.cancel.Click += new EventHandler(this.cancel_Click);
			this.label1.AutoSize = true;
			this.label1.Location = new Point(11, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(112, 17);
			this.label1.TabIndex = 4;
			this.label1.Text = "输入MOD名称";
			this.modName.Location = new Point(8, 36);
			this.modName.Name = "modName";
			this.modName.Size = new System.Drawing.Size(266, 22);
			this.modName.TabIndex = 0;
			this.modName.TextChanged += new EventHandler(this.modName_TextChanged);
			base.AcceptButton = this.create;
			base.AutoScaleDimensions = new SizeF(8f, 16f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.cancel;
			base.ClientSize = new System.Drawing.Size(286, 100);
			base.ControlBox = false;
			base.Controls.Add(this.modName);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.cancel);
			base.Controls.Add(this.create);
			base.Name = "NewMod";
			this.Text = "新Kenshi Mod";
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		private void modName_TextChanged(object sender, EventArgs e)
		{
			this.create.Enabled = !string.IsNullOrWhiteSpace(this.modName.Text);
		}
	}
}