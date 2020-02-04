using AliasPro.API.Rooms.Entities;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.API.Items.Interaction
{
    public interface IItemInteractor
    {
        void Compose(ServerMessage message, bool tradeItem = false);
		void OnPlaceItem();
		void OnPickupItem();
		void OnMoveItem();
		void OnUserWalkOn(BaseEntity entity);
        void OnUserWalkOff(BaseEntity entity);
        void OnUserInteract(BaseEntity entity, int state = 0);
        void OnCycle();
    }
}
