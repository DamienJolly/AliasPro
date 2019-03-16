using AliasPro.Item.Models;
using AliasPro.Network.Protocol;
using AliasPro.Sessions;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionDefault : IWiredInteractor
    {
        private readonly IItem _item;

        public WiredInteractionDefault(IItem item)
        {
            _item = item;
        }

        public void Compose(ServerPacket message)
        {

        }

        public void OnTrigger(ISession session)
        {

        }

        public void OnCycle()
        {

        }
    }
}
