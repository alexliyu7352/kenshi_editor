using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace forgotten_construction_set.PropertyGrid
{
	public class EnumProperty : DropdownListProperty
	{
		public EnumProperty()
		{
		}

		public override void CreateEditor(PropertyGrid grid, PropertyGrid.Section section, PropertyGrid.Item item, Rectangle rect)
		{
			base.CreateEditor(grid, section, item, rect);
			foreach (object value in Enum.GetValues(item.Value.GetType()))
			{
				this.mList.Items.Add(value);
			}
			this.mList.SelectedItem = item.Value;
		}
	}
}