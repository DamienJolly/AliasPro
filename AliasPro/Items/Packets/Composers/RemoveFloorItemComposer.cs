using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Items.Packets.Composers
{
    public class RemoveFloorItemComposer : IMessageComposer
    {
        private readonly IItem _item;

        public RemoveFloorItemComposer(IItem item)
        {
            _item = item;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RemoveFloorItemMessageComposer);
            message.WriteString(_item.Id + "");
            message.WriteBoolean(false);
            message.WriteInt((int)_item.PlayerId);
            message.WriteInt(0);
            return message;
        }
    }
}
