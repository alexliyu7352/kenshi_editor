using forgotten_construction_set.Components;
using forgotten_construction_set.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace forgotten_construction_set
{
	public class ChangeList : Form
	{
		private navigation nav;

		private GameData gameData;

		private IContainer components;

		private ChangeListTree tree;

		private TextBox filter;

		private System.Windows.Forms.ContextMenuStrip contextMenu;

		private ToolStripMenuItem openItem;

		private ToolStripMenuItem revertItem;

		private ToolStripSeparator toolStripSeparator1;

		private ToolStripMenuItem showNew;

		private ToolStripMenuItem showChanged;

		private ToolStripMenuItem showDeleted;

		public ItemFilter Filter
		{
			get;
			set;
		}

		public int SplitPosition
		{
			get;
			set;
		}

		public ChangeList(navigation nav)
		{
			this.InitializeComponent();
			base.ShowIcon = true;
			base.Icon = System.Drawing.Icon.FromHandle(Resources.ChangesetGroup_16x.GetHicon());
			this.nav = nav;
			this.gameData = nav.ou.gameData;
			base.Activated += new EventHandler(this.ChangeList_Activated);
			this.tree.MouseDoubleClick += new MouseEventHandler(this.Tree_MouseDoubleClick);
			this.tree.GameData = this.gameData;
		}

		private void ChangeList_Activated(object sender, EventArgs e)
		{
			this.tree.RefreshList();
		}

		private void contextMenu_Opening(object sender, CancelEventArgs e)
		{
			bool flag = true;
			foreach (ChangeListTree.TreeNode selectedItem in this.tree.SelectedItems)
			{
				flag = flag & (!(selectedItem is ChangeListTree.RootNode) ? false : this.tree.GetItem(selectedItem).getState() == GameData.State.OWNED);
			}
			this.revertItem.Text = (flag ? "删除" : "恢复");
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
			this.filter.Refresh();
			this.Filter = new ItemFilter(this.filter.Text);
			this.tree.Filter = this.Filter;
			this.tree.RefreshList();
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.tree = new ChangeListTree(this.components);
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.openItem = new ToolStripMenuItem();
			this.revertItem = new ToolStripMenuItem();
			this.toolStripSeparator1 = new ToolStripSeparator();
			this.showNew = new ToolStripMenuItem();
			this.showChanged = new ToolStripMenuItem();
			this.showDeleted = new ToolStripMenuItem();
			this.filter = new TextBox();
			this.contextMenu.SuspendLayout();
			base.SuspendLayout();
			this.tree.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.tree.ContextMenuStrip = this.contextMenu;
			this.tree.Filter = null;
			this.tree.FullRowSelect = true;
			this.tree.GameData = null;
			this.tree.Location = new Point(2, 2);
			this.tree.Name = "tree";
			this.tree.Nodes = null;
			this.tree.OwnerDraw = true;
			this.tree.ShowChanged = true;
			this.tree.ShowDeleted = true;
			this.tree.ShowNew = true;
			this.tree.Size = new System.Drawing.Size(443, 550);
			this.tree.SortKey = null;
			this.tree.TabIndex = 0;
			this.tree.UseCompatibleStateImageBehavior = false;
			this.tree.View = View.Details;
			this.tree.VirtualMode = true;
			this.contextMenu.Items.AddRange(new ToolStripItem[] { this.openItem, this.revertItem, this.toolStripSeparator1, this.showNew, this.showChanged, this.showDeleted });
			this.contextMenu.Name = "contextMenu";
			this.contextMenu.Size = new System.Drawing.Size(155, 142);
			this.contextMenu.Opening += new CancelEventHandler(this.contextMenu_Opening);
			this.openItem.Name = "openItem";
			this.openItem.Size = new System.Drawing.Size(154, 22);
			this.openItem.Text = "打开";
			this.openItem.Click += new EventHandler(this.openItem_Click);
			this.revertItem.Name = "revertItem";
			this.revertItem.Size = new System.Drawing.Size(154, 22);
			this.revertItem.Text = "恢复";
			this.revertItem.Click += new EventHandler(this.revertItem_Click);
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(151, 6);
			this.showNew.Checked = true;
			this.showNew.CheckOnClick = true;
			this.showNew.CheckState = CheckState.Checked;
			this.showNew.Name = "showNew";
			this.showNew.Size = new System.Drawing.Size(154, 22);
			this.showNew.Text = "新项目";
			this.showNew.Click += new EventHandler(this.viewOptions_Click);
			this.showChanged.Checked = true;
			this.showChanged.CheckOnClick = true;
			this.showChanged.CheckState = CheckState.Checked;
			this.showChanged.Name = "showChanged";
			this.showChanged.Size = new System.Drawing.Size(154, 22);
			this.showChanged.Text = "已改变的项目";
			this.showChanged.Click += new EventHandler(this.viewOptions_Click);
			this.showDeleted.Checked = true;
			this.showDeleted.CheckOnClick = true;
			this.showDeleted.CheckState = CheckState.Checked;
			this.showDeleted.Name = "showDeleted";
			this.showDeleted.Size = new System.Drawing.Size(154, 22);
			this.showDeleted.Text = "已删除的项目";
			this.showDeleted.Click += new EventHandler(this.viewOptions_Click);
			this.filter.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.filter.Location = new Point(2, 554);
			this.filter.Name = "filter";
			this.filter.Size = new System.Drawing.Size(443, 20);
			this.filter.TabIndex = 1;
			this.filter.TextChanged += new EventHandler(this.filter_TextChanged);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(447, 575);
			base.Controls.Add(this.filter);
			base.Controls.Add(this.tree);
			base.Name = "ChangeList";
			base.ShowIcon = false;
			this.Text = "变更列表";
			this.contextMenu.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		private void openItem_Click(object sender, EventArgs e)
		{
			foreach (ChangeListTree.TreeNode selectedItem in this.tree.SelectedItems)
			{
				this.nav.showItemProperties(this.tree.GetItem(selectedItem));
			}
		}

		private void revert(GameData.Item item)
		{
			if (item.getState() != GameData.State.OWNED)
			{
				item.revert();
				return;
			}
			this.nav.ou.gameData.deleteItem(item);
			this.nav.refreshListView();
		}

		private void revertChange(ChangeListTree.ChangeData change)
		{
			GameData.TripleInt tripleInt;
			GameData.Item item = this.tree.GetItem(change);
			ChangeListTree.TreeNode parent = change.Parent;
			bool flag = false;
			switch (change.Type)
			{
				case ChangeListTree.ChangeType.NAME:
				{
					item.Name = item.OriginalName;
					break;
				}
				case ChangeListTree.ChangeType.VALUE:
				{
					if (change.OldValue != null)
					{
						item[change.Key] = change.OldValue;
					}
					else
					{
						item.Remove(change.Key);
					}
					item.refreshState();
					break;
				}
				case ChangeListTree.ChangeType.NEWREF:
				{
					item.removeReference(change.Section, change.Key);
					break;
				}
				case ChangeListTree.ChangeType.MODREF:
				{
					tripleInt = item.OriginalValue(change.Section, change.Key);
					item.setReferenceValue(change.Section, change.Key, tripleInt);
					break;
				}
				case ChangeListTree.ChangeType.DELREF:
				{
					tripleInt = item.OriginalValue(change.Section, change.Key);
					item.addReference(change.Section, change.Key, new int?(tripleInt.v0), new int?(tripleInt.v1), new int?(tripleInt.v2));
					break;
				}
				case ChangeListTree.ChangeType.NEWINST:
				{
					item.removeInstance(change.Key);
					break;
				}
				case ChangeListTree.ChangeType.MODINST:
				{
					item.getInstance(change.Key).revert();
					break;
				}
				case ChangeListTree.ChangeType.INSTVALUE:
				{
					item.getInstance(change.Section)[change.Key] = change.OldValue;
					if (change.Parent.Children.Count != 1)
					{
						break;
					}
					change = change.Parent as ChangeListTree.ChangeData;
					break;
				}
				case ChangeListTree.ChangeType.INVALIDREF:
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.tree.Remove(change);
			}
			if (item.getState() == GameData.State.ORIGINAL)
			{
				this.tree.Remove(change.Parent.Parent ?? change.Parent);
			}
			this.nav.refreshState(item);
			this.nav.HasChanges = true;
		}

		private void revertItem_Click(object sender, EventArgs e)
		{
			this.tree.BeginUpdate();
			if (this.tree.SelectedItems.Count > 10)
			{
				System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
			}
			foreach (ChangeListTree.TreeNode selectedItem in this.tree.SelectedItems)
			{
				if (!(selectedItem is ChangeListTree.RootNode))
				{
					this.revertChange(selectedItem as ChangeListTree.ChangeData);
				}
				else
				{
					this.revert(this.tree.GetItem(selectedItem));
					this.tree.Remove(selectedItem);
				}
			}
			this.nav.HasChanges = true;
			this.tree.EndUpdate();
			System.Windows.Forms.Cursor.Current = Cursors.Default;
		}

		private void Tree_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (this.tree.GetItemElementAt(e.X, e.Y) == ChangeListTree.ItemElement.TEXT)
			{
				ListViewItem itemAt = this.tree.GetItemAt(e.X, e.Y);
				GameData.Item item = this.tree.GetItem(itemAt.Tag as ChangeListTree.TreeNode);
				if (item != null)
				{
					this.nav.showItemProperties(item);
				}
			}
		}

		private void viewOptions_Click(object sender, EventArgs e)
		{
			this.tree.ShowChanged = this.showChanged.Checked;
			this.tree.ShowDeleted = this.showDeleted.Checked;
			this.tree.ShowNew = this.showNew.Checked;
			this.tree.RefreshList();
		}
	}
}