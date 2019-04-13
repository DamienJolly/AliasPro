using AliasPro.API.Messenger;
using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;

namespace AliasPro.Messenger.Packets.Events
{
    public class DeclineFriendRequestEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.DeclineFriendRequestMessageEvent;

        private readonly IMessengerController _messengerController;

        public DeclineFriendRequestEvent(IMessengerController messengerController)
        {
            _messengerController = messengerController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            bool DeclineAll = clientPacket.ReadBool();
            
            if(DeclineAll)
            {
                session.Player.Messenger.RemoveAllRequests();
                await _messengerController.RemoveAllRequestsAsync(session.Player.Id);
            }
            else
            {
                clientPacket.ReadInt(); // dunno?
                uint targetId = (uint)clientPacket.ReadInt();

                session.Player.Messenger.RemoveRequest(targetId);
                await _messengerController.RemoveRequestAsync(session.Player.Id, targetId);
            }
        }
    }
}
