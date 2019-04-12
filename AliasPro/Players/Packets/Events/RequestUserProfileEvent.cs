using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Player.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Players.Packets.Composers;
using AliasPro.Sessions;

namespace AliasPro.Players.Packets.Events
{
    public class RequestUserProfileEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestUserProfileMessageEvent;

        private readonly IPlayerController _playerController;

        public RequestUserProfileEvent(IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async void HandleAsync(
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
