namespace AliasPro.Room.Models.Item.Interaction
{
    using Sessions;
    using AliasPro.Item.Models;
    using Network.Protocol;

    public class ItemInteractor
    {
        public static IItemInteractor GetItemInteractor(string interaction)
        {
            switch (interaction)
            {
                default: return new InteractionDefault();
            }
        }
    }

    public interface IItemInteractor
    {
        void Compose(ServerPacket message, IItem item);
        void OnUserWalkOn(ISession session, IRoom room, IItem item);
        void OnUserWalkOff(ISession session, IRoom room, IItem item);
        void OnUserInteract(ISession session, IRoom room, IItem item, int state);
        void OnCycle(IItem item);
    }
}
