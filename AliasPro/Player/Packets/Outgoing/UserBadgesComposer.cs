using System.Collections.Generic;

namespace AliasPro.Player.Packets.Outgoing
{
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;
    using Models.Badge;

    public class UserBadgesComposer : IPacketComposer
    {
        private readonly ICollection<IBadgeData> _badges;
        private readonly uint _playerId;

        public UserBadgesComposer(ICollection<IBadgeData> badges, uint playerId)
        {
            _badges = badges;
            _playerId = playerId;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.UserBadgesMessageComposer);
            message.WriteInt(_playerId);
            message.WriteInt(_badges.Count);
            foreach (IBadgeData badge in _badges)
            {
                message.WriteInt(badge.Slot);
                message.WriteString(badge.Code);
            };
            return message;
        }
    }
}
