using forgotten_construction_set.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace forgotten_construction_set
{
	public class Errors : Form
	{
		private List<Errors.Item> errors;

		private static Errors instance;

		private int sortColumn = -1;

		private IContainer components;

		private ListView errorList;

		private System.Windows.Forms.ContextMenuStrip contextMenu;

		private ColumnHeader cState;

		private ImageList errorIcons;

		private ColumnHeader cItem;

		private ToolStripMenuItem deleteItem;

		private ToolStripMenuItem ignoreError;

		private ColumnHeader cMod;

		private ColumnHeader cInfo;

		private CheckBox allMods;

		private Button bClose;

		private ToolStripMenuItem openItem;

		private ToolStripMenuItem clearChanges;

		private ToolStripMenuItem removeReferences;

		public static int Count
		{
			get
			{
				if (Errors.instance == null)
				{
					return 0;
				}
				return Errors.instance.errors.Count;
			}
		}

		private bool Unresolved
		{
			get
			{
				return false;
			}
		}

		public Errors()
		{
			this.InitializeComponent();
			Errors.instance = this;
			this.errorList.EnableDoubleBuferring();
			this.errors = new List<Errors.Item>();
			this.errorIcons.Images.Add(SystemIcons.Error.ToBitmap());
			this.errorIcons.Images.Add(SystemIcons.Warning.ToBitmap());
			this.errorIcons.Images.Add(SystemIcons.Information.ToBitmap());
		}

		public static void addError(Error type, GameData.Item item, string mod, string text = null)
		{
			Errors.Item item1 = new Errors.Item()
			{
				info = text,
				item = item,
				mod = mod,
				state = 0,
				type = type
			};
			Errors.instance.errors.Add(item1);
		}

		private void allMods_CheckedChanged(object sender, EventArgs e)
		{
			this.refreshList();
		}

		private void bClose_Click(object sender, EventArgs e)
		{
			if (!this.Unresolved)
			{
				base.Hide();
				return;
			}
			MessageBox.Show("在关闭之前解决所有错误");
		}

		public static void clear()
		{
			if (Errors.instance != null)
			{
				Errors.instance.errors.Clear();
				Errors.instance.errorList.Items.Clear();
				Errors.instance.Hide();
			}
		}

		private void clearChangesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			navigation mdiParent = (base.MdiParent as baseForm).nav;
			foreach (ListViewItem selectedItem in this.errorList.SelectedItems)
			{
				Errors.Item tag = selectedItem.Tag as Errors.Item;
				if (tag.item.getState() != GameData.State.MODIFIED)
				{
					continue;
				}
				tag.item.revert();
				tag.state = 3;
				this.setState(selectedItem, "reverted");
				mdiParent.refreshItemWindow(tag.item);
				mdiParent.HasChanges = true;
			}
		}

		private void contextMenu_Opening(object sender, CancelEventArgs e)
		{
			if (this.errorList.SelectedIndices.Count == 0)
			{
				e.Cancel = true;
			}
		}

		private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			GameData mdiParent = (base.MdiParent as baseForm).nav.ou.gameData;
			navigation _navigation = (base.MdiParent as baseForm).nav;
			foreach (ListViewItem selectedItem in this.errorList.SelectedItems)
			{
				Errors.Item tag = selectedItem.Tag as Errors.Item;
				if (tag.item == null)
				{
					continue;
				}
				_navigation.closeItemProperties(tag.item);
				mdiParent.deleteItem(tag.item);
				tag.item = null;
				tag.state = 2;
				this.setState(selectedItem, "deleted");
				_navigation.HasChanges = true;
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

		private void errorList_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			if (e.Column == this.sortColumn || this.errorList.Sorting == SortOrder.Descending)
			{
				this.errorList.Sorting = SortOrder.Ascending;
			}
			else
			{
				this.errorList.Sorting = SortOrder.Descending;
			}
			this.errorList.BeginUpdate();
			this.errorList.Sort();
			this.errorList.ListViewItemSorter = new Errors.ListViewItemComparer(e.Column, this.errorList.Sorting);
			this.errorList.EndUpdate();
		}

		private void errorList_DoubleClick(object sender, EventArgs e)
		{
			if (this.errorList.SelectedItems.Count == 0)
			{
				return;
			}
			baseForm mdiParent = base.MdiParent as baseForm;
			Errors.Item tag = this.errorList.SelectedItems[0].Tag as Errors.Item;
			mdiParent.nav.showItemProperties(tag.item);
		}

		private void Errors_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == System.Windows.Forms.CloseReason.UserClosing)
			{
				e.Cancel = true;
				if (this.Unresolved)
				{
					MessageBox.Show("在关闭之前解决所有错误");
					return;
				}
				base.Hide();
			}
		}

		public static void hide()
		{
			if (Errors.instance != null)
			{
				Errors.instance.Hide();
			}
		}

		private void ignoreToolStripMenuItem_Click(object sender, EventArgs e)
		{
			GameData mdiParent = (base.MdiParent as baseForm).nav.ou.gameData;
			foreach (ListViewItem selectedItem in this.errorList.SelectedItems)
			{
				Errors.Item tag = selectedItem.Tag as Errors.Item;
				if (tag.item == null)
				{
					continue;
				}
				tag.state = 1;
				this.setState(selectedItem, "ignored");
				selectedItem.ImageIndex = -1;
			}
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.errorList = new ListView();
			this.cInfo = new ColumnHeader();
			this.cItem = new ColumnHeader();
			this.cMod = new ColumnHeader();
			this.cState = new ColumnHeader();
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.openItem = new ToolStripMenuItem();
			this.clearChanges = new ToolStripMenuItem();
			this.deleteItem = new ToolStripMenuItem();
			this.ignoreError = new ToolStripMenuItem();
			this.errorIcons = new ImageList(this.components);
			this.allMods = new CheckBox();
			this.bClose = new Button();
			this.removeReferences = new ToolStripMenuItem();
			this.contextMenu.SuspendLayout();
			base.SuspendLayout();
			this.errorList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.errorList.Columns.AddRange(new ColumnHeader[] { this.cInfo, this.cItem, this.cMod, this.cState });
			this.errorList.ContextMenuStrip = this.contextMenu;
			this.errorList.FullRowSelect = true;
			this.errorList.Location = new Point(0, 0);
			this.errorList.Name = "errorList";
			this.errorList.ShowItemToolTips = true;
			this.errorList.Size = new System.Drawing.Size(708, 445);
			this.errorList.SmallImageList = this.errorIcons;
			this.errorList.TabIndex = 0;
			this.errorList.UseCompatibleStateImageBehavior = false;
			this.errorList.View = View.Details;
			this.errorList.ColumnClick += new ColumnClickEventHandler(this.errorList_ColumnClick);
			this.errorList.DoubleClick += new EventHandler(this.errorList_DoubleClick);
			this.cInfo.Text = "信息";
			this.cInfo.Width = 180;
			this.cItem.Text = "项目";
			this.cItem.Width = 85;
			this.cMod.Text = "Mod";
			this.cMod.Width = 81;
			this.cState.Text = "状态";
			this.cState.Width = 80;
			this.contextMenu.Items.AddRange(new ToolStripItem[] { this.openItem, this.clearChanges, this.deleteItem, this.removeReferences, this.ignoreError });
			this.contextMenu.Name = "contextMenu";
			this.contextMenu.Size = new System.Drawing.Size(213, 136);
			this.contextMenu.Opening += new CancelEventHandler(this.contextMenu_Opening);
			this.openItem.Name = "openItem";
			this.openItem.Size = new System.Drawing.Size(212, 22);
			this.openItem.Text = "打开";
			this.openItem.Click += new EventHandler(this.openItemToolStripMenuItem_Click);
			this.clearChanges.Name = "clearChanges";
			this.clearChanges.Size = new System.Drawing.Size(212, 22);
			this.clearChanges.Text = "清除修改";
			this.clearChanges.Click += new EventHandler(this.clearChangesToolStripMenuItem_Click);
			this.deleteItem.Name = "deleteItem";
			this.deleteItem.Size = new System.Drawing.Size(212, 22);
			this.deleteItem.Text = "删除项目";
			this.deleteItem.Click += new EventHandler(this.deleteToolStripMenuItem_Click);
			this.ignoreError.Name = "ignoreError";
			this.ignoreError.Size = new System.Drawing.Size(212, 22);
			this.ignoreError.Text = "忽略";
			this.ignoreError.Click += new EventHandler(this.ignoreToolStripMenuItem_Click);
			this.errorIcons.ColorDepth = ColorDepth.Depth8Bit;
			this.errorIcons.ImageSize = new System.Drawing.Size(16, 16);
			this.errorIcons.TransparentColor = Color.Transparent;
			this.allMods.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			this.allMods.AutoSize = true;
			this.allMods.Location = new Point(0, 451);
			this.allMods.Name = "allMods";
			this.allMods.Size = new System.Drawing.Size(146, 17);
			this.allMods.TabIndex = 1;
			this.allMods.Text = "显示所有的MOD的错误";
			this.allMods.UseVisualStyleBackColor = true;
			this.allMods.CheckedChanged += new EventHandler(this.allMods_CheckedChanged);
			this.bClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.bClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bClose.Location = new Point(633, 445);
			this.bClose.Name = "bClose";
			this.bClose.Size = new System.Drawing.Size(75, 23);
			this.bClose.TabIndex = 2;
			this.bClose.Text = "关闭";
			this.bClose.UseVisualStyleBackColor = true;
			this.bClose.Click += new EventHandler(this.bClose_Click);
			this.removeReferences.Name = "removeReferences";
			this.removeReferences.Size = new System.Drawing.Size(212, 22);
			this.removeReferences.Text = "移除无效的引用";
			this.removeReferences.Click += new EventHandler(this.removeReferences_Click);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.bClose;
			base.ClientSize = new System.Drawing.Size(711, 469);
			base.Controls.Add(this.allMods);
			base.Controls.Add(this.bClose);
			base.Controls.Add(this.errorList);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			base.Name = "Errors";
			this.Text = "错误";
			base.FormClosing += new FormClosingEventHandler(this.Errors_FormClosing);
			this.contextMenu.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		private void openItemToolStripMenuItem_Click(object sender, EventArgs e)
		{
			baseForm mdiParent = base.MdiParent as baseForm;
			foreach (object selectedItem in this.errorList.SelectedItems)
			{
				Errors.Item tag = ((ListViewItem)selectedItem).Tag as Errors.Item;
				mdiParent.nav.showItemProperties(tag.item);
			}
		}

		private void refreshList()
		{
			int num;
			string mdiParent = (base.MdiParent as baseForm).nav.ou.gameData.activeFileName;
			this.errorList.Items.Clear();
			this.errorList.BeginUpdate();
			foreach (Errors.Item error in this.errors)
			{
				if (error.mod != null && !this.allMods.Checked && !(error.mod == mdiParent))
				{
					continue;
				}
				ListViewItem listViewItem = new ListViewItem(error.info);
				ListViewItem listViewItem1 = listViewItem;
				if (error.state == 0)
				{
					num = (int)error.type;
				}
				else
				{
					num = -1;
				}
				listViewItem1.ImageIndex = num;
				listViewItem.SubItems.Add(string.Concat(new string[] { error.item.type.ToString(), ": ", error.item.stringID, " '", error.item.Name, "'" }));
				listViewItem.SubItems.Add((error.mod == null ? "" : error.mod));
				switch (error.state)
				{
					case 1:
					{
						listViewItem.SubItems.Add("ignored");
						break;
					}
					case 2:
					{
						listViewItem.SubItems.Add("cleared");
						break;
					}
					case 3:
					{
						listViewItem.SubItems.Add("deleted");
						break;
					}
					case 4:
					{
						listViewItem.SubItems.Add("fixed");
						break;
					}
					default:
					{
						listViewItem.SubItems.Add("");
						break;
					}
				}
				listViewItem.ToolTipText = error.info;
				listViewItem.Tag = error;
				this.errorList.Items.Add(listViewItem);
			}
			this.errorList.EndUpdate();
		}

		private void removeReferences_Click(object sender, EventArgs e)
		{
			navigation mdiParent = (base.MdiParent as baseForm).nav;
			foreach (ListViewItem selectedItem in this.errorList.SelectedItems)
			{
				Errors.Item tag = selectedItem.Tag as Errors.Item;
				int num = tag.item.removeInvalidReferences();
				if (num <= 0)
				{
					continue;
				}
				tag.item = null;
				tag.state = 4;
				this.setState(selectedItem, string.Concat("removed ", num, " references"));
				mdiParent.HasChanges = true;
			}
		}

		private void setState(ListViewItem i, string text)
		{
			i.SubItems[3].Text = text;
			i.ImageIndex = -1;
		}

		public static void show()
		{
			Errors.instance.refreshList();
			Errors.instance.Show();
			Errors.instance.BringToFront();
		}

		private class Item
		{
			public GameData.Item item;

			public string mod;

			public int state;

			public string info;

			public Error type;

			public Item()
			{
			}
		}

		private class ListViewItemComparer : IComparer
		{
			private int column;

			private SortOrder order;

			public ListViewItemComparer(int column, SortOrder order)
			{
				this.column = column;
				this.order = order;
			}

			public int Compare(object x, object y)
			{
				int num = string.Compare((x as ListViewItem).SubItems[this.column].Text, (y as ListViewItem).SubItems[this.column].Text);
				if (this.order == SortOrder.Descending)
				{
					num = -num;
				}
				return num;
			}
		}
	}
}