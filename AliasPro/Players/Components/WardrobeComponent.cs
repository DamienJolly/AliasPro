using AliasPro.API.Figure.Models;
using System.Collections.Generic;

namespace AliasPro.Players.Components
{
    public class WardrobeComponent
	{
		private readonly IDictionary<int, IWardrobeItem> _wardobeItems;

		public int SlotsAvailable => 10; //todo: 10 for vip, 5 for club, 0 for no subscription

		public WardrobeComponent(
            IDictionary<int, IWardrobeItem> wardobeItems)
        {
			_wardobeItems = wardobeItems;
        }

		public bool TryGetWardrobeItem(int slotId, out IWardrobeItem item) =>
			_wardobeItems.TryGetValue(slotId, out item);

		public bool TryAddWardrobeItem(IWardrobeItem item) =>
		   _wardobeItems.TryAdd(item.SlotId, item);

		public ICollection<IWardrobeItem> WardobeItems =>
			_wardobeItems.Values;
    }
}
