using AliasPro.API.Items;
using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Items.Interaction
{
    public class InteractionMusicDisc : IItemInteractor
    {
        private readonly IItem _item;

        public InteractionMusicDisc(IItem item)
        {
            _item = item;
        }

		public void Compose(ServerMessage message, bool tradeItem)
		{
            int.TryParse(_item.ItemData.ExtraData, out int songId);

            if (!tradeItem)
				message.WriteInt(_item.CurrentRoom != null ? songId : 8);
			message.WriteInt(0);
            message.WriteString(songId + "");
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
