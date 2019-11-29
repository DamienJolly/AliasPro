using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Network.Protocol;

namespace AliasPro.Items.Interaction
{
    public class InteractionClothing : IItemInteractor
    {
        private readonly IItem _item;

        public InteractionClothing(IItem item)
        {
            _item = item;
        }

		public void Compose(ServerPacket message, bool tradeItem)
		{
			if (!tradeItem)
				message.WriteInt(2);
			message.WriteInt(1);
			message.WriteString("");
		}

		public void OnPlaceItem()
		{

		}

		public void OnPickupItem()
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
