using AliasPro.API.Database;
using AliasPro.API.Moderation.Models;
using AliasPro.Moderation.Types;
using System.Data.Common;

namespace AliasPro.Moderation.Models
{
    internal class ModerationTicket : IModerationTicket
    {
        internal ModerationTicket(DbDataReader reader)
        {
            Id = reader.ReadData<int>("id");
            State = (ModerationTicketState)reader.ReadData<int>("state");
            Type = (ModerationTicketType)reader.ReadData<int>("type");
            Category = reader.ReadData<int>("category_id");
            Timestamp = reader.ReadData<int>("timestamp");
            Priority = reader.ReadData<int>("score");
            SenderId = reader.ReadData<int>("sender_id");
            SenderUsername = "Unknown";
            ReportedId = reader.ReadData<int>("reported_id");
            ReportedUsername = "Unknown";
            ModId = reader.ReadData<int>("mod_id");
            ModUsername = "Unknown";
            Caption = reader.ReadData<string>("caption");
            RoomId = reader.ReadData<int>("room_id");
        }

        public int Id { get; set; }
        public ModerationTicketState State { get; set; }
        public ModerationTicketType Type { get; set; }
        public int Category { get; set; }
        public int Timestamp { get; set; }
        public int Priority { get; set; }
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }
        public int ReportedId { get; set; }
        public string ReportedUsername { get; set; }
        public int ModId { get; set; }
        public string ModUsername { get; set; }
        public string Caption { get; set; }
        public int RoomId { get; set; }
    }
}
