using AliasPro.API.Items.Models;
using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Items.Packets.Composers
{
    public class LoveLockFinishedComposer : IPacketComposer
    {
        private readonly IItem _item;

        public LoveLockFinishedComposer(IItem item)
        {
			_item = item;
		}

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.LoveLockFinishedMessageComposer);
            message.WriteInt(_item.Id);
            return message;
        }
    }
}
