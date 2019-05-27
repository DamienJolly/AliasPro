using AliasPro.API.Database;
using AliasPro.API.Groups.Models;
using AliasPro.API.Groups.Types;
using AliasPro.Network.Protocol;
using System;
using System.Data.Common;

namespace AliasPro.Groups.Models
{
	internal class GroupMember : IGroupMember
	{
		internal GroupMember(DbDataReader reader)
		{
			PlayerId = reader.ReadData<int>("player_id");
			Username = reader.ReadData<string>("username");
			Figure = reader.ReadData<string>("figure");
			JoinData = reader.ReadData<int>("join_date");
			Rank = (GroupRank)reader.ReadData<int>("rank");
		}

		internal GroupMember(int playerId, string username, string figure, int joinData, GroupRank rank)
		{
			PlayerId = playerId;
			Username = username;
			Figure = figure;
			JoinData = joinData;
			Rank = rank;
		}

		public void Compose(ServerPacket message)
		{
			DateTime created = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(JoinData);
			message.WriteInt((int)Rank);
			message.WriteInt(PlayerId);
			message.WriteString(Username);
			message.WriteString(Figure);
			message.WriteString(created.Day + "/" + created.Month + "/" + created.Year);
		}

		public int PlayerId { get; set; }
		public string Username { get; set; }
		public string Figure { get; set; }
		public int JoinData { get; set; }
		public GroupRank Rank { get; set; }
	}
}
