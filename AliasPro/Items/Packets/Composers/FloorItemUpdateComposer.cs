using AliasPro.API.Network.Events;
using AliasPro.Items.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Items.Packets.Composers
{
    public class FloorItemUpdateComposer : IPacketComposer
    {
        private readonly IItem _item;

        public FloorItemUpdateComposer(IItem item)
        {
            _item = item;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.FloorItemUpdateMessageComposer);
            _item.ComposeFloorItem(message);
            return message;
        }
    }
}
