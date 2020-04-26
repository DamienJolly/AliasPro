using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.API.Rooms.Models;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Items.Interaction
{
	public abstract class ItemInteraction
	{
		public IItem Item;

        public IRoom Room => 
            Item.CurrentRoom;

		public ItemInteraction(IItem item)
		{
			Item = item;
		}

        public virtual void ComposeExtraData(ServerMessage message)
        {
            message.WriteInt(0);
            message.WriteString(Item.ExtraData);
        }

        public virtual void OnPlaceItem() { }

        public virtual void OnPickupItem() { }

        public virtual void OnMoveItem() { }

        public virtual void OnUserWalkOn(BaseEntity entity) { }

        public virtual void OnUserWalkOff(BaseEntity entity) { }

        public virtual void OnUserInteract(BaseEntity entity, int state = 0) { }

        public virtual void OnCycle() { }
    }
}
