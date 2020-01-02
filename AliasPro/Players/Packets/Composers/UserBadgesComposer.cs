using AliasPro.API.Network.Events;
using AliasPro.API.Players.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System.Collections.Generic;

namespace AliasPro.Players.Packets.Composers
{
    public class UserBadgesComposer : IPacketComposer
    {
        private readonly ICollection<IPlayerBadge> _badges;
        private readonly int _playerId;

        public UserBadgesComposer(ICollection<IPlayerBadge> badges, int playerId)
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
