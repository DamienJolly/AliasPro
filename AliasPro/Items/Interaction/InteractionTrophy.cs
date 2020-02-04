using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using AliasPro.Rooms.Entities;

namespace AliasPro.Items.Interaction
{
    public class InteractionTrophy : IItemInteractor
    {
        private readonly IItem _item;

        public InteractionTrophy(IItem item)
        {
            _item = item;
        }

		public void Compose(ServerMessage message, bool tradeItem)
		{
			if (!tradeItem)
				message.WriteInt(1);
			message.WriteInt(0);

			if (!string.IsNullOrEmpty(_item.ExtraData))
			{
				string[] data = _item.ExtraData.Split(";");

				message.WriteString(data[0] + (char)9 + data[1] + (char)9 + data[2]);
			}
			else
			{
				message.WriteString("Unknown" + (char)9 + "1-1-1970" + (char)9 + string.Empty);
			}
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
