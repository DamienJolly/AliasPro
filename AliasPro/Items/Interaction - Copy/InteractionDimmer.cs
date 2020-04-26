using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;

namespace AliasPro.Items.Interaction
{
    public class InteractionDimmer : IItemInteractor
    {
        private readonly IItem _item;

        public InteractionDimmer(IItem item)
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

        public async void OnPlaceItem()
		{
            if (_item.CurrentRoom.Moodlight != null)
            {
                _item.ExtraData = _item.CurrentRoom.Moodlight.GenerateExtraData;
                await _item.CurrentRoom.SendPacketAsync(new WallItemUpdateComposer(_item));
            }
        }

		public void OnPickupItem()
		{
            _item.ExtraData = "";
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
        
        public void OnUserInteract(BaseEntity entity, int state)
        {

        }

        public void OnCycle()
        {

        }
    }
}
