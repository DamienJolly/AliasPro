using AliasPro.API.Items.Models;

namespace AliasPro.Game.Catalog.Models
{
	public class CatalogItemData
	{
		public int Id { get; }
		public int Amount { get; }
		public IItemData ItemData { get; }
		public string Extradata { get; }

		public CatalogItemData(int id, int amount, IItemData itemData, string extraData)
		{
			Id = id;
			Amount = amount;
			ItemData = itemData;
			Extradata = extraData;
		}
	}
}
