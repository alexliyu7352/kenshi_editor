using forgotten_construction_set;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace forgotten_construction_set.dialog
{
    public partial class ConditionControl : UserControl
    {

        private static Dictionary<DialogConditionEnum, object> conditionDefaults;



        public GameData.Item CurrentLine
        {
            get;
            set;
        }

        public GameData.Item Item
        {
            get;
            set;
        }

        static ConditionControl()
        {
            ConditionControl.conditionDefaults = new Dictionary<DialogConditionEnum, object>();
        }

        public ConditionControl()
        {
            this.InitializeComponent();
            if (ConditionControl.conditionDefaults.Count == 0)
            {
                ConditionControl.createDefaults();
            }
        }

        public static void createDefaults()
        {
            ConditionControl.conditionDefaults.Add(DialogConditionEnum.DC_HAS_TAG, CharacterPerceptionTags_LongTerm.LT_NONE);
            ConditionControl.conditionDefaults.Add(DialogConditionEnum.DC_PERSONALITY_TAG, PersonalityTags.PT_NONE);
            ConditionControl.conditionDefaults.Add(DialogConditionEnum.DC_FACTION_VARIABLE, "");
        }

      
    }
}