using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomOwnerComposer : IMessageComposer
    {
        public ServerMessage Compose() =>
            new ServerMessage(Outgoing.RoomOwnerMessageComposer);
    }
}
