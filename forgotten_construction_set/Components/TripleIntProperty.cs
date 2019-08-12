using forgotten_construction_set;
using System;

namespace forgotten_construction_set.Components
{
	public class TripleIntProperty : ValueListProperty
	{
		public TripleIntProperty(int count)
		{
			base.Items = count;
			base.Type = ValueListProperty.ValueType.INT;
		}

		protected override object Parse(string[] parts)
		{
			GameData.TripleInt tripleInt = new GameData.TripleInt(0, 0, 0);
			if (base.Items > 0 && !int.TryParse(parts[0], out tripleInt.v0))
			{
				return null;
			}
			if (base.Items > 1 && !int.TryParse(parts[1], out tripleInt.v1))
			{
				return null;
			}
			if (base.Items > 2 && !int.TryParse(parts[2], out tripleInt.v2))
			{
				return null;
			}
			return tripleInt;
		}

		protected override string ValueString(object v)
		{
			GameData.TripleInt tripleInt = (GameData.TripleInt)v;
			switch (base.Items)
			{
				case 1:
				{
					return tripleInt.v0.ToString();
				}
				case 2:
				{
					return string.Format("{0,-4} {1}", tripleInt.v0, tripleInt.v1);
				}
				case 3:
				{
					return string.Format("{0,-4} {1,-4} {2}", tripleInt.v0, tripleInt.v1, tripleInt.v2);
				}
			}
			return "";
		}
	}
}