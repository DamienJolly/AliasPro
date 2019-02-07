using System.Collections.Generic;

namespace AliasPro.Player.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Models.Messenger;

    public class LoadFriendRequestsComposer : IPacketComposer
    {
        private readonly ICollection<IMessengerRequest> _requests;

        public LoadFriendRequestsComposer(ICollection<IMessengerRequest> requests)
        {
            _requests = requests;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.LoadFriendRequestsMessageComposer);
            message.WriteInt(_requests.Count);
            message.WriteInt(_requests.Count);
            foreach (IMessengerRequest request in _requests)
            {
                request.Compose(message);
            }
            return message;
        }
    }
}
