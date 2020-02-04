using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Items.Packets.Composers
{
    public class WallItemUpdateComposer : IMessageComposer
    {
        private readonly IItem _item;

        public WallItemUpdateComposer(IItem item)
        {
            _item = item;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.WallItemUpdateMessageComposer);
            _item.ComposeWallItem(message);
            message.WriteString(_item.PlayerUsername);
            return message;
        }
    }
}
