using AliasPro.API.Database;
using AliasPro.API.Rooms.Models;
using System.Data.Common;

namespace AliasPro.Rooms.Models
{
    internal class RoomPromotion : IRoomPromotion
    {
        public RoomPromotion(DbDataReader reader)
        {
            Id = reader.ReadData<int>("category_id");
            Category = reader.ReadData<int>("category_id");
            Title = reader.ReadData<string>("title");
            Description = reader.ReadData<string>("description");
            StartTimestamp = reader.ReadData<int>("created_timestamp");
            EndTimestamp = reader.ReadData<int>("end_timestamp");
        }

        public RoomPromotion(int category, string title, string description, int startTimestamp, int endtimestamp)
        {
            Id = -1;
            Category = category;
            Title = title;
            Description = description;
            StartTimestamp = startTimestamp;
            EndTimestamp = endtimestamp;
        }

        public int Id { get; set; }
        public int Category { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int StartTimestamp { get; set; }
        public int EndTimestamp { get; set; }
    }
}
