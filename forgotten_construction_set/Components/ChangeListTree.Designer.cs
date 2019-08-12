using forgotten_construction_set;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace forgotten_construction_set.Components
{
	public class ChangeListTree : ListView
	{
		private List<ChangeListTree.TreeNode> mappedNodes;

		private ListViewItem[] itemCache;

		private int cacheOffset;

		private List<ChangeListTree.TreeNode> mSelected;

		private int checkSize;

		private IContainer components;

		public ItemFilter Filter
		{
			get;
			set;
		}

		public forgotten_construction_set.GameData GameData
		{
			get;
			set;
		}

		public List<ChangeListTree.RootNode> Nodes
		{
			get;
			set;
		}

		public new List<ChangeListTree.TreeNode> SelectedItems
		{
			get
			{
				if (this.mSelected == null)
				{
					this.mSelected = new List<ChangeListTree.TreeNode>();
					foreach (int selectedIndex in base.SelectedIndices)
					{
						this.mSelected.Add(this.mappedNodes[selectedIndex]);
					}
				}
				return this.mSelected;
			}
		}

		public bool ShowChanged
		{
			get;
			set;
		}

		public bool ShowDeleted
		{
			get;
			set;
		}

		public bool ShowNew
		{
			get;
			set;
		}

		public string SortKey
		{
			get;
			set;
		}

		public ChangeListTree()
		{
			this.setup();
		}

		public ChangeListTree(IContainer container)
		{
			container.Add(this);
			this.setup();
		}

		private void collapseNode(ChangeListTree.TreeNode n)
		{
			if (n.Expanded)
			{
				int visibleChildNodeCount = n.getVisibleChildNodeCount();
				n.Expanded = false;
				int num = this.mappedNodes.IndexOf(n);
				this.mappedNodes.RemoveRange(num + 1, visibleChildNodeCount);
				base.VirtualListSize = this.mappedNodes.Count;
				this.shiftSelection(num + 1, -visibleChildNodeCount);
				this.itemCache = null;
				this.Invalidate();
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

		private void DrawColumnHeaderFunc(object sender, DrawListViewColumnHeaderEventArgs e)
		{
			e.DrawDefault = true;
		}

		private void DrawItemFunc(object sender, DrawListViewItemEventArgs e)
		{
		}

		private void DrawSubItemFunc(object sender, DrawListViewSubItemEventArgs e)
		{
			e.Graphics.FillRectangle((e.Item.Selected ? SystemBrushes.Highlight : Brushes.White), e.Bounds);
			if (e.ColumnIndex != 0)
			{
				TextRenderer.DrawText(e.Graphics, e.SubItem.Text, this.Font, e.Bounds, e.Item.ForeColor, TextFormatFlags.Default);
				return;
			}
			int left = e.Bounds.Left;
			Rectangle bounds = e.Bounds;
			Point point = new Point(left, bounds.Top);
			ChangeListTree.TreeNode tag = e.Item.Tag as ChangeListTree.TreeNode;
			point.X = point.X + tag.Level * 20;
			if ((e.Item.Tag as ChangeListTree.TreeNode).HasChildren)
			{
				int top = e.Bounds.Top;
				bounds = e.Bounds;
				int bottom = (top + bounds.Bottom) / 2;
				Rectangle rectangle = new Rectangle(point.X, bottom - 8, 16, 16);
				if (!Application.RenderWithVisualStyles)
				{
					Rectangle rectangle1 = rectangle;
					rectangle1.Inflate(-4, -4);
					e.Graphics.DrawRectangle(SystemPens.ControlDark, rectangle1);
					e.Graphics.DrawLine(SystemPens.ControlText, rectangle1.X + 2, rectangle1.Y + rectangle1.Height / 2, rectangle1.Right - 2, rectangle1.Y + rectangle1.Height / 2);
					if (!tag.Expanded)
					{
						e.Graphics.DrawLine(SystemPens.ControlText, rectangle1.X + rectangle1.Width / 2, rectangle1.Y + 2, rectangle1.X + rectangle1.Width / 2, rectangle1.Bottom - 2);
					}
				}
				else
				{
					(new VisualStyleRenderer((tag.Expanded ? VisualStyleElement.TreeView.Glyph.Opened : VisualStyleElement.TreeView.Glyph.Closed))).DrawBackground(e.Graphics, rectangle);
				}
			}
			point.X = point.X + 14;
			if (base.CheckBoxes)
			{
				System.Drawing.Size glyphSize = CheckBoxRenderer.GetGlyphSize(e.Graphics, CheckBoxState.UncheckedNormal);
				this.checkSize = glyphSize.Width;
				CheckBoxRenderer.DrawCheckBox(e.Graphics, point, (e.Item.Checked ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal));
				point.X = point.X + this.checkSize;
			}
			int x = point.X;
			int y = point.Y;
			bounds = e.Bounds;
			int right = bounds.Right - point.X;
			bounds = e.Bounds;
			Rectangle rectangle2 = new Rectangle(x, y, right, bounds.Height);
			TextRenderer.DrawText(e.Graphics, e.Item.Text, this.Font, rectangle2, e.Item.ForeColor, TextFormatFlags.Default);
		}

		private void expandNode(ChangeListTree.TreeNode n)
		{
			if (!n.Expanded && n.HasChildren)
			{
				n.Expanded = true;
				int num = this.mappedNodes.IndexOf(n);
				int virtualListSize = base.VirtualListSize;
				n.getVisibleChildNodes(this.mappedNodes, num);
				base.VirtualListSize = this.mappedNodes.Count;
				this.shiftSelection(num + 1, base.VirtualListSize - virtualListSize);
				this.itemCache = null;
				this.Invalidate();
			}
		}

		private string FormatTripleInt(object value, forgotten_construction_set.GameData.Desc desc)
		{
			if (value == null || desc == null)
			{
				return null;
			}
			forgotten_construction_set.GameData.TripleInt tripleInt = (forgotten_construction_set.GameData.TripleInt)value;
			switch (desc.flags)
			{
				case 0:
				{
					return null;
				}
				case 1:
				{
					return tripleInt.v0.ToString();
				}
				case 2:
				{
					return string.Format("{0,-4} {1}", tripleInt.v0, tripleInt.v1);
				}
				case 3:
				{
					return string.Format("{0,-4} {1,-4} {2}", tripleInt.v0, tripleInt.v1, tripleInt.v2);
				}
			}
			return null;
		}

		private void GameDataList_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.mSelected = null;
		}

		public forgotten_construction_set.GameData.Item GetItem(ChangeListTree.TreeNode node)
		{
			if (node == null)
			{
				return null;
			}
			if (node is ChangeListTree.RootNode)
			{
				return (node as ChangeListTree.RootNode).Item;
			}
			if (!(node is ChangeListTree.ChangeData))
			{
				return null;
			}
			return this.GetItem(node.Parent);
		}

		public ChangeListTree.ItemElement GetItemElementAt(int x, int y)
		{
			ListViewItem itemAt = base.GetItemAt(x, y);
			if (itemAt == null)
			{
				return ChangeListTree.ItemElement.NONE;
			}
			int level = (itemAt.Tag as ChangeListTree.TreeNode).Level * 20;
			if (x < level)
			{
				return ChangeListTree.ItemElement.INDENT;
			}
			if (x < level + 14)
			{
				return ChangeListTree.ItemElement.EXPAND;
			}
			if (base.CheckBoxes && x < level + 14 + this.checkSize)
			{
				return ChangeListTree.ItemElement.CHECKBOX;
			}
			return ChangeListTree.ItemElement.TEXT;
		}

		private void initialiseMap()
		{
			this.mappedNodes = new List<ChangeListTree.TreeNode>();
			foreach (ChangeListTree.TreeNode node in this.Nodes)
			{
				this.mappedNodes.Add(node);
				if (!node.Expanded)
				{
					continue;
				}
				node.getVisibleChildNodes(this.mappedNodes, -1);
			}
			base.VirtualListSize = this.mappedNodes.Count;
			this.Invalidate();
		}

		private void InitializeComponent()
		{
			base.SuspendLayout();
			base.FullRowSelect = true;
			base.View = System.Windows.Forms.View.Details;
			base.ResumeLayout(false);
		}

		public new void Invalidate()
		{
			this.itemCache = null;
			base.Invalidate();
		}

		private void MouseDownFunc(object sender, MouseEventArgs e)
		{
			if (e.Button != System.Windows.Forms.MouseButtons.Left)
			{
				return;
			}
			ListViewItem itemAt = base.GetItemAt(e.X, e.Y);
			if (itemAt != null)
			{
				ChangeListTree.TreeNode tag = itemAt.Tag as ChangeListTree.TreeNode;
				int level = tag.Level * 20;
				if (e.X < level)
				{
					return;
				}
				if (base.CheckBoxes && e.X > level + 14 && e.X < level + 14 + this.checkSize)
				{
					tag.Checked = !tag.Checked;
					itemAt.Checked = tag.Checked;
					if (this.ItemChecked != null)
					{
						this.ItemChecked(this, new ChangeListTree.ItemCheckedEventArgs(tag));
					}
					this.Invalidate();
					return;
				}
				if (e.Clicks > 1 || e.X < level + 14)
				{
					if (tag.Expanded)
					{
						this.collapseNode(tag);
						return;
					}
					this.expandNode(tag);
				}
			}
		}

		protected override void OnCacheVirtualItems(CacheVirtualItemsEventArgs e)
		{
			if (this.itemCache != null && e.StartIndex >= this.cacheOffset && e.EndIndex < this.cacheOffset + (int)this.itemCache.Length)
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

		protected override void OnRetrieveVirtualItem(RetrieveVirtualItemEventArgs e)
		{
			e.Item = this.RetrieveVirtualItem(e.ItemIndex);
		}

		public void RefreshList()
		{
			ChangeListTree.ChangeData changeDatum;
			this.Cursor = Cursors.WaitCursor;
			this.Nodes = new List<ChangeListTree.RootNode>();
			foreach (forgotten_construction_set.GameData.Item value in this.GameData.items.Values)
			{
				if (value.getState() == forgotten_construction_set.GameData.State.ORIGINAL || this.Filter != null && !this.Filter.Test(value) || value.getState() == forgotten_construction_set.GameData.State.OWNED && !this.ShowNew || value.getState() == forgotten_construction_set.GameData.State.MODIFIED && !this.ShowChanged || value.getState() == forgotten_construction_set.GameData.State.REMOVED && !this.ShowDeleted || value.getState() == forgotten_construction_set.GameData.State.LOCKED || value.getState() == forgotten_construction_set.GameData.State.LOCKED_REMOVED || value.getState() == forgotten_construction_set.GameData.State.REMOVED && value.OriginalName == null)
				{
					continue;
				}
				this.Nodes.Add(new ChangeListTree.RootNode(value));
			}
			this.Nodes = (
				from o in this.Nodes
				orderby o.Item.type, o.Item.Name
				select o).ToList<ChangeListTree.RootNode>();
			foreach (ChangeListTree.RootNode node in this.Nodes)
			{
				forgotten_construction_set.GameData.Item item = node.Item;
				forgotten_construction_set.GameData.State state = item.getState();
				if (state == forgotten_construction_set.GameData.State.OWNED)
				{
					continue;
				}
				if (item.Name != item.OriginalName)
				{
					ChangeListTree.ChangeData changeDatum1 = new ChangeListTree.ChangeData(ChangeListTree.ChangeType.NAME, item.OriginalName, item.Name, state)
					{
						Text = "Name"
					};
					node.Add(changeDatum1);
				}
				foreach (KeyValuePair<string, object> keyValuePair in item)
				{
					forgotten_construction_set.GameData.State state1 = item.getState(keyValuePair.Key);
					if (state1 == forgotten_construction_set.GameData.State.ORIGINAL || state1 == forgotten_construction_set.GameData.State.LOCKED)
					{
						continue;
					}
					node.Add(new ChangeListTree.ChangeData(ChangeListTree.ChangeType.VALUE, keyValuePair.Key, item.OriginalValue(keyValuePair.Key), item[keyValuePair.Key], state1));
				}
				foreach (string str in item.referenceLists())
				{
					forgotten_construction_set.GameData.Desc desc = forgotten_construction_set.GameData.getDesc(item.type, str);
					if (!(desc.defaultValue is forgotten_construction_set.GameData.TripleInt))
					{
						desc = null;
					}
					foreach (KeyValuePair<string, forgotten_construction_set.GameData.TripleInt> keyValuePair1 in item.referenceData(str, true))
					{
						forgotten_construction_set.GameData.State state2 = item.getState(str, keyValuePair1.Key);
						if (state2 == forgotten_construction_set.GameData.State.ORIGINAL || state2 == forgotten_construction_set.GameData.State.LOCKED || state2 == forgotten_construction_set.GameData.State.LOCKED_REMOVED)
						{
							continue;
						}
						ChangeListTree.ChangeData changeDatum2 = null;
						forgotten_construction_set.GameData.Item item1 = this.GameData.getItem(keyValuePair1.Key);
						if (state2 == forgotten_construction_set.GameData.State.REMOVED)
						{
							changeDatum2 = new ChangeListTree.ChangeData(ChangeListTree.ChangeType.DELREF, str, keyValuePair1.Key, null, null, state2);
						}
						else if (state2 == forgotten_construction_set.GameData.State.MODIFIED)
						{
							changeDatum2 = new ChangeListTree.ChangeData(ChangeListTree.ChangeType.MODREF, str, keyValuePair1.Key, this.FormatTripleInt(item.OriginalValue(str, keyValuePair1.Key), desc), this.FormatTripleInt(keyValuePair1.Value, desc), state2);
						}
						else if (state2 != forgotten_construction_set.GameData.State.OWNED)
						{
							if (state2 != forgotten_construction_set.GameData.State.INVALID)
							{
								continue;
							}
							changeDatum2 = new ChangeListTree.ChangeData(ChangeListTree.ChangeType.INVALIDREF, str, keyValuePair1.Key, null, this.FormatTripleInt(keyValuePair1.Value, desc), state2);
						}
						else
						{
							changeDatum2 = new ChangeListTree.ChangeData(ChangeListTree.ChangeType.NEWREF, str, keyValuePair1.Key, null, this.FormatTripleInt(keyValuePair1.Value, desc), state2);
						}
						changeDatum2.Text = string.Concat(str, " : ", (item1 == null ? keyValuePair1.Key : item1.Name));
						node.Add(changeDatum2);
					}
				}
				foreach (KeyValuePair<string, forgotten_construction_set.GameData.Instance> keyValuePair2 in item.instanceData())
				{
					forgotten_construction_set.GameData.State state3 = keyValuePair2.Value.getState();
					if (state3 == forgotten_construction_set.GameData.State.ORIGINAL || state3 == forgotten_construction_set.GameData.State.LOCKED || state3 == forgotten_construction_set.GameData.State.LOCKED_REMOVED)
					{
						continue;
					}
					changeDatum = (state3 != forgotten_construction_set.GameData.State.OWNED ? new ChangeListTree.ChangeData(ChangeListTree.ChangeType.MODINST, keyValuePair2.Key, state3) : new ChangeListTree.ChangeData(ChangeListTree.ChangeType.NEWINST, keyValuePair2.Key, state3));
					changeDatum.Text = string.Concat("Instance : ", keyValuePair2.Key);
					node.Add(changeDatum);
					if (state3 != forgotten_construction_set.GameData.State.MODIFIED)
					{
						continue;
					}
					foreach (KeyValuePair<string, object> value1 in keyValuePair2.Value)
					{
						if (keyValuePair2.Value.getState(value1.Key) != forgotten_construction_set.GameData.State.MODIFIED)
						{
							continue;
						}
						changeDatum.Add(new ChangeListTree.ChangeData(ChangeListTree.ChangeType.INSTVALUE, keyValuePair2.Key, value1.Key, keyValuePair2.Value.OriginalValue(value1.Key), keyValuePair2.Value[value1.Key], state3));
					}
				}
			}
			this.initialiseMap();
			this.Cursor = Cursors.Default;
		}

		public void Remove(ChangeListTree.TreeNode node)
		{
			if (!(node is ChangeListTree.RootNode))
			{
				node.Parent.Children.Remove(node);
			}
			else
			{
				this.Nodes.Remove(node as ChangeListTree.RootNode);
			}
			int num = this.mappedNodes.IndexOf(node);
			if (num >= 0)
			{
				int visibleChildNodeCount = node.getVisibleChildNodeCount() + 1;
				this.mappedNodes.RemoveRange(num, visibleChildNodeCount);
				base.VirtualListSize = base.VirtualListSize - visibleChildNodeCount;
				this.shiftSelection(num, -visibleChildNodeCount);
				this.mSelected = null;
				this.itemCache = null;
				this.Invalidate();
			}
		}

		private ListViewItem RetrieveVirtualItem(int index)
		{
			if (this.itemCache != null && index >= this.cacheOffset && index < this.cacheOffset + (int)this.itemCache.Length)
			{
				return this.itemCache[index - this.cacheOffset];
			}
			if (this.mappedNodes[index] is ChangeListTree.RootNode)
			{
				forgotten_construction_set.GameData.Item item = (this.mappedNodes[index] as ChangeListTree.RootNode).Item;
				ListViewItem listViewItem = new ListViewItem(item.Name);
				ListViewItem.ListViewSubItemCollection subItems = listViewItem.SubItems;
				itemType _itemType = item.type;
				subItems.Add(_itemType.ToString(), listViewItem.ForeColor, listViewItem.BackColor, listViewItem.Font);
				listViewItem.SubItems.Add("");
				listViewItem.Checked = this.mappedNodes[index].Checked;
				listViewItem.ForeColor = StateColours.GetStateColor(item.getState());
				listViewItem.Tag = this.mappedNodes[index];
				return listViewItem;
			}
			if (!(this.mappedNodes[index] is ChangeListTree.ChangeData))
			{
				return null;
			}
			ChangeListTree.ChangeData changeDatum = this.mappedNodes[index] as ChangeListTree.ChangeData;
			ListViewItem @checked = new ListViewItem(changeDatum.Text ?? changeDatum.Key);
			ListViewItem.ListViewSubItemCollection listViewSubItemCollections = @checked.SubItems;
			ChangeListTree.ChangeType type = changeDatum.Type;
			listViewSubItemCollections.Add(type.ToString(), @checked.ForeColor, @checked.BackColor, @checked.Font);
			@checked.SubItems.Add(changeDatum.ToString(), @checked.ForeColor, @checked.BackColor, @checked.Font);
			@checked.Checked = changeDatum.Checked;
			@checked.ForeColor = changeDatum.Colour;
			@checked.Tag = changeDatum;
			return @checked;
		}

		private void setup()
		{
			this.InitializeComponent();
			base.VirtualMode = true;
			base.Columns.Add("Item", 200);
			base.Columns.Add("Type", 100);
			base.Columns.Add("Info");
			base.OwnerDraw = true;
			this.DoubleBuffered = true;
			base.DrawItem += new DrawListViewItemEventHandler(this.DrawItemFunc);
			base.DrawSubItem += new DrawListViewSubItemEventHandler(this.DrawSubItemFunc);
			base.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(this.DrawColumnHeaderFunc);
			base.MouseDown += new MouseEventHandler(this.MouseDownFunc);
			base.SelectedIndexChanged += new EventHandler(this.GameDataList_SelectedIndexChanged);
		}

		private void shiftSelection(int from, int shift)
		{
			if (base.SelectedIndices.Count == 0)
			{
				return;
			}
			int[] numArray = new int[base.SelectedIndices.Count];
			base.SelectedIndices.CopyTo(numArray, 0);
			base.SelectedIndices.Clear();
			for (int i = 0; i < (int)numArray.Length; i++)
			{
				if (numArray[i] < from)
				{
					base.SelectedIndices.Add(numArray[i]);
				}
				else if (numArray[i] + shift >= from)
				{
					base.SelectedIndices.Add(numArray[i] + shift);
				}
			}
			this.mSelected = null;
		}

		public event ChangeListTree.ItemCheckedEventHandler ItemChecked;

		public class ChangeData : ChangeListTree.TreeNode
		{
			public Color Colour
			{
				get;
				set;
			}

			public string Key
			{
				get;
				set;
			}

			public object NewValue
			{
				get;
				set;
			}

			public object OldValue
			{
				get;
				set;
			}

			public string Section
			{
				get;
				set;
			}

			public string Text
			{
				get;
				set;
			}

			public ChangeListTree.ChangeType Type
			{
				get;
				set;
			}

			public ChangeData(ChangeListTree.ChangeType type, object oldVal, object newVal, forgotten_construction_set.GameData.State state)
			{
				this.Type = type;
				this.OldValue = oldVal;
				this.NewValue = newVal;
				this.Colour = StateColours.GetStateColor(state);
			}

			public ChangeData(ChangeListTree.ChangeType type, string key, object oldVal, object newVal, forgotten_construction_set.GameData.State state)
			{
				this.Type = type;
				this.OldValue = oldVal;
				this.NewValue = newVal;
				this.Key = key;
				this.Colour = StateColours.GetStateColor(state);
			}

			public ChangeData(ChangeListTree.ChangeType type, string section, string key, object oldVal, object newVal, forgotten_construction_set.GameData.State state)
			{
				this.Type = type;
				this.OldValue = oldVal;
				this.NewValue = newVal;
				this.Key = key;
				this.Section = section;
				this.Colour = StateColours.GetStateColor(state);
			}

			public ChangeData(ChangeListTree.ChangeType type, string key, forgotten_construction_set.GameData.State state)
			{
				this.Type = type;
				this.Key = key;
				this.Colour = StateColours.GetStateColor(state);
			}

			public override string ToString()
			{
				if (this.OldValue == null || this.NewValue == null)
				{
					if (this.NewValue == null)
					{
						return "";
					}
					return string.Concat("[", this.NewValue.ToString(), "]");
				}
				return string.Concat(new string[] { "[", this.OldValue.ToString(), "] => [", this.NewValue.ToString(), "]" });
			}
		}

		public enum ChangeType
		{
			NAME,
			VALUE,
			NEWREF,
			MODREF,
			DELREF,
			NEWINST,
			MODINST,
			INSTVALUE,
			INVALIDREF
		}

		public class ItemCheckedEventArgs : EventArgs
		{
			public ChangeListTree.TreeNode Node
			{
				get;
				private set;
			}

			public ItemCheckedEventArgs(ChangeListTree.TreeNode node)
			{
				this.Node = node;
			}
		}

		public delegate void ItemCheckedEventHandler(object sender, ChangeListTree.ItemCheckedEventArgs e);

		public enum ItemElement
		{
			NONE,
			TEXT,
			EXPAND,
			CHECKBOX,
			INDENT
		}

		public class RootNode : ChangeListTree.TreeNode
		{
			public forgotten_construction_set.GameData.Item Item
			{
				get;
				set;
			}

			public RootNode(forgotten_construction_set.GameData.Item item)
			{
				this.Item = item;
				base.Expanded = false;
				base.Checked = false;
			}
		}

		public class TreeNode
		{
			public bool Checked
			{
				get;
				set;
			}

			public List<ChangeListTree.TreeNode> Children
			{
				get;
				set;
			}

			public bool Expanded
			{
				get;
				set;
			}

			public bool HasChildren
			{
				get
				{
					if (this.Children == null)
					{
						return false;
					}
					return this.Children.Count > 0;
				}
			}

			public int Level
			{
				get;
				set;
			}

			public ChangeListTree.TreeNode Parent
			{
				get;
				private set;
			}

			public TreeNode()
			{
				this.Expanded = false;
				this.Checked = false;
				this.Level = 0;
			}

			public void Add(ChangeListTree.TreeNode n)
			{
				if (this.Children == null)
				{
					this.Children = new List<ChangeListTree.TreeNode>();
				}
				this.Children.Add(n);
				n.Level = this.Level + 1;
				n.Parent = this;
			}

			public int getVisibleChildNodeCount()
			{
				int visibleChildNodeCount = 0;
				if (this.Expanded && this.Children != null)
				{
					foreach (ChangeListTree.ChangeData child in this.Children)
					{
						visibleChildNodeCount = visibleChildNodeCount + 1 + child.getVisibleChildNodeCount();
					}
				}
				return visibleChildNodeCount;
			}

			public void getVisibleChildNodes(List<ChangeListTree.TreeNode> list, int after)
			{
				if (this.Expanded && this.Children != null)
				{
					foreach (ChangeListTree.ChangeData child in this.Children)
					{
						int num = after + 1;
						after = num;
						list.Insert(num, child);
						if (!child.Expanded)
						{
							continue;
						}
						child.getVisibleChildNodes(list, after);
					}
				}
			}
		}
	}
}