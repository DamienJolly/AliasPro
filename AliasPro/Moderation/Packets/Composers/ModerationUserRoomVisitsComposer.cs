﻿using AliasPro.API.Players.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System.Collections.Generic;

namespace AliasPro.Moderation.Packets.Composers
{
    public class ModerationUserRoomVisitsComposer : IMessageComposer
    {
        private readonly IPlayerData _playerData;
        private readonly ICollection<IPlayerRoomVisited> _roomVisits;

        public ModerationUserRoomVisitsComposer(IPlayerData playerData, ICollection<IPlayerRoomVisited> roomVisits)
        {
            _playerData = playerData;
            _roomVisits = roomVisits;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.ModerationUserRoomVisitsMessageComposer);
            message.WriteInt((int)_playerData.Id);
            message.WriteString(_playerData.Username);
            message.WriteInt(_roomVisits.Count);
            foreach (IPlayerRoomVisited visit in _roomVisits)
                visit.Compose(message);
            return message;
        }
    }
}
