using forgotten_construction_set;
using PropertyGrid;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace forgotten_construction_set.Components
{
	public class FCSEnumProperty : DropdownListProperty
	{
		private FCSEnum type;

		public FCSEnumProperty(FCSEnum e)
		{
			this.type = e;
		}

		public override void Apply()
		{
			base.setValue(this.type.parse(this.mList.SelectedItem.ToString()));
			base.DestroyEditor();
		}

		public override void CreateEditor(PropertyGrid.PropertyGrid grid, PropertyGrid.PropertyGrid.Section section, PropertyGrid.PropertyGrid.Item item, Rectangle rect)
		{
			base.CreateEditor(grid, section, item, rect);
			foreach (KeyValuePair<string, int> keyValuePair in this.type)
			{
				this.mList.Items.Add(keyValuePair.Key);
			}
			this.mList.SelectedItem = this.getAsString(item.Value);
			if (this.mList.SelectedItem == null)
			{
				this.mList.Text = item.Value.ToString();
			}
		}

		public override string getAsString(object value)
		{
			if (value is string || value is EnumValue)
			{
				return value.ToString();
			}
			string str = this.type.name((int)value) ?? value.ToString();
			return str;
		}
	}
}