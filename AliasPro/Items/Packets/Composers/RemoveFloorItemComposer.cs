using AliasPro.API.Items.Models;
using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Items.Packets.Composers
{
    public class RemoveFloorItemComposer : IPacketComposer
    {
        private readonly IItem _item;

        public RemoveFloorItemComposer(IItem item)
        {
            _item = item;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RemoveFloorItemMessageComposer);
            message.WriteString(_item.Id + "");
            message.WriteBoolean(false);
            message.WriteInt(_item.PlayerId);
            message.WriteInt(0);
            return message;
        }
    }
}
