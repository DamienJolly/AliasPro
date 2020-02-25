using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;

namespace AliasPro.Items.Interaction
{
    public class InteractionPressureTile : IItemInteractor
    {
        private readonly IItem _item;

        public InteractionPressureTile(IItem item)
        {
            _item = item;
        }

		public void Compose(ServerMessage message, bool tradeItem)
		{
			if (!tradeItem)
				message.WriteInt(1);
			message.WriteInt(0);
            message.WriteString(_item.ExtraData);
        }

		public void OnPlaceItem()
		{
			_item.ExtraData = "0";
		}

		public void OnPickupItem()
		{
			_item.ExtraData = "0";
		}

		public void OnMoveItem()
		{
			_item.ExtraData = "0";
		}

		public async void OnUserWalkOn(BaseEntity entity)
        {
			_item.ExtraData = "1";
			await _item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(_item));
		}

        public async void OnUserWalkOff(BaseEntity entity)
        {
			_item.ExtraData = "0";
			await _item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(_item));
		}
        
        public void OnUserInteract(BaseEntity entity, int state)
        {

        }

        public void OnCycle()
        {

        }
    }
}
