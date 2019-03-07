using System.Collections.Generic;

namespace AliasPro.Player.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Models.Badge;

    public class InventoryBadgesComposer : IPacketComposer
    {
        private readonly ICollection<IBadgeData> _badges;
        private readonly ICollection<IBadgeData> _wearableBadges;

        public InventoryBadgesComposer(ICollection<IBadgeData> badges, ICollection<IBadgeData> wearableBadges)
        {
            _badges = badges;
            _wearableBadges = wearableBadges;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.InventoryBadgesMessageComposer);
            message.WriteInt(_badges.Count);
            foreach (IBadgeData badge in _badges)
            {
                message.WriteInt(badge.Slot);
                message.WriteString(badge.Code);
            };

            message.WriteInt(_wearableBadges.Count);
            foreach (IBadgeData badge in _wearableBadges)
            {
                message.WriteInt(badge.Slot);
                message.WriteString(badge.Code);
            };
            return message;
        }
    }
}
