using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Catalog.Packets.Composers
{
    public class DiscountComposer : IMessageComposer
    {
        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.DiscountMessageComposer);
            message.WriteInt(100);
            message.WriteInt(6);
            message.WriteInt(1);
            message.WriteInt(1);
            message.WriteInt(2);
            {
                message.WriteInt(40);
                message.WriteInt(99);
            }
            return message;
        }
    }
}
