using System;
using System.Drawing;
using System.IO;

namespace forgotten_construction_set
{
	public class head
	{
		public static bool isAMod;

		private navigation nav;

		private const string TEXTURE_DDS = "Nvidia dds|*.dds";

		private const string TEXTURE_ANY = "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp";

		public GameData gameData = new GameData();

		private static string activeFilename;

		static head()
		{
			head.isAMod = false;
			head.activeFilename = "";
		}

		public head(navigation n)
		{
			this.nav = n;
		}

		public static void AutomaticChanges(GameData.Item item)
		{
			itemType _itemType = item.type;
		}

		private static void rename(GameData.Item item, string from, string to)
		{
			if (item.ContainsKey(from))
			{
				item[to] = item[from];
				item.Remove(from);
			}
		}

		public void setActiveFilename(string file)
		{
			head.activeFilename = Path.GetFileName(file);
		}

		public enum Ambience
		{
			Ashlands,
			Boneyard,
			Canyons,
			Coast,
			Desert,
			Forest,
			Jungle,
			Plains,
			Swamplands,
			Tarsands
		}

		public enum AnimationEvent
		{
			NONE,
			RELOAD
		}

		public enum AttachSlot
		{
			ATTACH_WEAPON,
			ATTACH_BACK,
			ATTACH_HAIR,
			ATTACH_HAT,
			ATTACH_EYES,
			ATTACH_BODY,
			ATTACH_LEGS,
			ATTACH_NONE,
			ATTACH_SHIRT,
			ATTACH_BOOTS,
			ATTACH_GLOVES,
			ATTACH_NECK,
			ATTACH_BACKPACK,
			ATTACH_BEARD,
			ATTACH_BELT
		}

		public enum Axis
		{
			X_AXIS,
			Y_AXIS,
			Z_AXIS
		}

		public enum BadyPartType
		{
			PART_TORSO,
			PART_LEG,
			PART_ARM,
			PART_HEAD
		}

		public enum BiomeMusic
		{
			Dunes,
			Savanah,
			Swamp,
			Tarsands,
			None
		}

		public enum BuildingPlacementGroundType
		{
			ANY,
			LAND,
			WATER
		}

		public enum BuildingRotation
		{
			ROTATION_NONE,
			ROTATION_CONSTANT,
			ROTATION_OUTPUT_BASED,
			ROTATION_WIND_SPEED,
			ROTATION_FACE_WIND_DIRECTION,
			ROTATION_TARGET
		}

		public enum BuildingShader
		{
			DEFAULT,
			ALPHA,
			FOLIAGE,
			DUAL,
			EMISSIVE
		}

		public enum BuildingSound
		{
			None,
			Bed,
			Dummy,
			Farm,
			Generator,
			Grain_Silo,
			House,
			Mine_Ore,
			Mine_Stone,
			Research_Bench,
			Store_Armor,
			Store_Building_Materials,
			Store_Flour,
			Store_Ore,
			Store_Prisoner,
			Store_Rum,
			Store_Stones,
			Stove,
			Well,
			Wind_Generator,
			Chain_Bench,
			Fabric_Loom,
			Leather_Bench,
			Medical_Desk,
			Plate_Station,
			Shop_Counter,
			Smithy,
			Stone_Processor,
			Armorer,
			Bar,
			Boxes,
			Campfire,
			Gate,
			General_Goods,
			Hive_Hut,
			Lamp,
			Long_House,
			Outhouse,
			Tailor,
			Tower,
			Travel,
			Turret,
			Wall_Metal,
			Wall_Station_House,
			Wall_Stone,
			Wall_Wood,
			Wood_Object_Big,
			Wood_Object_Small
		}

		public enum ColourChannel
		{
			RED,
			GREEN,
			BLUE,
			ALPHA
		}

		public enum DoorMaterial
		{
			Wood,
			Mechanical,
			Gate
		}

		public enum DoorState
		{
			CLOSED,
			OPEN,
			LOCKED,
			BROKEN
		}

		public enum EffectType
		{
			NONE,
			CAMERA,
			POINT,
			WANDERING,
			GLOBAL,
			CAMERA_RAIN,
			CAMERA_ACID_RAIN,
			POINT_LIGHTING,
			WANDERING_STORM,
			WANDERING_GAS,
			GLOBAL_POINT
		}

		public enum EffectVolumeType
		{
			SPHERE,
			CYLINDER
		}

		public enum Either
		{
			NO,
			YES,
			EITHER
		}

		public enum FarmLayout
		{
			GRID,
			CLUSTER,
			RANDOM
		}

		public enum FoliageVisibilityRange
		{
			CLOSE,
			MEDIUM,
			FAR,
			FEATURE
		}

		public enum GroundType
		{
			SAND,
			GRASS,
			CONCRETE,
			WOOD,
			METAL,
			WATER,
			MUD,
			SNOW,
			DIRT,
			ASH
		}

		public enum GunSounds
		{
			CROSSBOW,
			HARPOON_GUN,
			CROSSBOW_TURRET,
			HARPOON_TURRET
		}

		public enum HideStump
		{
			NONE,
			LEFT_ARM,
			RIGHT_ARM,
			BOTH_ARMS
		}

		public enum InteriorSound
		{
			None,
			Metal,
			Stone,
			Wicker,
			Wood
		}

		public enum InventorySound
		{
			Backpack,
			Building_Material,
			Fabric,
			Flour,
			Food,
			Kits,
			Leather,
			Luxury,
			Narcotics,
			Ore,
			Potato,
			Robotic_Component,
			Rum,
			Steel_Bar,
			Tools,
			Water,
			Wheat,
			Armor_Plating,
			Blueprints,
			Meat
		}

		public enum ItemShader
		{
			DEFAULT,
			ALPHA,
			DOUBLE_SIDED
		}

		private enum LeftRight
		{
			SIDENEITHER,
			SIDE_LEFT,
			SIDE_RIGHT,
			SIDE_BOTH
		}

		public enum LightEffect
		{
			NONE,
			PULSE,
			FLICKER,
			SHIMMER
		}

		public enum LightType
		{
			POINT,
			SPOT
		}

		public enum LimbSlot
		{
			LEFT_ARM = 50,
			RIGHT_ARM = 51,
			LEFT_LEG = 52,
			RIGHT_LEG = 53
		}

		public enum MapFeatureMode
		{
			UV_MAPPED,
			TRIPLANAR,
			TERRAIN,
			DUAL_TEXTURE,
			FOLIAGE,
			DUAL_TRIPLANAR,
			EMISSIVE
		}

		public enum MaterialType
		{
			MATERIAL_CLOTH,
			MATERIAL_LEATHER,
			MATERIAL_CHAIN,
			MATERIAL_METAL_PLATE
		}

		public enum MiningResource
		{
			NONE,
			IRON,
			STONE,
			COPPER,
			CARBON,
			WATER,
			GROUND
		}

		public enum MissionTypeEnum
		{
			MISSION_NULL,
			MISSION_ESCORT
		}

		public class obj
		{
			public int id;

			public string stringID;

			public string name;

			public itemType type;

			public head.subCategory subCategory;

			public bool mergeMode;

			public bool isActiveFile;

