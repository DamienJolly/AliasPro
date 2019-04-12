using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Items.Packets.Composers
{
    public class RemovePlayerItemComposer : IPacketComposer
    {
        private readonly uint _itemId;

        public RemovePlayerItemComposer(uint itemId)
        {
            _itemId = itemId;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RemovePlayerItemMessageComposer);
            message.WriteInt(_itemId);
            return message;
        }
    }
}
