namespace AliasPro.Player.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Models.Messenger;

    public class FriendRequestComposer : IPacketComposer
    {
        private readonly IMessengerRequest _request;

        public FriendRequestComposer(IMessengerRequest request)
        {
            _request = request;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.FriendRequestMessageComposer);
            _request.Compose(message);
            return message;
        }
    }
}