			public obj(itemType t, bool activeFile)
			{
				string str = ".xml";
				this.type = t;
				this.name = string.Concat(t.ToString(), this.id.ToString());
				if (t == itemType.AI_PACKAGE)
				{
					this.addEnum("unloaded func", UnloadedPlatoonJob.UPJOB_NONE, "", "the simplified behavior of the AI when in the unloaded state");
					this.addEnum("signal func", BlackboardSignalFunctions.SIGNAL_NONE, "", "set the squads signal function- what triggers this package starting/ending.  For contract jobs, you usually want SIG_TIMED_CONTRACT");
					this.@add("clears existing jobs", false, "", "");
					this.addList("Leader AI Goals", itemType.AI_TASK, "0,24,0", 99, "leader AI", "additional AI goals for the leader character, val1 and val2 is start and finish time of day (0-23) it can't run outside of these hours. Val3 is weight");
					this.addList("Squad AI Goals", itemType.AI_TASK, "0,24,0", 99, "leader AI", "additional AI goals for the other characters, val1 and val2 is start and finish time of day (0-23) it can't run outside of these hours.. Val3 is weight");
					this.addList("Squad2 AI Goals", itemType.AI_TASK, "0,24,0", 99, "leader AI", "additional AI goals for the other characters, val1 and val2 is start and finish time of day (0-23) it can't run outside of these hours. Val3 is weight");
					this.addList("Slave AI Goals", itemType.AI_TASK, "0,24,0", 99, "leader AI", "AI goals (obedience) for any enslaved characters, val1 and val2 is start and finish time of day (0-23) it can't run outside of these hours. Val3 is weight");
					this.addList("Leads to", itemType.AI_PACKAGE, "", 99, "leader AI", "If set, then the successful completion of this package will activate this next package");
					this.addList("inherits from", itemType.AI_PACKAGE, "", 99, "leader AI", "the AI package will also include everything from this one");
					this.addList("contract end talk passive", itemType.DIALOGUE, "", 99, "dialog", "If this is an AI contract, this is the dialog event that will trigger when it ends, this one will trigger if there is no [contract end dialog delivery] event, or if the player is KO or unreachable");
					this.addList("contract end dialog delivery", itemType.DIALOGUE, "", 99, "dialog", "If this is an AI contract, this is the dialog event that will trigger when it ends.  This one will trigger a PLAYER_TALK_TO event, and will override the passive one if there is one.  Use sparingly!");
				}
				if (t == itemType.FACTION_CAMPAIGN)
				{
					this.addEnum("key", WarCampaignEnum.ASSAULT_TOWN, "", "");
					this.@add("num forces", 20, "", "minimum number of men needed to do this campaign");
					this.@add("display name", "", "", "name to display in game");
					this.@add("territorial triggers", false, "territorial trigger", "if true then will trigger randomly if the player has a base within range of this factions towns, the campaign object must also be listed in the FACTION in order to trigger this way");
					this.@add("chance per day", 0f, "territorial trigger", "0-1 chance per day of triggering");
					this.@add("target population min", 0, "territorial trigger", "player town pop must be at least this");
					this.@add("target tech level >=", 0, "territorial trigger", "player faction TOWN size level must be at least this (0-3, 3 is crazy).  (not tech level anymore)");
					this.@add("range near", 0f, "territorial trigger", "at this range or nearer, the chance to trigger will be equal to [chance per day] (attack target range from home base in meters).  It scales down after this.");
					this.@add("range far", 0f, "territorial trigger", "at this range or higher, the chance to trigger will be 0 (attack target range from home base in meters)");
					this.@add("target characters", false, "", "false=only target bases, true=will target characters instead, and can trigger without a player base");
					this.@add("repeat limit", 4f, "territorial trigger", "limits frequency of repeating this campaign, in days.  Only applies to territorial triggers");
					this.@add("ignores chance mults", false, "territorial trigger", "this campaign will ignore the frequency multiplier in the options.  Use for traders");
					this.@add("is hostile", true, "", "sets the text of player messages to be scary");
					this.@add("ignores nogo zones", false, "", "this campaign will ignore the factions [no-go zones].  Use for the extra angry or desperate campaigns");
					this.@add("can talk before arrival", false, "", "if false then you won't be able to talk to the leader until he arrives");
					this.addEnum("travel speed loaded", MoveSpeed.NO_SPEED_CHANGE, "", "sets the squads travel speed");
					this.addEnum("travel speed unloaded", MoveSpeed.NO_SPEED_CHANGE, "", "sets the squads travel speed when out of the active area");
					this.addList("specific target NPC", itemType.CHARACTER, 1, "", "the assault will target this specific character (eg for assassination or rescue), and only triggers if this character exists");
					this.addList("inherits from", itemType.FACTION_CAMPAIGN, 1, "", "copies from this base");
					this.addList("pt1 AI leader", itemType.AI_PACKAGE, 1, "", "AI package used by the leader squad");
					this.addList("pt1 AI others", itemType.AI_PACKAGE, 1, "", "AI package used by the squads");
					this.addList("pt2 AI leader", itemType.AI_PACKAGE, 1, "", "AI package used by the leader squad");
					this.addList("pt2 AI others", itemType.AI_PACKAGE, 1, "", "AI package used by the squads");
					this.addList("dialog announcement", itemType.DIALOGUE, 1, "", "dialog to be used for the announcement, if the npc doesn't have any in his package");
					this.addList("special leader", itemType.CHARACTER, 1, "", "campaign can only run if this character is available to lead it.  Has to be a unique NPC");
					this.addList("squads to use", itemType.SQUAD_TEMPLATE, "100", 99, "100", "the army will be made up of any of these squads, if nothing is listed it will try to use TOWN [roaming spawns]");
					this.addList("retreat AI leader", itemType.AI_PACKAGE, 1, "", "AI package used by the leader squad");
					this.addList("retreat AI others", itemType.AI_PACKAGE, 1, "", "AI package used by the squads");
					this.addList("victory AI leader", itemType.AI_PACKAGE, 1, "", "AI package used by the leader squad");
					this.addList("victory AI others", itemType.AI_PACKAGE, 1, "", "AI package used by the squads");
					this.addList("victory trigger", itemType.FACTION_CAMPAIGN, "0, 0, 100", 1, "", "the next campaign to trigger if we win, val0+val1 is min-max time to trigger, val2 is the %random chance (is absolute chance, so if you list 2 things which total to less than 100, then there is a chance of nothing)");
					this.addList("loss trigger", itemType.FACTION_CAMPAIGN, "0, 0, 100", 1, "", "the next campaign to trigger if we lose, val0+val1 is min-max time to trigger, val2 is the %random chance (is absolute chance, so if you list 2 things which total to less than 100, then there is a chance of nothing)");
					this.addList("world state", itemType.WORLD_EVENT_STATE, "0", 99, "", "add requirements that the world has a certain state (1=true, 0=false) (AND combination)");
					this.addList("faction override", itemType.FACTION, "", 99, "", "force the campaign to trigger using this faction (as the attackers).  Use to allow other factions to trigger this campaign");
					this.addList("trigger player ally", itemType.FACTION_CAMPAIGN, 99, "", "triggers this campaign if its faction is an ally of the player");
				}
				if (t == itemType.FACTION_CAMPAIGN || t == itemType.DIALOGUE_LINE)
				{
					this.addList("unlock campaign", itemType.FACTION_CAMPAIGN, "", 1, "ai", "essentially sets the campaigns [territorial] setting to true, so doesn't actually trigger it, just makes it possible");
					this.addList("lock campaign", itemType.FACTION_CAMPAIGN, "", 1, "ai", "essentially sets the campaigns [territorial] setting to false");
				}
				if (t == itemType.WORLD_EVENT_STATE)
				{
					this.addList("NPC is", itemType.CHARACTER, "1", 99, "", "is this unique NPC alive (1) or dead (0) or imprisoned (2)");
					this.addList("NPC is NOT", itemType.CHARACTER, "1", 99, "", "is this unique NPC NOT alive (1) or dead (0) or imprisoned (2)");
					this.addList("town okay", itemType.TOWN, "1", 99, "", "is this town okay (1) or destroyed (0)");
					this.addList("player ally", itemType.FACTION, "1", 99, "", "is the faction an ally of the player");
					this.addList("player enemy", itemType.FACTION, "1", 99, "", "is the faction an enemy of the player");
					this.@add("player involvement", false, "", "if true, then this state will only be true if the player was involved in at least one of the specified states.  If false, then it won't care about player involvement either way");
					this.@add("notes", "", "", "not shown in game, just used for design notes");
				}
				if (t == itemType.SINGLE_DIPLOMATIC_ASSAULT)
				{
					this.@add("repeat timer hours min", 10, "timer", "timer for the repeated assaults");
					this.@add("repeat timer hours max", 10, "timer", "timer for the repeated assaults");
					this.@add("is aggressive", true, "category", "important characteristic for the AI to know.  Is this mission aggressive against it's target? Does it launch at allies or enemies?");
					this.addList("main squad", itemType.SQUAD_TEMPLATE, 1, "main squad", "The main leading squad");
					this.addList("dialogue", itemType.DIALOGUE, 1, "dialogue", "override dialog for the main squad leader");
					this.addList("dialogue announce", itemType.DIALOGUE, 1, "dialogue announce", "override announcement dialog for the main squad leader");
					this.addList("dialogue squad", itemType.DIALOGUE, 1, "dialogue referral", "dialog for rest of squad that will usually refer player to the leader");
					this.addList("conditions", itemType.DIALOG_ACTION, 99, "", "");
					this.addList("delivery AI package", itemType.AI_PACKAGE, 1, "package", "Specific, one-off completable packages that have top priority");
					this.addList("AI packages", itemType.AI_PACKAGE, 99, "packages", "main list of AI packages, an AI package is an entire shift in behavior");
					this.addList("fallback AI package", itemType.AI_PACKAGE, 1, "package", "The fallback AI package, when have nothing else to do (eg patrol, go home)");
				}
				if (t == itemType.DIPLOMATIC_ASSAULTS)
				{
					this.addList("assaults", itemType.SINGLE_DIPLOMATIC_ASSAULT, 999, "assaults", "");
				}
				if (t == itemType.DIALOGUE_PACKAGE)
				{
					this.addList("dialogs", itemType.DIALOGUE, "0,0,0", 999, "dialogs", "");
					this.addList("inheritsFrom", itemType.DIALOGUE_PACKAGE, 99, "in", "Any dialogue packages here will also be combined");
				}
				if (t == itemType.DIALOGUE)
				{
					this.@add("for enemies", false, "", "true if this dialog can still be triggered by player if enemies");
					this.@add("monologue", true, "", "true if this has no player replies, every line is the speaker");
					this.@add("locked", false, "special", "if locked then this conversation can only be used once it's been unlocked by a dialog line using the [unlocks] event dropdown");
					this.@add("one at a time", false, "special", "this prevents more than one person at a time running this dialog.  Use to prevent crowds all saying the same thing during events");
				}
				if (t == itemType.WORD_SWAPS)
				{
					this.@add("persistent use", false, "special", "if true then whichever wordswap gets chosen will be always used by that character for now on.");
				}
				if (t == itemType.DIALOGUE_LINE || t == itemType.WORD_SWAPS || t == itemType.DIALOGUE)
				{
					this.addList("conditions", itemType.DIALOG_ACTION, 99, "", "");
					this.addList("lines", itemType.DIALOGUE_LINE, "0", 99, "", "");
					this.addList("in town of", itemType.FACTION, 1, "in town of", "adds a condition: that we must be currently in a town/base belonging to the given faction");
					this.addList("locks", itemType.DIALOGUE, 99, "locks", "locks the given dialogue, use for special cases when you gotta lock something?");
					this.addList("unlocks", itemType.DIALOGUE, 2, "unlocks", "unlocks the given dialogue, must be actually in the npcs dialogue package somewhere for it to work.  Will cause THIS dialog to then become locked.  Used for conversations that progress to other conversations in stages.");
					this.addList("unlock but keep me", itemType.DIALOGUE, 2, "unlocks", "unlocks the given dialogue, must be actually in the npcs dialogue package somewhere for it to work.  Will NOT cause this dialog to then become locked.");
					this.addList("my faction", itemType.FACTION, 1, "my faction", "adds a condition: I belong to the given faction");
					this.addList("target has item", itemType.ITEM, 99, "item", "adds a condition: target has this item, or any of these items");
					this.addList("target faction", itemType.FACTION, 1, "target faction", "adds a condition: target belongs to the given faction");
					this.addList("target race", itemType.RACE, 99, "target race", "adds a condition: target squad has a member who is any of the given races.  Multiple races is treated as an OR condition");
					this.addList("no target race", itemType.RACE, 99, "target race", "adds a condition: target squad has NO member who is any of the given races.  Multiple races is treated as an AND condition");
					this.addList("my race", itemType.RACE, 99, "target race", "adds a condition: my race is any of the given races.  Multiple races is treated as an OR condition");
					this.addList("my subrace", itemType.RACE, 99, "race", "My specific sub-race.  Multiple races is treated as an OR condition");
					this.addList("change relations", itemType.FACTION, "0", 99, "race", "adjusts the relations with the given faction by this amount");
					this.addList("crowd trigger", itemType.DIALOGUE, "100", 1, "et", "triggers the given dialog to start on all other squadmembers. Use for things like group cheering, laughing and war cries.  Should usually be 1 liners.  The number is the % of squadmembers to trigger");
					this.addList("world state", itemType.WORLD_EVENT_STATE, "1", 99, "et", "adds this world state as a AND condition.  1=true, 0=false");
					this.addList("has package", itemType.DIALOGUE_PACKAGE, 99, "race", "condition: has a specific dialog package.  refers to speaker only, unless its on an interjection node");
					this.addList("give item", itemType.ITEM, "1", 99, "race", "action: give target this item.  Will conjure the item out of thin air.");
					this.addList("is character", itemType.CHARACTER, 99, "race", "condition: is a specific character. refers to speaker only, unless its on an interjection node");
					this.addList("target carrying character", itemType.CHARACTER, 99, "race", "condition: is carrying a specific character. refers to target");
					this.addList("target has item type", itemType.ITEM, 1, "target has item type", "adds a condition: does the target have an item of the same TYPE as this one (eg narcotics, meds, food)");
					this.addEnum("repetition limit", DialogRepetitionEnum.DR_NO_LIMIT, "main", "Sets a limit on how frequently a line can be played.");
					this.@add("chance permanent", 100f, "main", "percentage chance that the character will HAVE this dialog option.  Not 'use', 'have'.  Its a permanent assignment or absence, and is saved forever.  Use to make rare and occasional characters.");
					this.@add("chance temporary", 100f, "main", "percentage chance that the character will USE this dialog option.  It can be used for things like random descisions.");
					this.@add("score bonus", 0, "main", "This can artificially adjust the score to give a line priority");
					this.addEnum("target is type", CharacterTypeEnum.OT_NONE, "main", "Only for target characters of the given type (if set)");
					if (t == itemType.DIALOGUE_LINE || t == itemType.WORD_SWAPS)
					{
						this.addEnum("speaker", TalkerEnum.T_ME, "main", "specifies who is talking.  Interjectors are set in the green nodes. T_TARGET_WITH_RACE will return a target matching the [target race] list you assign to that line (or else fails)");
					}
				}
				if (t == itemType.DIALOGUE_LINE)
				{
					this.addList("effects", itemType.DIALOG_ACTION, 99, "", "");
					this.addStringLooping("text0", "", "text", "the dialog text(s)");
					GameData.getDesc(this.type, "text0").flags |= 16;
					this.addList("change AI", itemType.AI_PACKAGE, "0", 1, "ai", "permanently changes the AI package, doesn't affect the AI contract.  BE CAREFUL, remember it will change the whole squad, slaves and masters.  May want to use with DA_SEPARATE_TO_OWN_SQUAD.");
					this.addList("AI contract", itemType.AI_PACKAGE, "0", 1, "ai", "activates the given AI package as a contract job, val0 is the number of hours it lasts for.");
					this.addList("trigger campaign", itemType.FACTION_CAMPAIGN, "0, 0, 100", 1, "ai", "Triggers the given campaign, which will start after a random number of hours between val0-val1, val2 is %chance (is absolute chance, so if you list 2 things which total to less than 100, then there is a chance of nothing)");
					this.addList("interrupt", itemType.DIALOGUE, 1, "starts", "If the player interrupts us with a talking event, this will be the conversation chosen.  Reset at the end of conversation, or until changed");
					this.@add("interjection", null, "Base", "Is this an iterjection node. Do not edit.");
				}
				if (t == itemType.NEW_GAME_STARTOFF)
				{
					this.addList("squad", itemType.SQUAD_TEMPLATE, 99, "squad", "Starting squads");
					this.addList("town", itemType.TOWN, 1, "town", "starting location, random if left blank");
					this.@add("money", 100, "", "Starting cash");
					this.@add("difficulty", "Normal", "", "difficulty description");
					this.addText("description", "", "", "The in-game description.");
					this.@add("style", "", "", "The description of what playstyle it best suits.");
					this.addList("faction relations", itemType.FACTION, "0", 99, "Faction Relations", "-100 to +100\nrelationship with other factions, if not listed then assumed default");
					this.addList("research", itemType.RESEARCH, 99, "Faction Relations", "starting research techs");
					this.addList("force race", itemType.RACE, 99, "", "limits character creation to these races only");
					this.@add("force start pos", false, "position", "Don't start in a town, start at the coordinates specified");
					this.@add("start pos X", 4000, "position", "");
					this.@add("start pos Z", 4000, "position", "");
				}
				if (t == itemType.VENDOR_LIST)
				{
					this.@add("items count", 40, "items", "Total amount of items that are taken from the vendor list.\nThe actual number of used items will depend of the available space in the inventories.");
					this.addList("items", itemType.ITEM, "100", 99, "Items", "value is relative chance");
					this.addList("containers", itemType.CONTAINER, "100", 99, "Containers", "value is relative chance");
					this.addList("weapon manufacturers", itemType.WEAPON_MANUFACTURER, "100", 99, "Weapon manufacturers", "value is relative chance");
					this.addList("weapons", itemType.WEAPON, "100", 99, "Weapons", "value is relative chance");
					this.addList("crossbows", itemType.CROSSBOW, "100", 99, "Weapons", "val0 is relative chance");
					this.addList("robotics", itemType.LIMB_REPLACEMENT, "100", 99, "Weapons", "val0 is relative chance");
					this.addList("clothing", itemType.ARMOUR, "100", 99, "Clothing", "value is relative chance");
					this.addList("armour blueprints", itemType.ARMOUR, "100", 99, "blueprints", "Listing armour here will put the blueprints up for sale");
					this.addList("crossbow blueprints", itemType.CROSSBOW, "100", 99, "blueprints", "Listing crossbows here will put the blueprints up for sale");
					this.addList("backpack blueprints", itemType.CONTAINER, "100", 99, "blueprints", "Listing backpacks here will put the blueprints up for sale");
					this.addList("blueprints", itemType.RESEARCH, "100", 99, "blueprints", "value is relative chance");
					this.addList("maps", itemType.MAP_ITEM, "100", 99, "maps", "value is relative chance");
					this.@add("money item min", 0, "money", "Minimum amount of physical money item.");
					this.@add("money item max", 0, "money", "Maximum amount of physical money item.");
					this.@add("money item prob", 0f, "money", "Probability of having physical money item. [0-1]");
				}
				if (t == itemType.ITEM_PLACEMENT_GROUP)
				{
					this.@add("placeholder", "", "General", "Specifies the placeholder item mesh to be used in the level editor.\nDefault: Random");
					this.@add("random yaw", true, "General", "Whether the item orientation is random when the item is spawned");
					this.@add("min respawn time", 24, "Respawn", "Minimum item respawn time in hours.");
					this.@add("max respawn time", 48, "Respawn", "Maximum item respawn time in hours.");
					this.addList("items", itemType.ITEM, "100", 99, "items", "Items to put in this group, val0 is chance");
					this.addList("clothing", itemType.ARMOUR, "100", 99, "armours", "Armour to put in this group, val0 is chance");
					this.addList("armour blueprints", itemType.ARMOUR, "100", 99, "armour blueprints", "Armour blueprint to put in this group, val0 is chance");
					this.addList("blueprints", itemType.RESEARCH, "100", 99, "blueprints", "Blueprints to put in this group, val0 is chance");
					this.addList("containers", itemType.CONTAINER, "100", 99, "containers", "Containers to put in this group, val0 is chance");
					this.addList("weapons", itemType.WEAPON, "100", 99, "weapons", "Weapons to put in this group, val0 is chance");
					this.addList("weapon manufacturers", itemType.WEAPON_MANUFACTURER, "100", 99, "weapon manufacturers", "Weapon manufacturer based on the selected item, val0 is chance");
					this.addList("maps", itemType.MAP_ITEM, "100", 99, "maps", "Map items to put in this group, val0 is chance");
				}
				if (t == itemType.CHARACTER_PHYSICS_ATTACHMENT)
				{
					this.addFileName("file male", "", "Scythe|*.phs", "files", "");
					this.addFileName("file female", "", "Scythe|*.phs", "files", "");
					this.@add("bone name", "Bip01 Spine2", "", "the name of the bone to attach to");
					this.addList("material", itemType.MATERIAL_SPECS_CLOTHING, 99, "", "");
					this.addList("light data", itemType.LIGHT, 1, "", "if the scythe file has any lights, it will be created with this data");
				}
				if (t == itemType.ITEM || t == itemType.ARMOUR || t == itemType.WEAPON || t == itemType.CONTAINER || t == itemType.NEST_ITEM || t == itemType.MAP_ITEM || t == itemType.CROSSBOW || t == itemType.LIMB_REPLACEMENT)
				{
					this.@add("value", 0, "inventory", "Monetary value of this item");
					this.@add("weight kg", 1f, "inventory", "Weight of the item. For weapons this sets the minimum weight, otherwise is overwritten by the blunt damage multiplier.  40kg is equivalent to a multiplier of 1.0.  0kg is 0.0.  Katanas: 1-2kg, Sabres 2-10kg, Hackers 10-20kg, Heavies 20-40kg");
					this.@add("inventory footprint width", 1, "inventory", "size of the item in the inventory, width in 2d squares");
					this.@add("inventory footprint height", 1, "inventory", "size of the item in the inventory, width in 2d squares");
					if (t == itemType.ITEM || t == itemType.MAP_ITEM)
					{
						this.addEnum("inventory sound", head.InventorySound.Fabric, "inventory", "sound the item makes in the inventory");
					}
					if (t == itemType.CONTAINER)
					{
						this.addEnum("inventory sound", head.InventorySound.Backpack, "inventory", "sound the item makes in the inventory");
					}
					if (t == itemType.ARMOUR || t == itemType.LIMB_REPLACEMENT)
					{
						this.addList("material", itemType.MATERIAL_SPECS_CLOTHING, 1, "material", "");
					}
					else if (t == itemType.NEST_ITEM)
					{
						this.addList("material", itemType.MATERIAL_SPEC, 1, "material", "");
					}
					else if (t != itemType.WEAPON)
					{
						this.addList("material", itemType.MATERIAL_SPECS_CLOTHING, 1, "material", "");
					}
					if (t != itemType.MAP_ITEM)
					{
						this.addEnum("slot", head.AttachSlot.ATTACH_NONE, "", "grants special permission to go in certain inventory slots");
					}
					this.@add("has collision", true, "files", "Sets whether it has a collision mesh/box.");
					this.addFileName("physics file", "", string.Concat("Physics object|*", str), "files", "Used to represent items when dropped");
					this.addFileName("mesh", "", "Ogre mesh|*.mesh", "files", "the item mesh.  Sheathed if its a weapon");
					this.addFileName("ground mesh", "", "Ogre mesh|*.mesh", "files", "the item mesh when is on the ground (not equipped). If empty it uses the regular mesh.");
					this.addText("description", "", "", "Description of the item that is displayed in tooltips etc");
					this.@add("icon offset H", 0f, "inventory icon", "align the image horizontally");
					this.@add("icon offset V", -8f, "inventory icon", "align the image vertically");
					this.@add("icon zoom", 2.7f, "inventory icon", "scale the image (try to keep at 1.0), lower number makes it bigger");
					if (t != itemType.WEAPON)
					{
						this.addFileName("icon", "", "png|*.png", "files", "inventory icon image or building button image.\nOnly used if 'auto icon image' is false");
						this.@add("auto icon image", false, "inventory", "true to automatically generate the inventory icon");
						this.@add("icon offset pitch", 0f, "inventory icon", "rotate the item, in degrees");
						this.@add("icon offset roll", 0f, "inventory icon", "rotate the item, in degrees");
						this.@add("icon offset yaw", 90f, "inventory icon", "rotate the item, in degrees");
					}
					if (t != itemType.MAP_ITEM)
					{
						this.addList("races", itemType.RACE, 99, "material", "Only the listed races can equip this item");
						this.addList("races exclude", itemType.RACE, 99, "material", "none of the listed races can equip this item");
					}
				}
				if (t == itemType.ITEM || t == itemType.WEAPON || t == itemType.NEST_ITEM)
				{
					this.@add("auto icon image", true, "inventory", "true to automatically generate the inventory icon");
				}
				if (t == itemType.ITEM)
				{
					this.@add("artifact", false, "inventory", "true if this is a research artifact.  Adds a tag in the GUI tooltip.");
					this.@add("stackable", 1, "inventory", "number that can be stacked in inventory");
					this.@add("trade item", false, "item", "If true then player can always sell this item for full price");
					this.@add("charges", 1f, "item", "Amount of charge this item has if it is expendable");
					this.@add("profitability", 0.2f, "item", "If an item has ingredients then its price will be calculated from them, then this percentage will be added (0-1)");
					this.@add("quality", 1f, "item", "1-100, quality level of this item.  Eg a higher number would make a more effective medkit");
					this.@add("production time", 4, "item", "Number of hours it takes to craft this item (multiplied by the benches output multiplier)");
					this.@add("food crop", false, "item", "used to designate that this item is a base food ingredient, so it's affected by the global food price mult");
					this.addEnum("persistent", ItemPersistence.PERSIST_NONE, "item", "Rule to determine whether this item gets deleted when it gets unloaded");
					this.addEnum("item function", ItemFunction.ITEM_NO_FUNCTION, "item", "What this item does.");
					this.addList("ingredients", itemType.ITEM, "100", 5, "ingredients", "Used in crafting to show what this item is made of. Val1 is the amount, 100 = a 1:1 relationship, 50 = half of this ingredient is used up to make 1 of this. Also used to value an item.");
					this.addList("physics attachment", itemType.CHARACTER_PHYSICS_ATTACHMENT, 99, "", "A physics object that will also attach to the character.  It should be a Scythe file with one joint that is attached to the world- this joint will be found and attached to the character's bone");
				}
				if (t == itemType.NEST_ITEM)
				{
					this.@add("number of meshes", 0, "files", "If > 1 then you can include a batch of models, named filename1, filename2, filename3 etc.");
					this.addList("egg", itemType.ANIMAL_CHARACTER, 1, "", "animal that comes out of this egg (if it's an egg)");
					this.addList("cluster", itemType.NEST_ITEM, "1 10 200", 99, "", "items to cluster around this one, val0=num min, val1=max, val2=range");
					this.@add("trade item", false, "item", "If true then player can always sell this item for full price");
					this.@add("cluster min", 1, "item", "spawns several clustered together");
					this.@add("cluster max", 1, "item", "spawns several clustered together");
					this.@add("cluster range", 40, "item", "radius of the cluster area");
				}
				if (t == itemType.CONTAINER)
				{
					this.addList("physics attachment", itemType.CHARACTER_PHYSICS_ATTACHMENT, 99, "", "A physics object that will also attach to the character.  It should be a Scythe file with one joint that is attached to the world- this joint will be found and attached to the character's bone");
					this.@add("overall scale", 1f, "scale", "");
					this.addFileName("mesh female", "", "Ogre meshes|*.mesh", "files", "the mesh when worn by female characters, rigged with the standard character skeleton");
					this.@add("encumbrance effect", 1f, "inventory", "Multiplies the effect it has on encumbrance.  so < 1.0 will make stuff in this bag seem lighter, easier to carry.");
					this.@add("athletics mult", 1f, "inventory", "Penalty or bonus to movement speed when wearing this. < 1.0 will reduce max speed.");
					this.@add("combat speed mult", 1f, "inventory", "Penalty or bonus to combat speed when wearing this. < 1.0 will reduce max speed.");
					this.@add("combat skill bonus", 0, "inventory", "Penalty or bonus to combat attk/def skills when wearing this. ");
					this.@add("stealth mult", 0.5f, "inventory", "Penalty or bonus to stealth multiplier.");
					this.@add("storage size width", 10, "inventory", "size of the containers internal inventory");
					this.@add("storage size height", 10, "inventory", "size of the containers internal inventory");
					this.@add("stackable bonus minimum", 1, "inventory", "sets the stackable property of any item within to be at least this");
					this.@add("stackable bonus mult", 1f, "inventory", "multiplies the stackable property of any item stored within");
					this.@add("dont colorise", true, "", "if false then this item will be re-colored based on faction color scheme");
					this.addList("color", itemType.COLOR_DATA, 1, "color", "colorise this item, based on the colormap. Overrides faction coloriser.");
				}
				if (t == itemType.ATTACHMENT)
				{
					this.addEnum("attach slot", head.AttachSlot.ATTACH_HAIR, "properties", "The name of the slot that the item goes in");
					this.addFileName("mesh", "", "Ogre mesh|*.mesh", "mesh", "The item mesh");
					this.addFileName("mesh female", "", "Ogre mesh|*.mesh", "mesh", "The item mesh for female characters");
					this.addFileName("texture map", "", "Nvidia dds|*.dds", "mesh", "Texture for the male mesh");
					this.addFileName("texture map female", "", "Nvidia dds|*.dds", "mesh", "Texture for the female mesh");
					this.@add("alpha rejection", 128, "properties", "0-255 alpha cutoff.  A lower value gives smoother alpha blending, but can cause artifacting on complicated hair meshes.  A higher value reduces artifacts, but gives harder edges.");
					this.@add("specular", 0f, "properties", "Specular multiplier for head texture texture");
					this.@add("playable", true, "properties", "use to disable");
					this.addFileName("head texture", "", "Nvidia dds|*.dds", "head overlay", "Alpha overlay texture applied to the male character model");
					this.addFileName("head texture female", "", "Nvidia dds|*.dds", "head overlay", "Alpha overlay texture applied to the female character model");
					this.addEnum("head channel", head.ColourChannel.RED, "head overlay", "Color channel in head texture to use");
					this.addEnum("head channel female", head.ColourChannel.RED, "head overlay", "Color channel in head texture to use for female characters");
					this.addEnum("head alpha channel", head.ColourChannel.GREEN, "head overlay", "Color channel in head texture to use");
					this.addEnum("head alpha channel female", head.ColourChannel.GREEN, "head overlay", "Color channel in head texture to use for female characters");
					this.addEnum("hair alpha channel", head.ColourChannel.RED, "mesh", "Color channel on mesh texture map to use for alpha");
					this.addEnum("hair diffuse channel", head.ColourChannel.RED, "mesh", "Color channel on mesh texture map to use for diffuse brightness");
					this.@add("mipmap bias", 0f, "properties", "");
				}
				if (t == itemType.COLOR_DATA)
				{
					this.addColor("color 1", 0, "", "");
					this.addColor("color 2", 0, "", "");
				}
				if (t == itemType.WEAPON)
				{
					this.addFileName("bare sword", "", "Ogre mesh|*.mesh", "files", "the sword, bare blade, no sheath");
					this.addFileName("sheath", "", "Ogre mesh|*.mesh", "files", "sheath only, should line up with the sword mesh.");
					this.@add("length", 14, "", "The length of the weapon, the maximum reach");
					this.@add("cut damage multiplier", 1f, "damage", "Ranges around 0.5 to 1.5, this is multiplied by attack skill and is the bulk of the damage for a skilled swordsman");
					this.@add("blunt damage multiplier", 0f, "damage", "Ranges around 0.0 to 2.0.  This dictates weapon weight.  Is multiplied by stats and is the bulk of the damage for a heavy weapons guy. ");
					this.@add("pierce damage multiplier", 0f, "damage", "Ranges 0.5 to 1.5, this is multiplied by attack skill and is the bulk of the damage for a skilled swordsman");
					this.@add("min cut damage mult", 1f, "damage", "(multiplier of the equivalent value in material spec) Only handy for beginners, provides a minimum damage cap.  if damage < min then damage = rand(min)");
					this.@add("bleed mult", 1f, "damage", "multiplier for the amount of bleeding the weapon type causes");
					this.@add("animal damage mult", 1f, "damage", "damage bonus to animals");
					this.@add("human damage mult", 1f, "damage", "damage bonus to non-animals");
					this.@add("robot damage mult", 1f, "damage", "damage bonus to robotic creatures");
					this.@add("armour penetration", 0f, "damage", "0-1 value, subtracts this from the armour's defence as a percentage.  So 0.5 = armour loses 50% of its protection.  -0.5 = armour is 50% more effective against this weapon");
					this.@add("can block", true, "type", "true if this weapon can be used to block.  false for things like knives, fists, bows etc");
					this.addList("race damage", itemType.RACE, "125", 99, "", "bonus damage multiplier, in %, when attacking this race");
					this.addEnum("slot", head.AttachSlot.ATTACH_WEAPON, "inventory", "grants special permission to go in certain inventory slots");
					this.addEnum("skill category", WeaponCategory.SKILL_KATANAS, "type", "Indicates the weapon type.");
					this.addEnum("skill category animation override", WeaponCategory.ATTACK_NULL, "type", "use to override animations for unique weapons, mainly changes the guard stance");
					this.addEnum("material", head.WeaponMaterial.Metal, "type", "Material the weapon is made of");
					this.@add("material cost", 4, "craft", "amount of material needed to craft");
					this.@add("attack mod", 0, "mod", "bonus/penalty to melee attack skill");
					this.@add("defence mod", 0, "mod", "bonus/penalty to melee defence skill");
					this.@add("weight mult", 1f, "mod", "multiplier for weight, use it to make weapons lighter or heavier without affecting the blunt damage.  Should usually be kept at 1.0");
					this.@add("indoors mod", 0, "mod", "modifier to attk + def if we are indoors");
					this.@add("scale length", 1f, "scale", "");
					this.@add("scale width", 1f, "scale", "");
					this.@add("scale thickness", 1f, "scale", "");
					this.@add("overall scale", 1f, "scale", "");
					this.@add("icon offset H", -4f, "inventory icon", "align the image horizontally");
					this.@add("icon offset V", 0f, "inventory icon", "align the image vertically");
					this.@add("icon zoom", 1f, "inventory icon", "scale the image (try to keep at 1.0)");
				}
				if (t == itemType.MATERIAL_SPECS_WEAPON)
				{
					this.@add("scale length", 1f, "scale", "");
					this.@add("scale width", 1f, "scale", "");
					this.@add("scale thickness", 1f, "scale", "");
					this.@add("overall scale", 1f, "scale", "");
					this.@add("attack mod", 0, "mod", "bonus/penalty to melee attack skill");
					this.@add("defence mod", 0, "mod", "bonus/penalty to melee defence skill");
					this.addText("description", "", "", "description of this weapon model");
					this.@add("craft list fixed", false, "", "outdated weapons are removed from the list of craftable weapons, unless this is true.");
					this.addFileName("property map", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "material", "Metalness map in green channel");
				}
				if (t == itemType.WEAPON_MANUFACTURER)
				{
					this.addText("company description", "", "", "");
					this.@add("price mod", 1f, "", "1.0 multiplier == average prices");
					this.addList("weapon types", itemType.WEAPON, "100", 100, "weapon types", "the base weapon meshes to use.  val2 = chance");
					this.addList("weapon models", itemType.MATERIAL_SPECS_WEAPON, "40,100", 100, "weapon models", "the individual models of weapon\nval1 == overall level [1-100]\nval2 == chance");
					this.addList("faction owner", itemType.FACTION, 1, "faction", "optional, if this company is owned by a faction");
					this.@add("cut damage mod", 1f, "stats mod", "multiplier");
					this.@add("min cut damage", 1, "stats mod", "Base value.  Only handy for beginners, provides a minimum damage cap.  if damage < min then damage = rand(min)");
					this.@add("blunt damage mod", 1f, "stats mod", "multiplier");
					this.@add("weight mod", 1f, "stats mod", "multiplier");
				}
				if (t == itemType.ARMOUR)
				{
					this.addFileName("mesh female", "", "Ogre meshes|*.mesh", "files", "the mesh when worn by female characters, rigged with the standard character skeleton");
					this.addList("material female", itemType.MATERIAL_SPECS_CLOTHING, 1, "material", "");
					this.addList("body material", itemType.MATERIAL_SPECS_CLOTHING, 1, "material", "Material applied to the character model\nOnly used for shirt slot");
					this.addList("body material female", itemType.MATERIAL_SPECS_CLOTHING, 1, "material", "Material applied to the character model\nOnly used for shirt slot");
					this.addList("overlap items", itemType.ARMOUR, 99, "overlap items", "Any other items that might overlap this one.  If they are being worn at the same time, the overlap mesh will be used instead.  This will be a smaller version.");
					this.addFileName("overlap mesh", "", "Ogre meshes|*.mesh", "files", "Alternative mesh to use if one of the overlap items are also present.");
					this.addFileName("overlap mesh female", "", "Ogre meshes|*.mesh", "files", "Alternative mesh to use if one of the overlap items are also present.");
					this.addList("goes well with", itemType.ARMOUR, 999, "", "helps with NPC generation, for example if this is part of an armour set, its more likely to go with it");
					this.addList("goes badly with", itemType.ARMOUR, 999, "", "helps with NPC generation, helps avoid armour clashes");
					this.@add("hide beard", false, "", "Hide beard mesh when worn");
					this.@add("hide hair", true, "", "Hide hair mesh when worn (hats only)");
					this.@add("boot height", 0f, "", "Height this item shifts the character's feet off the ground");
					this.@add("cut into stun", 0.5f, "stats 0-1", "amount of cut damage that becomes stun damage instead, is a percentage of the cut resistance, 0-1");
					this.@add("blunt def bonus", 0f, "stats 0-1", "0-1 (additional).  Stats are generated by class and material enums, but this is an additional bonus.  Full amount will be added at max level, zero at level 0.");
					this.@add("cut def bonus", 0f, "stats 0-1", "0-1 (additional).  Stats are generated by class and material enums, but this is an additional bonus.  Full amount will be added at max level, zero at level 0.");
					this.@add("pierce def mult", 1f, "stats 0-1", "stats are generated by class and material enums, but this multiplier can influence them");
					this.@add("level bonus", 0, "stats 0-1", "adds or subtracts from the quality level (0-100 shoddy-standard etc) for the purposes of stat generation.  Use to add some variation between items.");
					this.addEnumLooping("weather protection0", WeatherAffecting.WA_NONE, "weather", "if this clothing protects from the weather");
					this.@add("weather protection amount", 0f, "weather", "[0-1] if this has any weather protection effects, then this is the strength (1.0 is total protection)");
					this.@add("relative price mult", 1f, "stats 0-1", "Affects price, as price is auto-generated for armour.");
					this.@add("fabrics amount", 0.2f, "stats 0-1", "Used when determining crafting materials cost.  material use (eg plates) is based on coverage, plus fabrics are consumed as a proportion of the main material use. This is that proportion.");
					this.@add("class name", "", "", "adds to description in []");
					this.addEnum("material type", head.MaterialType.MATERIAL_CLOTH, "stats 0-1", "Material this armour is made from. Affects sfx and stats.");
					this.addEnum("class", ArmourClass.GEAR_CLOTH, "stats 0-1", "Weight class. Affects stats.");
					this.addList("part coverage", itemType.LOCATIONAL_DAMAGE, "100", 10, "coverage", "body parts that are covered by this armour");
					this.addEnum("slot", head.AttachSlot.ATTACH_BODY, "", "grants special permission to go in certain inventory slots");
					this.addEnum("stigma", CharacterTypeEnum.OT_NONE, "", "if these clothes are associated with a certain type of character");
					this.@add("is locked", false, "locked", "true for things that are locked on, like slave shackles");
					this.@add("lock level", 20, "locked", "lock pick level for shackles");
					this.@add("hardness", 20f, "locked", "shackles resistance to cutting");
					this.@add("dont colorise", true, "", "if false then this item will be re-colored based on faction color scheme");
					this.addList("color", itemType.COLOR_DATA, 1, "color", "colorise this item, based on the colormap. Overrides faction coloriser.");
					this.@add("athletics mult", 1f, "skills", "Penalty or bonus to movement speed when wearing this. < 1.0 will reduce max speed.");
					this.@add("combat speed mult", 1f, "skills", "Penalty or bonus to combat speed when wearing this. < 1.0 will reduce max speed.");
					this.@add("combat attk bonus", 0, "skill bonus", "Penalty or bonus to combat attk/def skills when wearing this. ");
					this.@add("combat def bonus", 0, "skill bonus", "Penalty or bonus to combat attk/def skills when wearing this. ");
					this.@add("perception bonus", 0, "skill bonus", "Penalty or bonus to perception when wearing this. ");
					this.@add("stealth mult", 1f, "skills", "Penalty or bonus to stealth skills when wearing this. ");
					this.@add("ranged skill mult", 1f, "skills", "affects crossbow and friendly fire skills, but not turrets");
					this.@add("unarmed bonus", 0, "skill bonus", "adds or subtracts from the wearers Martial Arts skill");
					this.@add("fist injury mult", 1f, "skills", "multiplier for fist injury when punching things, use for things with padded gloves etc");
					this.@add("dodge mult", 1f, "skills", "multiplies the dodge skill");
					this.@add("dexterity mult", 1f, "skills", "multiplies dexterity");
					this.@add("asassination mult", 1f, "skills", "multiplies stat");
					this.@add("damage output mult", 1f, "skills", "multiplies overall damage that we do. Use if dexterity penalty isn't enough");
					GameData.setDesc(t, "", "hide parts", head.PartMapColours.White, "Hide parts of the body mesh. Parts are specified in the part map of the race.").flags = 256;
					this.addEnum("hide stump", head.HideStump.NONE, "", "Scale specified upper arms to zero when wearing this with mising limbs");
					GameData.addCondition(t, "hide stump", "slot", head.AttachSlot.ATTACH_BODY, true);
				}
				if (t == itemType.LIMB_REPLACEMENT)
				{
					this.addEnum("slot", head.LimbSlot.LEFT_ARM, "", "What part this replaces");
					this.addFileName("mesh female", "", "Ogre mesh|*.mesh", "files", "Mesh for female skeleton");
					this.@add("offset", 0f, "", "Height offset");
					this.@add("HP", 100, "stats", "max HP");
					this.@add("overall mult", 1f, "stats", "an overall multiplier for all the related stats");
					this.@add("swimming mult", 0.75f, "stats", "mult for swimming skill");
					this.@add("athletics mult", 1f, "stats legs", "run speed");
					this.@add("stealth mult", 1f, "stats legs", "mult for stealth skill");
					this.@add("value 1", 1000, "inventory", "price at best quality level");
					this.@add("strength mult", 1f, "stats arms", "");
					this.@add("dexterity mult", 1f, "stats arms", "");
					this.@add("thievery mult", 1f, "stats arms", "");
					this.@add("ranged mult", 1f, "stats arms", "");
					this.@add("unarmed damage bonus", 0, "stats", "");
					this.@add("craft time hrs", 10f, "", "time to craft in game hours");
					this.@add("HP 1", 100, "stats", "max HP");
					this.@add("swimming mult 1", 0.75f, "stats", "mult for swimming skill");
					this.@add("athletics mult 1", 1f, "stats legs", "run speed");
					this.@add("stealth mult 1", 1f, "stats legs", "mult for stealth skill");
					this.@add("strength mult 1", 1f, "stats arms", "at max quality");
					this.@add("dexterity mult 1", 1f, "stats arms", "at max quality");
					this.@add("thievery mult 1", 1f, "stats arms", "at max quality");
					this.@add("ranged mult 1", 1f, "stats arms", "at max quality");
					this.@add("unarmed damage bonus 1", 0, "stats", "");
					this.addList("ingredients", itemType.ITEM, "100", 5, "ingredients", "Used in crafting to show what this item is made of. Val1 is the amount, 100 = a 1:1 relationship, 50 = half of this ingredient is used up to make 1 of this. Also used to value an item.");
				}
				if (t == itemType.MAP_ITEM)
				{
					this.@add("artifact", false, "inventory", "true if this is a research artifact.  Adds a tag in the GUI tooltip.");
					this.@add("stackable", 1, "inventory", "number that can be stacked in inventory");
					this.@add("trade item", false, "item", "If true then player can always sell this item for full price");
					this.@add("charges", 1f, "item", "Amount of charge this item has if it is expendable");
					this.@add("profitability", 0.2f, "item", "If an item has ingredients then its price will be calculated from them, then this percentage will be added (0-1)");
					this.@add("quality", 1f, "item", "1-100, quality level of this item.  Eg a higher number would make a more effective medkit");
					this.@add("unlock count", 1, "map", "Total number of towns to randomly unlock. This value is clamped by the total sum of each town val0. (0 = all of them)");
					this.addList("towns", itemType.TOWN, "1, 100", 20, "towns", "List of towns this map could unlock and how many.\n1. Number of random towns of this type it unlocks. (0 = all of them)\n2. Probability of unlocking.");
				}
				if (t == itemType.BUILD_GRID)
				{
					this.@add("width", 4, "size", "");
					this.@add("length", 4, "size", "");
				}
				if (t == itemType.BOAT)
				{
					this.addList("grids", itemType.BUILD_GRID, "-1", 5, "", "adds building placement grids, val0 is floor num. -1 is the footprint for placing the building, 0-4 are for placing things ON the building");
					this.@add("buoyancy mult", 1f, "floating", "affects how strongly it floats");
					this.@add("buoyancy depth offset", 0f, "floating", "height offset, in units, that adjusts the waterline");
					this.@add("friction mult", 1f, "floating", "affects forward moving water resistance");
					this.@add("lateral friction mult", 1f, "floating", "affects sideways water resistance, a zero mult will result in a directionless boat, like a dustbin lid");
				}
				if (t == itemType.BOAT || t == itemType.BUILDING)
				{
					this.@add("scale", 1f, "type", "totally scales the entire building and colision by this multiplier.");
					this.@add("build materials", 1, "construction", "materials needed to build");
					this.@add("build speed mult", 1f, "construction", "Build time in hours = num [build materials] * [build speed mult]");
					this.@add("building height", 100f, "construction", "the height of this building, in units.  used to compute the construction shader");
					this.addEnum("select sound", head.BuildingSound.None, "audio", "Sound when building is selected");
					this.addEnum("exterior material", head.GroundType.CONCRETE, "audio", "Ground material for exterior parts");
					this.addEnum("interior material", head.GroundType.WOOD, "audio", "Ground material for interior parts");
					this.addEnum("interior ambience", head.InteriorSound.None, "audio", "Interior ambient sound type");
					this.addList("sounds", itemType.AMBIENT_SOUND, "0,0,0", 4, "audio", "Attached sound emitters.\nValues are x,y,z position offset in local coordinates");
					this.addText("Description", "", "type", "");
					this.@add("allow animals", false, "type", "Allow animals with 'cant go indoors' property to enter");
					this.addList("material", itemType.MATERIAL_SPEC, 1, "material", "base material.  If blank will use the local town material as the base.  Any parts without materials will also use this base material");
					this.addList("z incompatible parts", itemType.BUILDING_PART, 50, "parts", "If any parts in this list are used, then cannot use this part.");
					this.addList("parts", itemType.BUILDING_PART, "0,100", 50, "parts", "all the visual entities that make up the building.  Value1 is the group, one part will be chosen from each group, based on normalised percentage in value 2.  Group 0 is special, everything from there will be chosen based on val2 as an absolute percetage.");
					this.addList("interior", itemType.BUILDING_PART, "0,100", 50, "interior", "building parts that makes up the interior, they are only loaded when player characters are inside this building.");
					this.addList("interior mask", itemType.BUILDING_PART, 1, "mask", "the mesh will be used to mask the terrain out of the interior.  The xml collision will be used as the trigger hull to tell if we are indoors");
					this.addList("upgrades to", itemType.BUILDING, 1, "upgrades to", "if this can be upgraded into another building");
					this.addList("shares interiors with", itemType.BUILDING, 99, "", "other buildings that can use my interior list, MUST SHARE BOTH WAYS (in both buildings), also can only share with 1 other, because it's lazily coded");
					this.addList("construction", itemType.ITEM, "1", 50, "construction", "the materials needed to build this building");
				}
				if (t == itemType.BUILDING)
				{
					this.addFileName("icon", "", "Nvidia dds|*.dds|tga|*.tga|png|*.png", "files", "inventory icon image or building button image");
					this.@add("building category", "misc", "type", "category it is listed in- Walls, Furniture ");
					this.@add("building group", "", "type", "building group name");
					this.@add("links together", false, "linking", "true for things like walls, that link together.  A linked building needs 2 actors (in scythe) called \"left side\" and \"right side\", these will be the link points");
					this.@add("link length", 0f, "linking", "");
					this.addEnum("link type", head.WallSection.NORMAL, "linking", "Type of wall section");
					this.@add("build threshold", 0f, "construction", "Percentage built before navmesh is updated. Only affects walls.");
					this.@add("cavernous", false, "audio", "Interior ambient sound is big and echoing");
					this.@add("interior integrity", 100f, "audio", "Interior ambient sound modifier [0-100]");
					this.@add("is foliage", false, "type", "set true for anything spawned by the foliage system.  Important not to forget this, or it will affect performance and cause bugs");
					this.@add("is feature", false, "type", "feature buildings do not belong to any town");
					this.@add("creates player town", true, "type", "If true it will create a player town (if there isn't one there already) when built.");
					this.@add("storage size width", 18, "inventory", "size of the containers internal inventory");
					this.@add("storage size height", 18, "inventory", "size of the containers internal inventory");
					this.@add("stackable bonus mult", 1f, "inventory", "multiplies the stackable property of any item stored within");
					this.@add("has inventory", false, "inventory", "true if this building is a container with its own inventory.  Usually for furniture, eg crates, safes, barrels, racks, shelves");
					this.addEnum("itemtype limit", itemType.ITEM, "inventory", "limits storage to a specific TYPE of item.");
					this.@add("inventory content name", string.Empty, "inventory", "Sets the group name label in the inventory window. The label is visible only if it has limited inventory items. If this value is empty, it will use the name of the limited item only if there is only one.");
					this.addList("limit inventory", itemType.ITEM, 1, "limit inventory", "If you list an item here, it will limit the buildings inventory to only store that type of item.");
					this.@add("max slope", 45f, "type", "Maximum slope angle of this builing");
					this.@add("vertical position tolerance", 20f, "type", "If the building only has 1 part, then this number is the allowance for vertical adjustment when placing the building");
					this.@add("match slope", false, "type", "Forces the building to tilt to match 100% to the terrain slope");
					this.@add("ceiling placement", false, "type", "Means that this can only be placed on a ceiling.  Must be interior only.");
					this.@add("is exterior furniture", false, "type", "use for things that attach to buildings, like signs");
					this.@add("is interior furniture", false, "type", "use for things that can only go indoors (when player placement mode)");
					this.@add("is node", false, "node", "makes this an invisible node");
					this.@add("is gateway", false, "type", "use for gateway type structures.  Basically something that has floors >=1, BUT you want to be able to walk underneath it on the terrain.  So mainly just for gates, to stop the path mesh being cut off.");
					this.@add("is wall gate", false, "type", "use for actual gates. Definition: A wall linked building with a door in it.");
					this.@add("is sign", false, "type", "Use for highlighting shop signs");
					this.@add("is shelter", false, "type", "Is this an acid shelter. Only affects buildings with path mode IGNORE");
					this.@add("enforce ceiling", false, "type", "means that there is no roof to stand on, characters are forced always to be properly indoors (unless on floor 0)");
					this.addEnum("node type", NodeType.NODE_GENERAL, "node", "");
					this.@add("interior terrain", false, "type", "building interior includes terrain for generating navmesh");
					this.@add("door axis", 0, "doors", "0,1,2 for xyz axis that door will slide along");
					this.@add("door navmesh axis", 0, "doors", "0,1,2 for xyz axis that door mark along the navmesh.  Usually leave it at 0");
					this.@add("door move dist", 30f, "doors", "distance that the door slides");
					this.@add("door move speed", 1f, "doors", "speed that the door opens");
					this.@add("door comes out", 0f, "doors", "distance the door come forwards out of the frame before it slides");
					this.@add("always locks", false, "doors", "for town gates, closed state is always locked");
					this.addEnum("door type", head.DoorMaterial.Wood, "doors", "Affects the sounds the door makes");
					this.@add("lock level bonus", 0, "security", "Lockpick difficulty bonus. The final value is based on faction. This bonus will influence it per-building.");
					this.@add("has lock", false, "security", "sets if the building has a lock");
					this.@add("hardness", 30f, "security", "sets hardness of the lock when breaking open");
					this.addList("doors", itemType.BUILDING, 50, "doors", "a door is a building, containing a buildingpart that has the IS DOOR flag set to true.");
					this.@add("output rate", 1f, "tech", "For training dummies, its the max skill you can train to.  For production buildings, this is speed of production.  For guns, it is the amount of pierce damage, for walls it is the % bonus to mounted turret stats. ");
					this.@add("max operators", -1, "tech", "if > -1 it will override the number of max workers.  You need to have all operators working to achieve the official Output rate.");
					this.@add("desirability", 0, "tech", "Score that affects desirability of the building overall.  Important for public buildings and shops.");
					this.addEnum("path mode", head.PathMode.OBSTACLE, "type", "How collision interacts:\nIGNORE - characters can walk through it.\nPROJECTED - Same as obstacle but the collision is a 2d projection. Use for small obstacles.\nOBSTACLE - Have to walk around. Use for large items.\nWALKABLE - Characters can walk on top of this building");
					this.addFileName("destroyed boundary", "", string.Concat("PhysX xml object|*", str), "type", "Navmesh cutter for destroyed interior");
					this.@add("destroyed navmesh", false, "type", "Does the exterior navmesh need rebuilding when this building is destroyed.\nOnly needed for things like walls");
					this.addList("snaps to", itemType.BUILDING, 50, "snaps to", "makes this building aggressively snap to others.  Used for ramps and stairs.");
					this.addList("wall subsections", itemType.BUILDING, 10, "wall subsections", "Additional shorter parts of walls");
					this.addList("bird attractor", itemType.WILDLIFE_BIRDS, "1000,10", 99, "bird attractor", "This building attracts nearby birds.\nValue 1: Radius of attractor in meters.\nValue2: Maximum birds to attract");
					GameData.setDesc(t, "lights", itemType.LIGHT, new GameData.vec(), new GameData.quat(), "Light instances attached to this building");
					GameData.setDesc(t, "nodes", itemType.NULL_ITEM, new GameData.vec(), new GameData.quat(), "Positional nodes for attachments and AI");
					this.addList("functionality", itemType.BUILDING_FUNCTIONALITY, 1, "functionality", "");
					this.addList("gun turret", itemType.GUN_DATA, 1, "gun turret", "data to make this a turret");
					this.addList("farm data", itemType.FARM_DATA, 1, "", "Additional info for farm buildings");
					this.@add("power output", 0, "energy", "Amount of power output to the base, or power drain if negative.  Also represents output power of batteries.");
					this.@add("power capacity", 0, "energy", "Amount of power storage this adds to the base, measured in seconds.  Batteries.");
				}
				if (t == itemType.BUILDING_FUNCTIONALITY)
				{
					this.addList("weapon crafts", itemType.MATERIAL_SPECS_WEAPON, 99, "produces", "Items this building can craft");
					this.addList("armour crafts", itemType.ARMOUR, 99, "produces", "Items this building can craft");
					this.addList("item crafts", itemType.ITEM, "5", 99, "produces", "Items this building can craft, val1 is the max number to store");
					this.addList("robotics crafts", itemType.LIMB_REPLACEMENT, 99, "produces", "Items this building can craft");
					this.addList("crossbow crafts", itemType.CROSSBOW, 99, "produces", "Items this building can craft");
					this.addList("backpack crafts", itemType.CONTAINER, 99, "produces", "Items this building can craft");
					this.addList("produces", itemType.ITEM, "5", 1, "produces", "Item this building produces, (non-crafting), val1 is the max number to store");
					this.addList("consumes", itemType.ITEM, "5,100", 50, "consumes", "Items this building consumes, val1 is the max number to store, val2 is the rate- 100 is 1:1- 1 input makes 1 output, 200 means it takes 2 to make 1 output etc");
					this.addList("animation", itemType.ANIMATION, 1, "animation", "animation played when operated");
					this.addList("animation KO", itemType.ANIMATION, 1, "animation KO", "animation played when operated but KO (eg in a cage)");
					this.addList("animation dazed", itemType.ANIMATION, 1, "animation dazed", "animation played when operated and wounded");
					this.addList("animation medic", itemType.ANIMATION, 1, "animation dazed", "animation played when medicking");
					this.addList("animation lockpick", itemType.ANIMATION, 1, "animation dazed", "animation played when picking locks");
					this.addList("special tool", itemType.ITEM, 1, "", "stuff like pickaxes, just used for appearance sake");
					this.addEnum("function", BuildingFunction.BF_ANY, "function", "Does this building do anything?");
					this.addEnum("stat used", StatsEnumerated.STAT_NONE, "function", "The skill that is employed when using this building");
					this.@add("hunger rate", 1f, "function", "multiplies hunger rate when using this machine");
					this.@add("overrides ingredients", false, "function", "if true then the consumption items listed here override the INGREDIENTS of the OUTPUT item.  Use in rare cases you want to make an item out of the wrong ingredients eg biofuels");
					this.@add("occupant selection", false, "function", "Eg for beds and prisons, when we select this building it will select the occupant instead");
					this.@add("has progress bar when used", false, "function", "");
					this.@add("use range", 20f, "function", "Max distance character can be from building origin to use it.");
					this.@add("production mult", 1f, "function", "If this building produces or consumes anything, this is the overall speed multiplier.  For generators its the fuel consumption rate, so lower is better (0.01==half hour in-game)");
					this.@add("output per day", false, "function", "For production buildings, output rate will be used as items produced per day, if working at full capacity.  Otherwise its just a multiplier.");
					this.@add("max operators", 1, "function", "Max number of people who can operate it at the same time.  0 means its fully automated.");
					this.@add("restrains movement", false, "function", "if its a cage that ties up the occupant, preventing them from moving and healing etc");
					this.@add("flags", 0, "function", "Extra info that can mean different stuff depending on functionality type");
					this.@add("string", "", "function", "used for displaying info messages like with corpse cooking or burning");
					this.addEnum("world resource mining", head.MiningResource.NONE, "function", "water, farming, iron, stone, oil: related resource in the ground that affects production rate if it's a mine.");
					this.@add("tech level", 0, "tech", "Used for research benches to mark the current research level, or used by crafting benches to cap the maximum craftable item level");
					this.addList("animation events", itemType.ANIMATION_EVENT, "0", 12, "", "Audio events to trigger during the animation.\nValue is how far through the animation to play [0-100]");
				}
				if (t == itemType.FARM_DATA)
				{
					this.addEnum("layout", head.FarmLayout.GRID, "layout", "Plant layout type");
					this.@add("amount", 16, "layout", "Number of plants");
					this.@add("spacing", 1f, "layout", "Distance between plants");
					this.@add("jitter", 0f, "layout", "For grid layout, how messy are the lines. [0-1]");
					this.@add("aspect", 1f, "layout", "Shape of the farm, 1=square, 0=single line");
					this.@add("inside", false, "layout", "Plants do not use terrain height. Use for putting plants inside a structure");
					this.@add("growth time", 72f, "growth", "Game time in hours for a plant to fully grow in an optimal environment");
					this.@add("minimum fertility", 0f, "growth", "The minumum fertility the area can have to be viable");
					this.@add("fertility effect", 1f, "growth", "Amount fertility effects growth rate");
					this.@add("drought multiplier", 0f, "growth", "Multiplier for growth speed when there is no water [0-1]");
					this.@add("consumption rate", 5f, "growth", "Consumption rate of water per day");
					this.@add("arid", 0f, "farming", "multiplier that determines the crops class, ie how it grows in different biomes");
					this.@add("green", 1f, "farming", "multiplier that determines the crops class, ie how it grows in different biomes");
					this.@add("swamp", 0f, "farming", "multiplier that determines the crops class, ie how it grows in different biomes");
					this.@add("death time", 8f, "death", "Game time in hours for plants to completely die at the end if its life");
					this.@add("drought death time", 16f, "death", "Game time in hours for plants to completely die from dehydration.\n0 = never");
					this.@add("death threshold", 0.4f, "death", "How far into the death time before plants are unrecoverable [0-1]");
					this.addColor("death colour", 16777215, "death", "Dead plant colour multiplier");
					this.addList("plants", itemType.FARM_PART, "0,100", 99, "", "Plant meshes.\nValue1 is the group, one part will be chosen from each group, based on normalised percentage in value2.");
					this.@add("output per plant", 1f, "harvest", "Number of items harvested per plant");
					this.@add("harvest rate", 20f, "harvest", "Number of plants a person can harvest in a day");
					this.@add("harvest time", 10f, "harvest", "Hours before plants start dying when they are fully grown");
					this.@add("clear rate", 100f, "harvest", "Number of dead plants that can be cleared in a day");
				}
				if (t == itemType.FARM_PART)
				{
					this.addFileName("mesh", "", "Ogre mesh|*.mesh", "mesh", "Plant mesh file");
					this.addList("material", itemType.MATERIAL_SPEC, 1, "", "mesh material");
					this.@add("scale start", 0.1f, "growth", "Starting scale of plants");
					this.@add("scale end", 1f, "growth", "Scale of fully grown plants");
					this.@add("scale variance", 0f, "growth", "Random variance to plant scale");
					this.@add("offset start", 0f, "growth", "Vertical offset. This will genarally be the mesh height * starting scale");
					this.@add("offset end", 0f, "growth", "Final vertical offset. Generally zero for most cases");
					this.@add("delay", 0f, "growth", "How far through the plants growth cycle for this item to appear [0-1]");
				}
				if (t == itemType.GUN_DATA || t == itemType.CROSSBOW)
				{
					this.@add("range", 1000, "gun stuff", "max range of shots");
					this.@add("num shots", 1, "gun stuff", "");
					this.@add("reload time min", 1f, "gun stuff", "fastest time to reload in seconds (at max skill)");
					this.@add("reload time max", 5f, "gun stuff", "slowest time to reload in seconds (at min skill)");
					this.@add("accuracy deviation at 0 skill", 4f, "gun stuff", "accuracy rating, This is the random vector offset, in degrees, at worst skill level 0.  worst possible skill is 5 degrees, suggest 2-5 degrees, 0 degrees = perfect accuracy");
					this.@add("accuracy perfect skill", 70, "gun stuff", "skill level required to get 100% accuracy (deviation 0) with this weapon.");
					this.@add("aim speed", 1f, "gun stuff", "rotation speed, for aiming");
					this.@add("shot speed", 1f, "gun stuff", "speed that the projectile travels");
					this.@add("barrel pos Z", 5f, "positioning", "relative position of the end of the gun barrel");
					this.@add("barrel pos Y", 0f, "positioning", "relative position of the end of the gun barrel");
					this.@add("radius", 1f, "positioning", "for more than single-shot guns, this is the radius that the barrels are out from the center, will be used to position the harpoons");
					this.addFileName("live ammo", "", ".mesh|*.mesh", "ammo", "mesh file for the harpoon that shoots out and stabs things");
					this.addFileName("extra mesh loaded", "", ".mesh|*.mesh", "ammo", "additional mesh file for when the gun is loaded (eg crossbow string drawn back)");
					this.addEnum("shoot sound", head.GunSounds.HARPOON_TURRET, "gun stuff", "Sound the gun makes when firing");
					this.addFileName("extra mesh unloaded", "", ".mesh|*.mesh", "ammo", "additional mesh file for when the gun is unloaded (eg crossbow string slack)");
					this.addList("ammo", itemType.ITEM, 3, "", "the ammunition used.  if nothing here then magic infinite ammo");
				}
				if (t == itemType.CROSSBOW)
				{
					this.addFileName("mesh", "", "Ogre mesh|*.mesh", "files", "the weapon, no sheath");
					this.addFileName("sheath", "", "Ogre mesh|*.mesh", "files", "");
					this.@add("pierce damage min 0", 30f, "damage", "min damage at worst quality");
					this.@add("pierce damage max 0", 50f, "damage", "max damage at worst quality");
					this.@add("pierce damage min 1", 30f, "damage", "min damage at best quality");
					this.@add("pierce damage max 1", 50f, "damage", "max damage at best quality");
					this.@add("bleed mult", 1f, "damage", "(at worst quality) multiplier for the amount of bleeding the weapon type causes");
					this.@add("bleed mult 1", 1f, "damage", "(at best quality) multiplier for the amount of bleeding the weapon type causes");
					this.@add("animal damage mult", 1f, "race damage", "damage bonus to animals");
					this.@add("human damage mult", 1f, "race damage", "damage bonus to non-animals");
					this.@add("robot damage mult", 1f, "race damage", "damage bonus to robotic creatures");
					this.addList("race damage", itemType.RACE, "125", 99, "", "bonus damage multiplier, in %, when attacking this race");
					this.addList("material", itemType.MATERIAL_SPECS_WEAPON, 1, "", "");
					this.addList("reload anim", itemType.ANIMATION, 1, "", "the animation played for reloading");
					this.@add("range 1", 1000, "gun stuff", "(at best quality) max range of shots");
					this.@add("reload time min 1", 1f, "gun stuff", "fastest time to reload in seconds (at max skill) (at best quality)");
					this.@add("reload time max 1", 5f, "gun stuff", "slowest time to reload in seconds (at min skill) (at best quality)");
					this.@add("accuracy deviation at 0 skill 1", 4f, "gun stuff", "(at best quality) accuracy rating, This is the random vector offset, in degrees, at worst skill level 0.  worst possible skill is 5 degrees, suggest 2-5 degrees, 0 degrees = perfect accuracy");
					this.@add("shot speed 1", 1f, "gun stuff", "(at best quality) speed that the projectile travels");
					this.@add("auto icon image", true, "inventory", "true to automatically generate the inventory icon");
					this.addEnum("slot", head.AttachSlot.ATTACH_WEAPON, "inventory", "grants special permission to go in certain inventory slots");
					this.@add("craft time hrs", 20f, "craft", "time to craft in game hours");
					this.addList("ingredients", itemType.ITEM, "100", 5, "ingredients", "Used in crafting to show what this item is made of. Val1 is the amount, 100 = a 1:1 relationship, 50 = half of this ingredient is used up to make 1 of this. Also used to value an item.");
					this.@add("equip offset", 1.2f, "", "when its equipped on the characters back, this can offset the position to make it look right, higher number moves it further away");
					this.@add("weight", 1f, "", "");
					this.@add("value 1", 1000, "inventory", "price at best quality level");
					this.@add("scale length", 1f, "scale", "");
					this.@add("scale width", 1f, "scale", "");
					this.@add("scale thickness", 1f, "scale", "");
					this.@add("overall scale", 1f, "scale", "");
					this.@add("icon offset H", 0f, "inventory icon", "align the image horizontally");
					this.@add("icon offset V", 0f, "inventory icon", "align the image vertically");
					this.@add("icon zoom", 1f, "inventory icon", "scale the image (try to keep at 1.0)");
				}
				if (t == itemType.BUILDING_PART)
				{
					this.@add("density", 1f, "physics", "Relative physical mass, not used for static buildings.");
					this.@add("is unwalkable roof", false, "", "true for roof parts that cannot be walked on, will ");
					this.addFileName("phs or mesh", "", "Scythe physics object|*.phs;*.mesh", "files", "Scythe .phs file, used to represent the building.  Can also use a mesh.");
					this.addFileName("xml collision", "", string.Concat("PhysX xml object|*", str), "files", "An optional, static physX collision file.");
					this.addFileName("destroyed mesh", "", "Meshes|*.mesh", "files", "Mesh to use for destroyed versions of this building.\nGround floor will use normal mesh if blank.\nUpper floors will be removed if blank.");
					this.addFileName("destroyed collision", "", string.Concat("PhysX xml object|*", str), "files", "Destroyed version of the collision. Ignored if no destroyed mesh is set.");
					this.@add("offset X", 0f, "offsetting", "Offset the position of this model.  Useful if this part wants to rotate around its origin, or if it is a position marker node.");
					this.@add("offset Y", 0f, "offsetting", "Offset the position of this model.  Useful if this part wants to rotate around its origin, or if it is a position marker node.");
					this.@add("offset Z", 0f, "offsetting", "Offset the position of this model.  Useful if this part wants to rotate around its origin, or if it is a position marker node.");
					this.@add("is for position marker", false, "offsetting", "If set, model will not be offset, [offset XYZ] will be for marking a spot that characters need to stand at to use the building.");
					this.@add("rotation speed min", 0f, "offsetting", "speed is in degrees per second");
					this.@add("rotation speed max", 0f, "offsetting", "speed is in degrees per second");
					this.addEnum("rotation axis", head.Axis.Z_AXIS, "offsetting", "");
					this.addEnum("rotation function", head.BuildingRotation.ROTATION_NONE, "offsetting", "");
					this.@add("wind speed rotation min", 0f, "weather", "Minimum wind speed for minimum rotation speed. Wind speed lower than this value would not rotate.");
					this.@add("wind speed rotation max", 100f, "weather", "Maximum wind speed for maximum rotation speed. Wind speed higher than this value would rotate at maximum speed");
					this.@add("wind speed rotation danger", 200f, "weather", "Danger wind speed. When the wind speed reaches this value the building would get damaged over time.");
					this.@add("material match chance", 100, "", "If you have chosen a 'material match' in the lists, this is the percentage chance of it being used, So normally 100, but you may want occasional variants.");
					this.@add("material match base chance", 0, "", "Percentage chance of using same as base BUILDING material.  Will override all else");
					this.@add("building floor", 0, "", "floor of the building. 0 for base (never invisible).  1= roof or 1st floor, 2 = next floor etc");
					this.addList("material", itemType.MATERIAL_SPEC, 1, "material", "material.  If you pick nothing it will use the material collection of the base BUILDING");
					this.addList("material match", itemType.BUILDING_PART, 9, "match", "specify if you want the material to match the material of another part.  If you pick more than one the others will be used if the first part does not exist.  If you pick nothing and pick no material it will use a random material from the base BUILDING's list");
					this.addList("z incompatible parts", itemType.BUILDING_PART, 50, "parts", "If any parts in this list are used, then cannot use this part.");
					this.addList("parts", itemType.BUILDING_PART, "0,100", 50, "parts", "all the visual entities that make up the building.  Value1 is the group, one part will be chosen from each group, based on normalised percentage in value 2.  Group 0 is special, everything from there will be chosen based on val2 as an absolute percetage.");
					this.@add("is door", false, "doors", "use for doors.");
					this.@add("passable", false, "", "nothing will collide with us, the collision hull will only be used for clicking.");
					this.@add("is stairs", false, "", "true if the part is stairs");
					this.@add("affects footprint", false, "footprint", "affects where we can or cant place the building");
					this.@add("above ground", false, "footprint", "sets if this footprint needs to be above or below ground for the building placement to be valid");
					this.@add("footprint vertical", 0f, "footprint", "sets the vertical position relative to the origin");
					this.addEnum("ground type", head.BuildingPlacementGroundType.ANY, "footprint", "Sets where the footprint can be placed.");
					GameData.setDesc(t, "effects", itemType.EFFECT, new GameData.vec(), new GameData.quat(), "Effects attached to this building");
					GameData.setDesc(t, "lights", itemType.LIGHT, new GameData.vec(), new GameData.quat(), "Light instances attached to this building");
					GameData.setDesc(t, "nodes", itemType.NULL_ITEM, new GameData.vec(), new GameData.quat(), "Positional nodes for attachments and AI");
				}
				if (t == itemType.MATERIAL_SPECS_CLOTHING || t == itemType.MATERIAL_SPEC || t == itemType.MATERIAL_SPECS_WEAPON)
				{
					this.addFileName("texture map", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "material", "Diffuse texture. Gloss is encoded in the alpha channel");
					this.addFileName("normal map", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "material", "Normal map texture.");
					this.addFileName("property map", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "material", "Additional texture data:\nRed channel: Alpha, Green channel: Metalness, Blue channel: Glow");
				}
				if (t == itemType.MATERIAL_SPECS_CLOTHING)
				{
					this.@add("paint factor 1", 0f, "coloring", "how the colorisation mask affects the diffuse.  1.0 means that the colored mask REPLACES the diffuse (eg for paint), 0.0 means that it multiplies, keeping the base luminosity from the diffuse (eg for dyed cloth).");
					this.@add("paint factor 2", 0f, "coloring", "how the colorisation mask affects the diffuse.  1.0 means that the colored mask REPLACES the diffuse (eg for paint), 0.0 means that it multiplies, keeping the base luminosity from the diffuse (eg for dyed cloth).");
					this.addFileName("property map", "", "Nvidia dds|*.dds", "material", "Additional texture properties:\nRed channel is the metalness map.\nGreen channel applies colour 1.\nBlue channel applies colour 2.\n\nAlpha channel is transparency");
					this.addEnum("material type", head.ItemShader.DEFAULT, "material", "Shader to use");
				}
				if (t == itemType.MATERIAL_SPEC)
				{
					this.@add("tile X", 1f, "material", "optional tiling of textures");
					this.@add("tile Y", 1f, "material", "optional tiling of textures");
					this.addEnum("material type", head.BuildingShader.DEFAULT, "material", "Defines how the building is textured:\nDefault - default shader\nAlpha - uses normal map alpha for transparency\nFoliage - Uses double sided foliage shader with alpha\nDual - Blends between two texture sets using vertex alpha data\nEmissive - Normal map alpha channel becomes emissive glow amount");
					this.addFileName("texture map 2", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "material", "Optional second texture. Blend between them with vertex alpha");
					this.addFileName("normal map 2", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "material", "Optional second normal map. Blend between them with vertex alpha");
					this.addFileName("property map 2", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "material", "Optional second property map. Blend between them with vertex alpha");
					this.@add("scaffolding tex scale", 4f, "construction", "tile the scaffolding material used when under construction");
					this.addList("material", itemType.MATERIAL_SPEC, "100", 99, "material", "If anything is listed here, the material becomes a collection: any current material settings are ignored and it will randomly choose a material from this list.  val1 is probability, 0 will be set to 100");
				}
				if (t == itemType.MATERIAL_SPECS_COLLECTION)
				{
					this.addList("material", itemType.MATERIAL_SPEC, "100", 99, "material", "One or more materials to choose from, val1 is probability, 0 will be set to 100");
				}
				if (t == itemType.REPEATABLE_BUILDING_PART_SLOT)
				{
					this.addList("z incompatible parts", itemType.BUILDING_PART, 50, "parts", "If any parts in this list are used, then cannot use this part.");
					this.addList("parts", itemType.BUILDING_PART, "0,100", 50, "parts", "All parts to be used for the random slots.  val2 is chance, val1 is group");
				}
				if (t == itemType.PERSONALITY)
				{
					this.addEnumLooping("tags always0", PersonalityTags.PT_NONE, "tags always", "NPCs are very likely to have this tag.  ");
					this.addEnumLooping("tags common0", PersonalityTags.PT_NONE, "tags common", "NPCs are likely to have this tag");
					this.addEnumLooping("tags rare0", PersonalityTags.PT_NONE, "tags rare", "NPCs rarely have this tag");
					this.addEnumLooping("tags never0", PersonalityTags.PT_NONE, "tags never", "these tags will never be used");
				}
				if (t == itemType.STATS)
				{
					this.@add("strength", 1f, "attributes", "Character strength [0-100]");
					this.@add("dexterity", 1f, "attributes", "Dexterity [0-100]");
					this.@add("toughness2", 1f, "attributes", "Toughness [0-100]");
					this.@add("attack", 1f, "combat skills", "Melee attack skill [0-100]");
					this.@add("defence", 1f, "combat skills", "Melee defence skill [0-100]");
					this.@add("dodge", 1f, "combat skills", "Dodging skill [0-100]");
					this.@add("unarmed", 1f, "combat skills", "Martial art skill [0-100]");
					this.@add("athletics", 1f, "movement", "Athletics skill [0-100]");
					this.@add("swimming", 1f, "movement", "Swimming skill [0-100]");
					this.@add("medic", 1f, "science skills", "Field medic skill [0-100]");
					this.@add("engineer", 1f, "science skills", "Engineering skill [0-100]");
					this.@add("robotics", 1f, "science skills", "Robotics skill [0-100]");
					this.@add("science", 1f, "science skills", "Science skill [0-100]");
					this.@add("weapon smith", 1f, "trade skills", "Weapon smithing skill [0-100]");
					this.@add("armour smith", 1f, "trade skills", "Armour smithing skill [0-100]");
					this.@add("bow smith", 1f, "trade skills", "Crossbow smithing skill [0-100]");
					this.@add("labouring", 1f, "trade skills", "Labouring skill [0-100]");
					this.@add("farming", 1f, "trade skills", "Farming skill [0-100]");
					this.@add("cooking", 1f, "trade skills", "Cooking skill [0-100]");
					this.@add("stealth", 1f, "stealth skills", "Sneaking skill [0-100]");
					this.@add("thievery", 1f, "stealth skills", "Stealing skill [0-100]");
					this.@add("assassin", 1f, "stealth skills", "Assasination skill [0-100]");
					this.@add("lockpicking", 1f, "stealth skills", "Lock picking skill [0-100]");
					this.@add("turrets", 1f, "weapon skills", "Skill operating turrets [0-100]");
					this.@add("katana", 1f, "weapon skills", "Skill wielding katanas [0-100]");
					this.@add("sabres", 1f, "weapon skills", "Skill wielding sabres [0-100]");
					this.@add("hackers", 1f, "weapon skills", "Skill wielding hackers [0-100]");
					this.@add("heavy weapons", 1f, "weapon skills", "Skill wielding heavy weapons [0-100]");
					this.@add("blunt", 1f, "weapon skills", "Skill wielding blunt weapons [0-100]");
					this.@add("poles", 1f, "weapon skills", "Skill wielding polearms [0-100]");
					this.@add("bow", 1f, "weapon skills", "Crossbow skill [0-100]");
					this.@add("ff", 1f, "weapon skills", "Precision shooting skill [0-100]");
				}
				if (t == itemType.CHARACTER || t == itemType.ANIMAL_CHARACTER)
				{
					this.addList("imprisonment triggers", itemType.FACTION_CAMPAIGN, 99, "", "faction assault that is triggered if this character is captured");
					this.addList("death triggers", itemType.FACTION_CAMPAIGN, 99, "", "faction assault that is triggered if this character dies");
					this.addList("death items", itemType.ITEM, "1", 20, "Inventory", "items that spawn in NPCs inventory on death (eg trophies)");
					this.addList("faction", itemType.FACTION, 1, "Owning Faction", "faction that this character belongs to");
					this.addList("stats", itemType.STATS, 1, "Stats", "the skills and stats of this character");
					this.addList("personality", itemType.PERSONALITY, 1, "Personality", "the personality of this character");
					this.addList("race", itemType.RACE, "100", 1, "Race", "Specify race, will override race in .body file.");
					this.@add("unique", false, "type", "This character will only ever spawn once");
					this.@add("bounty chance", 0, "bounty", "0-100 chance of having a bounty");
					this.@add("bounty amount", 0, "bounty", "");
					this.@add("bounty amount fuzz", 0, "bounty", "+/- amount to randomise the bounty ");
					this.addList("bounty factions", itemType.FACTION, 1, "", "if the character has a bounty, this lists the factions it is wanted by.  Defaults to the empire");
					this.@add("assigns bounties", false, "bounty", "true if this guy can put a bounty on someone he sees committing a crime");
					this.@add("female chance", 0, "type", "0-100 percentage chance of this character being female");
					this.@add("stats randomise", 3, "levels", "Randomises characters stats by +/- this amount");
					this.@add("combat stats", 0, "levels", "If no stats data assigned, this will set the combat stats");
					this.@add("unarmed stats", 0, "levels", "If no stats data assigned, this will set the unarmed+dodge stats");
					this.@add("stealth stats", 0, "levels", "If no stats data assigned, this will set the sneak stats");
					this.@add("strength", 0, "levels", "If no stats data assigned, this will set strength");
					this.@add("ranged stats", 0, "levels", "If no stats data assigned, this will set ranged weapon skills");
					this.@add("faction importance", 1f, "", "Multiplier for the effect on faction relations any actions relating to this character.  eg set low for a peasant, high for a noble or diplomat.");
					this.addList("AI Goals", itemType.AI_TASK, "0,100", 999, "AI Goals", "Various goals of the AI, and their weight (priority) (val2, as a percentage 0-100+) usually keep to default 100");
					this.addList("schedule", itemType.DAY_SCHEDULE, 999, "schedule", "Events we do during the day.  val1 is the hour of the day 0-23");
					this.addList("starting health", itemType.LOCATIONAL_DAMAGE, "100", 20, "starting health", "Used mainly for your starting squads, val1 sets the starting health of a bodypart from -100 to +100.  < -100 means amputated");
					this.addList("animation files", itemType.ANIMATION_FILE, 99, "animation", "Files containing additional or override animations for this character");
					if (t == itemType.CHARACTER)
					{
						this.addList("color", itemType.COLOR_DATA, 1, "", "overrides the factions uniform color scheme (but not the item color scheme if there is one)");
						this.addList("unique replacement spawn", itemType.CHARACTER, 1, "", "if this is a unique character that already exists or has died and we try to spawn, it will spawn this guy instead, otherwise it won't spawn anyone.");
						this.addList("dialogue", itemType.DIALOGUE, 1, "Dialogue", "Extra PLAYER_TALK_TO_ME events added onto the dialog package");
						this.addList("dialogue package", itemType.DIALOGUE_PACKAGE, 99, "Dialogue", "What the character says, in different situations");
						this.addList("dialogue package player", itemType.DIALOGUE_PACKAGE, 99, "Dialogue", "Dialog package for when the NPCs is a player character");
						this.addList("announcement dialogue", itemType.DIALOGUE, 99, "announcement dialogue", "Makes the character announce on his own");
						this.addList("inventory", itemType.ITEM, "1", 20, "Inventory", "");
						this.addList("blueprints", itemType.RESEARCH, "1", 20, "blueprints", "blueprints in inventory");
						this.addList("backpack", itemType.CONTAINER, "1, 100", 10, "Backpack", "You can have more than one, in the case of a backpack salesman.  val1 is absolute chance");
						this.addList("crossbows", itemType.CROSSBOW, "1, 10", 10, "Weapons", "val0 is quantity, val1 is chance (chances aren't relative, so total < 100 means chance of nothing)");
						this.addList("weapons", itemType.WEAPON, "1, 0, 100", 10, "Weapons", "val0 is quantity, val1 is slot (0=hip, 1=back), val2 is absolute chance (<100 total means chance of nothing)");
						this.addList("weapon level", itemType.WEAPON_MANUFACTURER, "0", 10, "Weapon level", "Determines level of weapon we have, val0 is relative chance");
						this.addList("clothing", itemType.ARMOUR, "1,100", 10, "Clothing", "val1 is quantity, val2 is percentage chance of having it (normalised with other items of same type).  You can add duplicate gear (eg 4 helmets) and it will only choose one.  If val1 is < 0 then that will count as a possible [no item] for that clothing type (eg if you want a chance of having no hat at all)");
						this.addList("robot limbs", itemType.LIMB_REPLACEMENT, "50,100", 20, "Inventory", "val1 is quality, val2 is relative chance if multiple items are listed for the same limb.\nIf val1 is negative it will count as keeping the original limb");
						this.addList("vendors", itemType.VENDOR_LIST, "100", 10, "Vendors", "inventory list if this character is a shopkeeper, val0 is chance multiplier");
						this.addFileName("body", "", "body files|*.bod2", "", "The body shape of this character.  Leave blank for random body.");
						this.addFileName("mesh", "", "Ogre Mesh file|*.mesh", "", "The mesh of this character.  Leave blank for random mesh.");
						this.@add("named", false, "type", "Has a name randomly chosen from the name list.");
						this.@add("shaved", false, "type", "has head shaved, like a slave.  Sheks will have their horns cut.");
						this.addEnum("slave", SlaveStateEnum.NOT_SLAVE, "type", "sets initial slave state.");
						this.@add("money min", 0, "money", "Amount of money I have");
						this.@add("money max", 200, "money", "Amount of money I have");
						this.@add("wages", 0, "money", "Daily wages I get");
						this.@add("dumb", false, "", "Special case for characters that can't talk normally.  Prevents them becoming interjectors or talking to NPCs");
						this.@add("wears uniform", false, "type", "If true, the npcs clothing is branded as a faction uniform.  This reduces re-sale value and also means wearing it has implications.");
						this.@add("armour upgrade chance", 5, "levels", "0-100% chance that armour will be 1 level higher than usual");
						this.addEnum("armour grade", ArmourRarity.GEAR_CHEAP, "levels", "Sets the quality level of this characters armour");
						this.addList("shopping", itemType.ITEM, "1,100,0", 999, "shopping", "if this npc goes shopping, this is what they buy. val0 is the quantity to buy, val1 is the chance of adding this to the shopping list (0-100).  val2 is the priority, so 0 first, then 1, then 2 etc.  You are not listing specific items, you are listing item TYPES by function, so add bread, and they will shop for food in general.  Make sure they have enough money and inv space.");
						this.@add("is trader", false, "tags", "true if this char is a trader.  Must have either a large backpack on or a shop.  And a vendor item list.");
						this.addEnum("NPC class", CharacterTypeEnum.OT_MILITARY, "tags", "classifies this NPC type, so the AI knows who's who");
						this.@add("money item min", 0, "money", "Minimum amount of physical money item.");
						this.@add("money item max", 0, "money", "Maximum amount of physical money item.");
						this.@add("money item prob", 0f, "money", "Probability of having physical money item. [0-1]");
					}
					if (t == itemType.ANIMAL_CHARACTER)
					{
						this.addList("animations", itemType.ANIMAL_ANIMATION, 99, "animations", "animations");
						this.addList("turning anim", itemType.ANIMAL_ANIMATION, "100", 1, "animations", "animation used when turning on the spot, val is speed multiplier %");
						this.addList("strafe anim", itemType.ANIMAL_ANIMATION, "100", 1, "animations", "animation used when strafing, val is speed multiplier %");
						this.addList("backpack", itemType.CONTAINER, 1, "", "the creatures backpack");
						this.addList("inventory", itemType.VENDOR_LIST, "100", 99, "", "the creatures inventory contents if he has a backpack, val0 is chance");
						this.addList("weapon", itemType.WEAPON, 1, "", "the creatures built-in weapon");
						this.addList("weapon level", itemType.WEAPON_MANUFACTURER, 1, "", "the creatures built-in weapon");
						this.@add("inventory w", 4, "", "size of inventory, make sure it matches to fit its death items");
						this.@add("inventory h", 8, "", "size of inventory, make sure it matches to fit its death items");
						this.@add("scale min", 0.3f, "life", "size of a newborn");
						this.@add("scale max", 1f, "life", "size of an old one");
						this.@add("lifespan", 20f, "life", "number of days it takes for the animal to become fully grown");
						this.@add("HP mult", 1f, "life", "modifies HP amounts, for making special boss monsters");
						this.@add("turn rate", 1f, "movement", "Max speed it can turn around");
						this.@add("animal strength", 20, "stats", "Animals have a hard set strength level");
						this.@add("smell blood", 0f, "stats", "sets the smell threshold for hunting behavior to trigger.  number is essentially the number of corpses.");
						this.@add("smell eggs", 0f, "stats", "sets the smell threshold for hunting behavior to trigger, number is essentially the number of eggs in the zone.");
						this.addEnum("attack type", WeaponCategory.ATTACK_GIRAFFE, "combat", "used to choose correct combat animations");
						this.addList("bird attractor", itemType.WILDLIFE_BIRDS, "1000,10", 99, "bird attractor", "This animal attracts nearby birds.\nValue 1: Radius in meters.\nValue2: Maximum birds to attract");
					}
				}
				if (t == itemType.RACE)
				{
					this.addFileName("male mesh", "", "Ogre meshes|*.mesh", "mesh", "male character mesh for this race");
					this.addFileName("female mesh", "", "Ogre meshes|*.mesh", "mesh", "female character mesh for this race");
					this.addFileName("female ragdoll", "", "Scythe physics|*.phs", "ragdoll", "female character ragdoll file for this race");
					this.addFileName("male ragdoll", "", "Scythe physics|*.phs", "ragdoll", "male character ragdoll file for this race");
					this.addFileName("male carry ragdoll", "", "Scythe physics|*.phs", "ragdoll", "");
					this.addFileName("female carry ragdoll", "", "Scythe physics|*.phs", "ragdoll", "");
					this.addFileName("attachment points", "", "Scythe physics|*.phs", "", "Additional attachment point information");
					this.@add("carry bone", "", "ragdoll", "Name of the bone used to be carried by characters. (Animals only)");
					this.@add("carriable", true, "ragdoll", "Sets if the race can be picked up and carried.");
					this.@add("filenames prefix", "", "", "prefix used on filenames relating to this race, used to identify art files to use");
					this.@add("playable", true, "", "Sets whether the race is playable by the player.\nFor it to be completely playable it must have a editor limits file and be on any race group.");
					this.@add("no shirts", false, "", "race has no inventory shirt slot");
					this.@add("gigantic", false, "", "prevents the entity from going invisible at long range");
					this.@add("no hats", false, "", "race has no inventory hat slot");
					this.@add("no shoes", false, "", "race has no inventory boots slot");
					this.@add("swims", true, "", "if false then this race walks on the bottom of the lake rather than swimming on the surface");
					this.@add("single gender", false, "", "if no male/female version");
					this.@add("cant enter buildings", false, "", "true for large animals that cant fit through doors, prevents pathfinding performance problems");
					this.@add("buy value", 100, "", "percentage of normal value when bought or sold as a slave");
					this.addList("natural armour", itemType.ARMOUR, 1, "", "a natural armour that is added on top of anything they are wearing");
					this.addEnumLooping("stats good0", StatsEnumerated.STAT_NONE, "stats", "any stats this race is better at");
					this.addEnumLooping("stats bad0", StatsEnumerated.STAT_NONE, "stats", "any stats this race is weaker in");
					this.addEnum("heal stat", StatsEnumerated.STAT_MEDIC, "stats", "the first aid skill that is used when healing this race");
					this.@add("is robot", false, "stats", "If true then attributes (str, dex etc) are fixed and can only be upgraded with parts");
					this.@add("vampiric", false, "stats", "If true then when eating a character will suck nutrition instead of eating limbs");
					this.@add("heal rate", 1f, "stats", "multiplier for healing speed");
					this.@add("bleed rate", 1f, "stats", "multiplier for bleeding speed.  0.0 means they are a bloodless race");
					this.@add("strength", 1f, "stats", "multipliers for this attribute");
					this.@add("dexterity", 1f, "stats", "multipliers for this attribute");
					this.@add("hunger rate", 1f, "stats", "rate that this race gets hungry");
					this.@add("vision range mult", 1f, "stats", "multiplier for how far they can see (ie to detect other characters)");
					this.addText("description", "", "", "general description of this race to show in the character editor");
					this.@add("extra attack slots", 0, "stats", "adds to the global [max num attack slots] for this race.  use for big things.");
					this.addEnumLooping("weather immunity0", WeatherAffecting.WA_NONE, "", "any particular weather types this race is immune to");
					this.addFileName("editor limits", "", "XML|*.xml", "editor", "min/max proportion limits in the character editor");
					this.@add("swim speed mult", 1f, "movement", "the swim speed multiplier");
					this.@add("swim offset", 1f, "movement", "num units above/below the water, relative to head position");
					this.@add("speed min skill", 70, "movement", "the max run speed at lowest athletics skill");
					this.@add("speed max skill", 120, "movement", "the max run speed at highest athletics skill");
					this.@add("hull size X", 5f, "movement", "size of the click hull");
					this.@add("hull size Y", 20f, "movement", "size of the click hull");
					this.@add("hull size Z", 5f, "movement", "size of the click hull");
					this.@add("combat move speed mult", 1f, "movement", "affects combat speed, 1.0 is standard for human");
					this.@add("pathfind footprint radius", 2.5f, "movement", "radius of the entity in pathfinding terms (normally half of hull size X)");
					this.@add("pathfind acceleration", 15f, "movement", "acceleration of the entity.  Should be lower for bigger creatures.");
					this.@add("walk speed", 15f, "movement", "Speed when walking.");
					this.@add("water avoidance", 0f, "movement", "How far characters will go to avoid water. Negative values will try to stay in the water.");
					this.@add("min blood", 75f, "stats", "Amount of blood, at lowest strength/age");
					this.@add("max blood", 150f, "stats", "Amount of blood, at highest strength/age");
					this.@add("self healing", false, "stats", "If true, then this race will slowly heal without bandaging");
					this.addColor("blood colour", 8388608, "blood", "Blood colour");
					this.@add("blood vertical", 0.15f, "blood", "Vertical stretching for character blood overlay");
					this.@add("blood horizontal", 1, "blood", "Horizontal tiling for character blood overlay. (integer 1+)");
					this.addFileName("body texture male", "", "dds textures|*.dds", "skin", "Diffuse map. Alpha channel is gloss");
					this.addFileName("body texture female", "", "dds textures|*.dds", "skin", "Diffuse map. Alpha channel is gloss");
					this.addFileName("property map male", "", "dds textures|*.dds", "skin", "Properties map.\nRed channel: Skin tone mask\nGreen channel: Metalness\nBlue channel: Glow");
					this.addFileName("property map female", "", "dds textures|*.dds", "skin", "Properties map.\nRed channel: Skin tone mask\nGreen channel: Metalness\nBlue channel: Glow");
					this.addFileName("nm male", "", "dds textures|*.dds", "skin", "Normal map");
					this.addFileName("nm female", "", "dds textures|*.dds", "skin", "Normal map");
					this.addFileName("nm male strong", "", "dds textures|*.dds", "skin", "Muscular variant of normal map");
					this.addFileName("nm female strong", "", "dds textures|*.dds", "skin", "Muscular variant of normal map");
					this.addFileName("nm male skinny", "", "dds textures|*.dds", "skin", "Starving variant of normal map");
					this.addFileName("nm female skinny", "", "dds textures|*.dds", "skin", "Starving variant of normal map");
					this.addFileName("nm male aged", "", "Nvidia dds|*.dds", "skin", "Normal map for aged characters");
					this.addFileName("nm female aged", "", "Nvidia dds|*.dds", "skin", "Normal map for aged characters");
					this.addFileName("flayed texture male", "", "Nvidia dds|*.dds", "skin", "");
					this.addFileName("flayed texture female", "", "Nvidia dds|*.dds", "skin", "");
					this.addFileName("flayed normal male", "", "Nvidia dds|*.dds", "skin", "");
					this.addFileName("flayed normal female", "", "Nvidia dds|*.dds", "skin", "");
					this.addList("combat anatomy", itemType.LOCATIONAL_DAMAGE, "100, 100", 25, "Combat Anatomy (health)", "list bodypart locations that can be hit on a character, and chance of hitting each part (val 0), and total health (val 1)");
					this.addList("AI Goals", itemType.AI_TASK, "4,100", 999, "AI Goals", "Various goals of the AI, and their priority (val2, as a percentage 0-100+) (matches the enum [taskPriority], generally you want 3-5, maybe 6) (8=EMOTIONAL, 7=PLAYER_ORDER, 6=CONSTANT, 3-5=CHOOSABLE_GOALS)");
					this.addList("hairs", itemType.ATTACHMENT, "100", 999, "hairs", "hairstyles available for this race, val1 is % of how commonly used.");
					this.addList("hair colors", itemType.COLOR_DATA, "100", 999, "hair colors", "hair colors available for this race, val1 is % of how commonly used.");
					this.addList("personality", itemType.PERSONALITY, 1, "Personality", "the personality of the characters");
					this.addList("fists lv1", itemType.WEAPON, 1, "Personality", "for humans, the weapon to use as fists at level 1");
					this.addList("fists lv100", itemType.WEAPON, 1, "Personality", "for humans, the weapon to use as fists at max level");
					this.addList("special food", itemType.ITEM, 1, "Personality", "allows us to eat ITEM_FOOD_RESTRICTED items");
					this.addFileName("part map male", "", "dds textures|*.dds", "", "Texture specifying body parts to hide when equipping certain armours. One channel per part.");
					this.addFileName("part map female", "", "dds textures|*.dds", "", "Texture specifying body parts to hide when equipping certain armours. One channel per part.");
					this.addList("heads male", itemType.HEAD, "100", 999, "head", "head textures for males");
					this.addList("heads female", itemType.HEAD, "100", 999, "head", "head textures for females");
					this.@add("portrait offset x", 0f, "portrait", "");
					this.@add("portrait offset y", 0.5f, "portrait", "");
					this.@add("portrait offset z", 0f, "portrait", "");
					this.@add("portrait distance", 10f, "portrait", "");
					this.@add("morph num", 5, "", "Number of different morph faces per race and gender. All of them are recreated on new game start");
					this.addEnum("sounds", head.RaceSound.HUMAN, "audio", "Sound effect group.");
					this.@add("barefoot", false, "audio", "Does this race use the barefoot footstep sound when not wearing shoes");
					this.addList("ambient sound", itemType.AMBIENT_SOUND, 1, "audio", "Emitter for a constant looped sound while character is alive");
					this.addList("limb replacement", itemType.LIMB_REPLACEMENT, 4, "", "Limb stump models for missing limbs with no prosthetic attached");
					this.addList("severed limbs", itemType.ITEM, "0", 4, "", "Severed limb items that are spawned when dismembered.\n values: 0:left arm, 1:right arm, 2:left leg, 3:right leg");
					this.addList("animation files", itemType.ANIMATION_FILE, 99, "animation", "Files containing additional or override animations for this race");
					GameData.addCondition(t, "female mesh", "single gender", false, true);
					GameData.addCondition(t, "female ragdoll", "single gender", false, true);
					GameData.addCondition(t, "female carry ragdoll", "single gender", false, true);
					GameData.addCondition(t, "part map female", "single gender", false, true);
					GameData.addCondition(t, "body texture female", "single gender", false, true);
					GameData.addCondition(t, "property map female", "single gender", false, true);
					GameData.addCondition(t, "nm female", "single gender", false, true);
					GameData.addCondition(t, "nm female strong", "single gender", false, true);
					GameData.addCondition(t, "nm female skinny", "single gender", false, true);
					GameData.addCondition(t, "nm female aged", "single gender", false, true);
					GameData.addCondition(t, "flayed texture female", "single gender", false, true);
					GameData.addCondition(t, "flayed normal female", "single gender", false, true);
				}
				if (t == itemType.ANIMATION_FILE)
				{
					this.addFileName("male animation", "", "Skeleton files|*.skeleton", "animation", "Skeleton file containing additional or overridden animations");
					this.@add("preprocess", true, "animation", "Preprocess with animation track exclusion fron ANIMATION data (needed for humans)");
				}
				if (t == itemType.RACE_GROUP)
				{
					this.addText("description", "", "General", "Description of the race group");
					this.addList("races", itemType.RACE, 20, "races", "List of races that belong to this race group.");
				}
				if (t == itemType.HEAD)
				{
					this.@add("playable", true, "general", "Sets if it's usable by the player");
					this.addFileName("texture map", "", "Nvidia dds|*.dds", "visual", "Diffuse texture for the head.");
					this.addFileName("normal map", "", "Nvidia dds|*.dds", "visual", "Optional normal map.");
					this.addFileName("property map", "", "Nvidia dds|*.dds", "visual", "Additional properties.\nRed channel: Skin tone mask\nGreen channel: Metalness\nBlue channel: Glow.");
					this.addFileName("aged normal map", "", "Nvidia dds|*.dds", "visual", "Optional normal map for old characters");
				}
				if (t == itemType.ITEMS_CULTURE)
				{
					this.addList("trade prices", itemType.ITEM, "150", 99, "Trade Prices", "Multiplier for trade prices default 100% (val1)");
					this.addList("illegal goods", itemType.ITEM, 99, "Illegal Goods", "Contraband");
					this.addList("forbidden items", itemType.ITEM, 100, "forbidden items", "Unspawnable items, things you won't find");
					this.addList("illegal buildings", itemType.BUILDING, 100, "forbidden items", "buildings that are considered illegal and will trigger the dialog event");
					this.addList("happy buildings", itemType.BUILDING, 100, "forbidden items", "buildings that are considered great and will make the faction happy");
				}
				if (t == itemType.FACTION)
				{
					this.addList("slave clothing", itemType.ARMOUR, 999, "residents", "what do our slaves wear");
					this.addList("bar squads", itemType.SQUAD_TEMPLATE, "1, 100", 999, "residents", "resident squads that spawn in bars. val1 is number, val2 is % chance");
					this.addList("personality", itemType.PERSONALITY, 1, "Personality", "the personality of the characters");
					this.addList("campaigns", itemType.FACTION_CAMPAIGN, 99, "", "various possible wars and missions this faction can do");
					this.@add("default relation", 0, "relation", "This is the default value for all faction relationships that are not specified in the \"Faction Relations\" list.\nRange -100 to +100");
					this.@add("business relations", -5, "relation", "Min relation to do business");
					this.@add("enemy classification", -10, "relation", "Max relation to consider as enemy, attack on sight");
					this.@add("effect of anger", 0.2f, "relation", "emotion is what affects the real relations. This shows how much.  So at 0.1x, an anger of 100 will cause relations to drop by 10.  This determines how easy it is to lose a factions trust.");
					this.@add("effect of happy", 0.2f, "relation", "emotion is what affects the real relations. This shows how much.  So at 0.1x, a happiness of 100 will cause relations to raise by 10.  This determines how easy it is to gain a factions trust.");
					this.@add("emotion fade rate", 1f, "relation", "multiplier for speed that emotion fades back to 0.  Higher makes them more volatile");
					this.@add("not real", false, "relation", "set true for factions that are not human, actions against this faction won't be seen as crime or aggresssion.  use for wildlife, non-factions etc");
					this.@add("cages lock level", 40, "locks", "for cages, sets the lockpick difficulty level");
					this.@add("doors lock level", 40, "locks", "for doors, sets the lockpick difficulty level");
					this.@add("containers lock level", 40, "locks", "for containers (chest, barrels, shop counters, etc.), sets the lockpick difficulty level");
					this.@add("lock level random", 10, "locks", "for doors and cages, randomises the lockpick difficulty level by this amount");
					this.addEnum("fundamental type", CharacterTypeEnum.OT_CIVILIAN, "type", "the basic classification of this faction");
					this.@add("anti slavery", false, "type", "true if this faction is against slavery, doesn't use or sell slaves and doesn't rat out escapees");
					this.@add("run away ratio of squad size", 0f, "flee", "0-1  For example: My squad size is 10 men.  Ratio is at 0.7.  If we have less than 70% of our men we flee.  So if 4 men go down, we flee.");
					this.@add("run away ratio relative to enemy", 0f, "flee", "2.0 ratio means we need to out number our enemy 2 to 1, otherwise we flee.  0.5 ratio means enemy has to outnumber us 2 to 1 in order to flee.");
					this.@add("trustworthy", 0f, "AI", "-100 <> +100.  How trustworthy the faction is. Do they keep their word or stab you in the back?");
					this.@add("heals strangers", false, "AI", "True if we give first aid to unconscious strangers/neutrals we find");
					this.@add("building cost mult", 1f, "", "multiplier for the cost to buy buildings from this faction");
					this.@add("max prosperity", 1000, "", "");
					this.@add("allow slaves weapons", false, "", "lets slaves have a weapon, for when they are conscripted");
					this.@add("road preference", 0.4f, "", "How much characters will try to use roads [0-1].\n0 completely ignores them, 1 uses them as much as posible");
					this.@add("num ranks", 1, "rank", "ranking levels within the faction, mainly for the player");
					this.@add("num negative ranks", 1, "rank", "ranking levels within the faction, mainly for the player");
					this.addStringLooping("rank0", "", "rank", "ranking levels within the faction, mainly for the player");
					this.addStringLooping("negative rank0", "", "rank", "ranking levels within the faction, mainly for the player");
					this.@add("offers bounties", false, "rank", "if this faction can put a bounty on your head, use for the major faction");
					this.@add("roaming population", 50, "", "number of NPCs in roaming squads, basically adds this number to the max population count for every town to allow short-range roaming squads to appear around the towns");
					this.@add("armors 0", 50, "armour vendor quality chance", "chances of vendors having this armour quality");
					this.@add("armors 1", 100, "armour vendor quality chance", "chances of vendors having this armour quality (cheap)");
					this.@add("armors 2", 50, "armour vendor quality chance", "chances of vendors having this armour quality (standard)");
					this.@add("armors 3", 2, "armour vendor quality chance", "chances of vendors having this armour quality (good)");
					this.@add("armors 4", 0, "armour vendor quality chance", "chances of vendors having this armour quality");
					this.@add("armors 5", 0, "armour vendor quality chance", "chances of vendors having this armour quality");
					this.addList("races", itemType.RACE, "100", 10, "Population race %", "list races that are in this faction, and the percentage they make up the whole.  Should all total together to 100, but it does not matter as it is relative");
					this.addList("relations", itemType.FACTION, "0", 99, "Faction Relations", "-100 to +100l\nrelationship with other factions, if not listed then assumed 0 (neutral)");
					this.addList("coexistence", itemType.FACTION, 99, "Relations", "the faction will try to co-exist with these factions.  They can still fight, but will usually avoid targeting neutrals unprovoked.  Usually.  Stops excessive warfare and wiping out of small villages etc.");
					this.addList("legal system", itemType.FACTION, 1, "ls", "who this faction uses to report and enforce crimes and bounties");
					this.addList("AI fallback", itemType.AI_PACKAGE, 999, "AI fallback", "fallback AI if we can't do anything else.  Something easy like run away or fight to the death.");
					this.addList("squad default", itemType.SQUAD_TEMPLATE, 1, "default squad", "the various squad types, or divisions, available");
					this.addList("squads", itemType.SQUAD_TEMPLATE, 999, "Squad", "the various squad types, or divisions, available");
					this.addList("dialog default", itemType.DIALOGUE_PACKAGE, 999, "d", "any characters that don't have a dialogue package will get this one assigned. Make it super generic.");
					this.addList("residents", itemType.SQUAD_TEMPLATE, "1,1", 999, "residents", "default residents if a town has no residents assigned.  val0 is number of them, but set it to zero to set one as the default filler resident. val1 is the priority level, higher number is more important");
					this.addList("default resident", itemType.SQUAD_TEMPLATE, 1, "residents", "default resident, used as filler");
					this.addList("special squads", itemType.UNIQUE_SQUAD_TEMPLATE, "0", 999, "Squad", "the special squads, with unique NPCs, the ones that make decisions and launch attacks and stuff, val1 is the number to maintain (which will be multiplied by faction/town health and population)");
					this.addList("biomes", itemType.BIOME_GROUP, 999, "biome", "the biomes that make up our territory.  Important for triggering assault campaigns when player builds in our area.");
					this.addList("no-go zones", itemType.BIOME_GROUP, 999, "biome", "the biomes that this faction won't go to, stops assaults");
					this.addList("color", itemType.COLOR_DATA, 1, "color", "color scheme for clothings");
					this.addList("item spawns resident", itemType.VENDOR_LIST, "100", 1, "", "vendor list to use for spawning random building contents in containers.  Overridden if the squad has a vendor list");
					this.addList("item spawns HQ", itemType.VENDOR_LIST, "100", 1, "", "vendor list to use for spawning random building contents in containers.  Overridden if the squad has a vendor list");
					this.addList("item spawns resident small", itemType.VENDOR_LIST, "100", 1, "", "vendor list to use for spawning random building contents in containers.  Overridden if the squad has a vendor list");
					this.addList("item spawns bar", itemType.VENDOR_LIST, "100", 1, "", "vendor list to use for spawning random building contents in containers.  Overridden if the squad has a vendor list");
					this.addList("item spawns treasure", itemType.VENDOR_LIST, "100", 1, "", "vendor list to use for spawning random building contents in containers.  Overridden if the squad has a vendor list");
					this.addList("item spawns armoury", itemType.VENDOR_LIST, "100", 1, "", "(or barracks) vendor list to use for spawning random building contents in containers.  Overridden if the squad has a vendor list");
					this.addList("trade culture", itemType.ITEMS_CULTURE, 100, "", "controls items in this faction, by removing certain items altogether or modifying prices");
					this.@add("faces weirdness", 0.2f, "", "Probability of weird face morphs in the faction");
					this.addEnum("squad formation", head.SquadFormation.RANDOM, "AI", "Sets which type of formation the faction's squads uses.");
					this.addList("buildings replacements", itemType.BUILDINGS_SWAP, 0, "replacements", "List of buildings to replace in towns.");
					this.addList("hairstyles", itemType.ATTACHMENT, 99, "", "Hairstyles used by this faction. Leave blank to allow all styles");
				}
				if (t == itemType.FACTION_TEMPLATE)
				{
					this.@add("default relation", 0, "relation", "This is the default value for all faction relationships that are not specified in the \"Faction Relations\" list.\nRange -100 to +100");
					this.@add("squad size min", 1, "stats", "");
					this.@add("squad size max", 20, "stats", "");
					this.@add("combat stats min", 1, "stats", "how strong will this squad be if it is max size");
					this.@add("combat stats max", 40, "stats", "how strong will this squad be if it is min size");
					this.@add("leader levels min", 1, "leaders", "how many levels, or tiers, of rank there are after basic grunts");
					this.@add("leader levels max", 1, "leaders", "how many levels, or tiers, of rank there are after basic grunts");
					this.@add("leader increase min", 5, "leaders", "how much leader stats are increased with each tier");
					this.@add("leader increase max", 10, "leaders", "how much leader stats are increased with each tier");
					this.@add("skill cap", 60, "leaders", "cap the max skill a leader can have");
					this.@add("face weirdness", 0.1f, "general", "0-1 amount of population that has weird faces");
					this.@add("strength/size balanced", true, "general", "if true, the combat stats are balanced against the squad sizes eg bigger&weaker or smaller&stronger.  If false then they are independent and lead to more potentially unbalanced factions");
					this.addEnum("fundamental type", CharacterTypeEnum.OT_BANDIT, "general", "");
					this.addEnum("armour cap", ArmourRarity.GEAR_QUALITY, "stats", "cap the max armour level a leader can have (leaders can go above the max)");
					this.addEnum("armour min", ArmourRarity.GEAR_PROTOTYPE, "stats", "");
					this.addEnum("armour max", ArmourRarity.GEAR_STANDARD, "stats", "");
					this.addList("AI fallback", itemType.AI_PACKAGE, 999, "AI fallback", "fallback AI if we can't do anything else.  Something easy like run away or fight to the death.");
					this.addList("clothing", itemType.ARMOUR, 999, "clanthoue", "list of gear to choose from");
					this.addList("weapon level", itemType.WEAPON_MANUFACTURER, 999, "clanthoue", "list of gear to choose from");
					this.addList("weapon models", itemType.WEAPON, 999, "clanthoue", "list of gear to choose from");
					this.addList("races", itemType.RACE, "100", 1, "clanthoue", "list of races and percentage that makes up the faction");
				}
				if (t == itemType.SQUAD_TEMPLATE || t == itemType.UNIQUE_SQUAD_TEMPLATE)
				{
					this.addList("personality", itemType.PERSONALITY, 1, "Personality", "the personality of the characters");
					this.addList("generation info", itemType.FACTION_TEMPLATE, "", 9, "Squad", "extra info if you want a more randomised squad");
					this.addList("leader", itemType.CHARACTER, "1", 1, "Squad", "the character templates making up each squad, val1 is number of them");
					this.addList("slaves", itemType.SQUAD_TEMPLATE, "", 999, "Squad", "any slaves owned by this squad");
					this.addList("prisoners", itemType.SQUAD_TEMPLATE, "", 999, "Squad", "any prisoners, will be spawned in the cages, if there are no cages available then they won't be spawned");
					this.addList("race override", itemType.RACE, "100", 999, "Squad", "forces all the characters to be this race.");
					this.addList("squad", itemType.CHARACTER, "1, 0", 999, "Squad", "the character templates making up each squad, val0 is min number of them, val1 is max number (0=non random)");
					this.addList("squad2", itemType.CHARACTER, "1, 0", 999, "Squad 2", "optional separate half of squad");
					this.addList("animals", itemType.ANIMAL_CHARACTER, "1, 1, 100", 999, "animals", "the character templates making up each squad, val0 is min number of them, val1 is max number, val2 is the age: 100=adult, 0=baby");
					this.addList("animals2", itemType.ANIMAL_CHARACTER, "1, 1, 100", 999, "animals", "the character templates making up each squad, val0 is min number of them, val1 is max number, val2 is the age: 100=adult, 0=baby");
					this.addList("choosefrom list", itemType.CHARACTER, "1", 999, "Choosefrom list", "the game will randomly choose characters out of this list [val1] is the chance.");
					this.@add("num random chars max", 0, "Choosefrom list", "the max number of random chars to choose from the [choosefrom list].");
					this.@add("num random chars", 0, "Choosefrom list", "the min number of random chars to choose from the [choosefrom list].");
					this.addList("world state", itemType.WORLD_EVENT_STATE, "1", 99, "et", "adds this world state as a AND condition for this squad to spawn.  1=true, 0=false");
					this.addEnum("force speed", MoveSpeed.NO_SPEED_CHANGE, "", "Forces the characters to always try and move at this speed");
					this.addEnum("crossbow levels", ArmourRarity.GEAR_CHEAP, "vendors", "avg gear quality level, has chance of being 1 higher or 1 lower");
					this.addEnum("robotics levels", ArmourRarity.GEAR_CHEAP, "vendors", "avg gear quality level, has chance of being 1 higher or 1 lower");
					this.addList("faction", itemType.FACTION, 1, "Faction", "Optional, Needed sometimes, often for town residents");
					this.addList("building", itemType.BUILDING, 1, "building", "(optional) For town residents, the preferred building type that this squad lives in");
					this.addList("building dislike", itemType.BUILDING, 1, "building", "(optional) For town residents, prefers NOT to use this building");
					this.addList("nest", itemType.TOWN, 1, "", "(optional) here you could add a nest, which will populate the building with debris and loot like a nest does");
					this.@add("layout interior", "", "town residents", "Layout name to use for the specified building");
					this.@add("layout exterior", "", "town residents", "Layout name to use for the specified building");
					this.@add("building name", "", "town residents", "sets the name to display in the buildings GUI panel, if blank then the interior layout name will be used");
					this.@add("building ruined", false, "town residents", "true makes this building a ruin");
					this.@add("public day", true, "town residents", "If the squad has a home building, sets it public or private");
					this.@add("public night", false, "town residents", "If the squad has a home building, sets it public or private");
					this.@add("public beds", false, "town residents", "Can the player use their beds?");
					this.@add("bed usage cost", 100, "town residents", "How much it costs to use a bed");
					this.addEnum("building designation", BuildingDesignation.BD_NONE, "town residents", "how this building is considered by the AI");
					this.addEnum("initial door state", head.DoorState.CLOSED, "building", "How the building door is when first loaded.");
					this.addList("dialog leader", itemType.DIALOGUE_PACKAGE, 999, "d", "Override dialog for the leader. overrides anything personal.");
					this.addList("dialog squad", itemType.DIALOGUE_PACKAGE, 999, "d", "Dialog for the entire squad other than the leader. Overrides any packages set in the character.");
					this.addList("dialog animal", itemType.DIALOGUE_PACKAGE, 999, "d", "Dialog for any animals in the squad.");
					this.@add("malnourished", false, "general", "true to use the skinny normal map for characters");
					this.@add("roaming military", false, "", "If true then forces this squad to count as a roaming squad, meaning it counts as mobile military forces, even if it has a hometown");
					this.@add("dont multiply", false, "", "stops this squad size from being affected by the squad size multiplier in the options window");
					this.@add("animal age random", 0.1f, "", "for animals in the squad, affects how much their age can be randomised (0-1)");
					this.@add("blood smell mult", 0f, "AI", "Mainly for animals, affects this squads attraction to the smell of blood and death.  1 is a moderate effect, about 4 is very strong.  You can also go negative to make the squad avoid dangerous zones.");
					this.@add("patrol approaches towns", true, "AI", "for randomly patrolling squads, this determines if they go for nearby towns (hostile or otherwise).");
					if (t == itemType.SQUAD_TEMPLATE)
					{
						this.addList("housemates", itemType.SQUAD_TEMPLATE, "1, 0", 99, "", "this squad will also spawn these other squads.  Useful when you want to have multiple separate squads living in one building.  vals are min/max number of them to spawn");
						this.@add("sell home", false, "", "true if this squad is willing to sell its home building");
						this.addList("AI packages", itemType.AI_PACKAGE, "0", 99, "packages", "main list of AI packages, an AI package is an entire shift in behavior.  val0 is priority level, so a higher number package will alway override a lower number, and won't go back to the lower one until the upper one is completed.");
						this.addList("vendors", itemType.VENDOR_LIST, "100", 10, "Vendors", "inventory list if this squad is a shop squad, val0 is percentage of items from that vendor");
						this.addList("special items", itemType.ITEM, "1", 10, "Vendors", "Use this list to spawn any unique special items.  Every item in this list will be spawned in the squad's building somewhere.");
						this.addList("special map items", itemType.MAP_ITEM, "1", 10, "Vendors", "Use this list to spawn any unique special map items.  Every item in this list will be spawned in the squad's building somewhere.");
						this.@add("vendors fill total amount", 15, "vendors", "Sets the total maximum amount of items the vendor can have.");
						this.@add("is trader", false, "vendors", "True if this squad is a trading squad. Must have a vendor item list.");
						this.@add("vendors refresh time", 24f, "vendors", "Time in hours to refresh the trader's inventory items.");
						this.@add("buy mult", 1f, "vendors", "price multiplier, when the npc buys and sells");
						this.@add("buys stolen", false, "vendors", "true if he buys stolen goods");
						this.@add("buys illegal", false, "vendors", "true if he buys illegal goods like drugs.  NOTE this only applies to the laws of the town they are in, eg a to a swamper drugs are not illegal");
						this.@add("vendor money", 0, "vendors", "Money available to vendors for buying goods.\nLeave at 0 to use default value calculated from vendor list");
						this.@add("regenerates", false, "town residents", "if true for a resident squad, they will slowly regenerate over time, to restore losses and prevent towns getting worn away to nothing");
						this.@add("item artifacts base value", 0, "content", "The base value of item artifacts the squad can get. (0 = No artifacts).  Town needs to also have a max artifact value set for this squad to get anything");
						this.@add("gear artifacts base value", 0, "content", "The base value of gear artifacts (weapons and armours) the squad can get. (0 = No artifacts).  Town needs to also have a max artifact value set for this squad to get anything");
					}
					if (t == itemType.UNIQUE_SQUAD_TEMPLATE)
					{
						this.addList("missions", itemType.DIPLOMATIC_ASSAULTS, 99, "missh", "The different types of mission we are capable of doing to a target, val1 is weight 0-100+");
						this.@add("replacement time", 24, "death", "time it takes for the faction to replace this squad if it's wiped out");
						this.@add("persistent", true, "death", "makes this a non-permanent squad");
					}
				}
				if (t == itemType.AI_SCHEDULE)
				{
					this.addList("packages", itemType.AI_PACKAGE, "0", 99, "p", "AI packages that we switch between, val0 is the time of day in hours 0-23");
				}
				if (t == itemType.AI_TASK)
				{
					this.addEnum("enum", head.taskType.IDLE, "what is", "");
					this.addEnum("classification", taskPriority.TP_NON_URGENT, "details", "URGENT = eg player orders, combat, first aid.  NON_URGENT = eg sleeping, eating, surgery. The stuff you just don't consider on a battlefield. FLUFF = ambient stuff that achieves nothing eg chatting.  ");
					this.addEnum("targeting", head.TaskTargetType.TARGET_SPECIFIC, "details", "the target of the task");
					this.addEnum("ending", head.TaskEndEvent.TEE_NOTHING, "details", "what happens when the task is completed, does it get removed for myself or the whole squad, or does it just remain active?");
				}
				if (t == itemType.TOWN)
				{
					this.addList("trade culture", itemType.ITEMS_CULTURE, 99, "nht", "specifying a culture object for a town will add overrides to that of the faction culture (only works for the price mults)");
					this.addList("material", itemType.MATERIAL_SPEC, 1, "Building material", "Sets the material used for this town");
					this.addList("residents", itemType.SQUAD_TEMPLATE, "1,2", 999, "residents", "residents of this town.  val0 is number of them, but set it to zero to set one as the default filler resident.  If no residents are listed, then it will use the residents listed in the FACTION as default.  val1 is the priority level, higher number is more important");
					this.addList("bar squads", itemType.SQUAD_TEMPLATE, "1, 100", 999, "residents", "resident squads that spawn in bars. val1 is number, val2 is % chance");
					this.addList("default resident", itemType.SQUAD_TEMPLATE, 1, "residents", "default resident, used as filler");
					this.addList("roaming squads", itemType.SQUAD_TEMPLATE, "0", 999, "Squad", "the roaming squads that actually leave town and do stuff, they are tied to the town population");
					this.addList("debris", itemType.NEST_ITEM, "0, 10, 100", 999, "", "debris items to scatter around the area, val0+1 is min/max num to spawn, val2 is the mesh scale multiplier (%)");
					this.addList("debris building", itemType.BUILDING, "0, 10, 100", 999, "", "for nests only, building must have no navmesh effect.  debris items to scatter around the area, val0 is num to spawn (+has 50% randomisation) (set to 0 for a 50% chance of 1), val1+2 is the min and max cluster range around the centrepoints of the nest)");
					this.addList("faction", itemType.FACTION, 1, "Faction", "Town is owned by this faction");
					this.addList("loot spawn", itemType.VENDOR_LIST, "0, 10", 99, "", "random items to spawn scattered around, val0+1 is random min max number of items");
					this.addList("world state", itemType.WORLD_EVENT_STATE, "1", 99, "et", "(nests or override towns) adds this world state as a AND condition for this nest/town to spawn.  1=true, 0=false");
					this.addList("override town", itemType.TOWN, 99, "et", "(towns) here you can specify a replacement town, with a world state.  If the world state is true it will repopulate the town with the new one.  Use it to change and devastate factions.");
					this.@add("num centrepoints", 4, "spawn junk", "used for debris buildings, they are spawned in clusters, and this is the number of clusters");
					this.@add("size radius", 350f, "spawn junk", "affects the overall size of the nest");
					this.@add("spawn in town centre", false, "spawn junk", "if true then the nest junk will spawn for any town, otherwise its just for nests");
					this.@add("no-foliage range", 2000f, "", "no foliage will be spawned within this range");
					this.@add("town radius mult", 1f, "", "multiplies the town radius, should normally be 1 except in special cases, like if a town is so big that the AI considers itself out of town before it leaves the gates");
					this.@add("unexplored name", "", "", "optional mysterious name for when town is unexplored");
					this.@add("nest resident population", 15, "nests", "the max population value, number of animals to spawn for this if its a nest");
					this.@add("is public", true, "", "Lets the public visit.  False for things like military bases");
					this.@add("is secret", false, "", "use for crafty secret towns, stops the GUI from displaying buildings factions and giving away suprises");
					this.@add("residents override", false, "", "if true residents list will replace faction default, else will add to it");
					this.addEnum("type", TownType.TOWN_OUTPOST, "", "type of town");
					this.@add("item artifacts min value", 0, "content", "The minimum value of item artifacts in the town.");
					this.@add("item artifacts max value", 0, "content", "The maximum value of item artifacts in the town. (0 = No artifacts).  IMPORTANT: make sure there is at least 1 resident in this town with the same or higher artifact value, otherwise the artifactts will be lost");
					this.@add("gear artifacts min value", 0, "content", "The minimum value of gear artifacts (weapons and armours) in the town.");
					this.@add("gear artifacts max value", 0, "content", "The maximum value of gear artifacts (weapons and armours) in the town. (0 = No artifacts).  IMPORTANT: make sure there is at least 1 resident in this town with the same or higher artifact value, otherwise the artifactts will be lost");
				}
				if (t == itemType.LOCATIONAL_DAMAGE)
				{
					this.@add("collapses", true, "end result", "If it reaches low level, does the character collapse");
					this.addEnum("collapse part", head.RagdollPart.WHOLE, "end result", "part of the body that goes limp if collapses=true");
					this.@add("death", true, "end result", "If flesh/organ damage reaches 0, does the character die");
					this.@add("severance", true, "end result", "if flesh damage reaches 0, does the bodypart get severed?");
					this.@add("KO mult", 1f, "end result", "amount that this part adds to KO time if damaged < 0");
					this.addEnum("body part type", head.BadyPartType.PART_TORSO, "", "flags the type of part this is");
					this.@add("affects move speed", 0f, "damage effects 0-1", "If this area is damaged, will it damage move speed?/n1=full damage -> zero's the stat/n0=no effect");
					this.@add("affects skills", 0f, "damage effects 0-1", "If this area is damaged, will it damage skill?/n1=full damage -> zero's the stat/n0=no effect");
					this.@add("bone name", "Bip01 Spine2", "", "Name of the bone in the skeleton that represents this body part");
					this.@add("bone name 2", "", "", "optional additional bone");
					this.@add("bone name 3", "", "", "optional additional bone");
					this.addList("pain anim", itemType.ANIMATION, "0", 1, "pain overlay animation", "animation played as an overlay to express the pain, eg clutching the wound.  number is the percentage of max health at which the animation activates");
				}
				if (t == itemType.COMBAT_TECHNIQUE)
				{
					this.@add("anim name", "", "animation", "Name of the animation.  Note that all blocks should be upper body animation only, and all attacks should be full-body animation");
					this.@add("is block", false, "", "True if its a block, false if its an attack");
					this.@add("is dodge", false, "", "True if its a dodge");
					this.@add("is prone", false, "", "True if fighting from a prone position");
					this.@add("is stumble dodge", false, "", "if its a dodge, is it used for stumble breaks instead of just normal dodges");
					this.@add("gains ground", false, "animation", "true if the character actually moves away from the origin in the animation.  False if he stays on the spot.");
					this.@add("num techniques", 1, "", "number of attacks, like for combos.  Obviously blocks can only have 1.");
					this.@add("anim speed mult", 1f, "animation", "adjust the speed the animation should be played");
					this.@add("anim hesitate point", 0.2f, "animation", "(0-1 value) Point when the sword is raised.\nWhen the character hesitates with the attack, he will raise the sword and hesitate.");
					this.addEnum("attack direction 1", CutDirection.CUT_DOWNWARD, "hits", "down/left/right/thrust/downleft/downright");
					this.addEnum("attack direction 2", CutDirection.CUT_DOWNWARD, "hits", "down/left/right/thrust/downleft/downright");
					this.addEnum("low strike", false, "hits", "use to designate this as a low technique eg aiming at the legs");
					this.addEnum("max simultaneous hits", 99, "hits", "limits the maximum number of targets you can hit with one sweep");
					this.@add("num frames", 1f, "hits", "how many frames in the animation (used to calculate impact point times)");
					this.@add("anim stop frame 1", 1f, "hits", "what frame the first attack stops dead, (so the blood can stop spraying off the blade). \n(frame number, or decimal if numframes==1)");
					this.@add("anim blocked frame 1", 0.8f, "hits", "for attacks only, if the opponent blocks it, what frame the first attack makes impact with the opponents blocking weapon, (frame number, or decimal if numframes==1)");
					this.@add("power 1", 100, "hits", "power that the first attack has, as a percentage of capability.  So normally use 100, but use a lower number for little attacks.  For balance you should half the power for fast double combos where the second attack is often too fast to block.");
					this.@add("anim stop frame 2", 1f, "hits", "what frame the first attack stops dead, (so the blood can stop spraying off the blade). \n(frame number, or decimal if numframes==1)e");
					this.@add("anim blocked frame 2", 0.8f, "hits", "for attacks only, if the opponent blocks it, what frame the first attack makes impact with the opponents blocking weapon, as a 0-1 value.\n(frame number, or decimal if numframes==1)");
					this.@add("power 2", 100, "hits", "power that the second attack has, as a percentage of capability.  So normally use 100, but use a lower number for little attacks.");
					this.addEnum("limb 1", head.RobotLimb.NULL_LIMB, "hits", "which limb we are using to hit with");
					this.addEnum("limb 2", head.RobotLimb.NULL_LIMB, "hits", "which limb we are using to hit with");
					this.@add("acceptable end time", 0.95f, "hits", "0-1 (attacks only) at what point is it acceptable for this animation to be terminated by something else (eg need to block)");
					this.@add("max skill", 999f, "skills", "0-100 max skill at which we can use this technique");
					this.@add("min skill", -100f, "skills", "0-100 min skill at which we can use this technique");
					this.@add("max encumbrance", 999f, "skills", "0-100% encumbrance, above this we can't use the technique.  eg use to stop jump kicks in heavy armour");
					this.@add("attack distance", 0f, "", "max distance at which this attack can be considered for use (not including mei-distance).  Is the disance the biped root travels in animation.");
					this.@add("attack distance min vs static", 0f, "", "Used when attacking a static target that cannot retreat, if we bump into the target then this value is too low.  Min distance at which this attack can be considered for use (not including mei-distance).  Can be higher than [attack distance].  If its a combo with a lot of forward motion you should disable it by entering a really high number");
					this.@add("chance", 1f, "", "just a multiplier for chance of being chosen.  use to make fancy attacks less frequent");
					this.addEnum("animal", WeaponCategory.SKILL_KATANAS, "type", "if you list an animal type here, then this attack is just for certain animals, not humans");
					this.addList("events", itemType.ANIMATION_EVENT, "0", 12, "", "Audio events to trigger during the animation.\nValue is how far through the animation to play (frame number, or % if numframes==1)");
				}
				if (t == itemType.COMBAT_TECHNIQUE || t == itemType.ANIMATION)
				{
					this.@add("disabled", false, "", "disables this animation, will be considered non-existent");
					this.@add("katanas", true, "type", "valid weapon types for this animation");
					this.@add("sabre", true, "type", "valid weapon types for this animation");
					this.@add("1 handed", true, "type", "can be used with one arm?");
					this.@add("heavy weapons", true, "type", "valid weapon types for this animation");
					this.@add("blunt", true, "type", "valid weapon types for this animation");
					this.@add("hackers", true, "type", "valid weapon types for this animation");
					this.@add("unarmed", true, "type", "valid weapon types for this animation");
					this.@add("polearm", true, "type", "valid weapon types for this animation");
				}
				if (t == itemType.RESEARCH)
				{
					this.@add("category", "Core", "", "");
					this.@add("level", 1, "", "level of research desk required");
					this.@add("is level upgrade", false, "", "true if this is a level up for the current tech level");
					this.@add("blueprint only", false, "", "if true then this tech doesnt appear by itself, you have to find the blueprints somewhere");
					this.@add("time", 12, "", "time required, in game hours");
					this.@add("money", 0, "", "Money required");
					this.addText("description", "", "", "");
					this.addList("improve buildings", itemType.BUILDING, 99, "", "Building improvements will affect the given buildings");
					this.@add("production mult", 1f, "building improvements", "multiplies the buildings [output rate]");
					this.@add("power increase", 0, "building improvements", "Adds this amount to the buildings [power output]");
					this.@add("power capacity increase", 0, "building improvements", "Adds this amount to the buildings [power capacity]");
					this.@add("repeats", 0, "repeats", "If > 0 this research can be repeated for cumulative upgrades.  val1 of enabled items/buildings etc will represent the repeat number that the item is unlocked at (0 first, then 1, then 2....)");
					this.@add("repeat mult", 1f, "repeats", "if repeating, time/money costs are multiplied by this");
					string str1 = "Value 1 will designate which repetition this item is unlocked on\nValue 2 overrides the tech level of this research item at this repetition value\nValues are ignored if research has no repetitions";
					this.addList("requirements", itemType.RESEARCH, "0", 99, "", "Research required to unlock this item\nValue is research repetition number needed");
					this.addList("enable buildings", itemType.BUILDING, "0,0", 99, "", string.Concat("Unlock buildings in the build menu\n", str1));
					this.addList("enable weapon type", itemType.WEAPON, "0,0", 99, "", string.Concat("Unlock weapon model in crafting benched\n", str1));
					this.addList("enable weapon model", itemType.MATERIAL_SPECS_WEAPON, "0,0", 99, "", string.Concat("Enable weapon models to be crafted\n", str1));
					this.addList("enable armour", itemType.ARMOUR, "0,0", 99, "", string.Concat("Enable armour to be crafted\n", str1));
					this.addList("enable item", itemType.ITEM, "0,0", 99, "", string.Concat("Enable items to be crafted", str1));
					this.addList("enable robotics", itemType.LIMB_REPLACEMENT, "0,0", 99, "", string.Concat("Enable robotic limbs to be crafted\n", str1));
					this.addList("enable crossbow", itemType.CROSSBOW, "0,0", 99, "", string.Concat("Enable crossbows to be crafted\n", str1));
					this.addList("enable backpack", itemType.CONTAINER, "0,0", 99, "", string.Concat("Enable backpacks to be crafted\n", str1));
					this.addList("cost", itemType.ITEM, "1", 99, "", "the cost in resources");
					this.addList("blueprint item", itemType.ITEM, 1, "", "Means that this is a physical blueprint item that needs to be found or bought.");
				}
				if (t == itemType.ANIMATION || t == itemType.ANIMAL_ANIMATION)
				{
					if (t == itemType.ANIMATION)
					{
						this.addEnum("category", CharacterAnimCategory.ANIM_NORMAL, "", "determines the state where this animation may be used");
						this.addEnum("has weapon R", head.Either.EITHER, "", "does anim support/require holding weapon?");
						this.addEnum("has weapon L", head.Either.NO, "", "does anim support/require holding weapon?");
						this.addEnum("is action", false, "", "true if this is an action, a standalone anim not to be bothered by other movement and idle anims");
						this.@add("prone", false, "category", "true if used when lying prone in some way");
						this.@add("L leg damage min", 0f, "injury 0-100", "min health amount for this anim to be useable");
						this.@add("L leg damage max", 100f, "injury 0-100", "max health amount for this anim to be useable");
						this.@add("L leg damage ideal", 100f, "injury 0-100", "health amount for this anim to be used at full weight");
						this.@add("R leg damage min", 0f, "injury 0-100", "min health amount for this anim to be useable");
						this.@add("R leg damage max", 100f, "injury 0-100", "max health amount for this anim to be useable");
						this.@add("R leg damage ideal", 100f, "injury 0-100", "health amount for this anim to be used at full weight");
						this.@add("uses right arm", false, "category", "true if this animation requires use of the right arm");
						this.@add("uses left arm", false, "category", "true if this animation requires use of the left arm");
						this.@add("override L arm", false, "overrides", "true for this bodypart to totally override all previous anims");
						this.@add("override R arm", false, "overrides", "true for this bodypart to totally override all previous anims");
						this.@add("override head", false, "overrides", "true for this bodypart to totally override all previous anims");
						this.@add("slave anim", "", "overrides", "this will force any anim-slaves to play the named animation");
						this.@add("delete L arm", false, "track exclusion", "used to disable this bodyparts influence on the animation, the animation on this part will be null");
						this.@add("delete R arm", false, "track exclusion", "used to disable this bodyparts influence on the animation, the animation on this part will be null");
						this.@add("delete below waist", false, "track exclusion", "used to disable this bodyparts influence on the animation, the animation on this part will be null");
						this.@add("delete body head", false, "track exclusion", "used to disable this bodyparts influence on the animation, the animation on this part will be null");
						this.@add("delete weapons", false, "track exclusion", "used to disable this bodyparts influence on the animation, the animation on this part will be null");
						this.@add("delete tail", true, "track exclusion", "used to disable this bodyparts influence on the animation, the animation on this part will be null");
						this.@add("delete spine0", false, "track exclusion", "specifically just deletes the 1st spine bone, if you have problems with different anims pitching the body angle everywhere");
						this.addEnum("weather type", WeatherAffecting.WA_NONE, "affects characters", "makes this a weather overlay animation, used during this weather affect");
					}
					this.addEnum("stealth mode", head.Either.EITHER, "", "if can be used for stealth mode");
					this.@add("anim name", "", "", "the actual string name of the animation");
					this.@add("layer", "all", "", "body area affected by anim : upper, lower, all, overlay");
					this.@add("play speed", 1f, "", "animation play speed multiplier, for movement anims, this is multiplied by movement speed, so should be small like 0.02, tune until feet match ground speed");
					this.@add("relocates", false, "", "true if the root bone ends up somewhere other than 0,0 position");
					this.@add("disables movement", false, "", "true if disables waypoint movement while playing, eg for stumble anims, getting up, things that can't be interrupted by player orders.");
					this.@add("strafe speed max", 0f, "movement", "");
					this.@add("move speed", 0f, "movement", "if its a movement animation, this is the ideal speed it travels at");
					this.@add("min speed", 0f, "movement", "if its a movement animation, this is the ideal speed of the next anim below");
					this.@add("max speed", 0f, "movement", "if its a movement animation, this is the ideal speed of the next anim above");
					this.addEnum("is combat mode", head.Either.EITHER, "", "if can be used for combat, either means it can play any time");
					this.@add("idle chance", 100, "idle", "relative chance of this idle being chosen");
					this.@add("idle time min", 10, "idle", "time this idle will play for, if it is a loop");
					this.@add("idle time max", 40, "idle", "time this idle will play for, if it is a loop");
					this.@add("idle", false, "idle", "true if used for standing around, false if its an actual action or movement");
					this.@add("synchs", false, "", "true if this can sync with upper if lower, or vise versa");
					this.@add("loop", false, "", "duh");
					this.@add("synch offset", 0f, "", "If the animation was not correctly synched with the others, you can fix it here, is 0-1");
					this.@add("carrying left", false, "category", "true if the character is carrying something on that side");
					this.@add("carrying right", false, "category", "true if the character is carrying something on that side");
					this.@add("being carried", false, "category", "true the char is being carried");
					this.@add("reverse looping", false, "entry/exit", "for idle anims, instead of looping the anim will reverse when it endes, then forward again.  This eliminates the need for idle anims to be perfectly looped");
					this.@add("normalise", true, "", "if true then all anims with this flag that are currently playing, will always total up to 1.0 in weight.  Set to false for things like overlay anims");
					this.addList("stumbles", itemType.LOCATIONAL_DAMAGE, 99, "", "uses this animation as the stumble when this bodypart is hit");
					this.addEnum("stumble from", CutOrigination.FRONT, "stumble", "direction the impact has occurred from");
					this.@add("big stumble", true, "stumble", "used for higher-damage impacts");
					this.@add("chance", 100, "stumble", "weight/chance of being chosen vs other valid stumbles");
					this.@add("delete root", false, "track exclusion", "used to disable this bodyparts influence on the animation, the animation on this part will be null");
					this.addList("events", itemType.ANIMATION_EVENT, "0", 12, "", "Audio events to trigger during the animation.\nValue is how far through the animation to play [0-100]");
				}
				if (t == itemType.CONSTANTS)
				{
					this.@add("immediate blood loss", 1f, "combat damage", "Multiplier for initial blood loss caused by a cut.  (different to the steady bleeding)");
					this.@add("damage resistance randomised", true, "combat damage", "if true then a characters damage resistance will be used as a max, and use a random value each time they are hit.  If false, then will be applied straight");
					this.@add("damage resistance max", 0.5f, "combat damage", "at max skill level (toughness), the amount of damage resistance. 0-1, 1 being 100% damage resistance");
					this.@add("damage resistance min", -0.5f, "combat damage", "at 0 skill level (toughness), the amount of damage resistance. 0-1, 1 being 100% damage resistance, minus numbers amplify the damage");
					this.@add("stumble damage max", 50, "combat damage", "at max skill level (toughness), the amount of damage a character must recieve in order to make him stumble.  Multiplied by toughness");
					this.@add("min dismantle materials percentage", 60f, "", "When you dismantle a building, you randomly get back between this% and 100% of your materials back");
					this.@add("minimum lockpick chance", 5f, "", "If your chance of picking a lock is lower than this % then it cannot be attempted");
					this.@add("blunt damage 1", 0f, "combat damage", "amount of blunt damage done at lvl 1");
					this.@add("blunt damage 99", 100f, "combat damage", "amount of blunt damage done at lvl 100");
					this.@add("cut damage 1", 20f, "combat damage", "amount of cut damage done at lvl 1");
					this.@add("cut damage 99", 100f, "combat damage", "amount of cut damage done at lvl 100");
					this.@add("bow damage 1", 1f, "combat damage", "multiplier for the crossbow damage done at 0 grade");
					this.@add("bow damage 99", 1f, "combat damage", "multiplier for the crossbow damage done at highest grade");
					this.@add("pierce damage multiplier", 1f, "combat damage", "global multiplier, affects all damage calculations for this damage type");
					this.@add("unarmed damage mult", 1.5f, "combat damage", "global multiplier, affects all damage calculations for this damage type");
					this.@add("extra blood loss from bodyparts", 1f, "medical", "When bodyparts are wounded below 0 and unbandaged they cause a slow bloodloss.  This multiplies it.");
					this.@add("bleed rate", 0.01f, "medical", "master volume setting for rate of blood loss from cuts:  cutDamage * this * TIME");
					this.@add("stun recovery rate", 1f, "medical", "affects rate of recovery for stun damage");
					this.@add("blood recovery rate", 0.3f, "medical", "rate that blood is regenerated by the body");
					this.@add("bleeding clot rate", 0.0005f, "medical", "speed that bleeding stops by itself, if left untreated.");
					this.@add("blunt permanent organ damage", 0.2f, "combat damage", "0-1.  Affects stun/permanent damage ratio of blunt attacks.\norganDamage = bluntDamage * thisValue;  \n organStun = bluntDamage * (1-thisValue);");
					this.@add("medic speed mult", 1f, "medic", "multiplier for medic heal rate");
					this.@add("heal rate mult", 1f, "medic", "multiplier for overall healing rate");
					this.@add("resting heal rate mult", 1f, "medic", "multiplier for healing rate when resting in a bed");
					this.@add("degeneration mult 1", 3f, "medical", "multiplier for wound degeneration/bleed rate at toughness level 1");
					this.@add("degeneration mult 99", 0.5f, "medical", "multiplier for wound degeneration/bleed rate at toughness level 99");
					this.@add("knockout mult 1", 3f, "medical", "multiplier for KO time at toughness level 1");
					this.@add("knockout mult 99", 3f, "medical", "multiplier for KO time at toughness level 99");
					this.@add("knockout time base", 30f, "medical", "base KO time added to the overall time ( KO=base+(damageTotal*mult); )");
					this.@add("bodypart degeneration rate", 1f, "medical", "rate that untreated injuries degenerate, leading to death");
					this.@add("max toughness ko point", 99f, "medical", "the negative HP at which a character goes into a coma, if toughness stat is 100");
					this.@add("min toughness ko point", 10f, "medical", "the negative HP at which a character goes into a coma, if toughness stat is 0");
					this.@add("XP rate medic 1", 10f, "medic", "nuber of seconds of usage for medical skill to increase by one point, when at level 1");
					this.@add("XP rate medic 99", 300f, "medic", "nuber of seconds of usage for medical skill to increase by one point, when at level 9");
					this.@add("medkit drain 1", 1f, "medic", "rate that medkits are used up by beginners");
					this.@add("medkit drain 99", 0.05f, "medic", "rate that medkits are used up by experts");
					this.@add("robot wear rate", 1f, "medic", "rate that robots get wear damage");
					this.@add("armor price mult", 1f, "prices", "");
					this.@add("sword price mult", 1f, "prices", "");
					this.@add("clothing price mult", 1f, "prices", "");
					this.@add("loot price mult GEAR", 0.15f, "prices", "mult the price player gets for selling armour & weapons as loot");
					this.@add("loot price mult", 0.4f, "prices", "mult the price player gets for selling regular items as loot (stolen, or non-trade items)");
					this.@add("loot price mult player armour", 1f, "prices", "price multiplier for the player selling armour that he crafted himself");
					this.@add("loot price mult player weapons", 0.5f, "prices", "price multiplier for the player selling weapons that he crafted himself");
					this.@add("trade price mult", 1f, "prices", "");
					this.@add("crossbow price mult", 1f, "prices", "");
					this.@add("robotics price mult", 1f, "prices", "");
					this.@add("trade profit margins", 0.5f, "prices", "0.5 means +/- a 0.5 multiplier.  So 0.5 means prices will range from 50% to 150%");
					this.@add("global price mult", 1f, "prices", "");
					this.@add("blueprint price mult", 1.5f, "prices", "just affects armour blueprints");
					this.@add("skill diff xp 2x bonus", 10f, "xp", "if we are this many skill levels below our opponent, we have a 2x bonus to xp.  Half this many levels would be 1.5x, and so on.  This scales, all the way up to about 6x.  ");
					this.@add("skill diff xp 0x penalty", 20f, "xp", "if we are this many skill levels above our opponent, we have a 0x bonus to xp (never goes below 0.1x though).");
					this.@add("max num attack slots", 2, "combat balance", "max num attackers at one time [1-5].  higher number means combat looks better and characters don't wait around in battles, but also means the outcome is more dependant on numbers than character skill.");
					this.@add("min strength xp mult", 0.05f, "xp", "min strength xp rate");
					this.@add("weight strength diff 1x", 20, "xp", "the diff (weapon weight minus strength) for strength xp rate to equal 1.0x");
					this.@add("weight strength diff max", 1.5f, "xp", "the max strength xp rate");
					this.@add("xp rate strength", 1f, "xp", "global multiplier for strength xp rate from combat");
					this.@add("xp rate strength from walking", 1f, "xp", "global multiplier for strength xp gain when walking with heavy encumbrance");
					this.@add("xp rate athletics", 1f, "xp", "global multiplier for athletics xp rate from running around");
					this.@add("xp rate toughness", 1.33f, "xp", "global multiplier for toughness XP rate");
					this.@add("encumbrance base", 10f, "weight", "How much weight we can carry if we have a strength of 0");
					this.@add("carry weight mult", 1.2f, "weight", "general multiplier for how much weight chars can carry.  ");
					this.@add("weapon inventory weight mult", 0.5f, "weight", "general multiplier for how much heavy weapons weigh.  Weapon weight is generated based on blunt damage.  This changes that resulting inventory weight without changing its damage or speed in any way.");
					this.@add("carry person weight", 40f, "weight", "Weight of a person when carrying them");
					this.@add("encumbrance hunger rate", 0.7f, "hunger", "the multiplier added to your hunger rate at 100% encumbrance.  0=no effect, 1=double hunger speed");
					this.@add("starvation time", 24f, "hunger", "number of in-game hours it takes for a character to lose 100 points of hunger.  So this number x3 is the time from full to death");
					this.@add("fed recovery rate mult", 4f, "hunger", "rate of hunger recovery when fed, in relation to rate of starvation");
					this.@add("bed hunger rate", 0.4f, "hunger", "multiplier for hunger rate when sleeping");
					this.@add("food quality mult", 1f, "hunger", "global multiplier for the nutrition values of all food items");
					this.@add("food price mult", 1f, "prices", "global multiplier for prices of all food/crops");
					this.@add("attack chance factor", 0.05f, "combat balance", "0-1.  eg for an attack skill difference of 10 (+1 added as base):  1.0 = 11x chance of attack,  0.5 = 6x,  0.1 = 2x,  0.05 = 1.5x ");
					this.@add("damage multiplier", 8f, "Global balance", "global multiplier, affects all damage calculations");
					this.@add("exp gain multiplier", 3f, "xp", "global multiplier, affects rate of experience gain");
					this.@add("base block chance", 80f, "block success", "0-100.  Chance of a successful block, if skills (meleeAttack vs meleeDefence) are matched");
					this.@add("block chance reduction per 10levels", 10f, "block success", "0-100.  if our block skill is weaker by 10pts, base chance is reduced by this much");
					this.@add("block chance increase per 10levels", 5f, "block success", "0-100.  if our block skill is stronger by 10pts, base chance is increased by this much");
					this.@add("production speed", 1f, "GAME SPEEDS", "Multiplier for overall production speeds");
					this.@add("prison time", 1f, "GAME SPEEDS", "Multiplier for time you have to spend in prison for your crimes");
					this.@add("research level increase rate", 1.25f, "GAME SPEEDS", "Multiplier for how much research time increases for each tech level.");
					this.@add("research rate", 1f, "GAME SPEEDS", "Multiplier for overall research TIMES, so <1 is faster");
					this.@add("build speed", 1f, "GAME SPEEDS", "Multiplier for overall construction rate.");
					this.@add("animation blend rate", 4f, "settings", "Affects speed of blending between animations.  1 is very slow, more is faster.  Default is around 4");
					this.@add("sunrise", 6f, "Weather", "Sunrise time [0-24]");
					this.@add("sunset", 20f, "Weather", "Sunset hour [0-24]");
					this.@add("latitude", 0f, "Weather", "Latitude for sun angle [-90 - 90]");
					this.@add("exposure key", 0.4f, "Lighting", "HDR Exposure key");
					this.@add("exposure min", 0.8f, "Lighting", "Minimum brightness exposure. Higher values makes the world appear darker.");
					this.@add("exposure max", 0.95f, "Lighting", "Maximum brightness exposure. Lower values can make the world appear washed out in bright areas.");
					this.@add("night darkness", 1f, "Lighting", "How dark it is at night [0-1]\nMultiplier for 'exposure min'");
					this.@add("sun brightness", 0.2f, "Lighting", "Global brightness multipler for direct sunlight");
					this.@add("ambient brightness", 0.65f, "Lighting", "Global brightness multipler for ambient lighting");
					this.@add("max squad size", 20, "faction", "Sets the maximum amount of characters per squad");
					this.@add("max squads", 10, "faction", "Sets the maximum amount of squads in a faction");
					this.@add("max faction size", 100, "faction", "Sets the maximum amount of characters in a faction. Maximum 256.");
					this.@add("days per year", 100, "Time", "Sets the amount of days in a year");
					this.@add("appearance random deviation percentage", 0.5f, "Appearance", "Sets the standard deviation from the center of the range when calculating a random appearance for the character.\nThe value represents the percentage of the total range (0.0 = min, 1.0 = max).");
				}
				if (t == itemType.FOLIAGE_MESH)
				{
					this.addFileName("collision", "", string.Concat("xml|*", str, "|phs|*.phs"), "collision", "The matching collision mesh (optional).  Can leave blank for grass/bushes or if you use a .phs file.  Collision will be harder on performance. Dont include any meshes if its a scythe .phs file");
					this.@add("navmesh cutter", 0f, "collision", "Radius of hole to cut out of the navmesh. Only used if no collision set.\nLess accurate than using a collision mesh but faster.");
					this.@add("walkable", false, "collision", "Collision mesh added to navmesh as a walkable surface");
					this.addFileName("mesh", "", "Mesh|*.mesh|Scythe|*.phs", "visual", "The actual mesh");
					this.addFileName("texture map", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "visual", "diffuse texture, final versions must be dds format");
					this.addFileName("normal map", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "visual", "optional normal map.  Much slower performance, only use for large features like rock formations and boulders.  final versions must be dds format");
					this.@add("tile X", 1f, "visual", "texture tiling");
					this.@add("tile Y", 1f, "visual", "texture tiling");
					this.@add("grass spot", 0f, "visual", "Object adds a splodge of biome grass texture to the ground.\nValue is the radius.");
					this.@add("specular mult", 0f, "visual", "Specular multiplier (0-1)");
					this.@add("alpha threshold", 128, "visual", "Alpha threshold for main main texture if material type is FOLIAGE [0-255]");
					this.addEnum("material type", head.MapFeatureMode.UV_MAPPED, "visual", "Material mode");
					this.addFileName("texture map 2", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "visual", "second diffuse texture for dual texture modes");
					this.addFileName("normal map 2", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "visual", "second normal map for dual texture modes");
					this.addFileName("leaves mesh", "", "Mesh|*.mesh", "second mesh", "(optional) If this is a tree and you need 2 separate textures (eg solid tiled texture for the trunk, and transparent one for the leaves), then add the leaves mesh here.  It will be loaded along with the trunk.  ");
					this.addFileName("leaves texture", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "second mesh", "Texture for the leaves, will be rendered transparent and double-sided.");
					this.addFileName("leaves normal", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "second mesh", "Normal map for the leaves, will be rendered transparent and double-sided.");
					this.@add("leaves alpha threshold", 80, "second mesh", "Alpha threshold for leaves texture [0-255]");
					this.@add("max slope", 10f, "placement", "Max slope this foliage will grow on.  Its not in degrees!  Higher number means steeper slopes.");
					this.@add("min slope", 0f, "placement", "Min slope this foliage will grow on, with this you could make trees that only grow out of cliffs for example.  Its not in degrees!  Higher number means steeper slopes.");
					this.@add("wind factor", 1f, "", "If wind is on, this is the amount of sway (1.0 default)");
					this.@add("slope align", true, "placement", "Makes the mesh rotate to match the slope of the terrain, instead of just facing up.  Affects performance when moving camera,");
					this.@add("floating", false, "placement", "Mesh will appear on the water surface if created underwater.");
					this.@add("keep upright", true, "placement", "if false, the mesh will be randomly rotated.");
					this.@add("avoid towns", true, "placement", "Object is removed within town radius");
					this.@add("use accurate trace", false, "placement", "To get the correct terrain height to place the mesh, it has to do a trace.  This will make the trace more accurate, but slower.  If meshes are hovering above the ground now and then, turn this on.  Should not need it for larger objects. Give meshes long stems to avoid the hovering problem if possible, rather than turning this on.  If used, you can should compensate by reducing the LOD range.");
					this.@add("limit to grass areas", false, "placement", "if true, then this mesh will only spawn in terrain areas covered in the grass texture");
					this.@add("road avoidance", 10f, "placement", "Foliage will not be placed within this distance of a road.\nNote: Distance is from object centre");
					this.@add("clustered", false, "clustering", "activates clustering");
					this.@add("cluster num min", 3, "clustering", "number of objects in a cluster");
					this.@add("cluster num max", 10, "clustering", "number of objects in a cluster");
					this.@add("cluster radius min", 100f, "clustering", "size of a cluster");
					this.@add("cluster radius max", 400f, "clustering", "size of a cluster");
					this.@add("child cluster radius", 0f, "child foliage", "Sets the radius for placing child foliage to avoid overlapping.\nThe total radius is the child plus the parent radius.");
					this.@add("max altitude", 20000f, "placement", "Max altitude where foliage will grow.");
					this.@add("min altitude", 0f, "placement", "Max altitude where foliage will grow.");
					this.@add("vertical offset min", 0f, "placement", "here you can adjust the vertical positioning.  Set to negative number to make the mesh sink into the ground.  It is automatically adjusted by the objects scale");
					this.@add("vertical offset max", 0f, "placement", "here you can adjust the vertical positioning.  Set to negative number to make the mesh sink into the ground.  It is automatically adjusted by the objects scale");
					this.@add("min height", 1f, "size", "size is random between the min and max here.  1.0 is normal, 2.0 is double etc");
					this.@add("max height", 1f, "size", "size is random between the min and max here.  1.0 is normal, 2.0 is double etc");
					this.addList("meshes", itemType.FOLIAGE_MESH, "0", 50, "meshes", "Add a child meshes here.  These meshes will cluster around their parents.  Clustering will be forced to [true].");
					this.addList("sub grass", itemType.FOLIAGE_MESH, 50, "sub grass", "Add grass here to have it grow around the mesh.  It will use the grasses clustering settings");
					this.addList("building type", itemType.BUILDING, "100", 1, "building type", "Sets associated building data to the mesh. Use to turn the foliage object into a production building (for mines)");
					this.addList("sounds", itemType.AMBIENT_SOUND, "0,0,0", 1, "sounds", "Ambient sounds attached to this object.\nDo not over use!");
				}
				if (t == itemType.FOLIAGE_LAYER)
				{
					this.addEnum("visibility range", head.FoliageVisibilityRange.MEDIUM, "Performance", "Sets the distance at which the foliage is visible.");
					this.@add("wind", false, "big stuff", "Makes stuff sway in the wind, like trees, not rocks.");
					this.addList("meshes", itemType.FOLIAGE_MESH, "0", 50, "meshes", "Add all the meshes here if its not a grass layer.  the value sets the amount to create, the density.");
					this.addList("grass", itemType.GRASS, 50, "grass", "Add all the grass types.  The number you enter is the channel number (0-2).  Grass on the same channel will use the same coverage map and grow in the same places.  Grass will want to be kept in a separate foliage layer to other meshes, because it generally needs shorter lod range and smaller page sizes.  ");
				}
				if (t == itemType.GRASS)
				{
					this.@add("cross quads", true, "grass", "makes the grass sprites into cross shapes, makes it look thicker from all angles, but means double the number of sprites");
					this.@add("max slope", 10f, "grass", "Max slope this foliage will grow on (affects grass only).  Higher number means steeper slopes.");
					this.@add("max altitude", 20000f, "grass", "Max altitude where foliage will grow.");
					this.@add("min altitude", 0f, "grass", "Max altitude where foliage will grow.");
					this.@add("wind factor", 1f, "", "Multiplier.  How strongly the grass is affected by wind.");
					this.@add("min width", 1f, "size", "size is random between the min and max here.  1.0 is normal, 2.0 is double etc");
					this.@add("min height", 1f, "size", "size is random between the min and max here.  1.0 is normal, 2.0 is double etc");
					this.@add("max width", 1f, "size", "size is random between the min and max here.  1.0 is normal, 2.0 is double etc");
					this.@add("max height", 1f, "size", "size is random between the min and max here.  1.0 is normal, 2.0 is double etc");
					this.@add("density", 1f, "grass", "multiplier.  Overall density of the grass");
					this.@add("blackout", true, "distribution 2", "generates a second noise map and subtracts it from the main one, erasing random areas.  makes the grass patches into more isolated blobs");
					this.@add("blackout noise scale", 10f, "distribution 2", "scales the whole blackout noisemap.  Its good for this to be a different number to the other noisemap scale, because it will make it all less uniform looking.  smaller number makes the patches smaller");
					this.@add("blackout zero cutoff", 0.57f, "distribution 2", "after noise map is generated, any pixels (ranging 0 to 1) below this number will be set to zero (higher number will make smaller, narrow patches, lower number will make bigger ones).  NOTE:  you only need to adjust this a very small amount, like ranging from 0.4 to 0.6.");
					this.@add("noise scale", 1.5f, "distribution", "scales the whole thing, smaller number makes the patches smaller");
					this.@add("zero cutoff", 0.5f, "distribution", "after noise map is generated, any pixels (ranging 0 to 1) below this number will be set to zero (higher number will make smaller, narrow patches, lower number will make bigger ones).  NOTE:  you only need to adjust this a very small amount, like ranging from 0.4 to 0.6.");
					this.@add("brightness boost", 3f, "distribution", "after noise map generation and zero cutoff, all pixels are multiplied by this number.  A higher number than 1.0 will make the patches more dense and solid around the edges.  It makes them harder.");
					this.@add("cap", 1f, "distribution", "sets the max a pixel can be.  Normally 1.0, but you can lower it slightly to reduce the overall density");
					this.addFileName("grass sprite", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "grass", "The grass sprite, if this is grass");
					this.addFileName("color map", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "grass", "used mainly for grass.  This color map will colorise the grass, to help create different patches of color and variation.");
					this.addList("child grass", itemType.GRASS, 50, "child grass", "Add another grass type to mix in, it will share the same coverage area.");
				}
				if (t == itemType.BIOME_GROUP)
				{
					this.addList("homeless spawns", itemType.SQUAD_TEMPLATE, "1", 99, "", "Spawns these squads around the biome.  val1 is the relative chance of being spawned");
					this.addList("nests", itemType.TOWN, "100", 99, "", "Spawns these nests around the biome.  val1 is the weighted chance");
					this.addList("birds", itemType.WILDLIFE_BIRDS, "0", 50, "wildlife", "Value is the average number spawned per zone. Set to 0 to allow creatures spawned in adjacent biomes to enter.");
					this.@add("homeless spawn amount", 1f, "", "how many homeless squads spawn in the area.  number is approximately squads per zone");
					this.@add("num nests", 6, "nests", "target number of nests to spawn in the biome");
					this.@add("nests at fixed markers only", false, "nests", "if true then this biome only places nests at pre-placed TOWN_NEST_MARKER positions");
					this.addList("faction gen", itemType.FACTION_TEMPLATE, "1", 99, "", "For procedural faction generation at game start. Value is the number of factions to make");
					this.addColor("index", 16777215, "General", "Color index in areas map");
					this.addEnum("music", head.BiomeMusic.None, "audio", "Music bank to play in this biome");
					this.addEnum("ambience", head.Ambience.Desert, "audio", "Music bank to play in this biome");
					this.addList("seasons", itemType.SEASON, "0,1", 99, "", "Seasons.\n1.- Season order.\n2.- Duration percentage (Relative other seasons duration and to a game year).");
					this.addList("resources", itemType.ENVIRONMENT_RESOURCES, "", 99, "", "resources availability");
					this.addList("arrival dialog", itemType.DIALOGUE, "", 99, "", "Triggers this dialog on the player squad when they enter the biome.  Only if >1 not-KO character in the squad");
					this.@add("weather strength multiplier min", 1f, "Weather", "Minimum strength multiplier value for season weather strength interpolation value");
					this.@add("weather strength multiplier max", 1f, "Weather", "Maximum strength multiplier value for season weather strength interpolation value");
					this.@add("acidic ground", 0f, "Weather", "0-1, makes the ground burn peoples feet if not wearing shoes");
					this.@add("acidic water", 0f, "Weather", "0-1, makes the water acidic, prevents swimming");
					this.@add("farm water usage", 1f, "resources", "Multiplier for water usage rate for farms in this biome group");
				}
				if (t == itemType.ENVIRONMENT_RESOURCES)
				{
					this.@add("water altitude min", 0, "coverage resources", "");
					this.@add("water altitude max", 500, "coverage resources", "");
					this.@add("water altitude fade", 200, "coverage resources", "");
					this.@add("farming altitude min", 0, "coverage resources", "");
					this.@add("farming altitude max", 800, "coverage resources", "");
					this.@add("farming altitude fade", 300, "coverage resources", "");
					this.@add("stone mult", 1f, "stone", "general multiplier, affects the max");
					this.@add("stone noise zoom", 1f, "stone", "affects the scale of the perlin noise coverage map.  smaller number results in smaller, more frequent patches");
					this.@add("stone noise cutoff", 0.4f, "stone", "0-1, affects the perlin noise cutoff.  Higher value will result in smaller patches, lower number means bigger patches.  Does not affect frequency of patches");
					this.@add("arid", 1f, "farming", "yield for this class of crops");
					this.@add("green", 0f, "farming", "yield for this class of crops");
					this.@add("swamp", 0f, "farming", "yield for this class of crops");
					this.@add("water mult", 0.5f, "resources", "");
					this.@add("iron mult", 1f, "resources", "");
					this.@add("farming mult", 0.5f, "resources", "");
					this.@add("carbon mult", 0f, "resources", "");
					this.@add("copper mult", 0f, "resources", "");
					this.@add("water min", 0.1f, "coverage resources", "");
					this.@add("farming min", 0f, "coverage resources", "");
				}
				if (t == itemType.BIOMES)
				{
					this.addColor("index", 0, "General", "Color index in biome map");
					this.addColor("ambient light", 16777215, "Lighting", "Ambient light colour multiplier");
					this.@add("sun brightness", 1f, "Lighting", "Sunlight brightness multiplier for this biome.");
					this.addColor("ground colour", 12623936, "General", "Average colour of the ground");
					this.@add("fade distance", 4000f, "General", "Distance until textures fade out to ground colour");
					this.@add("brightness fix", 1f, "General", "Ground texture brightness multiplier");
					this.addFileName("water normal", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "water", "Water normal map");
					this.addFileName("turbulence map", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "water", "Water turbulence map");
					this.addFileName("texture scum", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "water", "Water scum map");
					this.addFileName("texture scum normal", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "water", "Water scum normal map");
					this.addColor("water color", 8256, "water", "Water color");
					this.@add("water strength", 1f, "water", "Water normal map strength");
					this.@add("water gloss", 0.9f, "water", "Water gloss");
					this.@add("water visibility", 10f, "water", "How deep into water you can see");
					this.@add("water distortion", 120f, "water", "Water rippleyness. Around 120 is reasonable.");
					this.@add("water glow", 0f, "water", "Creepy water glowyness of doom");
					this.@add("scum scale X", 20f, "water", "Number of times to repeat texture");
					this.@add("scum scale Y", 20f, "water", "Number of times to repeat texture");
					this.@add("scum distortion", 1f, "water", "Distortion of the scum map by water normals");
					this.@add("water scale X", 20f, "water", "Number of times to repeat texture");
					this.@add("water scale Y", 20f, "water", "Number of times to repeat texture");
					this.addList("foliage", itemType.FOLIAGE_LAYER, 50, "foliage", "");
					this.addFileName("texture base", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "0 - base", "base diffuse texture, used when all other channels are < 1");
					this.addFileName("texture base normal", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "0 - base", "base normal map, used when all other channels are < 1");
					this.@add("tiling X 0", 50f, "0 - base", "Number of times to repeat texture");
					this.@add("tiling Y 0", 50f, "0 - base", "Number of times to repeat texture");
					this.@add("absorbance 0", 0.55f, "0 - base", "Base water absorbance for this layer. Determines rain wetness effect [0-1].");
					this.addFileName("texture slope", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "1 - slope", "diffuse texture for this layer");
					this.addFileName("texture slope normal", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "1 - slope", "diffuse texture for this layer");
					this.@add("tiling X 1", 70f, "1 - slope", "Number of times to repeat texture");
					this.@add("tiling Y 1", 70f, "1 - slope", "Number of times to repeat texture");
					this.@add("slope fade 1", 5f, "1 - slope", "blur range in degrees");
					this.@add("slope max 1", 90f, "1 - slope", "0-90  Slope range in degrees.  0 is flat");
					this.@add("slope min 1", 0f, "1 - slope", "0-90  Slope range in degrees.  0 is flat");
					this.@add("absorbance 1", 0.2f, "1 - slope", "Base water absorbance for this layer. Determines rain wetness effect [0-1].");
					this.addFileName("texture vertical", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "2 - cliff", "diffuse texture for this layer (vertical surfaces)");
					this.addFileName("texture vertical normal", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "2 - cliff", "diffuse texture for this layer (vertical surfaces)");
					this.@add("tiling X 2", 40f, "2 - cliff", "Number of times to repeat texture");
					this.@add("tiling Y 2", 10f, "2 - cliff", "Number of times to repeat texture");
					this.@add("slope fade 2", 5f, "2 - cliff", "blur range in degrees");
					this.@add("slope max 2", 90f, "2 - cliff", "0-90  Slope range in degrees.  0 is flat");
					this.@add("slope min 2", 0f, "2 - cliff", "0-90  Slope range in degrees.  0 is flat");
					this.@add("distort amplitude", 0f, "2 - cliff", "Amplitude of vertical texture distortion");
					this.@add("distort wavelength", 1000f, "2 - cliff", "Wavelength of vertical texture distortion");
					this.@add("overlay mult vertical", 1f, "2 - cliff", "Colour overlay strength multiplier");
					this.@add("absorbance 2", 0.5f, "2 - cliff", "Base water absorbance for this layer. Determines rain wetness effect [0-1].");
					this.addFileName("texture dirt", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "4 - dirt", "dirt texture ");
					this.addFileName("texture dirt normal", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "4 - dirt", "dirt texture ");
					this.@add("tiling X dirt", 40f, "4 - dirt", "Number of times to repeat texture");
					this.@add("tiling Y dirt", 40f, "4 - dirt", "Number of times to repeat texture");
					this.@add("overlay mult dirt", 1f, "4 - dirt", "Colour overlay strength multiplier");
					this.@add("absorbance dirt", 0.3f, "4 - dirt", "Base water absorbance for this layer. Determines rain wetness effect [0-1].");
					this.addFileName("texture road", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "5 - road", "road texture ");
					this.addFileName("texture road normal", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "5 - road", "road texture ");
					this.@add("tiling X road", 40f, "5 - road", "Number of times to repeat texture");
					this.@add("tiling Y road", 40f, "5 - road", "Number of times to repeat texture");
					this.@add("overlay mult road", 1f, "5 - road", "Colour overlay strength multiplier");
					this.@add("absorbance road", 0.25f, "5 - road", "Base water absorbance for this layer. Determines rain wetness effect [0-1].");
					this.addFileName("texture grass", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "3 - grass", "texture used for grass coverage");
					this.addFileName("texture grass normal", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "3 - grass", "texture used for grass coverage");
					this.@add("tiling X grass", 40f, "3 - grass", "Number of times to repeat texture");
					this.@add("tiling Y grass", 40f, "3 - grass", "Number of times to repeat texture");
					this.@add("slope fade grass", 5f, "3 - grass", "blur range in degrees");
					this.@add("slope max grass", 20f, "3 - grass", "0-90  Slope range in degrees.  0 is flat");
					this.@add("slope min grass", 0f, "3 - grass", "0-90  Slope range in degrees.  0 is flat");
					this.@add("overlay mult grass", 1f, "3 - grass", "Colour overlay strength multiplier");
					this.@add("absorbance grass", 0.44f, "3 - grass", "Base water absorbance for this layer. Determines rain wetness effect [0-1].");
					this.addEnum("ground type", head.GroundType.SAND, "audio", "Material the ground consists of");
					this.addEnum("slope type", head.GroundType.DIRT, "audio", "Ground type for the slope material");
					this.addEnum("grass type", head.GroundType.DIRT, "audio", "Ground type for the grass material");
					this.addEnum("dirt type", head.GroundType.DIRT, "audio", "Ground type for the dirt material");
					this.addEnum("road type", head.GroundType.CONCRETE, "audio", "Ground type for roads");
					this.addList("ground effects", itemType.EFFECT, "0,0,100", 6, "Effects", "Effect for the ground on character movement.\n1- Layer number that applies the effect. \n2- Minimum movement speed to emit the effect.\n3- Probability (0-100) of emitting the effect with each step. (100 = always)");
					this.addList("water effects", itemType.EFFECT, "0,0", 6, "Effects", "Water spash ground effects for character movenment. Only one will be selected with the highest valid values.\n1- Minimum water depth. \n2- Minimum movement speed.");
				}
				if (t == itemType.MAP_FEATURES)
				{
					this.addFileName("mesh", "", "mesh|*.mesh", "General", "Object mesh");
					this.addFileName("collision", "", string.Concat("xml|*", str, "|phs|*.phs"), "General", "Object collision data");
					this.addEnum("texture mode", head.MapFeatureMode.UV_MAPPED, "General", "How the texture is mapped to the object.\nTerrain mode uses textures from the current biome.");
					this.@add("distance", 2000f, "General", "Visibility distance for feature");
					this.@add("hidden", false, "General", "Feature is not visible. Used for markers to attach effects.");
					this.addFileName("texture map", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "Material", "Object texture map");
					this.addFileName("normal map", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "Material", "Object normal map");
					this.addFileName("property map", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "Material", "Object property map\nRed channel: Transparency\nBlue channel: Metalness");
					this.@add("tile X", 1f, "Material", "Texture tiling");
					this.@add("tile Y", 1f, "Material", "Texture tiling");
					this.@add("alpha threshold", 0, "Material", "Alpha threshold [0-255]. 0 disables alpha.");
					this.@add("local coordinates", false, "Material", "Use local coordinates instead of global coordinates for triplanar and terrain mapping");
					this.addFileName("texture map 2", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "Material", "second texture map for dual texture mode");
					this.addFileName("normal map 2", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "Material", "Object normal map for dual texture mode");
					this.addFileName("property map 2", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "Material", "Object property map for dual texture mode");
					GameData.setDesc(t, "effects", itemType.EFFECT, new GameData.vec(), new GameData.quat(), "Effects");
					this.addList("sounds", itemType.AMBIENT_SOUND, "0,0,0", 1, "sounds", "Ambient sounds attached to this feature.\nValues are offset position");
					this.addList("bird attractor", itemType.WILDLIFE_BIRDS, "1000,10", 99, "bird attractor", "This feature attracts nearby birds.\nValue 1: Radius in meters.\nValue2: Maximum birds to attract");
					GameData.addCondition(t, "texture map", "texture mode", head.MapFeatureMode.TERRAIN, false);
					GameData.addCondition(t, "normal map", "texture mode", head.MapFeatureMode.TERRAIN, false);
					GameData.addCondition(t, "texture map 2", "texture mode", new head.MapFeatureMode[] { head.MapFeatureMode.DUAL_TEXTURE, head.MapFeatureMode.DUAL_TRIPLANAR }, true);
					GameData.addCondition(t, "normal map 2", "texture mode", new head.MapFeatureMode[] { head.MapFeatureMode.DUAL_TEXTURE, head.MapFeatureMode.DUAL_TRIPLANAR }, true);
					GameData.addCondition(t, "property map 2", "texture mode", new head.MapFeatureMode[] { head.MapFeatureMode.DUAL_TEXTURE, head.MapFeatureMode.DUAL_TRIPLANAR }, true);
					GameData.addCondition(t, "local cordinates", "texture mode", new head.MapFeatureMode[] { head.MapFeatureMode.TRIPLANAR, head.MapFeatureMode.DUAL_TRIPLANAR, head.MapFeatureMode.TERRAIN }, true);
					GameData.addCondition(t, "alpha threshold", "texture mode", head.MapFeatureMode.FOLIAGE, true);
					GameData.addCondition(t, "tile X", "texture mode", head.MapFeatureMode.TERRAIN, false);
					GameData.addCondition(t, "tile Y", "texture mode", head.MapFeatureMode.TERRAIN, false);
				}
				if (t == itemType.WILDLIFE_BIRDS)
				{
					this.addFileName("mesh", "", "mesh|*.mesh", "general", "Bird mesh");
					this.addFileName("texture", "", "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp", "general", "Bird texure");
					this.@add("distance", 500000f, "general", "Maximum distance from camera that birds exist");
					this.@add("speed", 500f, "movement", "Movement speed");
					this.@add("height", 800f, "movement", "Height above the ground to fly at");
					this.@add("ground height", 1000f, "movement", "Target altitude to fly over. Use this to focus on mountains or shorelines etc");
					this.@add("ground strength", 1f, "movement", "How much ground height affects movement [0-1]");
					this.@add("contour strength", 1f, "movement", "How much birds follow contours, or travel straight [0-1]");
					this.@add("flocking", true, "movement", "Do these birds form flocks?");
					this.@add("circling", false, "movement", "Do these birds circle?");
					this.@add("spawn height min", 0f, "spawn", "Minimum ground height to spawn at");
					this.@add("spawn height max", 4000f, "spawn", "Maximum ground height to spawn at");
					this.@add("spawn count", 1, "spawn", "Number of birds to spawn at each point. Use for spawning flocks.");
					this.@add("scale min", 1f, "general", "Minimum scale multiplier");
					this.@add("scale max", 1f, "general", "Maximum scale multiplier");
				}
				if (t == itemType.LIGHT)
				{
					this.@add("brightness", 1f, "general", "The brightness of the light source");
					this.@add("variance", 0f, "general", "Brightness varies by up to this amount per light instance");
					this.@add("radius", 100f, "general", "Light falloff radius.");
					this.addColor("diffuse", 16777215, "general", "Light diffuse colour");
					this.addEnum("type", head.LightType.POINT, "general", "Type of light source");
					this.addEnum("effect", head.LightEffect.NONE, "general", "Dynamic effect for the light");
					this.@add("inner", 45f, "spotlight", "Spotlight inner cone angle");
					this.@add("outer", 40f, "spotlight", "Spotlight outer cone angle");
					this.@add("falloff", 1f, "spotlight", "Spotlight falloff curve between inner and outer cones.\n1 is linear, >1 for soft edges, <1 for hard edges.\nMust be greater than 0.");
					this.@add("landscape", false, "shadows", "Does the landscape cast shadows");
					this.@add("buildings", false, "shadows", "Do the buildings cast shadows");
					this.@add("characters", false, "shadows", "Does characters cast shadows");
				}
				if (t == itemType.WEATHER)
				{
					this.addList("effects", itemType.EFFECT, "1,60,600", 1, "Effects", "Weather visual/game effects.\n1- Maximum count of effects that there can be active at the same time. (0 = infinite). \n2.- Minimum respawn time (in real seconds). \n3.- Maximum respawn time (in real seconds). (If min and max equals 0, the maximum count it's spawned all at once)");
					this.@add("wind speed min", 0f, "Wind", "Minimum wind speed (unit/s)");
					this.@add("wind speed max", 30f, "Wind", "Maximum wind speed (unit/s)");
					this.@add("wind update time", 360, "Wind", "Time in minutes to change wind speed and direction");
					this.@add("wind update limit", 20, "Wind", "The limit variation for the wind update [0-360]");
					this.@add("fog enabled", false, "Fog", "Enables fog");
					this.@add("fog distance min", 0f, "Fog", "Fog visibility minimum distance. (Based on the wind speed)");
					this.@add("fog distance max", 0f, "Fog", "Fog visibility maximum distance. (Based on the wind speed)");
					this.@add("fog wind min", 0f, "Fog", "Wind speed for minimum fog density.");
					this.@add("fog wind max", 0f, "Fog", "Wind speed for maximum fog density.");
					this.addColor("fog color", 16777215, "Fog", "Fog color.");
					this.@add("clouds density", 0f, "Sky", "Density/coverage of clouds on the sky.");
					this.addColor("sky color mult", 16777215, "Sky", "Sky color multiplier.");
					this.@add("effect strength min", 1f, "General", "Minimum effect strength");
					this.@add("effect strength max", 1f, "General", "Maximum effect strength");
					this.@add("rain intensity", 0f, "Audio", "Intensity of the rain sounds [0-100]");
					this.@add("wind intensity", 0f, "Audio", "Intensity of the wind sounds at maximum strength [0-100]");
					this.@add("dust", 0f, "Dust", "Dust effect [0-1]");
					this.@add("dust slope", 0f, "Dust", "Slope steepness that dust sticks to. [0-1]");
					this.@add("dust inside", 0.33f, "Dust", "Multiplier for dust effect inside buildings");
					this.@add("wetness", 0f, "General", "How wet everything gets [0-1]");
					this.@add("heat haze", 0f, "General", "Strength of heat haze effect [0-1]");
					this.addEnum("affect type", WeatherAffecting.WA_NONE, "affects characters", "does this weather cause any kind of direct affect to characters");
					this.@add("affect strength", 0.5f, "affects characters", "0-1");
					this.@add("start time", 0f, "Time of Day", "Limit this weather to specific times during the day (hour)");
					this.@add("end time", 0f, "Time of Day", "Limit this weather to specific times during the day (hour)");
				}
				if (t == itemType.SEASON)
				{
					this.addList("weathers", itemType.WEATHER, "1,1,120", 24, "Weather", "Weather list. \n1.- Probability of occurrence \n2.- Minimum duration in minutes \n3.- Maximum duration in minutes");
					this.@add("weather strength limit min", 0f, "Weather", "Minimum interpolation value for the weather strength [0-1]");
					this.@add("weather strength limit max", 1f, "Weather", "Maximum interpolation value for the weather strength [0-1]");
					this.addColor("sunlight color", 16777215, "Sun", "Color of the sun");
				}
				if (t == itemType.EFFECT)
				{
					this.@add("particle system", string.Empty, "General", "Particle system name");
					this.@add("wind affected", false, "Weather", "Indicates whether the particle is affected by wind");
					this.@add("wind speed mult", 1f, "Weather", "Wind speed multiplier for particles [0-1]");
					this.@add("min wind span rate", 0f, "Weather", "Sets the minimum wind speed needed for spawning any particles");
					this.@add("max wind span rate", 0f, "Weather", "Sets the maximum wind speed needed for spawning the full amount of particles.");
					this.@add("wind direction emission", false, "Weather", "Set whether the initial direction of the emitted particle (in X,Z) is based on the wind direction");
					this.addColor("colour multiplier", 16777215, "Weather", "Colour multiplier. (For weather effects only)");
					this.@add("sky colour multiplier", 1f, "Weather", "Sky colour multiplier. (For weather effects only)");
					this.@add("min altitude", 200f, "Position", "Sets the minimum altitude from which the system is spawned. (For weather effects only)");
					this.@add("max altitude", 100000f, "Position", "Sets the maximum altitude from which the system is spawned. (For weather effects only)");
					this.@add("min slope", 0f, "Position", "Sets the minimum slope from which the system is spawned. [0, 1] (For weather effects only)");
					this.@add("max slope", 1f, "Position", "Sets the maximum slope from which the system is spawned. [0, 1] (For weather effects only)");
					this.@add("min time to live", 0f, "Time", "Sets the minimum time to live of the particle in seconds. (For weather effects only)");
					this.@add("max time to live", 30f, "Time", "Sets the maximum time to live of the particle in seconds. (For weather effects only)");
					this.@add("particle fade out delay", 0f, "Blending", "(Particle) Delay time in seconds before starting the fade out.");
					this.@add("fog fade in duration", 0f, "Blending", "(Fog) Fade in duration in seconds. (0 = Disabled)");
					this.@add("fog fade out duration", 0f, "Blending", "(Fog) Fade out duration in seconds. (0 = Disabled)");
					this.@add("light fade in delay", 0f, "Blending", "(Lights) Delay time in seconds before starting the fade in.");
					this.@add("light fade out delay", 0f, "Blending", "(Lights) Delay time in seconds before starting the fade out.");
					this.@add("light fade in duration", 0f, "Blending", "(Lights) Fade in duration in seconds. (0 = Disabled)");
					this.@add("light fade out duration", 0f, "Blending", "(Lights) Fade in duration in seconds. (0 = Disabled)");
					this.addEnum("type", head.EffectType.NONE, "General", "Effect behavior in game (for the weather only). This determines how it's spawned and how it affects objects.\nNONE: Has no special behavior.\nCAMERA: The effect is camera based; is positioned in front of the camera and move with it. The effect affects the whole biome.\nCAMERA_RAIN: Subtype of CAMERA.\nCAMERA_ACID_RAIN: Subtype of CAMERA that does acid damage.\nPOINT: Randomly spawns the effect in the area.\nPOINT_LIGHTING: Subtype of POINT but it's spawn position is based on the amount of metal and affects the objects only once.\nGLOBAL: Similar to CAMERA but its spawn on the camera center on the ground. The effect affects the whole biome.\nWANDERING: Like POINT, it randomly spawns in the area but moves around.\nWANDERING_STORM: Subtype of WANDERING but stronger.\nWANDERING_GAS: Subtype of WANDERING but it does more damage.\nGLOBAL_POINT: A global effect that spawns as a point but the amount is on the camera area rather than the whole biome (MUST HAVE A MAX TIME TO LIVE!!!)");
					this.addEnum("affect type", WeatherAffecting.WA_NONE, "General", "adds any kind of collision damage affecting things like poison gas clouds");
					this.@add("effect strength min", 1f, "General", "Minimum effect strength (eg gas/acid damage)");
					this.@add("effect strength max", 1f, "General", "Maximum effect strength (eg gas/acid damage)");
					this.@add("effect radius", 0f, "General", "radius of the status affection, eg for gas clouds.  0 will be auto-radius, and won't be very accurate");
					this.@add("wandering speed", 10f, "General", "Movement speed for the effect (only if type is a movable type).");
					this.@add("ground colour", false, "General", "Multiply particle base colour by biome ground colour.");
					this.@add("maximum view distance", 0f, "View", "Maximum distance from an active area from which the effect is visible. (Only when is on an inactive area)");
					this.addList("fog volumes", itemType.EFFECT_FOG_VOLUME, 0, "fog volumes", "");
					this.addList("sound", itemType.AMBIENT_SOUND, 1, "attachments", "Emitted sound effect");
					GameData.setDesc(itemType.EFFECT, "lights", itemType.LIGHT, new GameData.vec(), new GameData.quat(), "Light instances attached to this effect.");
				}
				if (t == itemType.EFFECT_FOG_VOLUME)
				{
					this.addEnum("type", head.EffectVolumeType.CYLINDER, "General", "Geometric type of the volume");
					this.@add("additive colour", false, "Colouring", "Additive colouring.");
					this.addColor("colour", 16777215, "Colouring", "Colour of the volume.");
					this.@add("alpha", 1f, "Colouring", "Alpha of the volume");
					this.@add("radius", 1f, "General", "Radius of the volume");
					this.@add("distance", 1f, "General", "Density distance of the volume");
					this.@add("position x", 0f, "Position", "Offset position in X axis");
					this.@add("position y", 0f, "Position", "Offset position in Y axis");
					this.@add("position z", 0f, "Position", "Offset position in Z axis");
					this.@add("position 2 x", 0f, "Position", "Offset position of other side in X axis (only for cylinder)");
					this.@add("position 2 y", 0f, "Position", "Offset position of other side in Y axis (only for cylinder)");
					this.@add("position 2 z", 0f, "Position", "Offset position of other side in Z axis (only for cylinder)");
					this.@add("ground movement", false, "Movement", "Sets whether, when the volume moves, it maintains its altitude or is recalculated from the ground position.");
				}
				if (t == itemType.ARTIFACTS)
				{
					this.addList("items", itemType.ITEM, "1", 0, "items", "List of common items");
					this.addList("armours", itemType.ARMOUR, "1", 0, "armours", "List of armours of the 2nd highest quality (Specialist)");
					this.addList("armours hq", itemType.ARMOUR, "1", 0, "armours", "List of armours of the highest quality (Masterwork)");
					this.addList("weapons", itemType.MATERIAL_SPECS_WEAPON, "1", 0, "weapons", "List of weapons modes. Value 1 = the amount of each weapon spawn that the model has.");
					this.addList("crossbows", itemType.CROSSBOW, "1,1", 99, "weapons", "List of crossbows.\nValue 1: number of masterwork items\nValue 2: Number of specialist items");
					this.addList("robotics", itemType.LIMB_REPLACEMENT, "1,1", 99, "items", "List of robot limbs.\nValue 1: number of masterwork items\nValue 2: Number of specialist items");
				}
				if (t == itemType.BUILDINGS_SWAP)
				{
					this.addList("to replace", itemType.BUILDING, 0, "replacement", "List of buildings to be replaced.");
					this.addList("replace with", itemType.BUILDING, "100", 0, "replacement", "List of buildings to replace with. Val0 = chance of replacing using that building.");
				}
				if (t == itemType.ANIMATION_EVENT)
				{
					this.@add("event", "", "Audio", "Audio event name");
					this.@add("override state", "", "Audio", "State key to be overridden");
					this.@add("override value", "", "Audio", "State value to set");
					this.@add("stop", "", "Audio", "Audio event to be called when the this animation stops.");
					this.@add("ground effect", false, "Effects", "Ground impact effect spawned: dust or splash depending on location. The effects are listed in the biome data.");
					this.@add("bone", "", "Effects", "Character bone for the position offset of the ground effect.");
					this.addEnum("other", head.AnimationEvent.NONE, "Other", "Additional function this event triggers.");
				}
				if (t == itemType.AMBIENT_SOUND)
				{
					this.@add("emitter", "", "Audio", "Emitter type name");
					this.@add("intensity", 0f, "Audio", "Base intensity value [0-1]\n");
					this.@add("efficiency multiplier", 0f, "Audio", "Amount machine efficiency affects sound [0-1]. This is added to the base intensity value.");
					this.@add("needs power", false, "Audio", "Does this emitter turn off if its parent building has no power or is turned off");
					this.@add("chance", 1f, "Audio", "Probability that this sound will manifest [0-1]");
					this.@add("cutoff", 50f, "Audio", "Sound cutoff distance (meters).\nAttenuation distances are set up in the audio system, this is just to stop hundreds of silent audio objects causing problems");
				}
			}

