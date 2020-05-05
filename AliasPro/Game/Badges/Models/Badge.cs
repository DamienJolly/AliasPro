using AliasPro.API.Database;
using System.Data.Common;

namespace AliasPro.Game.Badges.Models
{
	public class BadgeData
	{
		public BadgeData(DbDataReader reader)
		{
			Id = reader.ReadData<int>("id");
			Code = reader.ReadData<string>("code");
			RequiredRight = reader.ReadData<string>("required_right");
		}

		public int Id { get; set; }
		public string Code { get; set; }
		public string RequiredRight { get; set; }
	}
}
