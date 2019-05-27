using AliasPro.API.Groups.Models;
using AliasPro.API.Database;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using AliasPro.Groups.Types;

namespace AliasPro.Groups.Models
{
	internal class Group : IGroup
	{
		internal Group(DbDataReader reader)
		{
			Id = reader.ReadData<int>("id");
			Name = reader.ReadData<string>("name");
			Description = reader.ReadData<string>("desc");
			CreatedAt = reader.ReadData<int>("created_at");
			OwnerId = reader.ReadData<int>("owner_id");
			RoomId = reader.ReadData<int>("room_id");
			State = (GroupState)reader.ReadData<int>("state");
			Badge = reader.ReadData<string>("badge");
			ColourOne = reader.ReadData<int>("colour1");
			ColourTwo = reader.ReadData<int>("colour2");
			Members = new Dictionary<int, IGroupMember>();
		}

		public bool TryGetMember(int playerId, out IGroupMember member) =>
			Members.TryGetValue(playerId, out member);

		public bool TryAddMember(IGroupMember member) =>
			Members.TryAdd(member.PlayerId, member);

		public bool RemoveMember(int playerId) =>
			Members.Remove(playerId);

		public bool IsOwner(int playerId)
		{
			if (!TryGetMember(playerId, out IGroupMember member))
				return false;

			return (int)member.Rank == 0;
		}

		public bool IsAdmin(int playerId)
		{
			if (!TryGetMember(playerId, out IGroupMember member))
				return false;

			return (int)member.Rank <= 1;
		}

		public bool IsMember(int playerId)
		{
			if (!TryGetMember(playerId, out IGroupMember member))
				return false;

			return (int)member.Rank <= 2;
		}

		public int GetMembers =>
			Members.Values.Where(member => (int)member.Rank <= 2).Count();

		public int GetRequests =>
			Members.Values.Where(member => (int)member.Rank == 3).Count();

		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int OwnerId { get; set; }
		public int CreatedAt { get; set; }
		public int RoomId { get; set; }
		public GroupState State { get; set; }
		public string Badge { get; set; }
		public int ColourOne { get; set; }
		public int ColourTwo { get; set; }
		public IDictionary<int, IGroupMember> Members { get; set; }
	}
}
