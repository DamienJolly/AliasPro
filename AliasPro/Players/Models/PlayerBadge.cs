using AliasPro.API.Database;
using AliasPro.API.Players.Models;
using System.Data.Common;

namespace AliasPro.Players.Models
{
    internal class PlayerBadge : IPlayerBadge
    {
        public PlayerBadge(DbDataReader reader)
        {
			BadgeId = reader.ReadData<int>("badge_id");
            Code = reader.ReadData<string>("code");
            Slot = reader.ReadData<int>("slot");
        }

		public PlayerBadge(int badgeId, string code, int slot = 0)
		{
			BadgeId = badgeId;
			Code = code;
			Slot = slot;
		}

		public int BadgeId { get; set; }
		public string Code { get; set; }
        public int Slot { get; set; }
    }
}
