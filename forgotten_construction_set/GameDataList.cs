using forgotten_construction_set.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace forgotten_construction_set
{
	public class GameDataList : ListView
	{
		private ListViewItem[] itemCache;

		private int cacheOffset;

		private List<GameData.Item> mSelected;

		private IContainer components;

		public ItemFilter Filter
		{
			get;
			set;
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new List<GameData.Item> Items
		{
			get;
			set;
		}

		public new List<GameData.Item> SelectedItems
		{
			get
			{
				if (this.mSelected == null)
				{
					this.mSelected = new List<GameData.Item>();
					foreach (int selectedIndex in base.SelectedIndices)
					{
						this.mSelected.Add(this.Items[selectedIndex]);
					}
				}
				return this.mSelected;
			}
		}

		public string SortKey
		{
			get;
			set;
		}

		public GameData Source
		{
			get;
			set;
		}

		public GameDataList()
		{
			base.VirtualMode = true;
			this.DoubleBuffered = true;
			this.InitializeComponent();
			base.ColumnClick += new ColumnClickEventHandler(this.GameDataList_ColumnClick);
			base.SelectedIndexChanged += new EventHandler(this.GameDataList_SelectedIndexChanged);
			base.VirtualItemsSelectionRangeChanged += new ListViewVirtualItemsSelectionRangeChangedEventHandler(this.GameDataList_VirtualItemsSelectionRangeChanged);
			this.Items = new List<GameData.Item>();
			this.SortKey = "Name";
			base.Sorting = SortOrder.Ascending;
		}

		public void AddColumn(string name, string key)
		{
			ColumnHeader columnHeader = new ColumnHeader()
			{
				Text = name,
				Tag = key
			};
			columnHeader.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
			base.Columns.Add(columnHeader);
			this.itemCache = null;
		}

		public void AddColumn(string name, GameDataList.ColumnInfo info)
		{
			ColumnHeader columnHeader = new ColumnHeader()
			{
				Text = name,
				Tag = info
			};
			columnHeader.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
			base.Columns.Add(columnHeader);
			this.itemCache = null;
		}

		public void AddItem(GameData.Item item)
		{
			this.Items.Add(item);
			base.VirtualListSize = this.Items.Count;
		}

		public void AddItems(IEnumerable<GameData.Item> list, bool sort = false)
		{
			foreach (GameData.Item item in list)
			{
				this.Items.Add(item);
			}
			if (sort)
			{
				this.Items = (
					from o in this.Items
					orderby o.Name
					select o).ToList<GameData.Item>();
			}
			base.VirtualListSize = this.Items.Count;
			this.itemCache = null;
		}

		public GameDataList.ColumnInfo AddReferenceColumn(string name, string key, bool names, bool values)
		{
			GameDataList.ColumnInfo columnInfo = new GameDataList.ColumnInfo();
			ColumnHeader columnHeader = new ColumnHeader()
			{
				Text = name,
				Tag = columnInfo
			};
			columnHeader.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
			columnInfo.list = name;
			columnInfo.showNames = names;
			columnInfo.showValues = values;
			base.Columns.Add(columnHeader);
			this.itemCache = null;
			return columnInfo;
		}

		private ListViewItem createItem(GameData.Item item)
		{
			ListViewItem listViewItem = new ListViewItem(item.Name)
			{
				ForeColor = StateColours.GetStateColor(item.getState())
			};
			foreach (ColumnHeader column in base.Columns)
			{
				if (column.Index == 0)
				{
					continue;
				}
				if (column.Tag == null)
				{
					column.Tag = "Name";
				}
				string str = column.Tag.ToString();
				if (str == "Name")
				{
					listViewItem.SubItems.Add(item.Name);
				}
				else if (str == "StringID")
				{
					listViewItem.SubItems.Add(item.stringID);
				}
				else if (str == "Type")
				{
					listViewItem.SubItems.Add(item.type.ToString());
				}
				else if (str == "Ref")
				{
					listViewItem.SubItems.Add(item.refCount.ToString());
				}
				else if (column.Tag is GameDataList.ColumnInfo && item.hasReference(str))
				{
					GameDataList.ColumnInfo tag = column.Tag as GameDataList.ColumnInfo;
					string str1 = "";
					if (tag.showNames || tag.showValues)
					{
						GameData.Desc desc = GameData.getDesc(item.type, str);
						foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair in item.referenceData(str, false))
						{
							GameData.Item item1 = this.Source.getItem(keyValuePair.Key);
							if (tag.showNames)
							{
								str1 = string.Concat(str1, (item1 == null ? keyValuePair.Key : item1.Name));
							}
							if (!tag.showValues)
							{
								if (!tag.showNames)
								{
									continue;
								}
								str1 = string.Concat(str1, "; ");
							}
							else
							{
								if (desc.flags == 1)
								{
									str1 = string.Concat(new object[] { str1, " ", keyValuePair.Value.v0, "; " });
								}
								if (desc.flags == 2)
								{
									str1 = string.Concat(new object[] { str1, " ", keyValuePair.Value.v0, " ", keyValuePair.Value.v1, "; " });
								}
								if (desc.flags != 3)
								{
									continue;
								}
								str1 = string.Concat(new object[] { str1, " ", keyValuePair.Value.v0, " ", keyValuePair.Value.v1, " ", keyValuePair.Value.v2, "; " });
							}
						}
					}
					else
					{
						str1 = string.Concat("Size: ", item.getReferenceCount(str));
					}
					listViewItem.SubItems.Add(str1);
				}
				else if (str == "Data")
				{
					int referenceValue = 0;
					if (item.hasReference("cost"))
					{
						referenceValue = item.getReferenceValue("cost", 0).v0;
					}
					listViewItem.SubItems.Add(referenceValue.ToString());
				}
				else if (!item.ContainsKey(str))
				{
					listViewItem.SubItems.Add("-");
				}
				else
				{
					object bah = item[str];
					if (bah is int)
					{
						GameData.Desc desc1 = GameData.getDesc(item.type, str);
						if (desc1 != GameData.nullDesc && desc1.defaultValue.GetType().IsEnum)
						{
							if (desc1.flags != 256)
							{
								string name = Enum.GetName(desc1.defaultValue.GetType(), bah);
								if (name != null)
								{
									bah = name;
								}
							}
							else
							{
								bah = (new BitSetProperty(desc1.defaultValue.GetType(), false)).getAsString(bah);
							}
						}
					}
					listViewItem.SubItems.Add(bah.ToString());
				}
			}
			return listViewItem;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void GameDataList_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			ColumnHeader item = base.Columns[e.Column];
			this.Refresh();
		}

		private void GameDataList_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.mSelected = null;
		}

		private void GameDataList_VirtualItemsSelectionRangeChanged(object sender, ListViewVirtualItemsSelectionRangeChangedEventArgs e)
		{
			this.OnSelectedIndexChanged(e);
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
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

		public override void Refresh()
		{
			base.VirtualListSize = this.Items.Count;
			base.Refresh();
		}

		public void RefreshItem(GameData.Item item)
		{
			int num = this.Items.IndexOf(item);
			if (this.itemCache != null && num >= this.cacheOffset && num < this.cacheOffset + (int)this.itemCache.Length)
			{
				this.itemCache[num - this.cacheOffset] = this.createItem(item);
				this.Refresh();
			}
		}

		public void RemoveColumn(string name)
		{
			foreach (ColumnHeader column in base.Columns)
			{
				if (column.Text != name)
				{
					continue;
				}
				base.Columns.Remove(column);
				this.itemCache = null;
				return;
			}
			this.itemCache = null;
		}

		public void RemoveItem(GameData.Item item)
		{
			this.Items.Remove(item);
			base.VirtualListSize = this.Items.Count;
			this.itemCache = null;
		}

		private ListViewItem RetrieveVirtualItem(int index)
		{
			if (this.itemCache == null || index < this.cacheOffset || index >= this.cacheOffset + (int)this.itemCache.Length)
			{
				return this.createItem(this.Items[index]);
			}
			return this.itemCache[index - this.cacheOffset];
		}

		public void UpdateItems(GameData source, itemType? type = null, string filter = "")
		{
			if (!type.HasValue)
			{
				this.Filter = new ItemFilter(filter);
			}
			else
			{
				this.Filter = new ItemFilter(type.Value, filter);
			}
			this.Source = source;
			List<string> list = (
				from i in this.Items
				select i.stringID).ToList<string>();
			this.Items.Clear();
			List<GameData.Item> items = new List<GameData.Item>();
			foreach (GameData.Item value in source.items.Values)
			{
				if (!this.Filter.Test(value))
				{
					continue;
				}
				items.Add(value);
			}
			this.itemCache = null;
			this.mSelected = null;
			foreach (string str in list)
			{
				int num = items.FindIndex((GameData.Item it) => it.stringID == str);
				if (num < 0)
				{
					continue;
				}
				this.Items.Add(items[num]);
				items.RemoveAt(num);
			}
			this.Items.AddRange(items);
			if (this.SortKey == "Name")
			{
				if (base.Sorting != SortOrder.Descending)
				{
					this.Items = (
						from o in this.Items
						orderby o.Name
						select o).ToList<GameData.Item>();
				}
				else
				{
					this.Items = (
						from o in this.Items
						orderby o.Name descending
						select o).ToList<GameData.Item>();
				}
			}
			else if (this.SortKey == "StringID")
			{
				if (base.Sorting != SortOrder.Descending)
				{
					this.Items = (
						from o in this.Items
						orderby o.stringID
						select o).ToList<GameData.Item>();
				}
				else
				{
					this.Items = (
						from o in this.Items
						orderby o.stringID descending
						select o).ToList<GameData.Item>();
				}
			}
			else if (this.SortKey == "Type")
			{
				if (base.Sorting != SortOrder.Descending)
				{
					this.Items = (
						from o in this.Items
						orderby o.type
						select o).ToList<GameData.Item>();
				}
				else
				{
					this.Items = (
						from o in this.Items
						orderby o.type descending
						select o).ToList<GameData.Item>();
				}
			}
			else if (this.SortKey == "Ref")
			{
				if (base.Sorting != SortOrder.Descending)
				{
					this.Items = (
						from o in this.Items
						orderby o.refCount
						select o).ToList<GameData.Item>();
				}
				else
				{
					this.Items = (
						from o in this.Items
						orderby o.refCount descending
						select o).ToList<GameData.Item>();
				}
			}
			else if (this.SortKey != null)
			{
				if (base.Sorting != SortOrder.Descending)
				{
					this.Items = this.Items.OrderBy<GameData.Item, object>((GameData.Item e) => {
						if (!e.ContainsKey(this.SortKey))
						{
							return null;
						}
						return e[this.SortKey];
					}).ToList<GameData.Item>();
				}
				else
				{
					this.Items = this.Items.OrderByDescending<GameData.Item, object>((GameData.Item e) => {
						if (!e.ContainsKey(this.SortKey))
						{
							return null;
						}
						return e[this.SortKey];
					}).ToList<GameData.Item>();
				}
			}
			base.VirtualListSize = this.Items.Count;
			base.SelectedIndices.Clear();
			base.Invalidate();
		}

		public class ColumnInfo
		{
			public string list;

			public bool showNames;

			public bool showValues;

			public ColumnInfo()
			{
			}

			public override string ToString()
			{
				return this.list;
			}
		}

		private class SortComparitor : IComparer<GameData.Item>
		{
			public SortOrder order;

			public string key;

			public SortComparitor(string key, SortOrder order)
			{
				this.key = key;
				this.order = order;
			}

			public int Compare(GameData.Item x, GameData.Item y)
			{
				if (!x.ContainsKey(this.key) || !y.ContainsKey(this.key))
				{
					return 0;
				}
				object bah = x[this.key];
				object obj = y[this.key];
				int num = (this.order == SortOrder.Descending ? -1 : 1);
				if ((bah is int || bah.GetType().IsEnum) && (obj is int || obj.GetType().IsEnum))
				{
					return ((int)bah).CompareTo((int)obj) * num;
				}
				if (!(bah is float) || !(obj is float))
				{
					return bah.ToString().CompareTo(obj.ToString()) * num;
				}
				return ((float)bah).CompareTo((float)obj) * num;
			}
		}
	}
}