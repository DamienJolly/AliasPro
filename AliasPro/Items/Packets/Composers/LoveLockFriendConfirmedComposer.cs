using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Items.Packets.Composers
{
    public class LoveLockFriendConfirmedComposer : IMessageComposer
    {
        private readonly IItem _item;

        public LoveLockFriendConfirmedComposer(IItem item)
        {
			_item = item;
		}

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.LoveLockFriendConfirmedMessageComposer);
            message.WriteInt((int)_item.Id);
            return message;
        }
    }
}
