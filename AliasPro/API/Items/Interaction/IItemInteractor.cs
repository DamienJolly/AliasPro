using AliasPro.API.Rooms.Entities;
using AliasPro.Network.Protocol;

namespace AliasPro.API.Items.Interaction
{
    public interface IItemInteractor
    {
        void Compose(ServerPacket message);
		void OnPlaceItem();
		void OnPickupItem();
		void OnUserWalkOn(BaseEntity entity);
        void OnUserWalkOff(BaseEntity entity);
        void OnUserInteract(BaseEntity entity, int state = 0);
        void OnCycle();
    }
}
