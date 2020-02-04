using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Players.Packets.Composers
{
    public class HomeRoomComposer : IMessageComposer
    {
        private readonly int _roomId;

        public HomeRoomComposer(int roomId)
        {
            _roomId = roomId;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.HomeRoomMessageComposer);
            message.WriteInt(_roomId);
            message.WriteInt(_roomId);
            return message;
        }
    }
}
