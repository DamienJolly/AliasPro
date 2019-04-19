using AliasPro.API.Chat.Models;
using AliasPro.API.Database;
using AliasPro.Network.Protocol;
using System;
using System.Data.Common;

namespace AliasPro.Chat.Models
{
    internal class ChatLog : IChatLog
    {
        internal ChatLog(DbDataReader reader)
        {
            PlayerId = reader.ReadData<int>("player_id");
            PlayerUsername = reader.ReadData<string>("username");
            Timestamp = reader.ReadData<int>("timestamp");
            Message = reader.ReadData<string>("message");
        }

        public void Compose(ServerPacket message)
        {
            //todo: make util
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dt = dt.AddSeconds(Timestamp).ToLocalTime();

            message.WriteString(dt.ToString("HH:mm"));
            message.WriteInt(PlayerId);
            message.WriteString(PlayerUsername);
            message.WriteString(Message);
            message.WriteBoolean(false); //todo: shouting
        }

        public int PlayerId { get; set; }
        public string PlayerUsername { get; set; }
        public int Timestamp { get; set; }
        public string Message { get; set; }
    }
}
