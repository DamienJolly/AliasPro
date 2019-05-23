using AliasPro.API.Database;
using AliasPro.API.Players.Models;
using System.Data.Common;

namespace AliasPro.Players.Models
{
    internal class PlayerBadge : IPlayerBadge
    {
        public PlayerBadge(DbDataReader reader)
        {
            Code = reader.ReadData<string>("code");
            Slot = reader.ReadData<int>("slot");
        }

		public PlayerBadge(string code, int slot = 0)
		{
			Code = code;
			Slot = slot;
		}

		public string Code { get; set; }
        public int Slot { get; set; }
    }
}
