using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Items.Packets.Composers;
using AliasPro.Network.Protocol;
using AliasPro.Rooms.Entities;

namespace AliasPro.Items.Interaction
{
    public class InteractionPressureTile : IItemInteractor
    {
        private readonly IItem _item;

        public InteractionPressureTile(IItem item)
        {
            _item = item;
        }

		public void Compose(ServerPacket message, bool tradeItem)
		{
			if (!tradeItem)
				message.WriteInt(1);
			message.WriteInt(0);
            message.WriteString(_item.Mode.ToString());
        }

		public void OnPlaceItem()
		{
			_item.Mode = 0;
		}

		public void OnPickupItem()
		{
			_item.Mode = 0;
		}

		public async void OnUserWalkOn(BaseEntity entity)
        {
			System.Console.WriteLine("works");
			_item.Mode = 1;
			await _item.CurrentRoom.SendAsync(new FloorItemUpdateComposer(_item));
		}

        public async void OnUserWalkOff(BaseEntity entity)
        {
			_item.Mode = 0;
			await _item.CurrentRoom.SendAsync(new FloorItemUpdateComposer(_item));
		}
        
        public void OnUserInteract(BaseEntity entity, int state)
        {

        }

        public void OnCycle()
        {

        }
    }
}
