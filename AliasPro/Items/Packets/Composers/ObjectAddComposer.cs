using AliasPro.API.Network.Events;
using AliasPro.Items.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Items.Packets.Composers
{
    public class ObjectAddComposer : IPacketComposer
    {
        private readonly IItem _item;

        public ObjectAddComposer(IItem item)
        {
            _item = item;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.ObjectAddMessageComposer);
            _item.ComposeFloorItem(message);
            message.WriteString(_item.PlayerUsername);
            return message;
        }
    }
}
