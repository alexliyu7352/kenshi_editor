using System;
using System.Drawing;

namespace forgotten_construction_set
{
	public class StateColours
	{
		public StateColours()
		{
		}

		public static Color getIntejectionColour(TalkerEnum value)
		{
			switch (value)
			{
				case TalkerEnum.T_INTERJECTOR1:
				{
					return Color.LightBlue;
				}
				case TalkerEnum.T_INTERJECTOR2:
				{
					return Color.LightGreen;
				}
				case TalkerEnum.T_INTERJECTOR3:
				{
					return Color.PaleTurquoise;
				}
			}
			return Color.White;
		}

		public static Color GetStateColor(GameData.State state)
		{
			switch (state)
			{
				case GameData.State.UNKNOWN:
				{
					return Color.Red;
				}
				case GameData.State.INVALID:
				{
					return Color.Red;
				}
				case GameData.State.ORIGINAL:
				{
					return Color.Black;
				}
				case GameData.State.OWNED:
				{
					return Color.Green;
				}
				case GameData.State.MODIFIED:
				{
					return Color.Blue;
				}
				case GameData.State.LOCKED:
				{
					return Color.DarkOrange;
				}
				case GameData.State.REMOVED:
				{
					return Color.LightGray;
				}
				case GameData.State.LOCKED_REMOVED:
				{
					return Color.LightGray;
				}
			}
			return Color.Black;
		}

		public static Color getTalkerColour(TalkerEnum value)
		{
			switch (value)
			{
				case TalkerEnum.T_ME:
				{
					return Color.Black;
				}
				case TalkerEnum.T_TARGET:
				{
					return Color.Maroon;
				}
				case TalkerEnum.T_TARGET_IF_PLAYER:
				{
					return Color.Maroon;
				}
				case TalkerEnum.T_INTERJECTOR1:
				{
					return Color.Blue;
				}
				case TalkerEnum.T_INTERJECTOR2:
				{
					return Color.Green;
				}
				case TalkerEnum.T_INTERJECTOR3:
				{
					return Color.DarkTurquoise;
				}
			}
			return Color.Maroon;
		}
	}
}