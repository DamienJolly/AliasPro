using AliasPro.Groups.Types;

namespace AliasPro.Groups.Utilities
{
	public class BadgePartUtility
	{
		public static BadgePartType GetBadgePartType(string type)
		{
			switch (type)
			{
				case "base": return BadgePartType.BASE;
				case "symbol": return BadgePartType.SYMBOL;
				case "base_color": return BadgePartType.BASE_COLOUR;
				case "symbol_color": return BadgePartType.SYMBOL_COLOUR;
				case "other_color": return BadgePartType.BACKGROUND_COLOUR;
				default: return BadgePartType.BASE;
			}
		}
	}
}
