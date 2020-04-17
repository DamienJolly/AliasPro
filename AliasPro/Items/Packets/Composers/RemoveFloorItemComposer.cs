using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Items.Packets.Composers
{
    public class RemoveFloorItemComposer : IMessageComposer
    {
        private readonly IItem _item;
        private readonly bool _inRoom;

        public RemoveFloorItemComposer(IItem item, bool inRoom = false)
        {
            _item = item;
            _inRoom = inRoom;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RemoveFloorItemMessageComposer);
            message.WriteString(_item.Id + "");
            message.WriteBoolean(_inRoom);
            message.WriteInt((int)_item.PlayerId);
            message.WriteInt(0);
            return message;
        }
    }
}
