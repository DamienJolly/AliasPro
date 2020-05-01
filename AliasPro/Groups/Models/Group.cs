using AliasPro.API.Groups.Models;
using AliasPro.API.Database;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using AliasPro.Groups.Types;
using AliasPro.Utilities;

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
			OwnerName = reader.ReadData<string>("owner_name");
			RoomId = reader.ReadData<int>("room_id");
			RoomName = reader.ReadData<string>("room_name");
			State = (GroupState)reader.ReadData<int>("state");
			Rights = reader.ReadData<string>("rights") == "1";
			Badge = reader.ReadData<string>("badge");
			ColourOne = reader.ReadData<int>("colour1");
			ColourTwo = reader.ReadData<int>("colour2");
			Members = new Dictionary<int, IGroupMember>();
		}

		internal Group(string name, string description, int ownerId, string ownerName, int roomId, string roomName, string badge, int colourOne, int colourTwo)
		{
			Id = 0;
			Name = name;
			Description = description;
			CreatedAt = (int)UnixTimestamp.Now;
			OwnerId = ownerId;
			OwnerName = ownerName;
			RoomId = roomId;
			RoomName = roomName;
			State = GroupState.OPEN;
			Rights = false;
			Badge = badge;
			ColourOne = colourOne;
			ColourTwo = colourTwo;
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
		public string OwnerName { get; set; }
		public int CreatedAt { get; set; }
		public int RoomId { get; set; }
		public string RoomName { get; set; }
		public GroupState State { get; set; }
		public bool Rights { get; set; }
		public string Badge { get; set; }
		public int ColourOne { get; set; }
		public int ColourTwo { get; set; }
		public IDictionary<int, IGroupMember> Members { get; set; }

		public int IdleTime { get; set; } = 0;
	}
}
