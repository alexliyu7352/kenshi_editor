using System;
using System.Collections.Generic;

namespace forgotten_construction_set
{
	public class FCSEnums
	{
		public static Dictionary<string, FCSEnum> types;

		static FCSEnums()
		{
			FCSEnums.types = new Dictionary<string, FCSEnum>();
		}

		public FCSEnums()
		{
		}
	}
}