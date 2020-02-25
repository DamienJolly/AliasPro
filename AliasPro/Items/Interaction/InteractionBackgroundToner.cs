using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Items.Packets.Composers;
using AliasPro.Rooms.Entities;

namespace AliasPro.Items.Interaction
{
    public class InteractionBackgroundToner : IItemInteractor
    {
        private readonly IItem _item;
		public int Hue = 126;
		public int Saturation = 126;
		public int Brightness = 126;

        public InteractionBackgroundToner(IItem item)
        {
            _item = item;

			string[] data = _item.ExtraData.Split(":");
			if (data.Length == 3)
			{
				Hue = int.Parse(data[0]);
				Saturation = int.Parse(data[1]);
				Brightness = int.Parse(data[2]);
			}
		}

		public void Compose(ServerMessage message, bool tradeItem)
		{
			if (!tradeItem)
				message.WriteInt(0);
			message.WriteInt(5);
			message.WriteInt(4);
            message.WriteInt(0);
            message.WriteInt(Hue);
            message.WriteInt(Saturation);
            message.WriteInt(Brightness);
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
        
        public void OnUserInteract(BaseEntity entity, int state)
        {

        }

        public void OnCycle()
        {

        }
    }
}
