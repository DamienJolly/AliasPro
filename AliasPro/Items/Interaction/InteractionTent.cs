using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Items.Interaction
{
    public class InteractionTent : IItemInteractor
    {
        private readonly IItem _item;
		public IList<BaseEntity> TentEntities;

        public InteractionTent(IItem item)
        {
            _item = item;
			TentEntities = new List<BaseEntity>();
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
			TentEntities.Add(entity);
		}

        public void OnUserWalkOff(BaseEntity entity)
        {
			TentEntities.Remove(entity);
		}
        
        public void OnUserInteract(BaseEntity entity, int state)
        {

        }

        public void OnCycle()
        {

        }
    }
}
