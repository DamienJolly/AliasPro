namespace AliasPro.Room.Models.Item.Interaction
{
    using Sessions;
    using AliasPro.Item.Models;
    using Network.Protocol;
    
    public class ItemInteractor
    {
        public static IItemInteractor GetItemInteractor(ItemInteraction interaction)
        {
            switch (interaction)
            {
                case ItemInteraction.WIRED_TRIGGER: return new InteractionWired();
                case ItemInteraction.WIRED_EFFECT: return new InteractionWired();
                case ItemInteraction.WIRED_CONDITION: return new InteractionWired();
                case ItemInteraction.VENDING_MACHINE: return new InteractionVendingMachine();
                case ItemInteraction.DEFAULT: default: return new InteractionDefault();
            }
        }
    }

    public interface IItemInteractor
    {
        void Compose(ServerPacket message, IItem item);
        void OnUserWalkOn(ISession session, IRoom room, IItem item);
        void OnUserWalkOff(ISession session, IRoom room, IItem item);
        void OnUserInteract(ISession session, IRoom room, IItem item, int state);
        void OnCycle(IRoom room, IItem item);
    }
}
