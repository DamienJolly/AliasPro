﻿using AliasPro.API.Groups.Models;
using AliasPro.API.Groups.Types;
using AliasPro.API.Network.Events;
using AliasPro.API.Players.Models;
using AliasPro.Network.Events.Headers;
using AliasPro.Network.Protocol;
using System;

namespace AliasPro.Groups.Packets.Composers
{
	public class GroupInfoComposer : IPacketComposer
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

		public ServerPacket Compose()
		{
			DateTime created = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(_group.CreatedAt);
			int playerRank = 0;
			if (_group.TryGetMember((int)_player.Id, out IGroupMember member))
			{
				switch (member.Rank)
				{
					case GroupRank.ADMIN: playerRank = 4; break;
					case GroupRank.MOD: playerRank = 3; break;
					case GroupRank.REQUESTED: playerRank = 2; break;
					case GroupRank.MEMBER: playerRank = 1; break;
				}
			}

			int lol = playerRank;

			ServerPacket message = new ServerPacket(Outgoing.GroupInfoMessageComposer);
			message.WriteInt(_group.Id);
			message.WriteBoolean(true); // ??
			message.WriteInt((int)_group.State);
			message.WriteString(_group.Name);
			message.WriteString(_group.Description);
			message.WriteString(_group.Badge);
			message.WriteInt(_group.RoomId);
			message.WriteString("unknown"); // room name
			message.WriteInt(playerRank);
			message.WriteInt(_group.GetMembers);
			message.WriteBoolean(false); // fav group
			message.WriteString(created.Day + "-" + created.Month + "-" + created.Year);
			message.WriteBoolean(playerRank >= 4);
			message.WriteBoolean(playerRank >= 3);
			message.WriteString("unknown"); // owner name
			message.WriteBoolean(_newWindow);
			message.WriteBoolean(false); // user can furni
			message.WriteInt(playerRank >= 4 ? _group.GetRequests : 0);
			message.WriteBoolean(true); // forum
			return message;
		}
	}
}
