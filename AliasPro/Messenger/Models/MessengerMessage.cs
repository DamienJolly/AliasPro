using AliasPro.API.Database;
using AliasPro.API.Messenger.Models;
using AliasPro.Utilities;
using System.Data.Common;

namespace AliasPro.Messenger.Models
{
    internal class MessengerMessage : IMessengerMessage
    {
        public MessengerMessage(DbDataReader reader)
        {
            TargetId = reader.ReadData<uint>("target_id");
            Message = reader.ReadData<string>("message");
            Timestamp = reader.ReadData<int>("timestamp");
        }

        public MessengerMessage(uint targetId, string message, int timestamp)
        {
            TargetId = targetId;
            Message = message;
            Timestamp = (int)UnixTimestamp.Now;
        }

        public uint TargetId { get; }
        public string Message { get; }
        public int Timestamp { get; }
    }
}
