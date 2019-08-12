using forgotten_construction_set;
using System;

namespace forgotten_construction_set.Components
{
	public class QuatProperty : ValueListProperty
	{
		public QuatProperty()
		{
			base.Items = 4;
			base.Type = ValueListProperty.ValueType.FLOAT;
		}

		protected override object Parse(string[] parts)
		{
			GameData.quat _quat = new GameData.quat();
			if (!float.TryParse(parts[0], out _quat.x))
			{
				return null;
			}
			if (!float.TryParse(parts[1], out _quat.y))
			{
				return null;
			}
			if (!float.TryParse(parts[2], out _quat.z))
			{
				return null;
			}
			if (!float.TryParse(parts[3], out _quat.w))
			{
				return null;
			}
			return _quat;
		}
	}
}