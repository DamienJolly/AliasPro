using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Network.Protocol;

namespace AliasPro.Items.Interaction
{
    public class InteractionEcotron : IItemInteractor
    {
        private readonly IItem _item;

		public readonly int itemId = -1;
		public readonly string ExtraData = "1-1-1997";

		public InteractionEcotron(IItem item)
        {
            _item = item;

			if (!string.IsNullOrEmpty(_item.ExtraData))
			{
				string[] data = _item.ExtraData.Split("\t");

				itemId = int.Parse(data[0]);
				ExtraData = data[1];
			}

		}

		public void Compose(ServerPacket message, bool tradeItem)
		{
			if (!tradeItem)
				message.WriteInt(1);
			message.WriteInt(0);
			message.WriteString(ExtraData);

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
