using forgotten_construction_set.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace forgotten_construction_set
{
	public class ToDo : Form
	{
		private navigation nav;

		private ListViewItem currentItem;

		private List<string> assignList;

		private string activeFile;

		private bool assignListSetup;

		private const string PREFIX = "data/__TODO__";

		private const string POSTFIX = ".txt";

		private Dictionary<string, ToDo.Item> items = new Dictionary<string, ToDo.Item>();

		private IContainer components;

		private ListView listView;

		private ColumnHeader columnState;

		private ColumnHeader columnPriority;

		private ColumnHeader columnItem;

		private ColumnHeader columnDesc;

		private ColumnHeader columnType;

		private System.Windows.Forms.ContextMenuStrip contextMenu;

		private ToolStripMenuItem openItemToolStripMenuItem;

		private ToolStripMenuItem stateToolStripMenuItem;

		private ToolStripMenuItem newToolStripMenuItem;

		private ToolStripMenuItem updatedToolStripMenuItem;

		private ToolStripMenuItem completeToolStripMenuItem;

		private ToolStripMenuItem priorityToolStripMenuItem;

		private ToolStripMenuItem highToolStripMenuItem;

		private ToolStripMenuItem mediumToolStripMenuItem;

		private ToolStripMenuItem lowToolStripMenuItem;

		private TextBox editDescription;

		private TextBox lockedDescription;

		private SplitContainer splitContainer1;

		private ToolStripSeparator toolStripSeparator1;

		private ToolStripMenuItem allMods;

		private ToolStripMenuItem completedItems;

		private ColumnHeader columnAssigned;

		private ToolStripMenuItem assignToToolStripMenuItem;

		private ToolStripMenuItem meToolStripMenuItem;

		public ToDo(Form parent, navigation nav)
		{
			base.MdiParent = parent;
			this.nav = nav;
			this.InitializeComponent();
			this.listView.ListViewItemSorter = new ToDo.ItemComparer();
			this.listView.EnableDoubleBuferring();
			this.assignList = new List<string>()
			{
				"-"
			};
		}

		public void AddItem(GameData.Item target, string description = "")
		{
			ToDo.Item item;
			if (this.items.ContainsKey(target.stringID))
			{
				item = this.items[target.stringID];
				if (item.active.description != null)
				{
					ToDo.SubItem subItem = item.active;
					subItem.description = string.Concat(subItem.description, description);
				}
				else
				{
					item.Description = "";
				}
				if (!this.listView.Items.ContainsKey(target.stringID))
				{
					this.addListItem(target.stringID, item);
				}
			}
			else
			{
				item = new ToDo.Item();
				this.items[target.stringID] = item;
				item.Description = description;
				item.item = target;
				this.addListItem(target.stringID, item);
				this.nav.HasChanges = true;
			}
			base.Show();
			this.listView.SelectedItems.Clear();
			int num = this.listView.Items.IndexOfKey(target.stringID);
			this.listView.EnsureVisible(num);
			this.listView.Items[num].Selected = true;
		}

		private void addListItem(string key, ToDo.Item data)
		{
			string str = (data.item == null ? key : data.item.Name);
			itemType _itemType = (data.item == null ? itemType.NULL_ITEM : data.item.type);
			ListViewItem listViewItem = this.listView.Items.Add(key, str, 0);
			listViewItem.SubItems.Add(_itemType.ToString());
			listViewItem.SubItems.Add(data.State.ToString());
			listViewItem.SubItems.Add(data.Priority.ToString());
			listViewItem.SubItems.Add(this.assignList[data.Assigned]);
			if (data.Description != null)
			{
				listViewItem.SubItems.Add(data.Description.Replace("\n", "  "));
			}
			listViewItem.Tag = data;
		}

		private void allMods_Click(object sender, EventArgs e)
		{
			this.updateList();
		}

		private void assignToolStripMenuItem_Click(object sender, EventArgs e)
		{
			int num = this.assignList.IndexOf((sender as ToolStripMenuItem).Text);
			if (num < 0)
			{
				num = 0;
			}
			foreach (ListViewItem selectedItem in this.listView.SelectedItems)
			{
				(selectedItem.Tag as ToDo.Item).Assigned = num;
				selectedItem.SubItems[4].Text = this.assignList[num];
			}
			this.nav.HasChanges = true;
		}

		public void Clear()
		{
			this.items.Clear();
			this.activeFile = null;
			this.assignList.Clear();
			this.assignList.Add("-");
			this.listView.Items.Clear();
			this.assignListSetup = false;
			base.Hide();
		}

		private void completedItems_Click(object sender, EventArgs e)
		{
			this.updateList();
		}

		private void completeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem selectedItem in this.listView.SelectedItems)
			{
				this.setState(selectedItem, ToDo.State.DONE);
			}
		}

		private void contextMenu_Opening(object sender, CancelEventArgs e)
		{
			bool count = this.listView.SelectedIndices.Count > 0;
			this.openItemToolStripMenuItem.Enabled = count;
			this.stateToolStripMenuItem.Enabled = count;
			this.priorityToolStripMenuItem.Enabled = count;
			this.assignToToolStripMenuItem.Enabled = count;
			if (count)
			{
				this.completeToolStripMenuItem.Checked = false;
				this.newToolStripMenuItem.Checked = false;
				this.updatedToolStripMenuItem.Checked = false;
				this.highToolStripMenuItem.Checked = false;
				this.mediumToolStripMenuItem.Checked = false;
				this.lowToolStripMenuItem.Checked = false;
				foreach (ListViewItem selectedItem in this.listView.SelectedItems)
				{
					switch ((selectedItem.Tag as ToDo.Item).State)
					{
						case ToDo.State.NEW:
						{
							this.newToolStripMenuItem.Checked = true;
							break;
						}
						case ToDo.State.UPDATED:
						{
							this.updatedToolStripMenuItem.Checked = true;
							break;
						}
						case ToDo.State.DONE:
						{
							this.completeToolStripMenuItem.Checked = true;
							break;
						}
					}
					switch ((selectedItem.Tag as ToDo.Item).Priority)
					{
						case ToDo.Priority.LOW:
						{
							this.lowToolStripMenuItem.Checked = true;
							continue;
						}
						case ToDo.Priority.MEDIUM:
						{
							this.mediumToolStripMenuItem.Checked = true;
							continue;
						}
						case ToDo.Priority.HIGH:
						{
							this.highToolStripMenuItem.Checked = true;
							continue;
						}
						default:
						{
							continue;
						}
					}
				}
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

		private void editDescription_TextChanged(object sender, EventArgs e)
		{
			if (this.currentItem != null)
			{
				ToDo.Item tag = this.currentItem.Tag as ToDo.Item;
				tag.Description = this.editDescription.Text;
				this.currentItem.SubItems[5].Text = tag.Description.Replace("\n", "  ");
				if (tag.State == ToDo.State.DONE)
				{
					this.setState(this.currentItem, ToDo.State.UPDATED);
				}
				this.nav.HasChanges = true;
			}
		}

		private void highToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem selectedItem in this.listView.SelectedItems)
			{
				this.setPriority(selectedItem, ToDo.Priority.HIGH);
			}
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.listView = new ListView();
			this.columnItem = new ColumnHeader();
			this.columnType = new ColumnHeader();
			this.columnState = new ColumnHeader();
			this.columnPriority = new ColumnHeader();
			this.columnAssigned = new ColumnHeader();
			this.columnDesc = new ColumnHeader();
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.openItemToolStripMenuItem = new ToolStripMenuItem();
			this.stateToolStripMenuItem = new ToolStripMenuItem();
			this.newToolStripMenuItem = new ToolStripMenuItem();
			this.updatedToolStripMenuItem = new ToolStripMenuItem();
			this.completeToolStripMenuItem = new ToolStripMenuItem();
			this.priorityToolStripMenuItem = new ToolStripMenuItem();
			this.highToolStripMenuItem = new ToolStripMenuItem();
			this.mediumToolStripMenuItem = new ToolStripMenuItem();
			this.lowToolStripMenuItem = new ToolStripMenuItem();
			this.assignToToolStripMenuItem = new ToolStripMenuItem();
			this.meToolStripMenuItem = new ToolStripMenuItem();
			this.toolStripSeparator1 = new ToolStripSeparator();
			this.allMods = new ToolStripMenuItem();
			this.completedItems = new ToolStripMenuItem();
			this.editDescription = new TextBox();
			this.lockedDescription = new TextBox();
			this.splitContainer1 = new SplitContainer();
			this.contextMenu.SuspendLayout();
			((ISupportInitialize)this.splitContainer1).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			base.SuspendLayout();
			this.listView.AllowColumnReorder = true;
			this.listView.Columns.AddRange(new ColumnHeader[] { this.columnItem, this.columnType, this.columnState, this.columnPriority, this.columnAssigned, this.columnDesc });
			this.listView.ContextMenuStrip = this.contextMenu;
			this.listView.Dock = DockStyle.Fill;
			this.listView.FullRowSelect = true;
			this.listView.HideSelection = false;
			this.listView.Location = new Point(0, 0);
			this.listView.Name = "listView";
			this.listView.ShowItemToolTips = true;
			this.listView.Size = new System.Drawing.Size(924, 312);
			this.listView.TabIndex = 0;
			this.listView.UseCompatibleStateImageBehavior = false;
			this.listView.View = View.Details;
			this.listView.ColumnClick += new ColumnClickEventHandler(this.listView_ColumnClick);
			this.listView.SelectedIndexChanged += new EventHandler(this.listView_SelectedIndexChanged);
			this.listView.SizeChanged += new EventHandler(this.listView_SizeChanged);
			this.listView.DoubleClick += new EventHandler(this.openItemToolStripMenuItem_Click);
			this.columnItem.Text = "Item";
			this.columnItem.Width = 120;
			this.columnType.Text = "Type";
			this.columnType.Width = 80;
			this.columnState.Text = "State";
			this.columnPriority.Text = "Priority";
			this.columnAssigned.Text = "Assigned To";
			this.columnAssigned.Width = 80;
			this.columnDesc.Text = "Description";
			this.columnDesc.Width = 500;
			this.contextMenu.Items.AddRange(new ToolStripItem[] { this.openItemToolStripMenuItem, this.stateToolStripMenuItem, this.priorityToolStripMenuItem, this.assignToToolStripMenuItem, this.toolStripSeparator1, this.allMods, this.completedItems });
			this.contextMenu.Name = "contextMenu";
			this.contextMenu.Size = new System.Drawing.Size(170, 142);
			this.contextMenu.Opening += new CancelEventHandler(this.contextMenu_Opening);
			this.openItemToolStripMenuItem.Name = "openItemToolStripMenuItem";
			this.openItemToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
			this.openItemToolStripMenuItem.Text = "Open Item";
			this.openItemToolStripMenuItem.Click += new EventHandler(this.openItemToolStripMenuItem_Click);
			this.stateToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.newToolStripMenuItem, this.updatedToolStripMenuItem, this.completeToolStripMenuItem });
			this.stateToolStripMenuItem.Name = "stateToolStripMenuItem";
			this.stateToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
			this.stateToolStripMenuItem.Text = "State";
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
			this.newToolStripMenuItem.Text = "New";
			this.newToolStripMenuItem.Click += new EventHandler(this.newToolStripMenuItem_Click);
			this.updatedToolStripMenuItem.Name = "updatedToolStripMenuItem";
			this.updatedToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
			this.updatedToolStripMenuItem.Text = "Updated";
			this.updatedToolStripMenuItem.Click += new EventHandler(this.updatedToolStripMenuItem_Click);
			this.completeToolStripMenuItem.Name = "completeToolStripMenuItem";
			this.completeToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
			this.completeToolStripMenuItem.Text = "Complete";
			this.completeToolStripMenuItem.Click += new EventHandler(this.completeToolStripMenuItem_Click);
			this.priorityToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.highToolStripMenuItem, this.mediumToolStripMenuItem, this.lowToolStripMenuItem });
			this.priorityToolStripMenuItem.Name = "priorityToolStripMenuItem";
			this.priorityToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
			this.priorityToolStripMenuItem.Text = "Priority";
			this.highToolStripMenuItem.Name = "highToolStripMenuItem";
			this.highToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
			this.highToolStripMenuItem.Text = "High";
			this.highToolStripMenuItem.Click += new EventHandler(this.highToolStripMenuItem_Click);
			this.mediumToolStripMenuItem.Name = "mediumToolStripMenuItem";
			this.mediumToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
			this.mediumToolStripMenuItem.Text = "Medium";
			this.mediumToolStripMenuItem.Click += new EventHandler(this.mediumToolStripMenuItem_Click);
			this.lowToolStripMenuItem.Name = "lowToolStripMenuItem";
			this.lowToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
			this.lowToolStripMenuItem.Text = "Low";
			this.lowToolStripMenuItem.Click += new EventHandler(this.lowToolStripMenuItem_Click);
			this.assignToToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.meToolStripMenuItem });
			this.assignToToolStripMenuItem.Name = "assignToToolStripMenuItem";
			this.assignToToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
			this.assignToToolStripMenuItem.Text = "Assign To";
			this.meToolStripMenuItem.Name = "meToolStripMenuItem";
			this.meToolStripMenuItem.Size = new System.Drawing.Size(91, 22);
			this.meToolStripMenuItem.Text = "Me";
			this.meToolStripMenuItem.Click += new EventHandler(this.assignToolStripMenuItem_Click);
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(166, 6);
			this.allMods.CheckOnClick = true;
			this.allMods.Name = "allMods";
			this.allMods.Size = new System.Drawing.Size(169, 22);
			this.allMods.Text = "Items for all mods";
			this.allMods.Click += new EventHandler(this.allMods_Click);
			this.completedItems.CheckOnClick = true;
			this.completedItems.Name = "completedItems";
			this.completedItems.Size = new System.Drawing.Size(169, 22);
			this.completedItems.Text = "Completed Items";
			this.completedItems.Click += new EventHandler(this.completedItems_Click);
			this.editDescription.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.editDescription.Location = new Point(3, 33);
			this.editDescription.Multiline = true;
			this.editDescription.Name = "editDescription";
			this.editDescription.ScrollBars = ScrollBars.Vertical;
			this.editDescription.Size = new System.Drawing.Size(918, 161);
			this.editDescription.TabIndex = 3;
			this.editDescription.TextChanged += new EventHandler(this.editDescription_TextChanged);
			this.editDescription.MouseDoubleClick += new MouseEventHandler(this.lockedDescription_MouseDoubleClick);
			this.lockedDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.lockedDescription.Location = new Point(3, 3);
			this.lockedDescription.Multiline = true;
			this.lockedDescription.Name = "lockedDescription";
			this.lockedDescription.ReadOnly = true;
			this.lockedDescription.Size = new System.Drawing.Size(918, 24);
			this.lockedDescription.TabIndex = 4;
			this.lockedDescription.MouseDoubleClick += new MouseEventHandler(this.lockedDescription_MouseDoubleClick);
			this.splitContainer1.Dock = DockStyle.Fill;
			this.splitContainer1.Location = new Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = Orientation.Horizontal;
			this.splitContainer1.Panel1.Controls.Add(this.listView);
			this.splitContainer1.Panel2.Controls.Add(this.editDescription);
			this.splitContainer1.Panel2.Controls.Add(this.lockedDescription);
			this.splitContainer1.Size = new System.Drawing.Size(924, 513);
			this.splitContainer1.SplitterDistance = 312;
			this.splitContainer1.TabIndex = 5;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(924, 513);
			base.Controls.Add(this.splitContainer1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			base.Name = "ToDo";
			this.Text = "Todo List";
			base.FormClosing += new FormClosingEventHandler(this.ToDo_FormClosing);
			base.Shown += new EventHandler(this.ToDo_Shown);
			this.contextMenu.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((ISupportInitialize)this.splitContainer1).EndInit();
			this.splitContainer1.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		private void listView_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			ToDo.ItemComparer listViewItemSorter = this.listView.ListViewItemSorter as ToDo.ItemComparer;
			if (listViewItemSorter.column == e.Column)
			{
				listViewItemSorter.direction = -listViewItemSorter.direction;
			}
			listViewItemSorter.column = e.Column;
			this.listView.Sort();
		}

		private void listView_SelectedIndexChanged(object sender, EventArgs e)
		{
			System.Drawing.Size clientSize;
			this.currentItem = null;
			if (this.listView.SelectedItems.Count != 1)
			{
				this.editDescription.Text = "";
				this.lockedDescription.Visible = false;
				this.editDescription.ReadOnly = true;
				TextBox height = this.editDescription;
				clientSize = this.editDescription.Parent.ClientSize;
				height.Height = clientSize.Height;
				this.editDescription.Top = 0;
				return;
			}
			ToDo.Item tag = this.listView.SelectedItems[0].Tag as ToDo.Item;
			if (tag.locked.description != null)
			{
				this.lockedDescription.Visible = true;
				this.lockedDescription.Text = tag.locked.description;
				System.Drawing.Size size = TextRenderer.MeasureText(this.lockedDescription.Text, this.lockedDescription.Font);
				this.lockedDescription.Height = size.Height + 8;
				TextBox textBox = this.editDescription;
				clientSize = this.editDescription.Parent.ClientSize;
				textBox.Height = clientSize.Height - this.lockedDescription.Height;
				this.editDescription.Top = this.lockedDescription.Height + 4;
			}
			else
			{
				this.lockedDescription.Visible = false;
				TextBox height1 = this.editDescription;
				clientSize = this.editDescription.Parent.ClientSize;
				height1.Height = clientSize.Height;
				this.editDescription.Top = 0;
			}
			this.editDescription.Text = (tag.active == null || tag.active.description == null ? "" : tag.active.description);
			this.editDescription.ReadOnly = false;
			this.currentItem = this.listView.SelectedItems[0];
			this.editDescription.Focus();
		}

		private void listView_SizeChanged(object sender, EventArgs e)
		{
			this.listView.Columns[this.listView.Columns.Count - 1].Width = -2;
		}

		public void LoadAll()
		{
			this.activeFile = string.Concat("data/__TODO__", Environment.UserName, ".txt");
			string[] files = Directory.GetFiles("data", "*.txt");
			for (int i = 0; i < (int)files.Length; i++)
			{
				string str = files[i].Replace("\\", "/");
				if (str.StartsWith("data/__TODO__"))
				{
					this.LoadData(str, str == this.activeFile);
				}
			}
			this.updateList();
		}

		public void LoadData(string file, bool active)
		{
			ToDo.Item item;
			string str;
			if (active)
			{
				this.activeFile = file;
			}
			if (File.Exists(file))
			{
				StreamReader streamReader = new StreamReader(file);
				string str1 = streamReader.ReadLine();
				List<int> nums = null;
				if (str1 != null && str1.StartsWith("MODS: "))
				{
					nums = new List<int>();
					string[] strArrays = str1.Substring(6).Split(new char[] { ',' });
					for (int i = 0; i < (int)strArrays.Length; i++)
					{
						string str2 = strArrays[i];
						if (!this.assignList.Contains(str2))
						{
							this.assignList.Add(str2);
						}
						nums.Add(this.assignList.IndexOf(str2));
					}
				}
				while (true)
				{
					string str3 = streamReader.ReadLine();
					str1 = str3;
					if (str3 == null)
					{
						break;
					}
					int num = str1.IndexOf("||");
					if (num > 0)
					{
						str = str1.Substring(num + 2);
					}
					else
					{
						str = null;
					}
					string str4 = str;
					string[] strArrays1 = (num > 0 ? str1.Substring(0, num).Split(new char[] { '|' }) : str1.Split(new char[] { '|' }));
					if (!this.items.ContainsKey(strArrays1[0]))
					{
						Dictionary<string, ToDo.Item> strs = this.items;
						string str5 = strArrays1[0];
						ToDo.Item item1 = new ToDo.Item();
						ToDo.Item item2 = item1;
						strs[str5] = item1;
						item = item2;
					}
					else
					{
						item = this.items[strArrays1[0]];
					}
					if (active && item.active == null)
					{
						item.active = new ToDo.SubItem();
					}
					ToDo.SubItem subItem = (active ? item.active : item.locked);
					subItem.state += int.Parse(strArrays1[1]);
					subItem.priority += int.Parse(strArrays1[2]);
					if (str4 != null)
					{
						str4 = str4.Replace("<br>", "\n");
						if (subItem.description != null)
						{
							ToDo.SubItem subItem1 = subItem;
							subItem1.description = string.Concat(subItem1.description, "\n", str4);
						}
						else
						{
							subItem.description = str4;
						}
					}
					if ((int)strArrays1.Length > 3 && nums != null)
					{
						string[] strArrays2 = strArrays1[3].Split(new char[] { '.' });
						int num1 = int.Parse(strArrays2[0]);
						if (num1 > subItem.assignedPrecidence)
						{
							subItem.assigned = nums[int.Parse(strArrays2[1])];
							subItem.assignedPrecidence = num1;
						}
					}
				}
				streamReader.Close();
				this.resolveItems();
			}
		}

		private void lockedDescription_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			int i;
			char text;
			TextBox textBox = sender as TextBox;
			string str = "";
			List<char> chrs = new List<char>(new char[] { ' ', '\t', '\n', '\r', '[', ']', '{', '}' });
			for (i = textBox.SelectionStart - 1; i >= 0 && !chrs.Contains(textBox.Text[i]); i--)
			{
				text = textBox.Text[i];
				str = string.Concat(text.ToString(), str);
			}
			for (i = textBox.SelectionStart; i < textBox.Text.Length && !chrs.Contains(textBox.Text[i]); i++)
			{
				text = textBox.Text[i];
				str = string.Concat(str, text.ToString());
			}
			if (str.EndsWith(".") || str.EndsWith(","))
			{
				str = str.Substring(0, str.Length - 1);
			}
			GameData.Item item = this.nav.ou.gameData.getItem(str);
			if (item != null)
			{
				this.nav.showItemProperties(item);
			}
		}

		private void lowToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem selectedItem in this.listView.SelectedItems)
			{
				this.setPriority(selectedItem, ToDo.Priority.LOW);
			}
		}

		private void mediumToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem selectedItem in this.listView.SelectedItems)
			{
				this.setPriority(selectedItem, ToDo.Priority.MEDIUM);
			}
		}

		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem selectedItem in this.listView.SelectedItems)
			{
				this.setState(selectedItem, ToDo.State.NEW);
			}
		}

		private void openItemToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem selectedItem in this.listView.SelectedItems)
			{
				this.nav.showItemProperties((selectedItem.Tag as ToDo.Item).item);
			}
		}

		public void resolveItems()
		{
			foreach (KeyValuePair<string, ToDo.Item> item in this.items)
			{
				item.Value.item = this.nav.ou.gameData.getItem(item.Key);
			}
		}

		public void SaveData()
		{
			if (this.activeFile == null)
			{
				return;
			}
			List<string> strs = new List<string>();
			SortedList<int, int> nums = new SortedList<int, int>();
			List<KeyValuePair<string, ToDo.Item>> keyValuePairs = new List<KeyValuePair<string, ToDo.Item>>();
			foreach (KeyValuePair<string, ToDo.Item> item in this.items)
			{
				ToDo.SubItem value = item.Value.active;
				if (value == null || (value.description == null || value.description.Length <= 0) && value.priority == 0 && value.state == 0)
				{
					continue;
				}
				keyValuePairs.Add(item);
				if (value.assignedPrecidence <= item.Value.locked.assignedPrecidence || nums.ContainsKey(value.assigned))
				{
					continue;
				}
				strs.Add(this.assignList[value.assigned]);
				nums[value.assigned] = strs.Count - 1;
			}
			if (keyValuePairs.Count > 0)
			{
				StreamWriter streamWriter = new StreamWriter(this.activeFile);
				streamWriter.WriteLine(string.Concat("MODS: ", string.Join(",", strs)));
				foreach (KeyValuePair<string, ToDo.Item> keyValuePair in keyValuePairs)
				{
					ToDo.SubItem subItem = keyValuePair.Value.active;
					string str = string.Concat(new object[] { keyValuePair.Key, "|", subItem.state, "|", subItem.priority });
					if (subItem.assignedPrecidence > keyValuePair.Value.locked.assignedPrecidence)
					{
						str = string.Concat(new object[] { str, "|", subItem.assignedPrecidence, ".", nums[subItem.assigned] });
					}
					if (subItem.description != null)
					{
						str = string.Concat(str, "||", subItem.description.Replace("\n", "<br>").Replace("\r", ""));
					}
					streamWriter.WriteLine(str);
				}
				streamWriter.Close();
			}
		}

		private void setPriority(ListViewItem item, ToDo.Priority priority)
		{
			(item.Tag as ToDo.Item).Priority = priority;
			item.SubItems[3].Text = priority.ToString();
			this.nav.HasChanges = true;
		}

		private void setState(ListViewItem item, ToDo.State state)
		{
			(item.Tag as ToDo.Item).State = state;
			item.SubItems[2].Text = state.ToString();
			this.nav.HasChanges = true;
		}

		public void setupAssignMenu()
		{
			this.assignToToolStripMenuItem.DropDownItems.Clear();
			foreach (string str in this.assignList)
			{
				ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(str, null, new EventHandler(this.assignToolStripMenuItem_Click));
				this.assignToToolStripMenuItem.DropDownItems.Add(toolStripMenuItem);
			}
		}

		private void ToDo_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == System.Windows.Forms.CloseReason.UserClosing)
			{
				e.Cancel = true;
				base.Hide();
			}
		}

		private void ToDo_Shown(object sender, EventArgs e)
		{
			if (!this.assignListSetup)
			{
				this.setupAssignMenu();
			}
		}

		private void updatedToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem selectedItem in this.listView.SelectedItems)
			{
				this.setState(selectedItem, ToDo.State.UPDATED);
			}
		}

		public void updateList()
		{
			int num = this.assignList.IndexOf(this.nav.ou.gameData.activeFileName);
			this.listView.Items.Clear();
			this.listView.BeginUpdate();
			foreach (KeyValuePair<string, ToDo.Item> item in this.items)
			{
				if (!this.allMods.Checked && item.Value.Assigned != num || !this.completedItems.Checked && item.Value.State == ToDo.State.DONE)
				{
					continue;
				}
				this.addListItem(item.Key, item.Value);
			}
			this.listView.EndUpdate();
		}

		private class Item
		{
			public ToDo.SubItem locked;

			public ToDo.SubItem active;

			public GameData.Item item;

			public int Assigned
			{
				get
				{
					if (this.active != null && this.active.assignedPrecidence > this.locked.assignedPrecidence)
					{
						return this.active.assigned;
					}
					return this.locked.assigned;
				}
				set
				{
					if (this.active == null)
					{
						this.active = new ToDo.SubItem();
					}
					this.active.assigned = value;
					this.active.assignedPrecidence = this.locked.assignedPrecidence + 1;
				}
			}

			public string Description
			{
				get
				{
					if (this.active == null || this.active.description == null)
					{
						return this.locked.description;
					}
					if (this.locked.description == null)
					{
						return this.active.description;
					}
					return string.Concat(this.locked.description, "\n", this.active.description);
				}
				set
				{
					if (this.active == null)
					{
						this.active = new ToDo.SubItem();
					}
					this.active.description = value;
				}
			}

			public ToDo.Priority Priority
			{
				get
				{
					int num = this.locked.priority;
					if (this.active != null)
					{
						num += this.active.priority;
					}
					if (num < 1)
					{
						return ToDo.Priority.LOW;
					}
					if (num != 1)
					{
						return ToDo.Priority.HIGH;
					}
					return ToDo.Priority.MEDIUM;
				}
				set
				{
					if (this.active == null)
					{
						this.active = new ToDo.SubItem();
					}
					this.active.priority = (int)value - this.locked.priority;
				}
			}

			public ToDo.State State
			{
				get
				{
					int num = this.locked.state;
					if (this.active != null)
					{
						num += this.active.state;
					}
					if (num < 1)
					{
						return ToDo.State.NEW;
					}
					if (num != 1)
					{
						return ToDo.State.DONE;
					}
					return ToDo.State.UPDATED;
				}
				set
				{
					if (this.active == null)
					{
						this.active = new ToDo.SubItem();
					}
					this.active.state = (int)value - this.locked.state;
				}
			}

			public Item()
			{
			}
		}

		private class ItemComparer : IComparer
		{
			public int column;

			public int direction;

			public ItemComparer()
			{
			}

			public int Compare(object x, object y)
			{
				ListViewItem listViewItem = x as ListViewItem;
				ListViewItem listViewItem1 = y as ListViewItem;
				if (listViewItem.SubItems.Count <= this.column || listViewItem1.SubItems.Count <= this.column)
				{
					return 0;
				}
				return listViewItem.SubItems[this.column].Text.CompareTo(listViewItem1.SubItems[this.column].Text) * this.direction;
			}
		}

		private enum Priority
		{
			LOW,
			MEDIUM,
			HIGH
		}

		private enum State
		{
			NEW,
			UPDATED,
			DONE
		}

		private class SubItem
		{
			public string description;

			public int priority;

			public int state;

			public int assigned;

			public int assignedPrecidence;

			public SubItem()
			{
			}
		}
	}
}