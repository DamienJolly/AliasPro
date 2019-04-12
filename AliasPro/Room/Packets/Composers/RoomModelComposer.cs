using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;

namespace AliasPro.Room.Packets.Composers
{
    public class RoomModelComposer : IPacketComposer
    {
        private readonly string _modelName;
        private readonly uint _roomId;

        public RoomModelComposer(string modelName, uint roomId)
        {
            _modelName = modelName;
            _roomId = roomId;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomModelMessageComposer);
            message.WriteString(_modelName);
            message.WriteInt(_roomId);
            return message;
        }
    }
}
