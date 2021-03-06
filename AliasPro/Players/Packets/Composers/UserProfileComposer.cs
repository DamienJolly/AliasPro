﻿using AliasPro.API.Groups.Models;
using AliasPro.API.Players.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using AliasPro.Utilities;
using System.Collections.Generic;

namespace AliasPro.Players.Packets.Composers
{
    public class UserProfileComposer : IMessageComposer
    {
        private readonly IPlayer _player;
        private readonly IPlayerData _targetPlayer;
        private readonly ICollection<IGroup> _targetGroups;
        private readonly int _friendCount;

        public UserProfileComposer(
            IPlayer player,
            IPlayerData targetPlayer,
            ICollection<IGroup> targetGroups,
            int friendCount)
        {
            _player = player;
            _targetPlayer = targetPlayer;
            _targetGroups = targetGroups;
            _friendCount = friendCount;
        }

        public ServerMessage Compose()
        {
            ServerMessage message = new ServerMessage(Outgoing.UserProfileMessageComposer);
            message.WriteInt((int)_targetPlayer.Id);
            message.WriteString(_targetPlayer.Username);
            message.WriteString(_targetPlayer.Figure);
            message.WriteString(_targetPlayer.Motto);
            message.WriteString(UnixTimestamp.FromUnixTimestamp(_targetPlayer.CreatedAt).ToString("dd-MM-yyyy"));
            message.WriteInt(_targetPlayer.Score);
            message.WriteInt(_friendCount);
            message.WriteBoolean(_player.Messenger.TryGetFriend(_targetPlayer.Id, out _));
            message.WriteBoolean(_player.Messenger.TryGetRequest(_targetPlayer.Id, out _));
            message.WriteBoolean(_targetPlayer.Online);

            message.WriteInt(_targetGroups.Count);
            foreach (IGroup group in _targetGroups)
            {
                message.WriteInt(group.Id);
                message.WriteString(group.Name);
                message.WriteString(group.Badge);
                message.WriteString(""); // group colour1
                message.WriteString(""); // group colour2
                message.WriteBoolean(_targetPlayer.IsFavoriteGroup(group.Id));
                message.WriteInt(group.OwnerId);
                message.WriteBoolean(group.IsOwner((int)_targetPlayer.Id));
            }

			message.WriteInt((int)UnixTimestamp.Now - _targetPlayer.LastOnline); // -1 if hidding offline
            message.WriteBoolean(true); // dunno??
            return message;
        }
    }
}
