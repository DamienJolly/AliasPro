using AliasPro.API.Network.Events;
using AliasPro.API.Players.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Players.Packets.Composers
{
    public class InventoryBadgesComposer : IPacketComposer
    {
        private readonly ICollection<IPlayerBadge> _badges;
        private readonly ICollection<IPlayerBadge> _wearableBadges;

        public InventoryBadgesComposer(ICollection<IPlayerBadge> badges, ICollection<IPlayerBadge> wearableBadges)
        {
            _badges = badges;
            _wearableBadges = wearableBadges;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.InventoryBadgesMessageComposer);
            message.WriteInt(_badges.Count);
            foreach (IPlayerBadge badge in _badges)
            {
                message.WriteInt(badge.BadgeId);
                message.WriteString(badge.Code);
            };

            message.WriteInt(_wearableBadges.Count);
            foreach (IPlayerBadge badge in _wearableBadges)
            {
                message.WriteInt(badge.Slot);
                message.WriteString(badge.Code);
            };
            return message;
        }
    }
}
