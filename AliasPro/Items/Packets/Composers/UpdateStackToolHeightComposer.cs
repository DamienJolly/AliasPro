using AliasPro.API.Items.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Items.Packets.Composers
{ 
    public class UpdateStackToolHeightComposer : IMessageComposer
    {
        private readonly IItem _item;

        public UpdateStackToolHeightComposer(IItem item)
        {
			_item = item;
        }

        public ServerMessage Compose()
		{
            ServerMessage message = new ServerMessage(Outgoing.UpdateStackToolHeightMessageComposer);
			message.WriteInt((int)_item.Id);
			message.WriteInt((int)(_item.Position.Z * 100));
            return message;
        }
    }
}
