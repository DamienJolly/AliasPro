using AliasPro.API.Messenger.Models;
using AliasPro.API.Network.Events;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Messenger.Packets.Composers
{
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
