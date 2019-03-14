namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    using Sessions;
    using AliasPro.Item.Models;
    using Network.Protocol;
    
    public class WiredInteractor
    {
        public static IWiredInteractor GetWiredInteractor(WiredInteraction interaction)
        {
            switch (interaction)
            {
                case WiredInteraction.DEFAULT: default: return new WiredInteractionDefault();
            }
        }
    }

    public interface IWiredInteractor
    {
        void Compose(ServerPacket message, IItem item);
        void OnTrigger(ISession session, IRoom room, IItem item);
        void OnCycle(IRoom room, IItem item);
    }
}
