using forgotten_construction_set;
using PropertyGrid;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace forgotten_construction_set.Components
{
	public class InstanceProperty : ButtonProperty
	{
		public navigation NavForm
		{
			get;
			set;
		}

		public GameData.Item ParentItem
		{
			get;
			set;
		}

		public GameData Source
		{
			get;
			set;
		}

		public InstanceProperty(GameData.Item item, navigation nav)
		{
			this.Source = nav.ou.gameData;
			this.NavForm = nav;
			this.ParentItem = item;
		}

		public override void Apply()
		{
			base.DestroyEditor();
		}

		protected override void ButtonPressed()
		{
			itemType data = itemType.NULL_ITEM;
			if (this.mItem.Data != null)
			{
				data = (itemType)this.mItem.Data;
			}
			this.NavForm.showItemInstance(this.ParentItem, this.mItem.Name, data);
		}

		public override void CreateEditor(PropertyGrid.PropertyGrid grid, PropertyGrid.PropertyGrid.Section section, PropertyGrid.PropertyGrid.Item item, Rectangle rect)
		{
			base.CreateEditor(grid, section, item, rect);
			if (this.mTextBox == null)
			{
				return;
			}
			this.mTextBox.Text = this.getString((GameData.Instance)item.Value);
			this.mTextBox.ReadOnly = true;
		}

		public override void DoubleClick(MouseEventArgs e)
		{
			this.ButtonPressed();
		}

		private string getString(GameData.Instance inst)
		{
			if (!inst.ContainsKey("ref"))
			{
				return "";
			}
			string item = inst.sdata["ref"];
			GameData.Item item1 = this.Source.getItem(item);
			if (item1 == null)
			{
				return item;
			}
			return item1.Name;
		}

		public override void Paint(PropertyGrid.PropertyGrid grid, PropertyGrid.PropertyGrid.Item item, Graphics g, Rectangle rect)
		{
			base.DrawText(this.getString((GameData.Instance)item.Value), grid.Font, g, rect, 2, 0, new Color?(item.TextColour));
			if (base.Editing == item)
			{
				rect.X = this.mButton.X;
				rect.Width = this.mButton.Width;
				ButtonRenderer.DrawButton(g, rect, this.mState);
			}
		}
	}
}