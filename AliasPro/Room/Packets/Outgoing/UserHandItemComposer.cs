namespace AliasPro.Room.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class UserHandItemComposer : IPacketComposer
    {
        private readonly int _virtualId;
        private readonly int _handItemId;

        public UserHandItemComposer(int virtualId, int handItemId)
        {
            _virtualId = virtualId;
            _handItemId = handItemId;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.UserHandItemMessageComposer);
            message.WriteInt(_virtualId);
            message.WriteInt(_handItemId);
            return message;
        }
    }
}
