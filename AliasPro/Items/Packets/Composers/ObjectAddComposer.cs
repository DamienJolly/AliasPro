using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Items.Packets.Composers
{
    public class ObjectAddComposer : IMessageComposer
    {
        private readonly IItem _item;

        public ObjectAddComposer(IItem item)
        {
            _item = item;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.ObjectAddMessageComposer);
            _item.ComposeFloorItem(message);
            message.WriteString(_item.PlayerUsername);
            return message;
        }
    }
}
