using AliasPro.API.Players;
using AliasPro.API.Players.Models;
using AliasPro.API.Sessions.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Players.Packets.Composers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliasPro.Players.Packets.Events
{
    public class RequestWearingBadgesEvent : IMessageEvent
    {
        public short Header => Incoming.RequestWearingBadgesMessageEvent;

        private readonly IPlayerController _playerController;

        public RequestWearingBadgesEvent(
            IPlayerController playerController)
        {
            _playerController = playerController;
        }

        public async Task RunAsync(
            ISession session,
            ClientMessage message)
        {
            int playerId = message.ReadInt();

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
