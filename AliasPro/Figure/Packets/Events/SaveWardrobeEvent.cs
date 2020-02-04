using AliasPro.API.Figure;
using AliasPro.API.Figure.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Figure.Models;
using AliasPro.Players.Types;
using System.Threading.Tasks;

namespace AliasPro.Figure.Packets.Events
{
    public class SaveWardrobeEvent : IMessageEvent
    {
        public short Id { get; } = Incoming.SaveWardrobeMessageEvent;
       
        private readonly IFigureController _figureController;

        public SaveWardrobeEvent(
            IFigureController figureController)
        {
            _figureController = figureController;
		}

        public async Task RunAsync(
            ISession session,
            ClientMessage clientPacket)
        {
			if (session.Player.Wardrobe == null) 
				return;

			int slotsAvailable = session.Player.Wardrobe.SlotsAvailable;
			if (slotsAvailable == 0) 
				return;

			int slotId = clientPacket.ReadInt();
			string look = clientPacket.ReadString();
			PlayerGender gender = 
				clientPacket.ReadString().ToLower() == "m" ? PlayerGender.MALE : PlayerGender.FEMALE;

			if (slotId <= 0 || slotId > slotsAvailable) 
				return;

			if (session.Player.Wardrobe.TryGetWardrobeItem(slotId, out IWardrobeItem item))
			{
				item.Figure = look;
				item.Gender = gender;

				await _figureController.UpdateWardrobeItemAsync(session.Player.Id, item);
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
