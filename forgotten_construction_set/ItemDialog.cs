using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace forgotten_construction_set
{
	public class ItemDialog : Form
	{
		private IContainer components;

		protected GameDataList listView1;

		private ColumnHeader columnHeader2;

		protected TextBox filter;

		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		private ToolStripMenuItem openToolStripMenuItem;

		protected Button bOk;

		protected Button bCancel;

		public string CustomFilter
		{
			get;
			set;
		}

		public List<GameData.Item> Items
		{
			get;
			private set;
		}

		public GameData.Item NewItem
		{
			get;
			set;
		}

		protected GameData Source
		{
			get;
			set;
		}

		protected itemType Type
		{
			get;
			set;
		}

		public ItemDialog(string title, GameData source, itemType typeFilter = itemType.ITEM, bool multiple = true, string customFilter = "", itemType newItem = itemType.ITEM)
		{
			this.InitializeComponent();
			this.Source = source;
			this.Type = typeFilter;
			this.listView1.UpdateItems(source, new itemType?(typeFilter), customFilter);
			this.Text = title;
			if (newItem != itemType.NULL_ITEM)
			{
				this.NewItem = new GameData.Item(newItem, "new item")
				{
					Name = "<new item>"
				};
				this.listView1.Items.Insert(0, this.NewItem);
				this.listView1.Refresh();
			}
			this.listView1.MultiSelect = multiple;
			this.listView1.MouseDoubleClick += new MouseEventHandler(this.listView1_MouseDoubleClick);
			this.bOk.Enabled = false;
			this.CustomFilter = customFilter;
		}

		private void bOk_Click(object sender, EventArgs e)
		{
			this.Items = this.listView1.SelectedItems;
			if (this.Items.Count > 0 && this.Items[0] == this.NewItem)
			{
				navigation nav = this.getNav();
				this.Items[0] = nav.ou.gameData.createItem(this.NewItem.type);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void filter_TextChanged(object sender, EventArgs e)
		{
			this.filter.BackColor = (this.filter.Text == "" ? SystemColors.Window : Color.Yellow);
			this.listView1.UpdateItems(this.Source, new itemType?(this.Type), string.Concat(this.filter.Text, ";", this.CustomFilter));
			if (this.NewItem != null)
			{
				this.listView1.Items.Insert(0, this.NewItem);
				this.listView1.Refresh();
			}
		}

		protected navigation getNav()
		{
			navigation _navigation = null;
			foreach (Form openForm in Application.OpenForms)
			{
				if (openForm.GetType() != typeof(baseForm))
				{
					continue;
				}
				_navigation = ((baseForm)openForm).nav;
			}
			return _navigation;
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.listView1 = new GameDataList();
			this.columnHeader2 = new ColumnHeader();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.openToolStripMenuItem = new ToolStripMenuItem();
			this.bOk = new Button();
			this.bCancel = new Button();
			this.filter = new TextBox();
			this.contextMenuStrip1.SuspendLayout();
			base.SuspendLayout();
			this.listView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.listView1.Columns.AddRange(new ColumnHeader[] { this.columnHeader2 });
			this.listView1.ContextMenuStrip = this.contextMenuStrip1;
			this.listView1.Filter = null;
			this.listView1.FullRowSelect = true;
			this.listView1.HideSelection = false;
			this.listView1.Location = new Point(0, 0);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(308, 435);
			this.listView1.Source = null;
			this.listView1.TabIndex = 2;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = View.Details;
			this.listView1.VirtualMode = true;
			this.listView1.SelectedIndexChanged += new EventHandler(this.listView1_SelectedIndexChanged);
			this.columnHeader2.Text = "名称";
			this.columnHeader2.Width = 241;
			this.contextMenuStrip1.Items.AddRange(new ToolStripItem[] { this.openToolStripMenuItem });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(104, 26);
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.openToolStripMenuItem.Text = "打开";
			this.openToolStripMenuItem.Click += new EventHandler(this.openItem_Click);
			this.bOk.Anchor = AnchorStyles.Bottom;
			this.bOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.bOk.Location = new Point(67, 460);
			this.bOk.Name = "bOk";
			this.bOk.Size = new System.Drawing.Size(75, 23);
			this.bOk.TabIndex = 3;
			this.bOk.Text = "好的";
			this.bOk.UseVisualStyleBackColor = true;
			this.bOk.Click += new EventHandler(this.bOk_Click);
			this.bCancel.Anchor = AnchorStyles.Bottom;
			this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bCancel.Location = new Point(165, 460);
			this.bCancel.Name = "bCancel";
			this.bCancel.Size = new System.Drawing.Size(75, 23);
			this.bCancel.TabIndex = 4;
			this.bCancel.Text = "取消";
			this.bCancel.UseVisualStyleBackColor = true;
			this.filter.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.filter.Location = new Point(0, 436);
			this.filter.Margin = new System.Windows.Forms.Padding(2);
			this.filter.Name = "filter";
			this.filter.Size = new System.Drawing.Size(308, 20);
			this.filter.TabIndex = 1;
			this.filter.TextChanged += new EventHandler(this.filter_TextChanged);
			base.AcceptButton = this.bOk;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
			base.CancelButton = this.bCancel;
			base.ClientSize = new System.Drawing.Size(307, 494);
			base.ControlBox = false;
			base.Controls.Add(this.filter);
			base.Controls.Add(this.bCancel);
			base.Controls.Add(this.bOk);
			base.Controls.Add(this.listView1);
			base.Name = "ItemDialog";
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "选择要引用的项";
			this.contextMenuStrip1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.bOk.PerformClick();
		}

		private void listView1_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.bOk.Enabled = this.listView1.SelectedItems.Count > 0;
		}

		protected void openItem_Click(object sender, EventArgs e)
		{
			navigation nav = this.getNav();
			foreach (GameData.Item selectedItem in this.listView1.SelectedItems)
			{
				if (selectedItem == this.NewItem)
				{
					continue;
				}
				nav.showItemProperties(selectedItem);
			}
		}
	}
}