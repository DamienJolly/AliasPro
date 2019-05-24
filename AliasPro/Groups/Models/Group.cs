using AliasPro.API.Groups.Models;
using AliasPro.API.Database;
using System.Data.Common;

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
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int OwnerId { get; set; }
		public int CreatedAt { get; set; }
		public int RoomId { get; set; }
		public string Badge { get; set; }
		public int ColourOne { get; set; }
		public int ColourTwo { get; set; }
	}
}
