using forgotten_construction_set.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace forgotten_construction_set
{
	public class AssetList : Form
	{
		private navigation nav;

		private ListViewItem[] itemCache;

		private int cacheOffset;

		private IContainer components;

		private ListView view;

		private System.Windows.Forms.ContextMenuStrip contextMenu;

		private ToolStripMenuItem openFile;

		private ToolStripMenuItem showItem;

		private ToolStripSeparator toolStripSeparator1;

		private ToolStripMenuItem showOriginal;

		private ToolStripMenuItem showModified;

		private ColumnHeader column;

		private TextBox filterText;

		private ToolStripMenuItem openFolder;

		private List<AssetList.Asset> Assets
		{
			get;
			set;
		}

		public AssetList(navigation nav)
		{
			this.InitializeComponent();
			base.ShowIcon = true;
			base.Icon = System.Drawing.Icon.FromHandle(Resources.AppRoot_16x.GetHicon());
			this.nav = nav;
			this.view.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(this.view_RetrieveVirtualItem);
			this.view.CacheVirtualItems += new CacheVirtualItemsEventHandler(this.view_CacheVirtualItems);
			this.view.VirtualMode = true;
			this.Assets = new List<AssetList.Asset>();
			this.refreshList();
		}

		private void AssetList_Resize(object sender, EventArgs e)
		{
			this.column.Width = this.view.ClientSize.Width;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void filterText_TextChanged(object sender, EventArgs e)
		{
			this.filterText.BackColor = (this.filterText.Text == "" ? Color.White : Color.Yellow);
			this.refreshList();
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.view = new ListView();
			this.column = new ColumnHeader();
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.openFile = new ToolStripMenuItem();
			this.openFolder = new ToolStripMenuItem();
			this.showItem = new ToolStripMenuItem();
			this.toolStripSeparator1 = new ToolStripSeparator();
			this.showOriginal = new ToolStripMenuItem();
			this.showModified = new ToolStripMenuItem();
			this.filterText = new TextBox();
			this.contextMenu.SuspendLayout();
			base.SuspendLayout();
			this.view.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.view.Columns.AddRange(new ColumnHeader[] { this.column });
			this.view.ContextMenuStrip = this.contextMenu;
			this.view.HeaderStyle = ColumnHeaderStyle.None;
			this.view.Location = new Point(0, 0);
			this.view.Name = "view";
			this.view.Size = new System.Drawing.Size(284, 242);
			this.view.Sorting = SortOrder.Ascending;
			this.view.TabIndex = 0;
			this.view.UseCompatibleStateImageBehavior = false;
			this.view.View = View.Details;
			this.view.KeyPress += new KeyPressEventHandler(this.view_KeyPress);
			this.view.MouseDoubleClick += new MouseEventHandler(this.view_MouseDoubleClick);
			this.column.Text = "文件";
			this.column.Width = 278;
			this.contextMenu.Items.AddRange(new ToolStripItem[] { this.openFile, this.openFolder, this.showItem, this.toolStripSeparator1, this.showOriginal, this.showModified });
			this.contextMenu.Name = "contextMenu";
			this.contextMenu.Size = new System.Drawing.Size(153, 142);
			this.openFile.Name = "openFile";
			this.openFile.Size = new System.Drawing.Size(152, 22);
			this.openFile.Text = "打开文件";
			this.openFile.Click += new EventHandler(this.openFile_Click);
			this.openFolder.Name = "openFolder";
			this.openFolder.Size = new System.Drawing.Size(152, 22);
			this.openFolder.Text = "打开文件夹";
			this.openFolder.Click += new EventHandler(this.openFolder_Click);
			this.showItem.Name = "showItem";
			this.showItem.Size = new System.Drawing.Size(152, 22);
			this.showItem.Text = "显示项目";
			this.showItem.Click += new EventHandler(this.showItem_Click);
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
			this.showOriginal.Checked = true;
			this.showOriginal.CheckOnClick = true;
			this.showOriginal.CheckState = CheckState.Checked;
			this.showOriginal.Name = "showOriginal";
			this.showOriginal.Size = new System.Drawing.Size(152, 22);
			this.showOriginal.Text = "原来的";
			this.showOriginal.Click += new EventHandler(this.viewOptions_Click);
			this.showModified.Checked = true;
			this.showModified.CheckOnClick = true;
			this.showModified.CheckState = CheckState.Checked;
			this.showModified.Name = "showModified";
			this.showModified.Size = new System.Drawing.Size(152, 22);
			this.showModified.Text = "修改过的";
			this.showModified.Click += new EventHandler(this.viewOptions_Click);
			this.filterText.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.filterText.Location = new Point(0, 243);
			this.filterText.Name = "filterText";
			this.filterText.Size = new System.Drawing.Size(284, 20);
			this.filterText.TabIndex = 1;
			this.filterText.TextChanged += new EventHandler(this.filterText_TextChanged);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(284, 262);
			base.Controls.Add(this.filterText);
			base.Controls.Add(this.view);
			base.Name = "AssetList";
			base.ShowIcon = false;
			this.Text = "资源";
			base.Resize += new EventHandler(this.AssetList_Resize);
			this.contextMenu.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public static bool OpenFile(string file)
		{
			bool flag;
			if (!file.StartsWith("."))
			{
				return false;
			}
			try
			{
				Process.Start(file);
				flag = true;
			}
			catch (Exception exception)
			{
				MessageBox.Show(string.Concat("File not found: ", file), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				flag = false;
			}
			return flag;
		}

		private void openFile_Click(object sender, EventArgs e)
		{
			AssetList.OpenFile(this.Assets[this.view.SelectedIndices[0]].file);
		}

		public static bool OpenFolder(string file)
		{
			bool flag;
			if (!file.StartsWith("."))
			{
				return false;
			}
			string str = file.Remove(file.LastIndexOf('\\'));
			try
			{
				if (!File.Exists(file))
				{
					Process.Start(new ProcessStartInfo()
					{
						FileName = str,
						UseShellExecute = true,
						Verb = "open"
					});
				}
				else
				{
					Process.Start("explorer.exe", string.Format("/select,\"{0}\"", file));
				}
				flag = true;
			}
			catch (Exception exception)
			{
				MessageBox.Show(string.Concat("Directory does not exist: ", str), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				flag = false;
			}
			return flag;
		}

		private void openFolder_Click(object sender, EventArgs e)
		{
			AssetList.OpenFolder(this.Assets[this.view.SelectedIndices[0]].file);
		}

		public void refreshList()
		{
			this.Assets.Clear();
			string str = this.filterText.Text.Trim();
			foreach (GameData.Item value in this.nav.ou.gameData.items.Values)
			{
				foreach (KeyValuePair<string, object> keyValuePair in value)
				{
					if (!(keyValuePair.Value is GameData.File) || string.IsNullOrWhiteSpace(keyValuePair.Value.ToString()))
					{
						continue;
					}
					GameData.State state = value.getState(keyValuePair.Key);
					if (state == GameData.State.ORIGINAL && !this.showOriginal.Checked || state == GameData.State.MODIFIED && !this.showModified.Checked)
					{
						continue;
					}
					AssetList.Asset asset = null;
					string str1 = keyValuePair.Value.ToString();
					if (!string.IsNullOrWhiteSpace(str) && CultureInfo.InvariantCulture.CompareInfo.IndexOf(str1, str, CompareOptions.IgnoreCase) < 0)
					{
						continue;
					}
					foreach (AssetList.Asset asset1 in this.Assets)
					{
						if (asset1.file != str1)
						{
							continue;
						}
						asset = asset1;
						goto Label0;
					}
				Label0:
					if (asset != null)
					{
						asset.count++;
					}
					else
					{
						this.Assets.Add(new AssetList.Asset(keyValuePair.Key, str1, value));
					}
				}
			}
			this.Assets = (
				from o in this.Assets
				orderby o.file
				select o).ToList<AssetList.Asset>();
			this.view.VirtualListSize = this.Assets.Count<AssetList.Asset>();
			this.view.SelectedIndices.Clear();
			this.itemCache = null;
			base.Invalidate();
		}

		private ListViewItem RetrieveVirtualItem(int index)
		{
			if (this.itemCache != null && index >= this.cacheOffset && index < this.cacheOffset + (int)this.itemCache.Length)
			{
				return this.itemCache[index - this.cacheOffset];
			}
			AssetList.Asset item = this.Assets[index];
			ListViewItem listViewItem = new ListViewItem()
			{
				Text = item.file,
				ForeColor = StateColours.GetStateColor(item.item.getState(item.key))
			};
			if (!File.Exists(item.file))
			{
				listViewItem.ForeColor = Color.Red;
			}
			return listViewItem;
		}

		private void showItem_Click(object sender, EventArgs e)
		{
			foreach (int selectedIndex in this.view.SelectedIndices)
			{
				this.nav.showItemProperties(this.Assets[selectedIndex].item);
			}
		}

		private void view_CacheVirtualItems(object sender, CacheVirtualItemsEventArgs e)
		{
			if (this.itemCache != null && e.StartIndex >= this.cacheOffset && e.EndIndex <= this.cacheOffset + (int)this.itemCache.Length)
			{
				return;
			}
			int endIndex = e.EndIndex - e.StartIndex + 1;
			ListViewItem[] listViewItemArray = new ListViewItem[endIndex];
			for (int i = 0; i < endIndex; i++)
			{
				listViewItemArray[i] = this.RetrieveVirtualItem(i + e.StartIndex);
			}
			this.itemCache = listViewItemArray;
			this.cacheOffset = e.StartIndex;
		}

		private void view_KeyPress(object sender, KeyPressEventArgs e)
		{
			this.filterText.Focus();
			this.filterText.Text = e.KeyChar.ToString();
			this.filterText.Select(1, 0);
			e.Handled = true;
		}

		private void view_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.showItem_Click(sender, new EventArgs());
		}

		private void view_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
		{
			e.Item = this.RetrieveVirtualItem(e.ItemIndex);
		}

		private void viewOptions_Click(object sender, EventArgs e)
		{
			this.refreshList();
		}

		private class Asset
		{
			public string key;

			public string file;

			public int count;

			public GameData.Item item;

			public Asset(string k, string f, GameData.Item i)
			{
				this.key = k;
				this.file = f;
				this.item = i;
				this.count = 1;
			}
		}
	}
}