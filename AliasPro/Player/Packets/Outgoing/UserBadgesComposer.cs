using System.Collections.Generic;

namespace AliasPro.Player.Packets.Outgoing
{
    using AliasPro.API.Player.Models;
    using Network.Events;
    using Network.Events.Headers;
    using Network.Protocol;

    public class UserBadgesComposer : IPacketComposer
    {
        private readonly ICollection<IPlayerBadge> _badges;
        private readonly uint _playerId;

        public UserBadgesComposer(ICollection<IPlayerBadge> badges, uint playerId)
        {
            _badges = badges;
            _playerId = playerId;
        }

        public ServerPacket Compose()
        {
            ServerPacket message = new ServerPacket(Outgoing.UserBadgesMessageComposer);
            message.WriteInt(_playerId);
            message.WriteInt(_badges.Count);
            foreach (IPlayerBadge badge in _badges)
            {
                message.WriteInt(badge.Slot);
                message.WriteString(badge.Code);
            };
            return message;
        }
    }
}
