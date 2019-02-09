namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class RoomSettingsSavedComposer : IPacketComposer
    {
        private readonly uint _roomId;

        public RoomSettingsSavedComposer(uint roomId)
        {
            _roomId = roomId;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomSettingsSavedMessageComposer);
            message.WriteInt(_roomId);
            return message;
        }
    }
}
