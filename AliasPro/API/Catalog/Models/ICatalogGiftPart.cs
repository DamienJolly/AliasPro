using AliasPro.Groups.Types;

namespace AliasPro.API.Catalog.Models
{
    public interface ICatalogGiftPart
	{
		int ItemId { get; set; }
		int SpriteId { get; set; }
		GiftPartType Type { get; set; }
	}
}
