using AliasPro.API.Network.Events;
using AliasPro.Items.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Items.Packets.Composers
{
    public class RemoveWallItemComposer : IPacketComposer
    {
        private readonly IItem _item;

        public RemoveWallItemComposer(IItem item)
        {
            _item = item;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RemoveWallItemMessageComposer);
            message.WriteString(_item.Id + "");
            message.WriteInt(_item.PlayerId);
            return message;
        }
    }
}
