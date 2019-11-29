using AliasPro.Groups.Types;

namespace AliasPro.Groups.Utilities
{
	public class GiftPartUtility
	{
		public static GiftPartType GetGiftPartType(string type)
		{
			switch (type)
			{
				case "gift": default: return GiftPartType.GIFT;
				case "wrapper": return GiftPartType.WRAPPER;
			}
		}
	}
}
