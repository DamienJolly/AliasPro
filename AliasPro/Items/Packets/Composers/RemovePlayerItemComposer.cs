using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Items.Packets.Composers
{
    public class RemovePlayerItemComposer : IMessageComposer
    {
        private readonly uint _itemId;

        public RemovePlayerItemComposer(uint itemId)
        {
            _itemId = itemId;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RemovePlayerItemMessageComposer);
            message.WriteInt((int)_itemId);
            return message;
        }
    }
}
