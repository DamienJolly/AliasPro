using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Items.Packets.Composers
{
    public class LoveLockStartComposer : IMessageComposer
    {
        private readonly IItem _item;

        public LoveLockStartComposer(IItem item)
        {
			_item = item;
		}

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.LoveLockStartMessageComposer);
            message.WriteInt((int)_item.Id);
			message.WriteBoolean(true);
            return message;
        }
    }
}
