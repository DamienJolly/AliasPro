namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    using AliasPro.Item.Models;
    using Network.Protocol;
    using AliasPro.Room.Models.Entities;

    public class WiredInteractor
    {
        public static IWiredInteractor GetWiredInteractor(WiredInteraction interaction, IItem item)
        {
            switch (interaction)
            {
                case WiredInteraction.MESSAGE: return new WiredInteractionMessage(item);
                case WiredInteraction.WALKS_ON_FURNI: return new WiredInteractionWalksOn(item);
                case WiredInteraction.WALKS_OFF_FURNI: return new WiredInteractionWalksOff(item);
                case WiredInteraction.REPEATER: return new WiredInteractionRepeater(item);
                case WiredInteraction.DEFAULT: default: return new WiredInteractionDefault(item);
            }
        }
    }

    public interface IWiredInteractor
    {
        void Compose(ServerPacket message);
        void SaveData(IClientPacket clientPacket);
        void OnTrigger(BaseEntity entity);
        void OnCycle();
        bool HasItem(uint itemId);
    }
}
