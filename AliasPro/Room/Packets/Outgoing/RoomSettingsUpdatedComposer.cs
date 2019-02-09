namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class RoomSettingsUpdatedComposer : IPacketComposer
    {
        private readonly uint _roomId;

        public RoomSettingsUpdatedComposer(uint roomId)
        {
            _roomId = roomId;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomSettingsUpdatedMessageComposer);
            message.WriteInt(_roomId);
            return message;
        }
    }
}