			public void @add(string n, string v, string category, string d)
			{
				GameData.setDesc(this.type, category, n, v, d);
			}

			public void @add(string n, int v, string category, string d)
			{
				GameData.setDesc(this.type, category, n, v, d);
			}

			public void @add(string n, float v, string category, string d)
			{
				GameData.setDesc(this.type, category, n, v, d);
			}

			public void @add(string n, bool v, string category, string d)
			{
				GameData.setDesc(this.type, category, n, v, d);
			}

			public void addColor(string n, int c, string category, string d)
			{
				Color color = Color.FromArgb(255, Color.FromArgb(c));
				GameData.setDesc(this.type, category, n, color, d);
			}

			public void addEnum(string n, object v, string category, string d)
			{
				GameData.setDesc(this.type, category, n, v, d);
			}

			public void addEnumLooping(string n, object v, string category, string d)
			{
				GameData.setDesc(this.type, category, n, v, d).limit = 9999;
			}

			public void addFileName(string n, string v, string filetype, string category, string desc)
			{
				GameData.setDesc(this.type, category, n, filetype, new GameData.File(v), desc);
			}

			public void addList(string n, itemType type, int maxSize, string category, string d)
			{
				this.addList(n, type, "", maxSize, category, d);
			}

			public void addList(string n, itemType type, string defaultValues, int maxSize, string category, string d)
			{
				string[] strArrays = defaultValues.Split(new char[] { ',' });
				int num = (defaultValues == "" ? 0 : (int)strArrays.Length);
				GameData.TripleInt tripleInt = new GameData.TripleInt(0, 0, 0);
				if (num > 0)
				{
					int.TryParse(strArrays[0], out tripleInt.v0);
				}
				if (num > 1)
				{
					int.TryParse(strArrays[1], out tripleInt.v1);
				}
				if (num > 2)
				{
					int.TryParse(strArrays[2], out tripleInt.v2);
				}
				GameData.setDesc(this.type, n, type, tripleInt, num, d);
			}

