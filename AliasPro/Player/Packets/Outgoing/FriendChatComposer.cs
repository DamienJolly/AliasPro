namespace AliasPro.Player.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class FriendChatComposer : IPacketComposer
    {
        private readonly uint _playerId;
        private readonly string _message;

        public FriendChatComposer(uint playerId, string message)
        {
            _playerId = playerId;
            _message = message;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.FriendChatMessageComposer);
            message.WriteInt(_playerId);
            message.WriteString(_message);
            message.WriteInt(0); //time
            return message;
        }
    }
}
