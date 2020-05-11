using AliasPro.Game.Catalog.Types;
using AliasPro.Game.Catalog.Utilities;

namespace AliasPro.Game.Catalog.Models
{
	public class CatalogGiftPartData
	{
		public CatalogGiftPartData(int itemId, int spriteId, string type)
		{
			ItemId = itemId;
			SpriteId = spriteId;
			Type = CatalogGiftPartUtility.GetGiftPartType(type);
		}

		public int ItemId { get; set; }
		public int SpriteId { get; set; }
		public CatalogGiftPartType Type { get; set; }
	}
}
