using AliasPro.API.Items.Interaction;
using AliasPro.API.Items.Models;
using AliasPro.API.Rooms.Entities;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Items.Interaction
{
    public class InteractionLandscape : IItemInteractor
    {
        private readonly IItem _item;

        public InteractionLandscape(IItem item)
        {
            _item = item;
        }

        public void Compose(ServerMessage message, bool tradeItem)
        {
            if (!tradeItem)
                message.WriteInt(4);
            message.WriteInt(0);
            message.WriteString(_item.ExtraData);
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
