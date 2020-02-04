using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Items.Packets.Composers
{
    public class LoveLockFinishedComposer : IMessageComposer
    {
        private readonly IItem _item;

        public LoveLockFinishedComposer(IItem item)
        {
			_item = item;
		}

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.LoveLockFinishedMessageComposer);
            message.WriteInt((int)_item.Id);
            return message;
        }
    }
}
