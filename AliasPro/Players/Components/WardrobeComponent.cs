using AliasPro.API.Figure.Models;
using System.Collections.Generic;

namespace AliasPro.Players.Components
{
    public class WardrobeComponent
	{
		private readonly IDictionary<int, IWardrobeItem> _wardobeItems;
		private readonly IDictionary<int, IClothingItem> _clothingItems;

		public int SlotsAvailable => 10; //todo: 10 for vip, 5 for club, 0 for no subscription

		public WardrobeComponent(
            IDictionary<int, IWardrobeItem> wardobeItems,
			IDictionary<int, IClothingItem> clothingItems)
        {
			_wardobeItems = wardobeItems;
			_clothingItems = clothingItems;
		}

		public bool TryGetWardrobeItem(int slotId, out IWardrobeItem item) =>
			_wardobeItems.TryGetValue(slotId, out item);

		public bool TryAddWardrobeItem(IWardrobeItem item) =>
		   _wardobeItems.TryAdd(item.SlotId, item);

		public ICollection<IWardrobeItem> WardobeItems =>
			_wardobeItems.Values;

		public ICollection<IClothingItem> ClothingItems =>
			_clothingItems.Values;
	}
}
