using AliasPro.API.Database;
using AliasPro.API.Rooms.Models;
using AliasPro.Utilities;
using System.Data.Common;

namespace AliasPro.Rooms.Models
{
    internal class RoomBan : IRoomBan
    {
        public RoomBan(DbDataReader reader)
        {
            PlayerId = reader.ReadData<int>("id");
            PlayerName = reader.ReadData<string>("username");
            ExpireTime = reader.ReadData<int>("expire_time");
        }

        public RoomBan(int playerId, string playerName, int expireTime)
        {
            PlayerId = playerId;
            PlayerName = playerName;
            ExpireTime = expireTime;
        }

        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int ExpireTime { get; set; }
        public bool HasExpired =>
            ExpireTime <= UnixTimestamp.Now;
    }
}
