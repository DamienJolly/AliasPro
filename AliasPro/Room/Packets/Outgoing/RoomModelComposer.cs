namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

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
