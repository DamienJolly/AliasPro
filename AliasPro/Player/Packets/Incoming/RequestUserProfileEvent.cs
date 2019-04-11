using System.Threading.Tasks;

namespace AliasPro.Player.Packets.Incoming
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Packets.Outgoing;
    using Sessions;
    using Models;
    using AliasPro.API.Player.Models;

    public class RequestUserProfileEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestUserProfileMessageEvent;

        private readonly IPlayerController _playerController;

        public RequestUserProfileEvent(IPlayerController playerController)
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

            await session.SendPacketAsync(new UserProfileComposer(targetPlayer));
        }
    }
}
