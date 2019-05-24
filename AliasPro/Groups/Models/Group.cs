using AliasPro.API.Groups.Models;
using AliasPro.API.Database;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;

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
			Badge = reader.ReadData<string>("badge");
			ColourOne = reader.ReadData<int>("colour1");
			ColourTwo = reader.ReadData<int>("colour2");
			Members = new Dictionary<int, IGroupMember>();
		}

		internal Group(int id, string name, string desc, int createdAt, int ownerId, int roomId, string badge, int colourOne, int colourTwo)
		{
			Id = id;
			Name = name;
			Description = desc;
			CreatedAt = createdAt;
			OwnerId = ownerId;
			RoomId = roomId;
			Badge = badge;
			ColourOne = colourOne;
			ColourTwo = colourTwo;
			Members = new Dictionary<int, IGroupMember>();
		}

		public bool TryGetMember(int playerId, out IGroupMember member) =>
			Members.TryGetValue(playerId, out member);

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
		public string Badge { get; set; }
		public int ColourOne { get; set; }
		public int ColourTwo { get; set; }
		public IDictionary<int, IGroupMember> Members { get; set; }
	}
}
