using AliasPro.API.Messenger;
using AliasPro.Network.Events;
using AliasPro.Network.Protocol;
using AliasPro.Sessions;
using System.Threading.Tasks;

namespace AliasPro.Messenger.Packets.Incoming
{
    using Network.Events.Headers;

    public class DeclineFriendRequestEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.DeclineFriendRequestMessageEvent;

        private readonly IMessengerController _messengerController;

        public DeclineFriendRequestEvent(IMessengerController messengerController)
        {
            _messengerController = messengerController;
        }

        public async Task HandleAsync(
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
