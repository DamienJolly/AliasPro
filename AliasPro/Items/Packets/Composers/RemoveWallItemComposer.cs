using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Items.Packets.Composers
{
    public class RemoveWallItemComposer : IMessageComposer
    {
        private readonly IItem _item;

        public RemoveWallItemComposer(IItem item)
        {
            _item = item;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RemoveWallItemMessageComposer);
            message.WriteString(_item.Id + "");
            message.WriteInt((int)_item.PlayerId);
            return message;
        }
    }
}
