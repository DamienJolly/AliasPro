using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Network.Protocol;

namespace AliasPro.Items.Interaction
{
    public class InteractionBadgeDisplay : IItemInteractor
    {
        private readonly IItem _item;

        public InteractionBadgeDisplay(IItem item)
        {
            _item = item;
        }

		public void Compose(ServerPacket message, bool tradeItem)
		{
			if (!tradeItem)
				message.WriteInt(0);
			message.WriteInt(2);
			message.WriteInt(4);
			message.WriteString("0");

			if (!string.IsNullOrEmpty(_item.ExtraData))
			{
				string[] data = _item.ExtraData.Split(";");

				message.WriteString(data[0]);
				message.WriteString(data[1]);
				message.WriteString(data[2]);
			}
			else
			{
				message.WriteString(string.Empty);
				message.WriteString("Unknown User");
				message.WriteString("Unknown Date");
			}

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
