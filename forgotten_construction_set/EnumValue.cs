using System;
using System.Runtime.CompilerServices;

namespace forgotten_construction_set
{
	public class EnumValue
	{
		private FCSEnum type;

		public FCSEnum Enum
		{
			get
			{
				return this.type;
			}
		}

		public string String
		{
			get
			{
				return this.type.name(this.Value);
			}
			set
			{
				this.Value = this.type.parse(value);
			}
		}

		public int Value
		{
			get;
			set;
		}

		public EnumValue(FCSEnum e, int value)
		{
			this.type = e;
			this.Value = value;
		}

		public EnumValue(FCSEnum e, string value)
		{
			this.type = e;
			this.String = value;
		}

		public override string ToString()
		{
			return this.String;
		}
	}
}