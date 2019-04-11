using AliasPro.API.Messenger.Models;
using AliasPro.Network.Events;
using AliasPro.Network.Protocol;

namespace AliasPro.Messenger.Packets.Outgoing
{
    using Network.Events.Headers;

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
