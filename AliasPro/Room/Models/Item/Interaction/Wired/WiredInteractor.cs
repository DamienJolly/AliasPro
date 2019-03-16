namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    using Sessions;
    using AliasPro.Item.Models;
    using Network.Protocol;
    
    public class WiredInteractor
    {
        public static IWiredInteractor GetWiredInteractor(WiredInteraction interaction, IItem item)
        {
            switch (interaction)
            {
                case WiredInteraction.MESSAGE: return new WiredInteractionMessage(item);
                case WiredInteraction.REPEATER: return new WiredInteractionRepeater(item);
                case WiredInteraction.DEFAULT: default: return new WiredInteractionDefault(item);
            }
        }
    }

    public interface IWiredInteractor
    {
        void Compose(ServerPacket message);
        void OnTrigger(ISession session);
        void OnCycle();
    }
}
