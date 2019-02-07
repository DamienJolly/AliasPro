using System.Threading.Tasks;

namespace AliasPro.Player.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;
    using Packets.Outgoing;
    using Models.Messenger;

    public class DeclineFriendRequestEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.DeclineFriendRequestMessageEvent;

        private readonly IPlayerController _playerController;

        public DeclineFriendRequestEvent(IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            bool DeclineAll = clientPacket.ReadBool();
            
            if(DeclineAll)
            {
                session.Player.Messenger.RemoveAllRequests();
                await _playerController.RemoveAllFriendRequestsAsync(session.Player.Id);
            }
            else
            {
                clientPacket.ReadInt(); // dunno?
                uint targetId = (uint)clientPacket.ReadInt();

                session.Player.Messenger.RemoveRequest(targetId);
                await _playerController.RemoveFriendRequestAsync(session.Player.Id, targetId);
            }
        }
    }
}
