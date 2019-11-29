using AliasPro.API.Catalog.Models;
using AliasPro.API.Database;
using AliasPro.Groups.Types;
using AliasPro.Groups.Utilities;
using System.Data.Common;

namespace AliasPro.Catalog.Models
{
    internal class CatalogGiftPart : ICatalogGiftPart
	{
		internal CatalogGiftPart(DbDataReader reader)
		{
			ItemId = reader.ReadData<int>("item_id");
			SpriteId = reader.ReadData<int>("sprite_id");
			Type = GiftPartUtility.GetGiftPartType(
				reader.ReadData<string>("type"));
		}

		public int ItemId { get; set; }
		public int SpriteId { get; set; }
		public GiftPartType Type { get; set; }
	}
}
