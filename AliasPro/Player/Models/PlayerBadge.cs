using AliasPro.API.Player.Models;
using AliasPro.Database;
using System.Data.Common;

namespace AliasPro.Player.Models
{
    internal class PlayerBadge : IPlayerBadge
    {
        public PlayerBadge(DbDataReader reader)
        {
            Code = reader.ReadData<string>("code");
            Slot = reader.ReadData<int>("slot");
        }

        public string Code { get; set; }
        public int Slot { get; set; }
    }
}
