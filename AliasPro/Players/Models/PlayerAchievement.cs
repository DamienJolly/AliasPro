using AliasPro.API.Database;
using AliasPro.API.Players.Models;
using System.Data.Common;

namespace AliasPro.Players.Models
{
    internal class PlayerAchievement : IPlayerAchievement
	{
        public PlayerAchievement(DbDataReader reader)
        {
            Id = reader.ReadData<int>("id");
            Progress = reader.ReadData<int>("progress");
        }

		public PlayerAchievement(int id, int progress)
		{
			Id = id;
			Progress = progress;
		}

		public int Id { get; set; }
        public int Progress { get; set; }
    }
}
