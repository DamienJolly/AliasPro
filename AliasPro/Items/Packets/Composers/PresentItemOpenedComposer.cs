using AliasPro.API.Items.Models;
using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Items.Packets.Composers
{
    public class PresentItemOpenedComposer : IPacketComposer
    {
        private readonly IItem _item;
        private readonly string _text;
        private readonly bool _unknown;

        public PresentItemOpenedComposer(IItem item, string text, bool unknown)
        {
            _item = item;
			_text = text;
			_unknown = unknown;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.PresentItemOpenedMessageComposer);
			message.WriteString(_item.ItemData.Type.ToLower());
			message.WriteInt(_item.ItemData.SpriteId);
			message.WriteString(_item.ItemData.Name);
			message.WriteInt(_item.Id);
			message.WriteString(_item.ItemData.Type.ToLower());
			message.WriteBoolean(_unknown);
			message.WriteString(_text);
			return message;
        }
	}
}
