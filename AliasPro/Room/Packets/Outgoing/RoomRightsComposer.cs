namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class RoomRightsComposer : IPacketComposer
    {
        private readonly int _rightLevel;

        public RoomRightsComposer(int rightLevel)
        {
            _rightLevel = rightLevel;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.RoomRightsMessageComposer);
            message.WriteInt(_rightLevel);
            return message;
        }
    }
}
