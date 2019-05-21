using AliasPro.API.Database;
using AliasPro.API.Moderation.Models;
using System.Data.Common;

namespace AliasPro.Moderation.Models
{
	internal class ModerationPreset : IModerationPreset
	{
		internal ModerationPreset(DbDataReader reader)
		{
			Type = reader.ReadData<string>("type");
			Data = reader.ReadData<string>("preset");
		}

		public string Type { get; set; }
		public string Data { get; set; }
	}
}
