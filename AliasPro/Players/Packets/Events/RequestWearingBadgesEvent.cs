using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Players.Packets.Composers;

namespace AliasPro.Players.Packets.Events
{
    public class RequestWearingBadgesEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestWearingBadgesMessageEvent;

        private readonly IPlayerController _playerController;

        public RequestWearingBadgesEvent(IPlayerController playerController)
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

            if (targetPlayer.Badge == null) return;

            await session.SendPacketAsync(new UserBadgesComposer(targetPlayer.Badge.WornBadges, targetPlayer.Id));
        }
    }
}
