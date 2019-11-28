using AliasPro.API.Figure;
using AliasPro.API.Figure.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Figure.Packets.Composers;
using AliasPro.Network.Events.Headers;
using System.Collections.Generic;

namespace AliasPro.Figure.Packets.Events
{
    public class RequestUserWardrobeEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestUserWardrobeMessengerEvent;
       
        private readonly IFigureController _figureController;

        public RequestUserWardrobeEvent(
            IFigureController figureController)
        {
            _figureController = figureController;
		}

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
			if (session.Player.Wardrobe == null) return;

			int slotsAvailable = session.Player.Wardrobe.SlotsAvailable;
			if (slotsAvailable == 0) return;

			IList<IWardrobeItem> validItems = new List<IWardrobeItem>();
			foreach (IWardrobeItem Item in session.Player.Wardrobe.WardobeItems)
			{
				if (Item.SlotId > slotsAvailable) continue;

				validItems.Add(Item);
			}

			await session.SendPacketAsync(new UserWardrobeComposer(validItems));
		}
    }
}
