﻿using AliasPro.API.Groups.Models;
using AliasPro.API.Groups.Types;
using AliasPro.API.Players.Models;
using AliasPro.Communication.Messages;
using AliasPro.Communication.Messages.Headers;
using AliasPro.Communication.Messages.Protocols;
using System;

namespace AliasPro.Groups.Packets.Composers
{
	public class GroupInfoComposer : IMessageComposer
	{
		private readonly IGroup _group;
		private readonly IPlayer _player;
		private readonly bool _newWindow;

		public GroupInfoComposer(
			IGroup group,
			IPlayer player,
			bool newWindow)
		{
			_group = group;
			_player = player;
			_newWindow = newWindow;
		}

		public ServerMessage Compose()
		{
			DateTime created = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(_group.CreatedAt);

			ServerMessage message = new ServerMessage(Outgoing.GroupInfoMessageComposer);
			message.WriteInt(_group.Id);
			message.WriteBoolean(true); // ??
			message.WriteInt((int)_group.State);
			message.WriteString(_group.Name);
			message.WriteString(_group.Description);
			message.WriteString(_group.Badge);
			message.WriteInt(_group.RoomId);
			message.WriteString(_group.RoomName);
			message.WriteInt(PlayerRank);
			message.WriteInt(_group.GetMembers);
			message.WriteBoolean(_player.IsFavoriteGroup(_group.Id));
			message.WriteString(created.Day + "-" + created.Month + "-" + created.Year);
			message.WriteBoolean(_group.IsOwner((int)_player.Id));
			message.WriteBoolean(_group.IsAdmin((int)_player.Id));
			message.WriteString(_group.OwnerName);
			message.WriteBoolean(_newWindow);
			message.WriteBoolean(_group.Rights);
			message.WriteInt(_group.IsAdmin((int)_player.Id) ? _group.GetRequests : 0);
			message.WriteBoolean(true); // forum
			return message;
		}

		private int PlayerRank
		{
			get
			{
				int playerRank = 0;
				if (_group.TryGetMember((int)_player.Id, out IGroupMember member))
				{
					switch (member.Rank)
					{
						case GroupRank.OWNER: playerRank = 4; break;
						case GroupRank.ADMIN: playerRank = 3; break;
						case GroupRank.REQUESTED: playerRank = 2; break;
						case GroupRank.MEMBER: playerRank = 1; break;
					}
				}

				return playerRank;
			}
		}
	}
}
