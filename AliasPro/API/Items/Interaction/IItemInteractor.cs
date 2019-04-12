using AliasPro.Network.Protocol;
using AliasPro.Room.Models.Entities;

namespace AliasPro.API.Items.Interaction
{
    public interface IItemInteractor
    {
        void Compose(ServerPacket message);
        void OnUserWalkOn(BaseEntity entity);
        void OnUserWalkOff(BaseEntity entity);
        void OnUserInteract(BaseEntity entity, int state = 0);
        void OnCycle();
    }
}
