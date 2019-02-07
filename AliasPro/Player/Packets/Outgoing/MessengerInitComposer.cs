namespace AliasPro.Player.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class MessengerInitComposer : IPacketComposer
    {
        private readonly int _maxFriends;

        public MessengerInitComposer(int maxFriends)
        {
            _maxFriends = maxFriends;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.MessengerInitMessageComposer);
            message.WriteInt(_maxFriends);
            message.WriteInt(300);
            message.WriteInt(800);
            message.WriteInt(0);
            message.WriteBoolean(true);
            return message;
        }
    }
}
