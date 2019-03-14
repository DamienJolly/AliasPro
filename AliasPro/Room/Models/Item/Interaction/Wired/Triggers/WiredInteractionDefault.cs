using AliasPro.Item.Models;
using AliasPro.Network.Protocol;
using AliasPro.Sessions;

namespace AliasPro.Room.Models.Item.Interaction.Wired
{
    public class WiredInteractionDefault : IWiredInteractor
    {
        public void Compose(ServerPacket message, IItem item)
        {

        }

        public void OnTrigger(ISession session, IRoom room, IItem item)
        {

        }

        public void OnCycle(IRoom room, IItem item)
        {
            System.Console.WriteLine("works");
        }
    }
}
