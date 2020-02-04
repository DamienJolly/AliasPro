using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Items.Packets.Composers
{
    public class FloorItemUpdateComposer : IMessageComposer
    {
        private readonly IItem _item;

        public FloorItemUpdateComposer(IItem item)
        {
            _item = item;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.FloorItemUpdateMessageComposer);
            _item.ComposeFloorItem(message);
            return message;
        }
    }
}
