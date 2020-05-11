using AliasPro.Game.Catalog.Types;

namespace AliasPro.Game.Catalog.Utilities
{
	public class CatalogGiftPartUtility
	{
		public static CatalogGiftPartType GetGiftPartType(string type)
		{
			switch (type.ToLower())
			{
				default:
				case "gift": 
					return CatalogGiftPartType.GIFT;

				case "wrapper": 
					return CatalogGiftPartType.WRAPPER;
			}
		}
	}
}
