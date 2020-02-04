using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomCreatedComposer : IMessageComposer
    {
        private readonly int _roomId;
        private readonly string _roomName;
        
        public RoomCreatedComposer(int roomId, string roomName)
        {
            _roomId = roomId;
            _roomName = roomName;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RoomCreatedMessageComposer);
            message.WriteInt(_roomId);
            message.WriteString(_roomName);
            return message;
        }
    }
}
