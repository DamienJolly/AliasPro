namespace AliasPro.Player.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Models.Messenger;
    using Utilities;

    public class FriendChatComposer : IPacketComposer
    {
        private readonly IMessengerMessage _privateMessage;

        public FriendChatComposer(IMessengerMessage privateMessage)
        {
            _privateMessage = privateMessage;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.FriendChatMessageComposer);
            message.WriteInt(_privateMessage.TargetId);
            message.WriteString(_privateMessage.Message);
            message.WriteInt((int)UnixTimestamp.Now - 
                _privateMessage.Timestamp);
            return message;
        }
    }
}
