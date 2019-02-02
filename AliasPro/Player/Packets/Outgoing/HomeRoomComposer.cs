namespace AliasPro.Player.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class HomeRoomComposer : IPacketComposer
    {
        private readonly int _roomId;

        public HomeRoomComposer(int roomId)
        {
            _roomId = roomId;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.HomeRoomMessageComposer);
            message.WriteInt(_roomId);
            message.WriteInt(_roomId);
            return message;
        }
    }
}
