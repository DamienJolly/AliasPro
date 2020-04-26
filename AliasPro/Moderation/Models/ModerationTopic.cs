using AliasPro.API.Database;
using AliasPro.API.Moderation.Models;
using System.Data.Common;

namespace AliasPro.Moderation.Models
{
	internal class ModerationTopic : IModerationTopic
	{
		internal ModerationTopic(DbDataReader reader)
		{
			Id = reader.ReadData<int>("id");
			Name = reader.ReadData<string>("name");
			Reply = reader.ReadData<string>("auto_reply");
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public string Reply { get; set; }
	}
}
