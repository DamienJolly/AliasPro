using System.Data.Common;

namespace AliasPro.Player.Models.Badge
{
    using Database;

    internal class BadgeData : IBadgeData
    {
        public BadgeData(DbDataReader reader)
        {
            Code = reader.ReadData<string>("code");
            Slot = reader.ReadData<int>("slot");
        }

        public string Code { get; set; }
        public int Slot { get; set; }
    }

    public interface IBadgeData
    {
        string Code { get; set; }
        int Slot { get; set; }
    }
}
