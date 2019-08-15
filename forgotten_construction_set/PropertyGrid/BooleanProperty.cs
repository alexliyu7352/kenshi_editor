using System;
using System.Drawing;
using System.Windows.Forms;

namespace forgotten_construction_set.PropertyGrid
{
	public class BooleanProperty : DropdownListProperty
	{
		public BooleanProperty()
		{
		}

		public override void CreateEditor(PropertyGrid grid, PropertyGrid.Section section, PropertyGrid.Item item, Rectangle rect)
		{
			base.CreateEditor(grid, section, item, rect);
			this.mList.Items.Add(false);
			this.mList.Items.Add(true);
			this.mList.SelectedItem = item.Value;
		}
	}
}