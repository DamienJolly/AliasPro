using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using AliasPro.Rooms.Entities;

namespace AliasPro.Items.Interaction
{
    public class InteractionDefault : IItemInteractor
    {
        private readonly IItem _item;

        public InteractionDefault(IItem item)
        {
            _item = item;
        }

		public void Compose(ServerMessage message, bool tradeItem)
		{
			if (!tradeItem)
				message.WriteInt(1);
			message.WriteInt(0);
            message.WriteString(_item.Mode.ToString());
        }

		public void OnPlaceItem()
		{

		}

		public void OnPickupItem()
		{

		}

		public void OnMoveItem()
		{

		}

		public void OnUserWalkOn(BaseEntity entity)
        {

        }

        public void OnUserWalkOff(BaseEntity entity)
        {

        }
        
        public async void OnUserInteract(BaseEntity entity, int state)
        {
			if (entity is PlayerEntity playerEntity)
				if (!_item.CurrentRoom.Rights.HasRights(playerEntity.Player.Id)) return;

			_item.Mode++;
            if (_item.Mode >= _item.ItemData.Modes)
				_item.Mode = 0;

			await _item.CurrentRoom.SendPacketAsync(new FloorItemUpdateComposer(_item));
        }

        public void OnCycle()
        {

        }
    }
}
