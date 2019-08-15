using System;
using System.Runtime.CompilerServices;

namespace forgotten_construction_set.PropertyGrid
{
	public class PropertySelectedArgs : EventArgs
	{
		public PropertyGrid.Item Item
		{
			get;
			private set;
		}

		public PropertyGrid.Section Section
		{
			get;
			private set;
		}

		public PropertySelectedArgs(PropertyGrid.Section section, PropertyGrid.Item item)
		{
			this.Section = section;
			this.Item = item;
		}
	}
}