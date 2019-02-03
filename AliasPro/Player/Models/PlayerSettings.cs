using System.Data.Common;

namespace AliasPro.Player.Models
{
    using Database;

    internal class PlayerSettings : IPlayerSettings
    {
        internal PlayerSettings(DbDataReader reader)
        {
            NaviX = reader.ReadData<int>("navi_x");
            NaviY = reader.ReadData<int>("navi_y");
            NaviWidth = reader.ReadData<int>("navi_width");
            NaviHeight = reader.ReadData<int>("navi_height");
            NaviHideSearches = reader.ReadData<bool>("navi_hide_searches");
        }

        public int NaviX { get; set; }
        public int NaviY { get; set; }
        public int NaviWidth { get; set; }
        public int NaviHeight { get; set; }
        public bool NaviHideSearches { get; set; }
    }

    public interface IPlayerSettings
    {
        int NaviX { get; set; }
        int NaviY { get; set; }
        int NaviWidth { get; set; }
        int NaviHeight { get; set; }
        bool NaviHideSearches { get; set; }
    }
}
