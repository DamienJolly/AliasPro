using AliasPro.API.Figure;
using AliasPro.API.Figure.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Figure.Packets.Composers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AliasPro.Figure.Packets.Events
{
    public class RequestUserWardrobeEvent : IMessageEvent
    {
        public short Header => Incoming.RequestUserWardrobeMessengerEvent;
       
        private readonly IFigureController _figureController;

        public RequestUserWardrobeEvent(
            IFigureController figureController)
        {
            _figureController = figureController;
		}

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
			if (session.Player.Wardrobe == null) 
                return;

			int slotsAvailable = session.Player.Wardrobe.SlotsAvailable;
			if (slotsAvailable == 0) 
                return;

			IList<IWardrobeItem> validItems = new List<IWardrobeItem>();
			foreach (IWardrobeItem Item in session.Player.Wardrobe.WardobeItems)
			{
				if (Item.SlotId > slotsAvailable) 
                    continue;

				validItems.Add(Item);
			}

			await session.SendPacketAsync(new UserWardrobeComposer(validItems));
		}
    }
}
