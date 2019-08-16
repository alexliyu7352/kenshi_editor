using forgotten_construction_set;
using forgotten_construction_set.PropertyGrid;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace forgotten_construction_set.Components
{
	public class GameDataItemProperty : ButtonProperty
	{
		public string CustomFilter
		{
			get;
			set;
		}

		public GameData Source
		{
			get;
			set;
		}

		public itemType TypeFilter
		{
			get;
			set;
		}

		public GameDataItemProperty(GameData source, itemType filter = itemType.ITEM, string customFilter = "")
		{
			this.Source = source;
			this.TypeFilter = filter;
			this.CustomFilter = customFilter;
		}

		public override void Apply()
		{
			GameData.Item item = this.Source.getItem(this.mTextBox.Text);
			if (item == null)
			{
				base.setValue(this.mTextBox.Text);
			}
			else
			{
				base.setValue(item);
			}
			base.DestroyEditor();
		}

		protected override void ButtonPressed()
		{
			PropertyGrid.PropertyGrid.Item item = this.mItem;
			PropertyGrid.PropertyGrid propertyGrid = this.mGrid;
			ItemDialog itemDialog = new ItemDialog("Referenced item", this.Source, this.TypeFilter, false, this.CustomFilter, itemType.NULL_ITEM);
			if (itemDialog.ShowDialog() == DialogResult.OK)
			{
				this.mItem = item;
				this.mGrid = propertyGrid;
				base.setValue(itemDialog.Items[0]);
				this.mItem = null;
				this.mGrid = null;
			}
		}

		public override void CreateEditor(PropertyGrid.PropertyGrid grid, PropertyGrid.PropertyGrid.Section section, PropertyGrid.PropertyGrid.Item item, Rectangle rect)
		{
			base.CreateEditor(grid, section, item, rect);
			this.mTextBox.Text = this.getString(item.Value, true);
		}

		private Color getColour(PropertyGrid.PropertyGrid.Item item)
		{
			if (item.Value == null || item.Value is GameData.Item)
			{
				return item.TextColour;
			}
			if (this.Source.getItem(item.Value.ToString()) == null)
			{
				return Color.Red;
			}
			return item.TextColour;
		}

		private string getString(object item, bool edit = false)
		{
			if (item == null)
			{
				return "";
			}
			if (!(item is GameData.Item))
			{
				return item.ToString();
			}
			GameData.Item item1 = item as GameData.Item;
			if (!edit && !(item1.Name == "") && !(item1.Name == "0"))
			{
				return item1.Name;
			}
			return item1.stringID;
		}

		public override void Paint(PropertyGrid.PropertyGrid grid, PropertyGrid.PropertyGrid.Item item, Graphics g, Rectangle rect)
		{
			base.DrawText(this.getString(item.Value, false), grid.Font, g, rect, 2, 0, new Color?(this.getColour(item)));
			if (base.Editing == item)
			{
				rect.X = this.mButton.X;
				rect.Width = this.mButton.Width;
				ButtonRenderer.DrawButton(g, rect, this.mState);
			}
		}
	}
}