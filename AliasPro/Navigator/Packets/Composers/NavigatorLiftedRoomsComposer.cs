using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Navigator.Packets.Composers
{
    public class NavigatorLiftedRoomsComposer : IMessageComposer
    {
        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.NavigatorLiftedRoomsMessageComposer);
            message.WriteInt(0);
            return message;
        }
    }
}
