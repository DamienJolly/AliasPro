namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Models;

    public class RoomCreatedComposer : IPacketComposer
    {
        private readonly IRoomData _roomData;
        
        public RoomCreatedComposer(IRoomData roomData)
        {
            _roomData = roomData;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomCreatedMessageComposer);
            message.WriteInt(_roomData.Id);
            message.WriteString(_roomData.Name);
            return message;
        }
    }
}
