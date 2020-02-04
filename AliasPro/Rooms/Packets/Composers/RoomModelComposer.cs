using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Rooms.Packets.Composers
{
    public class RoomModelComposer : IMessageComposer
    {
        private readonly string _modelName;
        private readonly uint _roomId;

        public RoomModelComposer(string modelName, uint roomId)
        {
            _modelName = modelName;
            _roomId = roomId;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.RoomModelMessageComposer);
            message.WriteString(_modelName);
            message.WriteInt((int)_roomId);
            return message;
        }
    }
}
