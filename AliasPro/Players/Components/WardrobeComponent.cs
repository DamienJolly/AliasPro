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

        public ICollection<IWardrobeItem> WardobeItems =>
			_wardobeItems.Values;
    }
}
