using AliasPro.API.Items;
using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;

namespace AliasPro.Items.Interaction
{
    public class InteractionJukeBox : IItemInteractor
    {
        private readonly IItem _item;

        public InteractionJukeBox(IItem item)
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

        }

		public void OnPickupItem()
		{
            _item.CurrentRoom.Trax.Stop();
            _item.ExtraData = "0";
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
            IRoom room = _item.CurrentRoom;
            if (room == null)
                return;

            if (state == -1)
                return;

            if (!room.Trax.CurrentlyPlaying)
            {
                room.Trax.Play();

                if (room.Trax.CurrentlyPlaying)
                    _item.ExtraData = "1";
                else
                    _item.ExtraData = "0";
            }
            else
            {
                room.Trax.Stop();
                _item.ExtraData = "0";
            }

            await room.SendPacketAsync(new FloorItemUpdateComposer(_item));
        }

        public void OnCycle()
        {
            if (_item.ExtraData == "1" && !_item.CurrentRoom.Trax.CurrentlyPlaying)
                OnUserInteract(null, 0);
        }
    }
}
