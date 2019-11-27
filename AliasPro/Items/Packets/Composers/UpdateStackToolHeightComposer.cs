using AliasPro.API.Items.Models;
using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Items.Packets.Composers
{ 
    public class UpdateStackToolHeightComposer : IPacketComposer
    {
        private readonly IItem _item;

        public UpdateStackToolHeightComposer(IItem item)
        {
			_item = item;
        }

        public ServerPacket Compose()
		{
            ServerPacket message = new ServerPacket(Outgoing.UpdateStackToolHeightMessageComposer);
			message.WriteInt(_item.Id);
			message.WriteInt((int)(_item.Position.Z * 100));
            return message;
        }
    }
}
