namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class RoomEntryInfoComposer : IPacketComposer
    {
        private readonly uint _roomId;
        private readonly bool _hasRights;

        public RoomEntryInfoComposer(uint roomId, bool hasRights)
        {
            _roomId = roomId;
            _hasRights = hasRights;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomEntryInfoMessageComposer);
            message.WriteInt(_roomId);
            message.WriteBoolean(_hasRights);
            return message;
        }
    }
}
