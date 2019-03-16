namespace AliasPro.Room.Models.Item.Interaction
{
    using Sessions;
    using AliasPro.Item.Models;
    using Network.Protocol;
    
    public class ItemInteractor
    {
        public static IItemInteractor GetItemInteractor(ItemInteraction interaction, IItem item)
        {
            switch (interaction)
            {
                case ItemInteraction.WIRED_TRIGGER: return new InteractionWired(item);
                case ItemInteraction.WIRED_EFFECT: return new InteractionWired(item);
                case ItemInteraction.WIRED_CONDITION: return new InteractionWired(item);
                case ItemInteraction.VENDING_MACHINE: return new InteractionVendingMachine(item);
                case ItemInteraction.DEFAULT: default: return new InteractionDefault(item);
            }
        }
    }

    public interface IItemInteractor
    {
        void Compose(ServerPacket message);
        void OnUserWalkOn(ISession session);
        void OnUserWalkOff(ISession session);
        void OnUserInteract(ISession session, int state = 0);
        void OnCycle();
    }
}
