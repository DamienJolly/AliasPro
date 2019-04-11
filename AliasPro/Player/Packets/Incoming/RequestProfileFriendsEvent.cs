using System.Threading.Tasks;

namespace AliasPro.Player.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Sessions;
    using Packets.Outgoing;
    using AliasPro.API.Player.Models;

    public class RequestProfileFriendsEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestProfileFriendsMessageEvent;

        private readonly IPlayerController _playerController;

        public RequestProfileFriendsEvent(IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async Task HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            uint playerId = (uint)clientPacket.ReadInt();

            if (!_playerController.TryGetPlayer(playerId, out IPlayer targetPlayer))
                return;

            //todo: remove/fix
            if (targetPlayer.Messenger == null) return;

            await session.SendPacketAsync(new ProfileFriendsComposer(targetPlayer.Id, targetPlayer.Messenger.Friends));
        }
    }
}
