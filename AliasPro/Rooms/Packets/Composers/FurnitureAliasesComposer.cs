using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class FurnitureAliasesComposer : IMessageComposer
    {
        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.FurnitureAliasesMessageComposer);
            message.WriteInt(0);
            return message;
        }
    }
}
