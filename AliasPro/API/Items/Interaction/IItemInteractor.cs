using AliasPro.API.Rooms.Entities;
using AliasPro.Network.Protocol;

namespace AliasPro.API.Items.Interaction
{
    public interface IItemInteractor
    {
        void Compose(ServerPacket message, bool tradeItem = false);
		void OnPlaceItem();
		void OnPickupItem();
		void OnMoveItem();
		void OnUserWalkOn(BaseEntity entity);
        void OnUserWalkOff(BaseEntity entity);
        void OnUserInteract(BaseEntity entity, int state = 0);
        void OnCycle();
    }
}
