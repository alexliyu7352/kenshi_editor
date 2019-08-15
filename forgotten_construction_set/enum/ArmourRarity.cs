using System;
using System.ComponentModel;
namespace forgotten_construction_set
{
	public enum ArmourRarity
	{
        [Description("GEAR_PROTOTYPE_原型")]
        GEAR_PROTOTYPE,
        [Description("GEAR_CHEAP_便宜")]
        GEAR_CHEAP,
        [Description("GEAR_STANDARD_标准")]
        GEAR_STANDARD,
        [Description("GEAR_GOOD_良好")]
        GEAR_GOOD,
        [Description("GEAR_QUALITY_专家")]
        GEAR_QUALITY,
        [Description("GEAR_MASTER_大师")]
        GEAR_MASTER
    }
}