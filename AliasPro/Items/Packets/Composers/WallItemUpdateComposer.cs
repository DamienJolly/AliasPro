using AliasPro.API.Network.Events;
using AliasPro.Items.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Items.Packets.Composers
{
    public class WallItemUpdateComposer : IPacketComposer
    {
        private readonly IItem _item;

        public WallItemUpdateComposer(IItem item)
        {
            _item = item;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.WallItemUpdateMessageComposer);
            _item.ComposeWallItem(message);
            message.WriteString(_item.PlayerUsername);
            return message;
        }
    }
}
