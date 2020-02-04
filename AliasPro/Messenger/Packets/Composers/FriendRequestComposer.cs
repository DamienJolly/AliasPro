using AliasPro.API.Messenger.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;

namespace AliasPro.Messenger.Packets.Composers
{
    public class FriendRequestComposer : IMessageComposer
    {
        private readonly IMessengerRequest _request;

        public FriendRequestComposer(IMessengerRequest request)
        {
            _request = request;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.FriendRequestMessageComposer);
            _request.Compose(message);
            return message;
        }
    }
}