			public void addStringLooping(string n, string v, string category, string d)
			{
				GameData.setDesc(this.type, category, n, v, d).limit = 9999;
			}

			public void addText(string n, string v, string category, string d)
			{
				GameData.setDesc(this.type, category, n, v, d).flags = 16;
			}
		}

		public enum PartMapColours
		{
			White,
			Red,
			Green,
			Blue,
			Yellow,
			Magenta,
			Cyan,
			Orange,
			Purple
		}

		public enum PathMode
		{
			IGNORE,
			PROJECTED,
			OBSTACLE,
			WALKABLE
		}

		public enum RaceSound
		{
			HUMAN,
			SHEK,
			HIVE,
			SKELETON,
			BEAK,
			BULL,
			CAGE,
			DOG,
			DUCK,
			GARRU,
			GOAT,
			GORILLO,
			IRONSPIDER,
			LEVIATHAN,
			PACK,
			SPIDER,
			TURTLE
		}

		public enum RagdollPart
		{
			NONE = 0,
			WHOLE = 1,
			RIGHT_ARM = 2,
			LEFT_ARM = 4,
			HEAD = 8,
			RIGHT_LEG = 16,
			LEFT_LEG = 32,
			CARRY_MODE = 2048
		}

		private enum RobotLimb
		{
			LEFT_ARM,
			RIGHT_ARM,
			LEFT_LEG,
			RIGHT_LEG,
			NULL_LIMB
		}

