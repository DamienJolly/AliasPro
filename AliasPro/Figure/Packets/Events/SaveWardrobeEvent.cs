using AliasPro.API.Figure;
using AliasPro.API.Figure.Models;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Figure.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Players.Types;

namespace AliasPro.Figure.Packets.Events
{
    public class SaveWardrobeEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.SaveWardrobeMessageEvent;
       
        private readonly IFigureController _figureController;

        public SaveWardrobeEvent(
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

			int slotId = clientPacket.ReadInt();
			string look = clientPacket.ReadString();
			PlayerGender gender = 
				clientPacket.ReadString().ToLower() == "m" ? PlayerGender.MALE : PlayerGender.FEMALE;

			if (slotId <= 0 || slotId > slotsAvailable) return;

			if (session.Player.Wardrobe.TryGetWardrobeItem(slotId, out IWardrobeItem item))
			{
				item.Figure = look;
				item.Gender = gender;
			}
			else
			{
				IWardrobeItem newItem = new WardrobeItem(slotId, look, gender);
				if (!session.Player.Wardrobe.TryAddWardrobeItem(newItem))
					return;

				await _figureController.AddWardrobeItemAsync(session.Player.Id, newItem);
			}
		}
    }
}
