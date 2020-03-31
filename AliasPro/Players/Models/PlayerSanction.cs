using AliasPro.API.Database;
using AliasPro.API.Players.Models;
using AliasPro.Players.Types;
using AliasPro.Utilities;
using System.Data.Common;

namespace AliasPro.Players.Models
{
    internal class PlayerSanction : IPlayerSanction
    {
        public PlayerSanction(DbDataReader reader)
        {
            switch (reader.ReadData<string>("type").ToUpper())
            {
                case "ALERT": default: Type = SanctionType.ALERT; break;
                case "MUTE": Type = SanctionType.MUTE; break;
                case "BAN": Type = SanctionType.BAN; break;
            }

            StartTime = reader.ReadData<int>("timestamp");
            ExpireTime = reader.ReadData<int>("expires");
            Reason = reader.ReadData<string>("reason");
            TopicId = reader.ReadData<int>("topic_id");
        }

        public PlayerSanction(SanctionType type, int expireTime, string reason, int topicId = -1)
        {
            Type = type;
            StartTime = (int)UnixTimestamp.Now;
            ExpireTime = expireTime;
            Reason = reason;
            TopicId = topicId;
        }

        public SanctionType Type { get; set; }
        public int StartTime { get; set; }
        public int ExpireTime { get; set; }
        public string Reason { get; set; }
        public int TopicId { get; set; }
    }
}
