using System;
using System.ComponentModel;

namespace forgotten_construction_set
{
	public enum ArmourClass
	{
        [Description("GEAR_CLOTH_布甲")]
		GEAR_CLOTH,
        [Description("GEAR_LIGHT_轻甲")]
        GEAR_LIGHT,
        [Description("GEAR_MEDIUM_中甲")]
        GEAR_MEDIUM,
        [Description("GEAR_HEAVY_重甲")]
        GEAR_HEAVY,
        [Description("GEAR_MAX_最大")]
        GEAR_MAX
    }
}