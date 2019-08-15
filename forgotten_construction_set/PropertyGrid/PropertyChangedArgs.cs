using System;
using System.Runtime.CompilerServices;

namespace forgotten_construction_set.PropertyGrid
{
	public class PropertyChangedArgs : EventArgs
	{
		public PropertyGrid.Item Item
		{
			get;
			private set;
		}

		public object OldValue
		{
			get;
			private set;
		}

		public PropertyGrid.Section Section
		{
			get;
			private set;
		}

		public PropertyChangedArgs(PropertyGrid.Section section, PropertyGrid.Item item, object oldValue)
		{
			this.Section = section;
			this.Item = item;
			this.OldValue = oldValue;
		}
	}
}