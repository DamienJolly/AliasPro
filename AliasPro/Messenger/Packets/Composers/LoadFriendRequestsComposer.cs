using AliasPro.API.Messenger.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Messenger.Packets.Composers
{
    public class LoadFriendRequestsComposer : IMessageComposer
    {
        private readonly ICollection<IMessengerRequest> _requests;

        public LoadFriendRequestsComposer(ICollection<IMessengerRequest> requests)
        {
            _requests = requests;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.LoadFriendRequestsMessageComposer);
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
