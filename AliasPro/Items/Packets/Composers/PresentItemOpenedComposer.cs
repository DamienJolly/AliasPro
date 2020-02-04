using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Items.Packets.Composers
{
    public class PresentItemOpenedComposer : IMessageComposer
    {
        private readonly IItem _item;
        private readonly string _extraData;
        private readonly bool _itemIsInRoom;

        public PresentItemOpenedComposer(IItem item, string extraData, bool itemIsInRoom)
        {
            _item = item;
			_extraData = extraData;
			_itemIsInRoom = itemIsInRoom;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.PresentItemOpenedMessageComposer);
			message.WriteString(_item.ItemData.Type.ToLower());
			message.WriteInt((int)_item.ItemData.SpriteId);
			message.WriteString(_item.ItemData.Name);
			message.WriteInt((int)_item.Id);
			message.WriteString(_item.ItemData.Type.ToLower());
			message.WriteBoolean(_itemIsInRoom);
			message.WriteString(_extraData);
			return message;
        }
	}
}
