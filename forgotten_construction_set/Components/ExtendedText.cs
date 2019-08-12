using forgotten_construction_set;
using PropertyGrid;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace forgotten_construction_set.Components
{
	public class ExtendedText : ButtonProperty
	{
		public ExtendedText()
		{
		}

		protected override void ButtonPressed()
		{
			PropertyGrid.PropertyGrid.Item item = this.mItem;
			PropertyGrid.PropertyGrid propertyGrid = this.mGrid;
			TextDialog textDialog = new TextDialog(this.mItem.Name, this.mItem.Value.ToString(), null);
			if (textDialog.ShowDialog() == DialogResult.OK)
			{
				this.mItem = item;
				this.mGrid = propertyGrid;
				char[] chrArray = new char[] { ' ', '\n', '\r' };
				base.setValue(textDialog.Value.TrimEnd(chrArray));
				this.mItem = null;
				this.mGrid = null;
			}
		}

		public override void Paint(PropertyGrid.PropertyGrid grid, PropertyGrid.PropertyGrid.Item item, Graphics g, Rectangle rect)
		{
			if (base.Editing == item)
			{
				base.Paint(grid, item, g, rect);
				return;
			}
			string str = item.Value.ToString();
			int num = str.IndexOf('\n');
			if (num == 0)
			{
				str = "...";
			}
			else if (num > 0)
			{
				str = string.Concat(str.Substring(0, num - 1), "...");
			}
			base.DrawText(str, grid.Font, g, rect, 2, 0, new Color?(item.TextColour));
		}
	}
}