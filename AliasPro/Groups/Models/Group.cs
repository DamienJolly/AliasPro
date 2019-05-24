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
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int OwnerId { get; set; }
		public int CreatedAt { get; set; }
		public int RoomId { get; set; }
	}
}
