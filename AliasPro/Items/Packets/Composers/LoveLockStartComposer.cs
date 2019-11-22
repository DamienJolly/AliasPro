using AliasPro.API.Items.Models;
using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Items.Packets.Composers
{
    public class LoveLockStartComposer : IPacketComposer
    {
        private readonly IItem _item;

        public LoveLockStartComposer(IItem item)
        {
			_item = item;
		}

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.LoveLockStartMessageComposer);
            message.WriteInt(_item.Id);
			message.WriteBoolean(true);
            return message;
        }
    }
}
