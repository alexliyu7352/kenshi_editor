using System;
using System.ComponentModel;
namespace forgotten_construction_set
{
	public enum taskPriority
    { 
        [Description("TP_JUST_ACTION_只是动作")]
        TP_JUST_ACTION,
        [Description("TP_FLUFF_悠闲")]
        TP_FLUFF,
        [Description("TP_NON_URGENT_非紧急")]
        TP_NON_URGENT,
        [Description("TP_URGENT_紧急")]
        TP_URGENT,
        [Description("TP_OBEDIENCE_服从")]
        TP_OBEDIENCE,
        [Description("TP_MAX_SIZE_不知道啥意思")]
        TP_MAX_SIZE
	}
}