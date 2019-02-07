namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class ForwardToRoomComposer : IPacketComposer
    {
        private readonly uint _roomId;

        public ForwardToRoomComposer(uint roomId)
        {
            _roomId = roomId;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.ForwardToRoomMessageComposer);
            message.WriteInt(_roomId);
            return message;
        }
    }
}