		public enum Sounds
		{
			Footstep,
			Attack
		}

		private enum SquadFormation
		{
			RANDOM,
			CARAVAN,
			MILITARY
		}

		public enum subCategory
		{
			NONE_CATEGORY,
			FURNITURE
		}

		public enum TaskEndEvent
		{
			TEE_NOTHING,
			TEE_REMOVE_MINE_ONLY,
			TEE_REMOVE_WHOLE_SQUADS
		}

		public enum TaskTargetType
		{
			TARGET_SPECIFIC,
			TARGET_SELF,
			TARGET_LEADER,
			TARGET_SQUAD_MISSION,
			TARGET_HOME_GATE
		}

		public enum taskType
		{
			NULL_TASK,
			MOVE_ON_FREE_WILL,
			BUILD,
			PICKUP,
			MELEE_ATTACK,
			FOCUSED_MELEE_ATTACK,
			EQUIP_WEAPON,
			UNEQUIP_WEAPON,
			FIND_WEAPON,
			CHOOSE_ENEMY_AND_ATTACK,
			CHOOSE_ATTACKER_OF_ALLY,
			ATTACK_CHARACTERS_ATTACKER,
			PLAYER_TALK_TO,
			ATTACK_ATTACKERS_OF,
			IDLE,
			PROTECT_ALLIES,
			ATTACK_ENEMIES,
			PROTECTION,
			RAID_TOWN,
			GO_HOMEBUILDING,
			STAND_AT_SHOPKEEPER_NODE,
			ATTACK_ENEMIES_AND_NEUTRALS,
			PATROL,
			ATTACK_TOWN,
			WANDERER,
			FIRST_AID_ORDER,
			LOOT_TARGET,
			CROUCH,
			STAND_UP,
			MOVE_CUS_ORDERED,
			HOLD_POSITION,
			STAY_CLOSE_TO_TARGET,
			SELF_PRESERVATION,
			QUELL_AGGRESSION,
			ATTACK_TROUBLE_MAKERS,
			RUN_AWAY,
			PATROL_TOWN,
			WANDER_TOWN,
			STAND_AT_GUARD_NODE_HOMEBUILDING,
			WANDERING_TRADER,
			GET_NEAR_TO,
			ATTACK_ENEMIES_OF_MY_SLAVEMASTER,
			NOT_BE_UNARMED,
			STAY_IN_HOME,
			FOLLOW_PLAYER_ORDER,
			BODYGUARD,
			CHASE,
			STAND_AT_GENERAL_NODE,
			STAND_AT_DEFENSIVE_NODE,
			STAND_AT_BUILDING_GUARD_NODE,
			STAND_AT_BUILDING_DEFENSIVE_NODE,
			STAND_AT_NODE,
			__aoeu__,
			TRAVEL_TO_TARGET_TOWN,
			REST,
			RECRUIT_AT_JOBCENTER,
			SWITCH_FOLLOW_ME_MODE_ON,
			JOB_REPAIR_ROBOT,
			JOB_MEDIC,
			GET_READY_FOR_ACTION,
			FIRST_AID_ROBOT,
			UNPROVOKED_FOCUSED_MELEE_ATTACK,
			STAND_STILL,
			SQUAD_WAIT_FOR_ME,
			MAKE_TARGET_STAND_STILL,
			GET_UP,
			FORCE_GET_UP,
			MOVE_ON_FREE_WILL_FAST,
			LIFT_PERSON,
			PUT_DOWN_OBJECT,
			PUT_DOWN_CHARACTER_IN_BED,
			ADD_MATERIALS_TO_BUILDING,
			OPEN_DOOR,
			CLOSE_DOOR,
			OPEN_DOOR_HERE,
			CLOSE_DOOR_HERE,
			PICK_LOCK,
			LOCK_DOOR,
			UNLOCK_DOOR,
			LOCK_DOOR_HERE,
			UNLOCK_DOOR_HERE,
			BASH_DOOR,
			MOVE_TO_BUILDING_DOOR,
			MOVE_TO_CURRENT_LOCATION_BUILDING_DOOR,
			OPEN_DOOR_FOR_CURRENT_LOCATION,
			OPEN_DOOR_FOR_DESTINATION,
			OPEN_UP_SHOP_DOORS,
			OPERATE_MACHINERY,
			DELIVER_RESOURCES,
			JOB_KEEP_EVERYTHING_RUNNING,
			UNJAM_ALL_MACHINES,
			UNJAM_MACHINE,
			COLLECT_OUTPUT_RESOURCE,
			FILL_MACHINE,
			WANT_TO_FILL_MACHINE,
			REPAIR,
			DISMANTLE,
			USE_TRAINING_DUMMY,
			USE_BED,
			PUT_SOMEONE_IN_BED,
			GET_PUT_IN_BED,
			DEFEAT_SQUAD,
			SEEK_AND_TALK_AND_SEND_SIGNAL,
			MAKE_ANNOUNCEMENT,
			ALWAYS_IMPOSSIBLE_TASK,
			FIND_AND_RESCUE,
			FIND_BED_AND_PUT_IN,
			USE_CAGE,
			PUT_IN_CAGE,
			KNOCKOUT_PRISONER,
			RELEASE_PRISONER,
			BREAKOUT_PRISONER,
			FIND_CAGE_AND_PUT_IN,
			EMPTY_MACHINE_OUTPUTS,
			GET_RID_OF_RESOURCES_IN_MY_INVENTORY,
			FIND_SOME_BUILDING_MATERIALS,
			GET_OUT_OF_BED,
			FIND_A_SHOP,
			SHOPPING,
			BUY_SHIT,
			MOVE_INSIDE_BUILDING,
			MOVE_TO_FORTIFICATION_GATE,
			OPEN_FORTIFICATION_GATE,
			BASH_GATE,
			OPERATE_STORAGE,
			JOB_BUILDER,
			TALKTO_NEAREST_PLAYER_CHARACTER,
			RUN_AWAY_HOMETOWN,
			RETREAT_HOMETOWN,
			MAKE_ANNOUNCEMENT_FAST,
			TRAVEL_TO_TARGET_TOWN_FAST,
			LOOT_FOOD_AND_STUFF,
			FIND_AND_KIDNAP,
			GET_OUT_OF_CAGE_LEGIT,
			KILL_CAGE_OCCUPANT,
			KILL_A_RANDOM_CAGE_OCCUPANT,
			FEED_CORPSE_INTO_MACHINE,
			DEAD_GUYS_GO_IN_THE_POT,
			FIND_A_DEAD_GUY,
			EAT_A_RANDOM_CAGE_OCCUPANT,
			UNLOCK_DOOR_PLAYER_ORDER,
			FOLLOW_SQUADLEADER,
			FIND_AND_RESCUE_LEADER,
			PROTECT_OWN_SQUAD,
			TERRITORIAL_AGGRESSION_BUT_DONT_LEAVE_HOME,
			GET_RE_EQUIPPED,
			USE_TURRET,
			STUMBLE_TASK_FORCED,
			FIND_AND_RESCUE_IF_THERES_BEDS,
			MAN_A_TURRET,
			PROSPECTING,
			EMPTYING_MACHINE,
			OPERATE_AUTOMATIC_MACHINERY,
			GO_HOME_AND_GO_TO_BED,
			GO_TO_THE_BAR_AND_DRINK,
			LOCK_ALL_MY_DOORS,
			ENTER_BUILDING,
			STAND_AT_GUARD_NODE_HOMETOWN_OUTSIDE,
			SHOO_STRANGERS_OUT_OF_MY_BUILDING,
			SEND_DIALOGUE_SIGNAL,
			SEND_DIALOGUE_SIGNAL_REPEAT,
			SEND_DIALOGUE_SIGNAL_WITHOUT_MOVING,
			LOCK_DOOR_FROM_INSIDE,
			MOVE_TO_BUILDING_DOOR_INSIDEPOS,
			FOLLOW_WHILE_TALKING,
			TOWN_STALKER,
			CHAIN_TARGET,
			CAPTURE_NEW_SLAVES,
			CARRY_WOUNDED_SLAVES,
			PUT_DOWN_CARRIED_DUDE_IF_THEY_CAN_WALK,
			LIFT_OBJECT_BUT_HEAL_FIRST,
			FOLLOW_SLAVEMASTER,
			SLAVE_GET_IN_MY_MASTERS_CAGE,
			GATHER_SLAVES_FROM_CAGES,
			GET_SLAVE,
			SLEEP_ON_FLOOR,
			HUNTING_BLOODSMELL,
			LOOT_THE_DEAD,
			LOOT_TO_REPLACE_MISSING_WEAPON,
			HUNT_MY_THIEF,
			MAN_THE_GATE,
			STRIP_TARGETS_WEAPONS,
			PROCESS_AND_STRIP_NEW_SLAVE,
			SLAVE_WATCHING,
			PUT_LOOT_IN_STORAGE,
			CUT_SHACKLES,
			BRUTE_FORCE_SHACKLES,
			_SLAVE_OBEDIENCE,
			WORK_THE_SLAVES,
			AUTO_LABOURING_MINES,
			AUTO_LABOURING_MINES_PRETEND,
			GO_TO_NEAREST_HQ,
			GO_TO_SOMEWHERE_FOR_DELIVERING_SLAVES,
			CAPTURE_ESCAPING_SLAVES,
			GIVE_ALL_MY_SLAVES_TO,
			LOCK_ALL_THE_CAGES,
			BEAT_CAGE_OCCUPANT,
			LOCK_ALL_MY_DOORS_FROM_OUTSIDE,
			LOCK_DOOR_FROM_OUTSIDE,
			MOVE_TO_BUILDING_DOOR_OUTSIDEPOS,
			LEAVE_BUILDING,
			PICK_LOCK_ON_SHACKLES,
			TOTAL_ESCAPE,
			ARREST_TARGET,
			HUNT_BOUNTIES,
			ARREST_TARGETS_CARRIED_PERSON,
			FIND_CAGE_AND_PUT_IN_IF_BOUNTY,
			GET_OUT_OF_CAGE_ESCAPE,
			GET_OUT_OF_BED_IF_ITS_EMERGENCY,
			INVESTIGATE_ALARMS,
			INVESTIGATE_ALARMS_ALLIES_ONLY,
			POLICE_FREE_PRISONERS_WHEN_DONE,
			LOOT_STOLEN_GOODS,
			LIFT_PERSON_SNATCHING_ALLOWED,
			RELAX_IN_TOWN_PACKAGE,
			TRAVEL_TO_TARGET_PACKAGE,
			RUN_AROUND_TOWN_LOOKING_FOR_PEOPLE,
			GATHER_SLAVES_FROM_CAGES_IF_ITS_AN_EXPORT_TOWN,
			GIVE_ALL_MY_SLAVES_TO_IF_ITS_AN_IMPORT_TOWN,
			TAKE_OFF_MY_SHACKLES,
			EAT_TARGET_ALIVE,
			PRETEND_TO_OPERATE_MACHINERY,
			MAN_A_TURRET_ON_BUILDING,
			PICKUP_INTRUDERS,
			TAKE_INTRUDER_OUTSIDE,
			LIFT_PERSON_PLAYER_ORDER,
			BASH_DOOR_PLAYER_ORDER,
			MELEE_ATTACK_ANIMAL,
			STEALTH_KNOCKOUT,
			STEALTH_KILL,
			EAT_A_RANDOM_DEAD_BODY,
			EAT_CROPS,
			FIND_CROPS_TO_EAT,
			EAT_A_RANDOM_KO_BODY,
			MAN_A_TURRET_PLAYER_JOB,
			SHOOT_AT_TARGET,
			WORSHIP_TARGET,
			FOGMAN_WORSHIP_VICTIM,
			LOOT_ANIMALS_JOB,
			GO_HOME_AND_GO_TO_BED_SECURE,
			LIFT_PERSON_SNATCHING_ALLOWED_IN_TOWN_ONLY,
			LOOT_RESOURCE_ITEMS_WE_HAVE_STORAGE_FOR,
			DITCH_ALL_RESOURCES,
			AQUIRE_FOOD_AT_HOMEBASE,
			GRAB_ONE_FOOD,
			GATHER_SLAVES_FROM_CAGES_IF_FEMALE_OR_BEAST,
			KIDNAP_ORDER,
			COLLECT_OUTPUT_RESOURCE_BUILD_MATS,
			DEFEAT_SQUAD_LIMIT_CHASE_RANGE,
			SPLINT_ORDER,
			SPLINT_JOB,
			ESCAPE_KIDNAP,
			ESCAPE_KIDNAP_STR,
			FOLLOW_URGENT_ESCAPE,
			FINAL_KIDNAPPER_CAGE_JOB,
			SIT_ON_THRONE,
			GET_OUT_OF_CAGE_OPPORTUNISTIC,
			GET_OUT_OF_BED_ONCE_HEALED,
			USE_BED_ORDER,
			EAT_FOOD_ON_GROUND,
			NEW_SLAVE_PROCESSING,
			SLEEP_ON_FLOOR_FAKE_AMBUSH,
			RANGED_ATTACK,
			RANGED_ATTACK_FOCUSED,
			EQUIP_CROSSBOW,
			UNEQUIP_CROSSBOW,
			RANGED_ATTACK_FOCUSED_UNPROVOKED,
			MOVE_IN_BOW_RANGE,
			STAND_AT_GUARD_NODE_HOMEBUILDING_INDOORS_ONLY,
			HEAL_MY_LEGS,
			ASSAULT_FORTIFICATIONS_PREFER_GATES,
			ASSAULT_FORTIFICATIONS_PREFER_WALLS,
			SMASH_BUILDING,
			PICKUP_INTRUDERS_TOWN,
			TAKE_INTRUDER_OUTSIDE_TOWN,
			SIT_AROUND,
			LIBERATE_ALL_THE_PRISONERS,
			ANIMAL_FETCH_A_LIMB,
			PLAY_BECAUSE_I_HAVE_A_LIMB_IN_MOUTH,
			CHASE_ALLY_DOGS_WITH_MOUTH_LIMBS,
			RUN_AWAY_FORCED,
			FIND_CAGE_AND_PUT_DEADGUY_IN,
			EAT_A_RANDOM_CAGE_OCCUPANT_MEASURED_RATE,
			SHOO_STRANGERS_OUT_OF_MY_BUILDING_IF_PRIVATE,
			LOOT_CONTAINER,
			CUT_LOCK,
			BRUTE_FORCE_LOCK,
			BASH_DOOR_HERE,
			PROTECT_ALLIES_STAY_IN_TOWN
		}

		public enum WallSection
		{
			NORMAL,
			CONNECTOR,
			LOWER_WEDGE,
			SINGLE,
			SHORT
		}

		public enum WeaponMaterial
		{
			Metal,
			Wood
		}
	}
}