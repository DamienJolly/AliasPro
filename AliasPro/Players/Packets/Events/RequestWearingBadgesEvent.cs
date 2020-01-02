using AliasPro.API.Network.Events;
using AliasPro.API.Network.Protocol;
using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Players.Packets.Composers;
using System.Collections.Generic;
using System.Linq;

namespace AliasPro.Players.Packets.Events
{
    public class RequestWearingBadgesEvent : IAsyncPacket
    {
        public short Header { get; } = Incoming.RequestWearingBadgesMessageEvent;

        private readonly IPlayerController _playerController;

        public RequestWearingBadgesEvent(
            IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async void HandleAsync(
            ISession session,
            IClientPacket clientPacket)
        {
            int playerId = clientPacket.ReadInt();

            ICollection<IPlayerBadge> targetBadges;

            if (_playerController.TryGetPlayer((uint)playerId, out IPlayer player) && player.Badge != null)
            {
                targetBadges = player.Badge.WornBadges;
            }
            else
            {
                IDictionary<string, IPlayerBadge> targetBadgesDictionary =
                    await _playerController.GetPlayerBadgesAsync((uint)playerId);
                targetBadges = WornBadges(targetBadgesDictionary.Values);
            }

            await session.SendPacketAsync(new UserBadgesComposer(targetBadges, playerId));
        }

        private ICollection<IPlayerBadge> WornBadges(ICollection<IPlayerBadge> badges) =>
            badges.Where(badge => badge.Slot > 0).OrderBy(badge => badge.Slot).ToList();
    }
}
