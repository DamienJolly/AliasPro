using AliasPro.API.Players.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Players.Packets.Composers
{
    public class UserBadgesComposer : IMessageComposer
    {
        private readonly ICollection<IPlayerBadge> _badges;
        private readonly int _playerId;

        public UserBadgesComposer(ICollection<IPlayerBadge> badges, int playerId)
        {
            _badges = badges;
            _playerId = playerId;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.UserBadgesMessageComposer);
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
