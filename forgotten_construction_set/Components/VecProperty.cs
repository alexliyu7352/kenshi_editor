using forgotten_construction_set;
using System;

namespace forgotten_construction_set.Components
{
	public class VecProperty : ValueListProperty
	{
		public VecProperty()
		{
			base.Items = 3;
			base.Type = ValueListProperty.ValueType.FLOAT;
		}

		protected override object Parse(string[] parts)
		{
			GameData.vec _vec = new GameData.vec();
			if (!float.TryParse(parts[0], out _vec.x))
			{
				return null;
			}
			if (!float.TryParse(parts[1], out _vec.y))
			{
				return null;
			}
			if (!float.TryParse(parts[2], out _vec.z))
			{
				return null;
			}
			return _vec;
		}
	}
}